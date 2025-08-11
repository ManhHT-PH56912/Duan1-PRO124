using UnityEngine;

public class BloodEffectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] bloodPrefabs;

    public void SpawnBlood(Vector3 position, bool flipX)
    {
        if (bloodPrefabs.Length == 0) return;

        // Random prefab máu
        int index = Random.Range(0, bloodPrefabs.Length);
        GameObject blood = Instantiate(bloodPrefabs[index], position, Quaternion.identity);

        // Thiết lập hướng máu theo hướng của kẻ địch
        Vector3 scale = blood.transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (flipX ? -1 : 1);
        blood.transform.localScale = scale;
        // Hủy sau 1 giây (tùy theo animation length)
        Destroy(blood, 1f);
    }
}
