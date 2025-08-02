using UnityEngine;
using System.Collections.Generic;
using DesignPatterns.Singleton;

public class LoadingSceneManager : Singleton<LoadingSceneManager>
{
    [SerializeField] private GameObject asyncLoadingPrefab;
    private Dictionary<SceneIndexs, AsyncLoading> activeLoaders = new();

    protected override void Awake()
    {
        base.Awake();
    }

    public void LoadScene(SceneIndexs targetScene)
    {
        if (asyncLoadingPrefab == null)
        {
            Debug.LogError("AsyncLoading prefab is not assigned.", this);
            return;
        }

        if (activeLoaders.ContainsKey(targetScene))
        {
            Debug.LogWarning($"Scene {targetScene} is already being loaded.", this);
            return;
        }

        GameObject loaderObj = Instantiate(asyncLoadingPrefab);
        AsyncLoading loader = loaderObj.GetComponent<AsyncLoading>();

        if (loader == null)
        {
            Debug.LogError("AsyncLoading component not found on instantiated prefab.", this);
            Destroy(loaderObj);
            return;
        }

        loader.OnLoadCompleted += () => activeLoaders.Remove(targetScene);
        loader.SetTargetScene(targetScene);
        activeLoaders.Add(targetScene, loader);
        loader.StartLoading();
    }

    private void OnValidate()
    {
        if (asyncLoadingPrefab == null)
        {
            Debug.LogWarning("AsyncLoading prefab is not assigned.", this);
        }
    }
}