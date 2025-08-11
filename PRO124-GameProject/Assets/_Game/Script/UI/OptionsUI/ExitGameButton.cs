using UnityEngine;

public class ExitGameButton : BaseButtom
{
    protected override void OnButtonClick()
    {
        Debug.Log("[UI] Exit Game Button Clicked!");

        // Hiện Yes/No dialog trước khi thoát
        if (YesNoDialog.Instance != null)
        {
            YesNoDialog.Instance.Show(
                "Do you want to exit to Main Menu?",
                confirmed =>
                {
                    if (confirmed)
                    {
                        Debug.Log("[UI] User confirmed exit to main menu");

                        // TODO: Save game ở đây nếu cần
                        // SaveGameManager.Instance.Save();

                        // game repause 
                        GameManager.Instance.TogglePause();

                        if (LoadingSceneManager.Instance != null)
                        {
                            LoadingSceneManager.Instance.LoadScene(SceneIndexs.MAIN_MENU);
                        }
                        else
                        {
                            Debug.LogWarning("[UI] LoadingSceneManager not found!");
                        }
                    }
                    else
                    {
                        Debug.Log("[UI] User canceled exit");
                    }
                });
        }
        else
        {
            Debug.LogError("[UI] YesNoDialog instance not found!");
        }
    }
}
