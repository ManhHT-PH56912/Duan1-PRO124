using UnityEngine;

public class TabButton : BaseButtom
{
    [Header("Tab tương ứng")]
    [SerializeField] private GameObject tabToShow;
    TabManager TabManager;

    void Start()
    {
        TabManager = TabManager.Instance;
    }

    protected override void OnButtonClick()
    {
        if (TabManager != null)
        {
            TabManager.ShowOnly(tabToShow);
        }
        else
        {
            Debug.LogError("TabManager instance not found!");
        }
    }
}
