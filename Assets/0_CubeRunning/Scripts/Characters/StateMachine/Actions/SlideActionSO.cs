using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(fileName = "SlideAction", menuName = "State Machines/Actions/Slide")]
public class SlideActionSO : StateActionSO<SlideAction>
{
    [Tooltip("Sliding speed on the XZ plane.")]
    public float slideSpeed = 10f;
}

public class SlideAction : StateAction
{
    private Protagonist protagonist;
    private SlideActionSO originSO => (SlideActionSO)base.OriginSO; // The SO this StateAction spawned from

    public override void Awake(StateMachine.StateMachine stateMachine)
    {
        protagonist = stateMachine.GetComponent<Protagonist>();
    }

    public override void OnUpdate()
    {
        Vector3 hitNormal = protagonist.lastHit.normal;
        protagonist.movementVector.x = (1f - hitNormal.y) * hitNormal.x * originSO.slideSpeed;
        protagonist.movementVector.z = (1f - hitNormal.y) * hitNormal.z * originSO.slideSpeed;
    }

    public override void OnStateExit()
    {
        protagonist.movementVector = Vector3.zero;
    }
}