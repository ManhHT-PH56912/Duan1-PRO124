using UnityEngine;

public class Arrow : MonoBehaviour
{
    [Header("Arrow Settings")]
    public float speed = 8f;
    public int damage = 10;
    public float lifeTime = 5f;

    private Vector2 direction;
    private float timer;
    public string wallTag = "Wall";

    public void Init(Vector2 dir)
    {
        direction = dir.normalized;
        timer = lifeTime;
        transform.localScale = new Vector3(Mathf.Sign(dir.x), 1, 1); // Quay theo hướng bắn
    }

    private void Update()
    {
        // Di chuyển
        transform.Translate(direction * speed * Time.deltaTime);

        // Tự hủy sau thời gian tồn tại
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            Destroy(gameObject);
        }
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
            Debug.Log("tên bắn " + damage + " damage!");
            Destroy(gameObject);
        }

        // Nếu chạm ground → hủy
        if (collision.CompareTag(wallTag))
        {
            Destroy(gameObject);
        }
    }
}