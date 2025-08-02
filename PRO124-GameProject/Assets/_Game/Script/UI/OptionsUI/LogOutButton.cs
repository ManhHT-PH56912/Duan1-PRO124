using System;
using Firebase.Auth;
using UnityEngine;

public class LogOutButton : BaseButtom
{
    private FirebaseAuth auth;
    private FirebaseUser currentUser;
    private LoadingSceneManager loadingSceneManager;

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        currentUser = auth.CurrentUser;
        if (loadingSceneManager == null)
        {
            loadingSceneManager = FindObjectOfType<LoadingSceneManager>();
        }
    }

    protected override void OnButtonClick()
    {
        Debug.Log("Log Out Button Clicked!");
        HandleLogOut();
    }

    public void HandleLogOut()
    {
        // // nếu là tài khoản ân danh thì xóa tài khoản
        // if (currentUser.IsAnonymous)
        // {
        //     DeleteAnonymousAccount();
        //     currentUser = null;
        //     loadingSceneManager.LoadScene(SceneIndexs.AUTH);
        //     Debug.Log("Anonymous user detected. Deleting account.");
        // }
        // else
        // {
        auth.SignOut();
        currentUser = null;
        loadingSceneManager.LoadScene(SceneIndexs.AUTH);
        // Debug.Log("User logged out.");
        // }
    }

    private void DeleteAnonymousAccount()
    {
        auth.CurrentUser.DeleteAsync().ContinueWith(task =>
        {
            if (task.IsCompleted && !task.IsFaulted)
            {
                Debug.Log("Anonymous account deleted successfully.");
            }
            else
            {
                Debug.LogError("Failed to delete anonymous account: " + task.Exception);
            }
        });
    }
}
