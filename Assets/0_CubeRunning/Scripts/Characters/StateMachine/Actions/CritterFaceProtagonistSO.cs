using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "CritterFaceProtagonist", menuName = "State Machines/Actions/Critter Face Protagonist")]
public class CritterFaceProtagonistSO : StateActionSO
{
    public TransformAnchor playerAnchor;
    protected override StateAction CreateAction() => new CritterFaceProtagonist();
}

public class CritterFaceProtagonist : StateAction
{
    TransformAnchor protagonist;
    Transform actor;

    public override void Awake(StateMachine.StateMachine stateMachine)
    {
        actor = stateMachine.transform;
        protagonist = ((CritterFaceProtagonistSO)OriginSO).playerAnchor;
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
}