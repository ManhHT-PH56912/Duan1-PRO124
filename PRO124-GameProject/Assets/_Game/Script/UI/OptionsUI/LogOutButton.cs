using UnityEngine;
using System.Collections;

public class LogOutButton : BaseButtom
{
    protected override void OnButtonClick()
    {
        Debug.Log("[UI] Log Out Button Clicked!");

        YesNoDialog.Instance.Show(
            "Are you sure you want to log out?",
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
        if (FirebaseManager.Instance != null)
        {
            FirebaseManager.Instance.SignOutAccount();
        }

        yield return new WaitForSeconds(2f);

        if (LoadingSceneManager.Instance != null)
        {
            LoadingSceneManager.Instance.LoadScene(SceneIndexs.AUTH);
        }
    }
}
