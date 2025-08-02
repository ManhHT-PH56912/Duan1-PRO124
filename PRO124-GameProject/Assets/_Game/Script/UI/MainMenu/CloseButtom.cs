using UnityEngine;

public class CloseButton : BaseButtom
{
    [SerializeField] private GameObject optionsUI;

    protected override void OnButtonClick()
    {
        Debug.Log("Close Button Clicked!");

        if (optionsUI != null)
        {
            optionsUI.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Options UI GameObject is not assigned.");
        }
    }
}
