using System.Collections.Generic;
using UnityEngine;

public class UIDialogueChoicesManager : MonoBehaviour
{
    [SerializeField] private UIDialogueChoiceFiller[] choiceButtons;

    public void FillChoices(List<Choice> choices)
    {
        if (choices == null) return;

        int maxCount = Mathf.Max(choices.Count, choiceButtons.Length);

        for (int i = 0; i < maxCount; i++)
        {
            if (i < choiceButtons.Length)
            {
                if (i < choices.Count)
                {
                    choiceButtons[i].gameObject.SetActive(true);
                    choiceButtons[i].FillChoice(choices[i], i == 0);
                }
                else
                {
                    choiceButtons[i].gameObject.SetActive(false);
                }
            }
            else
            {
                Debug.LogError("There are more choices than buttons");
            }
        }
    }
}