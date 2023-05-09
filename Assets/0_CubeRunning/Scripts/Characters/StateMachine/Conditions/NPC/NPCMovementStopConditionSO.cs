using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(menuName = "State Machines/Conditions/NPC Movement Stop Elapsed")]
public class NPCMovementStopConditionSO : StateConditionSO<NPCMovementStopCondition>
{
}

public class NPCMovementStopCondition : Condition
{
    private float startTime;
    private NPCMovement npcMovement;

    public override void Awake(StateMachine.StateMachine stateMachine)
    {
        npcMovement = stateMachine.GetComponent<NPCMovement>();
    }

    public override void OnStateEnter()
    {
        startTime = Time.time;
    }

    protected override bool Statement() => Time.time >= startTime + npcMovement.npcMovementConfig.StopDuration;
}