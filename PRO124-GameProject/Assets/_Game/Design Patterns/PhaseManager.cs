using UnityEngine;

public class PhaseManager : MonoBehaviour
{
    public static PhaseManager Instance;
    public int CurrentPhase { get; private set; } = 1;
    private EnemySpawner[] allSpawners;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // Lấy tất cả spawner trong scene
        allSpawners = FindObjectsOfType<EnemySpawner>();
    }

    public void SwitchToPhase(int phaseID)
    {
        // Thu hồi toàn bộ enemy hiện tại
        ObserverManager.Instance.ReturnAllToPool();

        // Spawn lại enemy chỉ thuộc phase mới
        foreach (var spawner in allSpawners)
        {
            if (spawner.phaseID == phaseID)
            {
                spawner.SpawnEnemy();
            }
        }
    }
    public void SetPhase(int newPhase)
    {
        CurrentPhase = newPhase;
        Debug.Log($"Phase changed to {newPhase}");
    }
}
