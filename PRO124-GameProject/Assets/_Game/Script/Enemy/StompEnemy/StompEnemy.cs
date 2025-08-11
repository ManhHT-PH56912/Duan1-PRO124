using UnityEngine;

public class StompEnemy : EnemyBase, IObserver
{
    [Header("Movement")]
    public float patrolSpeed = 2f;
    public float patrolDistance = 3f;

    [Header("Detection")]
    public Transform visual;
    public float stompOffsetY = 0.5f; // Độ cao để tính "đạp đầu"

    private Rigidbody2D rb;
    private Vector2 startPos;
    private int dir = 1;
    private bool reachedEdge = false;


    private void Awake()
    {
        Damage = 10; // Thiết lập sát thương mặc định
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;
    }

    [System.Obsolete]
    private void Update()
    {
        Patrol();
    }

    [System.Obsolete]
    private void Patrol()
    {
        rb.linearVelocity = new Vector2(patrolSpeed * dir, rb.linearVelocity.y);

        float dist = transform.position.x - startPos.x;

        // Khi đến biên, chỉ lật hướng 1 lần
        if (!reachedEdge && Mathf.Abs(dist) >= patrolDistance)
        {
            dir *= -1;
            reachedEdge = true;

            // Clamp lại vị trí để không vượt quá giới hạn
            float clampedX = startPos.x + Mathf.Clamp(dist, -patrolDistance, patrolDistance);
            transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
        }

        // Khi đã quay đầu và rời khỏi mép, cho phép quay đầu lần sau
        if (reachedEdge && Mathf.Abs(dist) < patrolDistance - 0.1f)
        {
            reachedEdge = false;
        }

        // Flip hình ảnh nếu có
        if (visual != null)
        {
            Vector3 scale = visual.localScale;
            scale.x = dir * Mathf.Abs(scale.x);
            visual.localScale = scale;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            float playerY = collision.transform.position.y;
            float enemyY = transform.position.y;

            // Nếu player đạp đầu
            if (playerY > enemyY + stompOffsetY)
            {
                Debug.Log("Player đạp chết StompEnemy!");

                // Player nảy lên
                PlayerJumpBounce bounce = collision.collider.GetComponent<PlayerJumpBounce>();
                if (bounce != null) bounce.Bounce();
                // Trả về pool hoặc hủy đối tượng
                ReturnPool();

            }
            else
            {
                // Gây sát thương
                PlayerStats playerStats = collision.collider.GetComponent<PlayerStats>();
                if (playerStats != null)
                {
                    playerStats.TakeDamage(Damage);
                    Debug.Log($"BoomEnemy gây {Damage} sát thương cho Player");
                }
            }
        }
    }


    public override void Die()
    {
        Debug.Log("Enemy bị đạp chết!");
        ReturnPool();
    }
    public override void OnSpawn()
    {
        startPos = transform.position;
        dir = 1;
        reachedEdge = false;
    }

    public override void TakeDamage(int damage, MonoBehaviour attacker) { }

    public override void Idle() { }

    public override void Move(float speed) { }

    public override void Attack() { }
}
