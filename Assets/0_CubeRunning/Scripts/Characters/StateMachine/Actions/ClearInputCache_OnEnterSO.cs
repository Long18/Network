using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "ClearInputCache_OnEnter", menuName = "State Machines/Actions/Clear Input Cache On Enter")]
public class ClearInputCache_OnEnterSO : StateActionSO
{
    protected override StateAction CreateAction() => new ClearInputCache_OnEnter();
}

public class ClearInputCache_OnEnter : StateAction
{
    private Protagonist protagonist;
    private InteractionManager interactionManager;

    public override void Awake(StateMachine.StateMachine stateMachine)
    {
        protagonist = stateMachine.GetComponent<Protagonist>();
        interactionManager = stateMachine.GetComponentInChildren<InteractionManager>();
    }

    public override void OnUpdate()
    {
    }

    public override void OnStateEnter()
    {
        protagonist.jumpInput = false;
        interactionManager.currentInteractionType = InteractionType.None;
    }
}