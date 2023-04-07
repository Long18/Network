using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(fileName = "SlideAction", menuName = "State Machines/Actions/Slide")]
public class SlideActionSO : StateActionSO<SlideAction>
{
}

public class SlideAction : StateAction
{
    private Protagonist protagonist;

    public override void Awake(StateMachine.StateMachine stateMachine)
    {
        protagonist = stateMachine.GetComponent<Protagonist>();
    }

    public override void OnUpdate()
    {
        float speed = -Physics.gravity.y * Protagonist.GRAVITY_MULTIPLIER * .4f;
        Vector3 hitNormal = protagonist.lastHit.normal;
        Vector3 slideDirection = new Vector3(hitNormal.x, -hitNormal.y, hitNormal.z);
        Vector3.OrthoNormalize(ref hitNormal, ref slideDirection);

        protagonist.movementVector = slideDirection * speed;
    }
}