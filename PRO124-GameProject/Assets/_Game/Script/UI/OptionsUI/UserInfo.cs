using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI uId;
    [SerializeField] private Button copyUIdButton;
    [SerializeField] private TextMeshProUGUI copyStatusText;

    private string currentUserId;

    private void Start()
    {
        gameObject.SetActive(false);
        currentUserId = FirebaseManager.Instance.GetUserId();
        uId.text = currentUserId;
        copyStatusText.text = ""; // Hide status initially

        copyUIdButton.onClick.AddListener(HandleCopyToClipboard);
    }

    private void HandleCopyToClipboard()
    {
        GUIUtility.systemCopyBuffer = currentUserId;
        Debug.Log($"Copied User ID to clipboard: {currentUserId}");

        // Show "Copy successful" text
        copyStatusText.text = "Copied!";

        // Optional: Hide after 2 seconds
        Invoke(nameof(HideCopyStatus), 2f);
    }

    private void HideCopyStatus()
    {
        copyStatusText.text = "";
    }
}
