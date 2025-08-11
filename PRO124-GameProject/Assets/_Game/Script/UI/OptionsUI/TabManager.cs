using UnityEngine;
using DesignPatterns.Singleton;
using UnityEngine.SceneManagement;

public class TabManager : Singleton<TabManager>
{
    [Header("Tab mở mặc định khi khởi chạy (tự tìm nếu để trống)")]
    [SerializeField] private GameObject defaultTab;

    protected override void Awake()
    {
        base.Awake();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        OpenDefaultTab();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        OpenDefaultTab();
    }

    /// <summary>
    /// Đóng tất cả tab (tag "TabUI") và chỉ mở tab được chỉ định
    /// </summary>
    public void ShowOnly(GameObject targetTab)
    {
        GameObject[] allTabs = GameObject.FindGameObjectsWithTag("TabUI");

        foreach (var tab in allTabs)
        {
            if (tab != null)
                tab.SetActive(false);
        }

        if (targetTab != null)
        {
            targetTab.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Target tab is null.");
        }
    }

    /// <summary>
    /// Mở tab mặc định khi vào scene
    /// </summary>
    private void OpenDefaultTab()
    {
        // Nếu defaultTab chưa gán → tự tìm tab đầu tiên
        if (defaultTab == null)
        {
            GameObject[] allTabs = GameObject.FindGameObjectsWithTag("TabUI");

            if (allTabs.Length > 0)
            {
                defaultTab = allTabs[0]; // Lấy tab đầu tiên tìm được
            }
        }

        // Mở tab mặc định nếu có
        if (defaultTab != null)
        {
            ShowOnly(defaultTab);
        }
        else
        {
            Debug.LogWarning("Không tìm thấy tab mặc định để mở.");
        }
    }
}
