using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DesignPatterns.Singleton;

public class YesNoDialog : Singleton<YesNoDialog>
{
    [Header("UI References")]
    [SerializeField] private GameObject dialogPanel;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    private Action<bool> onResult;

    protected override void Awake()
    {
        base.Awake();
        // Đảm bảo panel tắt khi khởi chạy
        if (dialogPanel != null)
            dialogPanel.SetActive(false);
    }

    public void Show(string message, Action<bool> callback)
    {
        if (dialogPanel == null || messageText == null || yesButton == null || noButton == null)
        {
            Debug.LogError("[YesNoDialog] Không thể Show vì UI chưa được gán hoặc tìm thấy!");
            return;
        }

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
