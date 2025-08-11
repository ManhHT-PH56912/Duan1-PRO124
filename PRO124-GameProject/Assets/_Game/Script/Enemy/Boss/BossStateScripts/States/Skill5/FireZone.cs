using UnityEngine;

public class FireZone : MonoBehaviour
{
    public float duration = 15f;
    private float timer = 0f;

    private float damageInterval = 1f; // 1 giây

    private float damageTimer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= duration)
            Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("vùng lửa");
            damageTimer += Time.deltaTime;
            if (damageTimer >= damageInterval)
            {
                damageTimer = 0f;

                PlayerStats stats = other.GetComponent<PlayerStats>();
                if (stats != null)
                {
                    stats.ApplyBurn(5f, 3f); // Gây 5 damage/giây, burn 3 giây sau khi rời
                }
            }
        }
    }

}
