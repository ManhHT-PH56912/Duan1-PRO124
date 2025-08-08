using UnityEngine;

public class SlashWave : MonoBehaviour
{
    public float speed = 8f;
    public float lifetime = 2f;
    public float damage = 10f;

    private Vector2 direction;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;

        // Lật sprite nếu hướng sang trái
        if (dir.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);

        lifetime -= Time.deltaTime;
        if (lifetime <= 0f)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("chém trúng");

            PlayerStats stats = other.GetComponent<PlayerStats>();
            if (stats != null)
            {
                stats.TakeDamage(damage);
            }
        }
    }
}
