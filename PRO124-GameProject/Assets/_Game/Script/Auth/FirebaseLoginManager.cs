using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine.UI;
using System;
using System.Text.RegularExpressions;
using System.Collections;

public readonly struct ColorTypes
{
    public static readonly Color Red = new(1f, 0f, 0f);
    public static readonly Color Green = new(0.33f, 0.5f, 0.18f);
    public static readonly Color Yellow = new(1f, 1f, 0f);
}


public class FirebaseLoginManager : MonoBehaviour
{
    private FirebaseAuth auth;
    private FirebaseUser currentUser;

    [Header("UI Panels")]
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private GameObject registerPanel;

    [Header("Login Inputs")]
    [SerializeField] private InputField loginEmailInput;
    [SerializeField] private InputField loginPasswordInput;

    [Header("Register Inputs")]
    [SerializeField] private InputField registerEmailInput;
    [SerializeField] private InputField registerPasswordInput;
    [SerializeField] private InputField confirmPasswordInput;

    [Header("UI Elements")]
    [SerializeField] private Text statusText;

    [Header("Buttons")]
    [SerializeField] private Button loginButton;
    [SerializeField] private Button registerButton;
    [SerializeField] private Button switchToRegisterButton;
    [SerializeField] private Button switchToLoginButton;
    [SerializeField] private Button loginAnonymousButton;

    private LoadingSceneManager loadingSceneManager;
    private Coroutine statusCoroutine;

    private void Start()
    {
        loadingSceneManager = FindObjectOfType<LoadingSceneManager>();
        InitializeFirebase();
        SetupUIEvents();
        ShowLoginForm();
    }

    private void InitializeFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                Debug.Log("Firebase initialized.");
            }
            else
            {
                LogError("Firebase init error", task.Exception);
            }
        });
    }

    private void SetupUIEvents()
    {
        loginButton.onClick.AddListener(OnLoginClicked);
        registerButton.onClick.AddListener(OnRegisterClicked);
        switchToRegisterButton.onClick.AddListener(ShowRegisterForm);
        switchToLoginButton.onClick.AddListener(ShowLoginForm);
        loginAnonymousButton.onClick.AddListener(OnAnonymousLoginClicked);
    }

    #region UI Logic

    private void ShowLoginForm()
    {
        loginPanel.SetActive(true);
        registerPanel.SetActive(false);
        ClearInputs();
    }

    private void ShowRegisterForm()
    {
        loginPanel.SetActive(false);
        registerPanel.SetActive(true);
        ClearInputs();
    }

    private void ClearInputs()
    {
        loginEmailInput.text = "";
        loginPasswordInput.text = "";
        registerEmailInput.text = "";
        registerPasswordInput.text = "";
        confirmPasswordInput.text = "";
        statusText.text = "";
    }

    #endregion

    #region Auth Logic

    private void OnRegisterClicked()
    {
        string email = registerEmailInput.text.Trim();
        string password = registerPasswordInput.text.Trim();
        string confirmPassword = confirmPasswordInput.text.Trim();

        if (!IsValidInput(email, password)) return;

        if (password != confirmPassword)
        {
            ShowStatus("Passwords do not match!", ColorTypes.Red);
            return;
        }

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                LogError("Registration failed", task.Exception);
                return;
            }

            currentUser = task.Result.User;
            ShowStatus("Registered successfully! Please login.", ColorTypes.Green);
            ShowLoginForm();
        });
    }

    private void OnLoginClicked()
    {
        string email = loginEmailInput.text.Trim();
        string password = loginPasswordInput.text.Trim();

        if (!IsValidInput(email, password)) return;

        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                LogError("Login failed", task.Exception);
                return;
            }

            currentUser = task.Result.User;
            StartCoroutine(ShowStatusThenLoadScene($"Logged in as: {currentUser.Email}", ColorTypes.Green));
        });
    }

    private void OnAnonymousLoginClicked()
    {
        auth.SignInAnonymouslyAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                LogError("Anonymous login failed", task.Exception);
                return;
            }

            currentUser = task.Result.User;
            StartCoroutine(ShowStatusThenLoadScene("Anonymous login successful", ColorTypes.Green));
        });
    }

    public void Logout()
    {
        if (auth != null)
        {
            auth.SignOut();
            currentUser = null;
        }
    }

    #endregion

    #region Helper Methods

    private bool IsValidInput(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            ShowStatus("Email and password are required.", ColorTypes.Red);
            return false;
        }

        var emailPattern = @"^[a-zA-Z0-9._%+-]+@gmail\.com$";
        if (!Regex.IsMatch(email, emailPattern))
        {
            ShowStatus("Invalid Gmail address format.", ColorTypes.Red);
            return false;
        }

        return true;
    }

    private void ShowStatus(string message, Color color)
    {
        if (statusCoroutine != null)
            StopCoroutine(statusCoroutine);

        statusCoroutine = StartCoroutine(ShowStatusRoutine(message, color));
    }

    private IEnumerator ShowStatusRoutine(string message, Color color)
    {
        statusText.text = message;
        statusText.color = color;
        yield return new WaitForSeconds(2f);
        statusText.text = "";
    }

    private IEnumerator ShowStatusThenLoadScene(string message, Color color)
    {
        statusText.text = message;
        statusText.color = color;
        yield return new WaitForSeconds(3f);
        loadingSceneManager.LoadScene(SceneIndexs.MAIN_MENU);
    }

    private void LogError(string prefix, Exception ex)
    {
        string message = ex?.InnerException?.Message ?? ex?.Message ?? "Unknown error";
        Debug.LogError($"{prefix}: {message}");
        ShowStatus($"{prefix}: {message}", Color.red);
    }

    #endregion
}
