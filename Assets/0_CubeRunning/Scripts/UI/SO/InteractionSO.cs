using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "InteractionSO", menuName = "UI/Interaction", order = 0)]
public class InteractionSO : ScriptableObject
{
    [SerializeField] private LocalizedString interactionName = default;
    [SerializeField] private Sprite interactionIcon = default;
    [SerializeField] private InteractionType interactionType = default;

    public Sprite InteractionIcon => interactionIcon;
    public LocalizedString InteractionName => interactionName;
    public InteractionType InteractionType => interactionType;
}