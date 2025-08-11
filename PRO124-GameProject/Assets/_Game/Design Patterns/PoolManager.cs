using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;
    private readonly Dictionary<string, Queue<GameObject>> pools = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

    }

    public GameObject Get(GameObject prefab, Vector3 pos, Quaternion rot)
    {
        string key = prefab.name;
        if (!pools.ContainsKey(key) || pools[key].Count == 0)
        {
            GameObject obj = Instantiate(prefab, pos, rot);
            obj.name = key;
            return obj;
        }

        var pooled = pools[key].Dequeue();
        pooled.transform.SetPositionAndRotation(pos, rot);
        pooled.SetActive(true);
        return pooled;
    }

    public void Return(GameObject obj)
    {
        string key = obj.name;
        if (!pools.ContainsKey(key)) pools[key] = new Queue<GameObject>();
        obj.SetActive(false);
        pools[key].Enqueue(obj);
    }
}
