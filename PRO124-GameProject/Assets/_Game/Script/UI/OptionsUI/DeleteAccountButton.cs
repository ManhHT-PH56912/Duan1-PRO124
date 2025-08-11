using System.Collections;
using UnityEngine;

public class DeleteAccountButton : BaseButtom
{
    protected override void OnButtonClick()
    {
        Debug.Log("[UI] Delete Account Button Clicked!");

        if (YesNoDialog.Instance != null)
        {
            YesNoDialog.Instance.Show(
                "Are you sure you want to delete your account?",
                confirmed =>
                {
                    if (confirmed)
                    {
                        Debug.Log("[UI] User confirmed account deletion");
                        StartCoroutine(HandleDeleteAccount());
                    }
                    else
                    {
                        Debug.Log("[UI] User canceled account deletion");
                    }
                });
        }
        else
        {
            Debug.LogError("[UI] YesNoDialog instance not found!");
        }
    }

    private IEnumerator HandleDeleteAccount()
    {
        var firebase = FirebaseManager.Instance;

        if (firebase != null && firebase.user != null)
        {
            if (firebase.user.IsAnonymous)
                firebase.DeleteAnonymousAccount();
            else
                firebase.DeleteAccount();
        }
        else
        {
            Debug.LogWarning("[UI] FirebaseManager instance or user is null. Cannot delete account.");
        }

        yield return new WaitForSeconds(2f);

        if (LoadingSceneManager.Instance != null)
        {
            LoadingSceneManager.Instance.LoadScene(SceneIndexs.AUTH);
        }
        else
        {
            Debug.LogWarning("[UI] LoadingSceneManager instance is null. Cannot load auth scene.");
        }
    }
}
