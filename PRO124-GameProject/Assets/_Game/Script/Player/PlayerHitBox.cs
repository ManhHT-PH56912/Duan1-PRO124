using UnityEngine;

public class PlayerAttackHitbox : MonoBehaviour
{
    private PlayerStats playerStats;
    public GameObject[] bloodEffects;
    private void Start()
    {
        playerStats = GetComponentInParent<PlayerStats>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyBase enemy = other.GetComponentInParent<EnemyBase>();
        if (enemy != null)
        {

            Debug.Log($"Gây {playerStats.attack} damage cho {enemy.name}");
            enemy.TakeDamage(playerStats.attack, this);
        }
    }
}
