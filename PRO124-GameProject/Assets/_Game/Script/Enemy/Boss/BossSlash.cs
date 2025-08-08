using UnityEngine;

public class BossSlash : MonoBehaviour
{
    public float damageAmount = 20f;
    private bool canDamage = false;

    public void EnableDamage()
    {
        canDamage = true;
    }

    public void DisableDamage()
    {
        canDamage = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!canDamage) return;

        if (other.CompareTag("Player"))
        {
            Debug.Log("Boss Slash hit the player!");
            PlayerStats playerStat = other.GetComponent<PlayerStats>();
            if (playerStat != null)
            {
                playerStat.TakeDamage(Mathf.RoundToInt(damageAmount));
                canDamage = false; // Gây sát thương 1 lần mỗi lần chém
            }
        }
    }
}
