using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(menuName = "State Machines/Conditions/Is NPC In Dialogue")]
public class IsNPCInDialogueSO : StateConditionSO<IsNPCDialogueCondition>
{
}

public class IsNPCDialogueCondition : Condition
{
    //Component references
    private NPCController stepControllerScript;

    public override void Awake(StateMachine.StateMachine stateMachine)
    {
        stepControllerScript = stateMachine.GetComponent<NPCController>();
    }

    protected override bool Statement()
    {
        if (stepControllerScript.IsInDialogue)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}