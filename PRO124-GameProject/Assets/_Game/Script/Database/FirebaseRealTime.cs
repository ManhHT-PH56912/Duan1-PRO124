using System;
using System.Collections;
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
                app = Firebase.FirebaseApp.DefaultInstance;
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
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
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
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
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
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                return;
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
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("DeleteAsync encountered an error: " + task.Exception);
                    return;
                }

                Debug.Log("User deleted successfully.");
            });
    }

    public void DeleteAnonymousAccount()
    {
        if (user != null && user.IsAnonymous)
        {
            user.DeleteAsync().ContinueWith(task =>
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

    public string GetUserId()
    {
        return user.UserId;
    }


    // CREATE hoặc UPDATE toàn bộ UserData
    public void CreateOrUpdateUserData(UserData data)
    {
        if (user == null)
        {
            Debug.LogError("Chưa đăng nhập!");
            return;
        }

        string json = JsonUtility.ToJson(data);
        db.Child("users").Child(user.UserId).SetRawJsonValueAsync(json).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
                Debug.LogError("Lưu dữ liệu thất bại: " + task.Exception);
            else
                Debug.Log("Lưu dữ liệu thành công!");
        });
    }

    // READ dữ liệu UserData
    public void ReadUserData(Action<UserData> callback)
    {
        if (user == null)
        {
            Debug.LogError("Chưa đăng nhập!");
            return;
        }

        db.Child("users").Child(user.UserId).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Đọc dữ liệu thất bại: " + task.Exception);
                callback?.Invoke(null);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    string json = snapshot.GetRawJsonValue();
                    UserData data = JsonUtility.FromJson<UserData>(json);
                    callback?.Invoke(data);
                }
                else
                {
                    Debug.LogWarning("Không tìm thấy dữ liệu người chơi!");
                    callback?.Invoke(null);
                }
            }
        });
    }

    // UPDATE 1 field cụ thể (không overwrite toàn bộ)
    public void UpdateUserField(string field, object value)
    {
        if (user == null)
        {
            Debug.LogError("Chưa đăng nhập!");
            return;
        }

        db.Child("users").Child(user.UserId).Child(field).SetValueAsync(value).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
                Debug.LogError("Cập nhật thất bại: " + task.Exception);
            else
                Debug.Log($"Field '{field}' đã được cập nhật!");
        });
    }

    // DELETE dữ liệu người chơi
    public void DeleteUserData()
    {
        if (user == null)
        {
            Debug.LogError("Chưa đăng nhập!");
            return;
        }

        db.Child("users").Child(user.UserId).RemoveValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
                Debug.LogError("Xóa dữ liệu thất bại: " + task.Exception);
            else
                Debug.Log("Dữ liệu người chơi đã bị xóa!");
        });
    }

}
