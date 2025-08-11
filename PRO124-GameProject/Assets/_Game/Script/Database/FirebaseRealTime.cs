using DesignPatterns.Singleton;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using Newtonsoft.Json;
using System;

public class FirebaseManager : Singleton<FirebaseManager>
{
    private FirebaseAuth auth;
    private DatabaseReference db;
    public FirebaseUser user;

    public PlayerDataModel playerData = new PlayerDataModel();
    public event Action OnPlayerDataUpdated;

    protected override void Awake() => base.Awake();

    private void Start() => InitializeFirebase();

    private void InitializeFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                db = FirebaseDatabase.DefaultInstance.RootReference;
                Debug.Log("Firebase initialized.");
            }
            else
            {
                Debug.LogError($"Firebase init failed: {task.Result}");
            }
        });
    }

    #region Authentication
    public void RegisterAccount(string email, string password, Action<bool, string> callback)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                callback(false, "Registration failed.");
                return;
            }

            user = task.Result.User;
            playerData.ResetDefaultData(user.UserId, email);
            WriteData();

            SyncAudioManager();
            callback(true, "Registered successfully!");
        });
    }

    public void SignAccount(string email, string password, Action<bool, string> callback)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                callback(false, "Login failed.");
                return;
            }

            user = task.Result.User;
            playerData.userId = user.UserId;
            playerData.email = email;

            ReadData();
            ListenToRealtimeData();

            callback(true, "Login successfully!");
        });
    }

    public void SignAnonymousAccount()
    {
        auth.SignInAnonymouslyAsync().ContinueWithOnMainThread(task =>
        {
            if (LogTaskErrors(task, nameof(SignAnonymousAccount))) return;

            user = task.Result.User;
            playerData.ResetDefaultData(user.UserId, user.Email ?? "");

            ReadData();
            ListenToRealtimeData();
        });
    }

    public void LogOut()
    {
        auth.SignOut();
        user = null;
        playerData = new PlayerDataModel();
        Debug.Log("User logged out.");
    }

    public void LogOutAnonymous()
    {
        auth.SignOut();
        user = null;
        playerData.ResetDefaultData("", "");
        Debug.Log("Anonymous user logged out.");
    }

    public void DeleteAccount()
    {
        if (user == null)
        {
            Debug.LogError("DeleteAccount failed: user is null!");
            return;
        }

        user.DeleteAsync().ContinueWithOnMainThread(task =>
        {
            if (LogTaskErrors(task, nameof(DeleteAccount))) return;

            LogOut();
            Debug.Log("User account deleted successfully.");
        });
    }

    public void DeleteAnonymousAccount()
    {
        if (user == null)
        {
            Debug.LogError("DeleteAnonymousAccount failed: user is null!");
            return;
        }

        user.DeleteAsync().ContinueWithOnMainThread(task =>
        {
            if (LogTaskErrors(task, nameof(DeleteAnonymousAccount))) return;

            LogOutAnonymous();
            Debug.Log("Anonymous account deleted successfully.");
        });
    }
    #endregion

    #region Database
    public void WriteData()
    {
        if (string.IsNullOrEmpty(playerData.userId))
        {
            Debug.LogError("WriteData failed: userId is empty!");
            return;
        }

        string json = JsonConvert.SerializeObject(playerData, Formatting.Indented);
        db.Child("users").Child(playerData.userId).SetRawJsonValueAsync(json);
    }

    public void ReadData()
    {
        if (!ValidateUserId(nameof(ReadData))) return;

        db.Child("users").Child(playerData.userId).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (LogTaskErrors(task, nameof(ReadData))) return;

            var snapshot = task.Result;
            if (snapshot.Exists)
            {
                string json = snapshot.GetRawJsonValue();
                playerData = JsonConvert.DeserializeObject<PlayerDataModel>(json) ?? new PlayerDataModel();
                playerData.EnsureDefaultItems();

                SyncAudioManager();
                UIManager.Instance?.OnPlayerDataChanged();
            }
            else
            {
                playerData.ResetDefaultData(user.UserId, playerData.email);
                WriteData();
                SyncAudioManager();
                UIManager.Instance?.OnPlayerDataChanged();
            }
        });
    }
    #endregion

    private void ListenToRealtimeData()
    {
        if (!ValidateUserId(nameof(ListenToRealtimeData))) return;

        db.Child("users").Child(playerData.userId).ValueChanged += (s, e) =>
        {
            if (e.Snapshot.Exists)
            {
                playerData = JsonConvert.DeserializeObject<PlayerDataModel>(e.Snapshot.GetRawJsonValue());
                OnPlayerDataUpdated?.Invoke();
                SyncAudioManager();
            }
        };
    }

    private void SyncAudioManager()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.playerData = playerData;
            AudioManager.Instance.ApplyAudioSettings();
        }
    }

    private bool LogTaskErrors(System.Threading.Tasks.Task task, string methodName)
    {
        if (task.IsFaulted)
        {
            Debug.LogError($"{methodName} error: {task.Exception}");
            return true;
        }
        return false;
    }

    private bool ValidateUserId(string methodName)
    {
        if (string.IsNullOrEmpty(playerData?.userId))
        {
            Debug.LogError($"{methodName}: No UserId available.");
            return false;
        }
        return true;
    }
}
