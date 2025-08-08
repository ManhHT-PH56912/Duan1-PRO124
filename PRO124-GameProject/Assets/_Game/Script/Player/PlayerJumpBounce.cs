using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerJumpBounce : MonoBehaviour
{
    public float bounceForce = 2f; // Lực nảy khi đạp enemy

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Bounce()
    {
        // Reset velocity trước khi nảy lên
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

        // Thêm lực theo hướng lên
        rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
    }
}
