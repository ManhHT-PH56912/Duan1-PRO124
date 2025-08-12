using UnityEngine;

public class CoinCollide : MonoBehaviour
{
    [SerializeField] private int coinValue = 1;
    [SerializeField] private AudioClip collectSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CollectCoin();
            Destroy(this.gameObject);
        }
    }

    private void CollectCoin()
    {
        Debug.Log("Coin collected!");
        // play sound effect or animation here if needed
        if (collectSound != null)
        {
            AudioManager.Instance.PlaySound(collectSound);
        }
        // Cập nhật điểm số
        ScoreManager.Instance.AddCoins(coinValue);
    }
}