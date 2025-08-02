using UnityEngine;

public class PlayerJumpBounce : MonoBehaviour
{
    [SerializeField] private float bounceForce;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Bounce()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, bounceForce);
    }
}
