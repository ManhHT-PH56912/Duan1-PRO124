using UnityEngine;

public class BloodEffectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] bloodPrefabs;

    public void SpawnBlood(Vector3 position)
    {
        if (bloodPrefabs.Length == 0) return;

        // Random prefab máu
        int index = Random.Range(0, bloodPrefabs.Length);
        GameObject blood = Instantiate(bloodPrefabs[index], position, Quaternion.identity);

        // Hủy sau 1 giây (tùy theo animation length)
        Destroy(blood, 1f);
    }
}
