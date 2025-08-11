using UnityEngine;

public class TabButton : BaseButtom
{
    [Header("Tab tương ứng")]
    [SerializeField] private GameObject tabToShow;

    protected override void OnButtonClick()
    {
        if (TabManager.Instance != null)
        {
            TabManager.Instance.ShowOnly(tabToShow);
            Debug.Log($"Tab actived");
        }
        else
        {
            Debug.LogError("TabManager instance not found!");
        }
    }
}
