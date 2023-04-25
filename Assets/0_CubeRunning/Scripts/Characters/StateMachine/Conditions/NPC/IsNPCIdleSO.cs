using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(fileName = "IsNPCIdle", menuName = "State Machines/Conditions/Is NPC Idle")]
public class IsNPCIdleSO : StateConditionSO<IsNPCIdleCondition>
{
}

public class IsNPCIdleCondition : Condition
{
    //Component references
    private NPC npcScript;

    public override void Awake(StateMachine.StateMachine stateMachine)
    {
        npcScript = stateMachine.GetComponent<NPC>();
    }

    protected override bool Statement()
    {
        if (npcScript.npcState == NPCState.Idle)
        {
            // We don't want to consume it because we want the townsfolk to stay idle
            return true;
        }
        else
        {
            return false;
        }
    }
}