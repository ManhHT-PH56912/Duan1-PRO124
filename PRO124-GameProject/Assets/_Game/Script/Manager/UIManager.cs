using UnityEngine;
using DesignPatterns.Singleton;

public class UIManager : Singleton<UIManager>
{
    [Header("References")]
    [SerializeField] private HealthManaBar healthManaBar;
    [SerializeField] private MonnyUI monnyUI;

    private FirebaseManager firebaseManager;

    protected override void Awake()
    {
        base.Awake();
        firebaseManager = FirebaseManager.Instance;

        if (healthManaBar == null)
            healthManaBar = FindFirstObjectByType<HealthManaBar>();

        if (monnyUI == null)
            monnyUI = FindFirstObjectByType<MonnyUI>();
    }

    private void Start()
    {
        ApplyPlayerDataToUI();
        if (firebaseManager != null)
            firebaseManager.OnPlayerDataUpdated += ApplyPlayerDataToUI;
    }

    private void OnDestroy()
    {
        if (firebaseManager != null)
            firebaseManager.OnPlayerDataUpdated -= ApplyPlayerDataToUI;
    }

    public void ApplyPlayerDataToUI()
    {
        if (firebaseManager?.playerData == null) return;

        var data = firebaseManager.playerData;
        if (healthManaBar != null)
        {
            healthManaBar.SetHealth(data.stats.health);
            healthManaBar.SetMana(data.stats.mana);
        }
        if (monnyUI != null)
            monnyUI.SetCoin(data.stats.coins);
    }

    public void OnPlayerDataChanged() => ApplyPlayerDataToUI();
}
