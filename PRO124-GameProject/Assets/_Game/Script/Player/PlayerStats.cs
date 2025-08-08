using System;
using System.Collections;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Health Stats")]
    public float maxHealth = 10000;
    public float currentHealth;

    [Header("Combat Stats")]
    public float attack = 10;       // Damage gây ra
    public float defense = 5;       // Giảm sát thương
    public float healingPower = 0;
    public float ManingPower = 0;
    [Header("Mana Stats")]
    public float maxMana = 10000;
    public float currentMana;
    public float manaRegenRate = 5f; // Tăng hiệu quả hồi máu

    // --- BURN SYSTEM ---
    private bool isBurning = false;
    private float burnDuration = 0f;
    private float burnTimer = 0f;

    private float burnDamageInterval = 1f; // mỗi 1 giây mất máu
    private float burnTickTimer = 0f;
    private float burnDamage = 0f;
    // --- Block Damge ---
    public bool isInvincible = false;

    private void Start()
    {
        currentHealth = maxHealth;
        currentMana = maxMana;
    }

    private void Update()
    {
        HandleBurn();
        RegenerateMana();

    }

    private void RegenerateMana()
    {
        currentMana = Mathf.Min(currentMana + manaRegenRate * Time.deltaTime, maxMana);
    }
    public bool UseMana(float amount)
    {
        if (currentMana >= amount)
        {
            currentMana -= amount;
            return true;
        }
        return false;
    }

    public void TakeDamage(float incomingDamage)
    {
        if (isInvincible)
        {
            Debug.Log("Bất Tử, không nhận sát thương!");
            return;
        }
        int finalDamage = Mathf.Max(1, Mathf.CeilToInt(incomingDamage * (100f / (100f + defense))));
        currentHealth -= finalDamage;

        Debug.Log($"Player nhận {finalDamage} damage (raw: {incomingDamage})");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float baseHeal)
    {
        float finalHeal = baseHeal + healingPower;
        currentHealth = Mathf.Min(currentHealth + finalHeal, maxHealth);
        Debug.Log($"Player hồi {finalHeal} máu");
    }

    private void Die()
    {
        Debug.Log("Player chết");
        // TODO: Trigger animation chết, disable điều khiển,...
    }

    // 🔥 Gọi từ FireZone mỗi lần player đang ở trong vùng lửa
    public void ApplyBurn(float damagePerSecond, float durationAfterExit)
    {
        isBurning = true;
        burnDuration = durationAfterExit;
        burnDamage = damagePerSecond;

        burnTimer = 0f;
        burnTickTimer = 0f;
    }

    private void HandleBurn()
    {
        if (!isBurning) return;

        burnTimer += Time.deltaTime;
        burnTickTimer += Time.deltaTime;

        if (burnTickTimer >= burnDamageInterval)
        {
            TakeDamage(burnDamage);
            burnTickTimer = 0f;
        }

        if (burnTimer >= burnDuration)
        {
            isBurning = false;
        }
    }
    // Gọi từ ngoài để bật bất tử
    public void SetInvincibility(float duration)
    {
        StartCoroutine(InvincibilityCoroutine(duration));
    }

    private IEnumerator InvincibilityCoroutine(float duration)
    {
        isInvincible = true;
        yield return new WaitForSeconds(duration);
        isInvincible = false;
    }
}
