using UnityEngine;
using System.Collections.Generic;
using DesignPatterns.Singleton;
using UnityEngine.UIElements;

public class TabManager : Singleton<TabManager>
{
    [Header("Tất cả các tab")]
    [SerializeField] private List<GameObject> allTabs;

    public void ShowOnly(GameObject targetTab)
    {
        foreach (var tab in allTabs)
        {
            if (tab != null)
            {
                tab.SetActive(false);
            }
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
}
