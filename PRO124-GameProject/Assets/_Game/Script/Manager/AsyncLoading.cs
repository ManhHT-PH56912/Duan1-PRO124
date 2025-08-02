using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class AsyncLoading : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Slider loadingSlider;
    [SerializeField] private TMP_Text progressText;
    [SerializeField] private GameObject fillObject;

    [Header("Loading Settings")]
    [SerializeField] private float minLoadTime = 2f;
    [SerializeField] private float maxLoadTime = 5f;
    [SerializeField] private float smoothSpeed = 3f;

    public event Action OnLoadStarted;
    public event Action OnLoadCompleted;

    private SceneIndexs targetScene;
    private AsyncOperation asyncOperation;
    private float currentProgress;
    private float targetProgress;
    private bool isReadyToActivate;
    private bool isLoading;

    private void Awake()
    {
        // Ensure the GameObject persists between scenes
        DontDestroyOnLoad(gameObject);
    }

    #region Public Methods
    public void SetTargetScene(SceneIndexs scene)
    {
        if (isLoading)
        {
            Debug.LogWarning($"Cannot change target scene to {scene} while loading is in progress.", this);
            return;
        }
        targetScene = scene;
    }

    public void StartLoading()
    {
        if (isLoading)
        {
            Debug.LogWarning("Loading already in progress.", this);
            return;
        }

        if (loadingSlider == null || progressText == null)
        {
            Debug.LogError("Required UI components are missing.", this);
            return;
        }

        isLoading = true;
        InitializeUI();
        StartCoroutine(LoadSceneRoutine());
        OnLoadStarted?.Invoke();
    }
    #endregion

    #region Core Loading Logic
    private IEnumerator LoadSceneRoutine()
    {
        float loadStartTime = Time.time;
        asyncOperation = SceneManager.LoadSceneAsync((int)targetScene);

        if (asyncOperation == null)
        {
            Debug.LogError($"Failed to start loading scene {targetScene}.", this);
            isLoading = false;
            yield break;
        }

        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            float elapsedTime = Time.time - loadStartTime;
            float operationProgress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            float timeBasedProgress = Mathf.Clamp01(elapsedTime / maxLoadTime);
            targetProgress = Mathf.Min(operationProgress, timeBasedProgress) * 100f;

            if (!isReadyToActivate && elapsedTime >= minLoadTime && asyncOperation.progress >= 0.9f)
            {
                isReadyToActivate = true;
                targetProgress = 100f;
            }

            yield return null;
        }

        // Cleanup after loading is complete
        isLoading = false;
        OnLoadCompleted?.Invoke();
        Destroy(gameObject);
    }

    private void Update()
    {
        if (!isLoading) return;

        currentProgress = Mathf.Lerp(currentProgress, targetProgress, smoothSpeed * Time.deltaTime);
        UpdateUI(currentProgress);

        if (isReadyToActivate && (currentProgress >= 99.9f || Input.anyKeyDown))
        {
            asyncOperation.allowSceneActivation = true;
        }
    }
    #endregion

    #region UI Management
    private void InitializeUI()
    {
        currentProgress = 0f;
        targetProgress = 0f;

        if (loadingSlider != null)
        {
            loadingSlider.minValue = 0f;
            loadingSlider.maxValue = 100f;
            loadingSlider.value = 0f;
        }

        if (fillObject != null)
        {
            fillObject.SetActive(false);
        }

        if (progressText != null)
        {
            progressText.text = "0%";
        }
    }

    private void UpdateUI(float progress)
    {
        progress = Mathf.Clamp(progress, 0f, 100f);

        if (loadingSlider != null)
        {
            loadingSlider.value = progress;
            if (fillObject != null)
            {
                fillObject.SetActive(progress > 0f);
            }
        }

        if (progressText != null)
        {
            progressText.text = $"{Mathf.RoundToInt(progress)}%";
        }
    }
    #endregion

    #region Validation
    private void OnValidate()
    {
        if (loadingSlider == null)
        {
            Debug.LogWarning("Loading Slider reference is missing.", this);
        }

        if (progressText == null)
        {
            Debug.LogWarning("Progress Text reference is missing.", this);
        }

        if (minLoadTime < 0f)
        {
            minLoadTime = 0f;
            Debug.LogWarning("Min Load Time cannot be negative.", this);
        }

        if (maxLoadTime < minLoadTime)
        {
            maxLoadTime = minLoadTime;
            Debug.LogWarning("Max Load Time cannot be less than Min Load Time.", this);
        }
    }
    #endregion
}