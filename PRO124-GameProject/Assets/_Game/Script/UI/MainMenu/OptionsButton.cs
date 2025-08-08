using UnityEngine;

public class OptionsButton : BaseButtom
{
    [SerializeField] private GameObject optionsUI;

    protected override void OnButtonClick()
    {
        Debug.Log("Options Button Clicked!");
        if (optionsUI != null)
        {
            optionsUI.SetActive(true); // Toggle Options UI
        }
        else
        {
            Debug.LogWarning("Options UI GameObject is not assigned.");
        }
    }
}
