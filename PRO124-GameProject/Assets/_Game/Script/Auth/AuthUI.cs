using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Collections;

public readonly struct ColorTypes
{
    public static readonly Color Red = Color.red;
    public static readonly Color Green = Color.green;
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

    private void Awake()
    {
        loginButton.onClick.AddListener(OnLoginClicked);
        loginAnonymousButton.onClick.AddListener(PlayNowClicked);
        registerButton.onClick.AddListener(OnRegisterClicked);
        switchToRegisterButton.onClick.AddListener(ShowRegisterForm);
        switchToLoginButton.onClick.AddListener(ShowLoginForm);
    }

    private void Start() => ShowLoginForm();

    #region UI
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

    #region Auth
    private void OnRegisterClicked()
    {
        string email = registerEmailInput.text.Trim();
        string password = registerPasswordInput.text.Trim();
        string confirmPassword = confirmPasswordInput.text.Trim();

        if (!IsValidInput(email, password)) return;
        if (password != confirmPassword)
        {
            SetStatus("Passwords do not match!", ColorTypes.Red);
            return;
        }

        FirebaseManager.Instance.RegisterAccount(email, password, (success, message) =>
        {
            SetStatus(message, success ? ColorTypes.Green : ColorTypes.Red);
            if (success) ShowLoginForm();
        });
    }

    private void OnLoginClicked()
    {
        string email = loginEmailInput.text.Trim();
        string password = loginPasswordInput.text.Trim();

        if (!IsValidInput(email, password)) return;

        FirebaseManager.Instance.SignAccount(email, password, (success, message) =>
        {
            SetStatus(message, success ? ColorTypes.Green : ColorTypes.Red);
            if (success) StartCoroutine(LoadMainMenuAfterDelay(1.5f));
        });
    }

    private void PlayNowClicked()
    {
        FirebaseManager.Instance.SignAnonymousAccount();
        SetStatus("Logged in as Guest", ColorTypes.Green);
        StartCoroutine(LoadMainMenuAfterDelay(1f));
    }
    #endregion

    #region Helper
    private bool IsValidInput(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            SetStatus("Email and password are required.", ColorTypes.Red);
            return false;
        }

        if (!Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@gmail\.com$"))
        {
            SetStatus("Invalid Gmail address format.", ColorTypes.Red);
            return false;
        }
        return true;
    }

    private void SetStatus(string message, Color color)
    {
        statusText.text = message;
        statusText.color = color;
        StartCoroutine(ClearStatusAfterDelay(3f));

    }
    private IEnumerator ClearStatusAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        statusText.text = "";
    }

    private IEnumerator LoadMainMenuAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        LoadingSceneManager.Instance.LoadScene(SceneIndexs.MAIN_MENU);
    }
    #endregion
}
