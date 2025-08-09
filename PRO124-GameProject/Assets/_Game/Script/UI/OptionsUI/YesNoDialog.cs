using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DesignPatterns.Singleton;

public class YesNoDialog : Singleton<YesNoDialog>
{
    [Header("UI References")]
    public GameObject dialogPanel;
    public TextMeshProUGUI messageText;
    public Button yesButton;
    public Button noButton;

    private Action<bool> onResult;

    protected override void Awake()
    {
        base.Awake();
        dialogPanel.SetActive(false);
    }

    /// <summary>
    /// Hiển thị hộp thoại Yes/No với message tùy chỉnh.
    /// </summary>
    /// <param name="message">Nội dung hiển thị</param>
    /// <param name="callback">Callback true nếu chọn Yes, false nếu chọn No</param>
    public void Show(string message, Action<bool> callback)
    {
        dialogPanel.SetActive(true);
        messageText.text = message;
        onResult = callback;

        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();

        yesButton.onClick.AddListener(() => HandleResult(true));
        noButton.onClick.AddListener(() => HandleResult(false));
    }

    private void HandleResult(bool result)
    {
        dialogPanel.SetActive(false);
        onResult?.Invoke(result);
    }
}
