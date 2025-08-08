using UnityEngine;

public class ArrowBoss : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 10;

    private Rigidbody2D rb;
    private bool isFlying = false;
    private Vector2 direction;
    public string groundTag = "Ground";

    public void Init(Vector2 dir)
    {
        direction = dir.normalized;
        isFlying = true;
        Destroy(gameObject, 3f);
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (isFlying)
        {
            rb.linearVelocity = direction * speed;
        }
    }

    public void StartFalling()
    {
        isFlying = false;
        rb.linearVelocity = Vector2.down * speed;
        rb.gravityScale = 1f; // hoặc cao hơn nếu muốn rơi nhanh
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Gây damage cho player
        if (collision.CompareTag("Player"))
        {
            PlayerStats stats = collision.GetComponent<PlayerStats>();
            if (stats != null)
            {
                stats.TakeDamage(damage);
            }
            Debug.Log("Arrow hit player for " + damage + " damage!");
            Destroy(gameObject);
        }

        // Nếu chạm ground → hủy
        if (collision.CompareTag(groundTag))
        {
            Destroy(gameObject);
        }
    }
}
