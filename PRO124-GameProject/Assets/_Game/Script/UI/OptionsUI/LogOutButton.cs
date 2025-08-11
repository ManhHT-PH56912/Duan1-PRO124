using UnityEngine;
using System.Collections;

public class LogOutButton : BaseButtom
{
    protected override void OnButtonClick()
    {
        Debug.Log("[UI] Log Out Button Clicked");

        // Gọi popup Yes/No
        if (YesNoDialog.Instance != null)
        {
            YesNoDialog.Instance.Show(
                "Are you sure you want to log out?",
                confirmed =>
                {
                    if (confirmed)
                    {
                        Debug.Log("[UI] User confirmed log out");
                        StartCoroutine(HandleLogOut());
                    }
                    else
                    {
                        Debug.Log("[UI] User canceled log out");
                    }
                });
        }
        else
        {
            Debug.LogError("[UI] Không tìm thấy YesNoDialog trong Scene!");
        }
    }

    private IEnumerator HandleLogOut()
    {
        var firebase = FirebaseManager.Instance;

        if (firebase != null)
        {
            if (firebase.user != null && firebase.user.IsAnonymous)
                firebase.LogOutAnonymous();
            else
                firebase.LogOut();
        }
        else
        {
            Debug.LogWarning("[UI] FirebaseManager instance is null. Cannot log out.");
        }

        yield return new WaitForSeconds(2f);

        var sceneManager = LoadingSceneManager.Instance;
        if (sceneManager != null)
            sceneManager.LoadScene(SceneIndexs.AUTH);
        else
            Debug.LogWarning("[UI] LoadingSceneManager instance is null. Cannot load auth scene.");
    }
}
