using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using DesignPatterns.Singleton;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.Networking;

public class FirebaseManager : Singleton<FirebaseManager>
{
    FirebaseApp app;
    FirebaseAuth auth;
    DatabaseReference db;
    public FirebaseUser user;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        InitializeFirebase();
    }


    private void InitializeFirebase()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                app = FirebaseApp.DefaultInstance;
                auth = FirebaseAuth.DefaultInstance;
                db = FirebaseDatabase.DefaultInstance.RootReference;
                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

    public void RegisterAccount(string email, string password)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
            }

            // Firebase user has been created.
            AuthResult result = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);
        });
    }


    public void SignAccount(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
            }

            user = auth.CurrentUser;

            AuthResult result = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);
        });
    }

    public void SignAnonymousAccount()
    {
        auth.SignInAnonymouslyAsync().ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInAnonymouslyAsync was canceled.");
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
            }

            user = auth.CurrentUser;


            AuthResult result = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);
        });
    }


    public void SignOutAccount()
    {
        FirebaseAuth.DefaultInstance.SignOut();
        user = auth.CurrentUser;
        Debug.Log("SignOutAccount OK!");
    }

    public void CkeckUser()
    {
        if (user == null)
        {
            Debug.Log("Không có tài khoản nòa đăng nhập!");
        }
        else
        {
            Debug.Log($"Bạn đang đăng nhập: {user.Email}");
        }
    }

    public void DeleteAccount()
    {
        Firebase.Auth.FirebaseUser user = auth.CurrentUser;
        user?.DeleteAsync().ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("DeleteAsync was canceled.");
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("DeleteAsync encountered an error: " + task.Exception);
                }

                Debug.Log("User deleted successfully.");
            });
    }

    public void DeleteAnonymousAccount()
    {
        FirebaseUser currentUser = auth.CurrentUser;
        if (currentUser != null && currentUser.IsAnonymous)
        {
            currentUser.DeleteAsync().ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("DeleteAsync was canceled.");
                }
                else if (task.IsFaulted)
                {
                    Debug.LogError("DeleteAsync encountered an error: " + task.Exception);
                }
                else
                {
                    Debug.Log("Anonymous user deleted successfully.");
                }
            });
        }
        else
        {
            Debug.Log("No anonymous user to delete or user is not signed in.");
        }
    }
    public DatabaseReference GetDatabaseRef()
    {
        return db;
    }


    public string GetUserId()
    {
        return user?.UserId;
    }

    public void WriteData(FirebaseUser user, string userid)
    {

    }

    public void ReadData()
    {

    }

    public void UpdateData(string key, object value)
    {
        db.Child("users").Child(GetUserId()).Child(key).SetValueAsync(value).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("User data updated successfully.");
            }
            else
            {
                Debug.LogError("Failed to update user data: " + task.Exception);
            }
        });
    }

    public void DeleteData(string key)
    {
        db.Child("users").Child(GetUserId()).Child(key).RemoveValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("User data deleted successfully.");
            }
            else
            {
                Debug.LogError("Failed to delete user data: " + task.Exception);
            }
        });
    }
}
