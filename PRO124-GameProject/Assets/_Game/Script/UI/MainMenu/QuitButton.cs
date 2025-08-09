using UnityEngine;

public class QuitButton : BaseButtom
{
    protected override void OnButtonClick()
    {
        Debug.Log("Quit Button Clicked!");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Quit in Editor
#endif
        // Quit the application
        Application.Quit();
    }
}
