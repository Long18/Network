using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonPrompt : MonoBehaviour
{
    [SerializeField] private Image interactionKeyBackground = default;
    [SerializeField] private TextMeshProUGUI interactionKeyText = default;
    [SerializeField] private Sprite controllerSprite = default;
    [SerializeField] private Sprite keyboardSprite = default;
    [SerializeField] private string interactionKeyboardCode = default;
    [SerializeField] private string interactionJoystickKeyCode = default;
    
    public void SetButtonPrompt(bool isKeyboard)
    {
        if (!isKeyboard)
        {
            interactionKeyBackground.sprite = controllerSprite;
            interactionKeyText.text = interactionJoystickKeyCode;
        }
        else
        {
            interactionKeyBackground.sprite = keyboardSprite;
            interactionKeyText.text = interactionKeyboardCode;
        }
    }
}