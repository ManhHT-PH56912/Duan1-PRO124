using UnityEngine;

public class OptionsButton : BaseButtom
{
    [SerializeField] private GameObject optionsUI;

    protected override void OnButtonClick()
    {
        Debug.Log("Options Button Clicked!");

        if (optionsUI != null)
        {
            bool isActive = optionsUI.activeSelf;
            optionsUI.SetActive(!isActive); // Toggle ON/OFF
        }
        else
        {
            Debug.LogWarning("Options UI GameObject is not assigned.");
        }
    }
}
