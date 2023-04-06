using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(menuName = "State Machines/Conditions/Is Picking Up")]
public class IsPickingUpSO : StateConditionSO<IsPickingUpCondition>
{
}

public class IsPickingUpCondition : Condition
{
    //Component references
    private InteractionManager interactScript;

    public override void Awake(StateMachine.StateMachine stateMachine)
    {
        interactScript = stateMachine.GetComponent<InteractionManager>();
    }

    protected override bool Statement()
    {
        if (interactScript.currentInteractionType == InteractionType.PickUp)
        {
            // Consume it
            interactScript.currentInteractionType = InteractionType.None;
            return true;
        }
        else
        {
            return false;
        }
    }
}