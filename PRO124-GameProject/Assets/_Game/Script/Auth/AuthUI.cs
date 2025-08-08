using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Collections;

public readonly struct ColorTypes
{
    public static readonly Color Red = new(1f, 0f, 0f);
    public static readonly Color Green = Color.green;
    public static readonly Color Yellow = new(1f, 1f, 0f);
}

public class AuthUI : MonoBehaviour
{
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

    private Coroutine statusCoroutine;

    private void Awake()
    {
        SetupUIEvents();
    }

    private void Start()
    {
        ShowLoginForm();
    }

    private void SetupUIEvents()
    {
        loginButton.onClick.AddListener(OnLoginClicked);
        loginAnonymousButton.onClick.AddListener(PlayNowClicked);
        registerButton.onClick.AddListener(OnRegisterClicked);
        switchToRegisterButton.onClick.AddListener(ShowRegisterForm);
        switchToLoginButton.onClick.AddListener(ShowLoginForm);
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

        FirebaseManager.Instance.RegisterAccount(email, password);
        ShowStatus("Registered successfully! Please login.", ColorTypes.Green);
        ShowLoginForm();
    }

    private void OnLoginClicked()
    {
        string email = loginEmailInput.text.Trim();
        string password = loginPasswordInput.text.Trim();

        if (!IsValidInput(email, password)) return;

        FirebaseManager.Instance.SignAccount(email, password);
        ShowStatus("Login successfully!", ColorTypes.Green);
        StartCoroutine(CheckLoginResult());
    }

    private IEnumerator CheckLoginResult()
    {
        yield return new WaitForSeconds(1.5f);

        var user = FirebaseManager.Instance.user;
        if (user != null)
        {
            StartCoroutine(ShowStatusThenLoadScene($"Logged in as: {user.Email}", ColorTypes.Green));
        }
        else
        {
            ShowStatus("Login failed or user is null.", ColorTypes.Red);
        }
    }

    private void PlayNowClicked()
    {
        FirebaseManager.Instance.SignAnonymousAccount();
        StartCoroutine(ShowStatusThenLoadScene($"Loggin with AnonymousA ccount", ColorTypes.Green));
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
        LoadingSceneManager.Instance.LoadScene(SceneIndexs.MAIN_MENU);
    }

    #endregion
}
