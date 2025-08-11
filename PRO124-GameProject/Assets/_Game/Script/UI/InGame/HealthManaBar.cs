using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthManaBar : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] Slider healthSlider;
    [SerializeField] TextMeshProUGUI healthText;

    [Header("Mana")]
    [SerializeField] Slider manaSlider;
    [SerializeField] TextMeshProUGUI manaText;

    private int maxHealth;
    private int maxMana;

    public void SetMaxHealth(int health)
    {
        maxHealth = health;
        healthSlider.maxValue = health;
        healthSlider.value = health;
        UpdateHealthText(health);
    }

    public void SetHealth(int health)
    {
        healthSlider.value = health;
        UpdateHealthText(health);
    }

    private void UpdateHealthText(int currentHealth)
    {
        healthText.text = $"{currentHealth}/{maxHealth}";
    }

    public void SetMaxMana(int mana)
    {
        maxMana = mana;
        manaSlider.maxValue = mana;
        manaSlider.value = mana;
        UpdateManaText(mana);
    }

    public void SetMana(int mana)
    {
        manaSlider.value = mana;
        UpdateManaText(mana);
    }

    private void UpdateManaText(int currentMana)
    {
        manaText.text = $"{currentMana}/{maxMana}";
    }

    // ✅ Hàm này để cập nhật cả 2 cùng lúc
    public void UpdateUI(int currentHealth, int currentMana)
    {
        SetHealth(currentHealth);
        SetMana(currentMana);
    }
}
