
// Concrete class for Quit button
using UnityEngine;

public class QuitButton : BaseButtom
{
    protected override void OnButtonClick()
    {
        Debug.Log("Quit Button Clicked!");
        // Quit the application
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Quit in Editor
#endif
    }
}
