using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Settings")]
    public GameObject enemyPrefab;
    public bool spawnOnStart = false;
    public int phaseID = 1; // Phase mà spawner này thuộc về

    private GameObject currentEnemy;

    private void Start()
    {
        if (spawnOnStart)
            SpawnEnemy();
    }

    public void SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogWarning($"{name}: Enemy prefab not assigned!");
            return;
        }


        if (currentEnemy != null && currentEnemy.activeInHierarchy)
            return;

        currentEnemy = PoolManager.Instance.Get(enemyPrefab, transform.position, Quaternion.identity);

        EnemyBase enemyBase = currentEnemy.GetComponent<EnemyBase>();
        if (enemyBase != null)
        {
            enemyBase.Init();
            enemyBase.OnSpawn();
        }
    }
    public void OnEnemyReturned()
    {
        currentEnemy = null;
    }

}
