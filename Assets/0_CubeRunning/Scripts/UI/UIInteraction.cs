using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInteraction : MonoBehaviour
{
    [SerializeField] private List<InteractionSO> listInteractions = default;
    [SerializeField] private Image interactionIcon = default;

    public void FillInteractionPanel(InteractionType type)
    {
        if (listInteractions != null && listInteractions.Exists(i => i.InteractionType == type))
        {
            Sprite icon = (listInteractions.Find(i => i.InteractionType == type)).InteractionIcon;
            interactionIcon.sprite = icon;
        }
    }
}