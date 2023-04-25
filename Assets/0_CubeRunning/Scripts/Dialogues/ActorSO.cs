using UnityEngine;
using UnityEngine.Localization;

public enum ActorID
{
    N = 0, // None
    SK = 1, // Shop Keeper
}

[CreateAssetMenu(fileName = "New Actor", menuName = "Dialogues/Actor", order = 0)]
public class ActorSO : DescriptionBaseSO
{
    [SerializeField] private ActorID actorID = default;
    [SerializeField] private LocalizedString actorName = default;

    public ActorID ActorID => actorID;
    public LocalizedString ActorName => actorName;
}