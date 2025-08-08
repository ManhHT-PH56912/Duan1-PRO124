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

    public void LaunchParabola(Vector2 target)
    {
        launched = true;
        startPoint = transform.position;
        endPoint = target;

        float peakHeight = 3f;
        float midX = (startPoint.x + endPoint.x) / 2f;
        peakPoint = new Vector2(midX, Mathf.Max(startPoint.y, endPoint.y) + peakHeight);
    }

    void Update()
    {
        if (!launched) return;

        timer += Time.deltaTime;
        float t = timer / travelTime;

        if (t >= 1f)
        {
            Explode();
            return;
        }

        Vector2 a = Vector2.Lerp(startPoint, peakPoint, t);
        Vector2 b = Vector2.Lerp(peakPoint, endPoint, t);
        transform.position = Vector2.Lerp(a, b, t);
    }

    private void Explode()
    {
        Vector2 spawnPosition = transform.position;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 10f, LayerMask.GetMask("Ground"));
        if (hit.collider != null)
        {
            spawnPosition.y = hit.point.y; // Đặt sát mặt đất
        }

        Instantiate(fireZonePrefab, spawnPosition, Quaternion.identity);
        Destroy(gameObject);
    }

}