using UnityEngine;

public class FireBomb : MonoBehaviour
{
    public GameObject fireZonePrefab;

    private Vector2 startPoint;
    private Vector2 endPoint;
    private Vector2 peakPoint;

    private float travelTime = 1.2f;
    private float timer = 0f;

    private bool launched = false;
    Animator animator;
    public GameObject FireBombEffect;

    public void LaunchParabola(Vector2 target)
    {
        launched = true;
        startPoint = transform.position;
        endPoint = target;
        animator = GetComponent<Animator>();
        float peakHeight = 3f;
        float midX = (startPoint.x + endPoint.x) / 2f;
        peakPoint = new Vector2(midX, Mathf.Max(startPoint.y, endPoint.y) + peakHeight);
    }

    void Update()
    {
        if (!launched) return;
        animator.Play("FireBomb");
        timer += Time.deltaTime;
        float t = timer / travelTime;

        if (t >= 1f)
        {
            Explode();
            return;
        }

        // Nội suy vị trí parabol
        Vector2 a = Vector2.Lerp(startPoint, peakPoint, t);
        Vector2 b = Vector2.Lerp(peakPoint, endPoint, t);
        Vector2 newPos = Vector2.Lerp(a, b, t);

        // Tính hướng di chuyển để xoay
        Vector2 velocity = (newPos - (Vector2)transform.position) / Time.deltaTime;
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        transform.position = newPos;
    }


    private void Explode()
    {
        Vector2 spawnPosition = transform.position;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 10f, LayerMask.GetMask("Ground"));
        if (hit.collider != null)
        {
            spawnPosition.y = hit.point.y + 0.5f;
        }

        Instantiate(FireBombEffect, spawnPosition, Quaternion.identity);
        Instantiate(fireZonePrefab, spawnPosition, Quaternion.identity);

        Destroy(gameObject);
    }

}