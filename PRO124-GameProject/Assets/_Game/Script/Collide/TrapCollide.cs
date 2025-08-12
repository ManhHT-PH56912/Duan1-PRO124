using UnityEngine;

public class TrapCollide : MonoBehaviour
{
    [SerializeField] private float damage = 2f;
    public GameObject trapEffect; // Reference to the trap effect prefab

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player has triggered a trap!");
            // Play the trap effect
            // You can add any additional logic here, such as reducing player health or triggering a game
            // over event.
            // trừ máu player 
            if (other.TryGetComponent<PlayerStats>(out var playerHealth))
            {
                playerHealth.TakeDamage(damage);
            }

            // Instantiate the trap effect at the player's position
            Instantiate(trapEffect, other.transform.position, Quaternion.identity);

            Destroy(trapEffect, 2f);
        }
    }
}