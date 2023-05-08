using UnityEngine;
using UnityEngine.Localization.Components;

public class UIDialogueChoiceFiller : MonoBehaviour
{
    [SerializeField] private LocalizeStringEvent choiceText = default;
    [SerializeField] private MultiInputButton actionButton = default;

    [Header("Broadcasting on")] [SerializeField]
    private DialogueChoiceChannelSO onChoiceMade = default;

    private Choice currentChoice;

    public void FillChoice(Choice choiceToFill, bool isSelected)
    {
        currentChoice = choiceToFill;
        choiceText.StringReference = choiceToFill.Response;
        actionButton.interactable = true;

        if (isSelected)
        {
            actionButton.UpdateSelected();
        }
    }

    public void ButtonClicked()
    {
        onChoiceMade.RaiseEvent(currentChoice);
    }
}