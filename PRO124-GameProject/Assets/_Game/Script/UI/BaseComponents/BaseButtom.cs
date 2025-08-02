using UnityEngine;
using UnityEngine.UI;

// Abstract base class for button functionality
public abstract class BaseButtom : MonoBehaviour
{
    protected Button button;

    // Initialize the button component
    protected virtual void Awake()
    {
        button = GetComponent<Button>();
        if (button == null)
        {
            Debug.LogError($"{gameObject.name} requires a Button component.");
            return;
        }
        button.onClick.AddListener(OnButtonClick);
    }

    // Abstract method for button click behavior
    protected abstract void OnButtonClick();

    // Clean up listeners to prevent memory leaks
    protected virtual void OnDestroy()
    {
        if (button != null)
        {
            button.onClick.RemoveListener(OnButtonClick);
        }
    }
}