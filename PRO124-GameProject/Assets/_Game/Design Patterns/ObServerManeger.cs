using System.Collections.Generic;
using UnityEngine;

public class ObserverManager : MonoBehaviour
{
    public static ObserverManager Instance;
    private readonly List<IObserver> observers = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddObserver(IObserver obs)
    {
        if (!observers.Contains(obs)) observers.Add(obs);
    }

    public void RemoveObserver(IObserver obs) => observers.Remove(obs);

    public void ReturnAllToPool()
    {
        foreach (var obs in observers.ToArray())
            obs.ReturnPool();
    }

}
