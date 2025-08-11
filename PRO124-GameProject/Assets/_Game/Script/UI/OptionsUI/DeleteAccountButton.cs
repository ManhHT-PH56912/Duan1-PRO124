using System.Collections;
using UnityEngine;

public class DeleteAccountButton : BaseButtom
{
    protected override void OnButtonClick()
    {
        Debug.Log("[UI] Delete Account Button Clicked!");
        YesNoDialog.Instance.Show(
        "Are you sure you want to Delete account?",
        confirmed =>
        {
            if (confirmed)
            {
                Debug.Log("[UI] User confirmed log out.");
                StartCoroutine(HandleLogOut());
            }
            else
            {
                Debug.Log("[UI] User canceled log out.");
            }
        });
    }

    private IEnumerator HandleLogOut()
    {
        // Optional: add a loading indicator here

        if (FirebaseManager.Instance != null && FirebaseManager.Instance.user != null)
        {
            FirebaseManager.Instance.DeleteAccount();
        }
        else if (FirebaseManager.Instance != null && FirebaseManager.Instance.user != null && FirebaseManager.Instance.user.IsAnonymous)
        {
            FirebaseManager.Instance.DeleteAnonymousAccount();
        }

        yield return new WaitForSeconds(2f); // Delay for feedback or animation

        if (LoadingSceneManager.Instance != null)
        {
            LoadingSceneManager.Instance.LoadScene(SceneIndexs.AUTH);
        }
    }
}
