using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCFaceProtagonist", menuName = "State Machines/Actions/NPC Face Protagonist")]
public class NPCFaceProtagonistSO : StateActionSO
{
    public TransformAnchor playerAnchor;
    protected override StateAction CreateAction() => new NPCFaceProtagonist();
}

public class NPCFaceProtagonist : StateAction
{
    TransformAnchor protagonist;
    Transform actor;
    Quaternion rotationOnEnter;

    public override void Awake(StateMachine.StateMachine stateMachine)
    {
        actor = stateMachine.transform;
        protagonist = ((NPCFaceProtagonistSO)OriginSO).playerAnchor;
        rotationOnEnter = actor.rotation;
    }

    public override void OnUpdate()
    {
        if (protagonist.isSet)
        {
            Vector3 relativePos = protagonist.Value.position - actor.position;
            relativePos.y = 0f; // Force rotation to be only on Y axis.

            Quaternion rotation = Quaternion.LookRotation(relativePos);
            actor.rotation = rotation;
        }
    }

    public override void OnStateEnter()
    {
    }

    public override void OnStateExit()
    {
        actor.rotation = rotationOnEnter;
    }
}