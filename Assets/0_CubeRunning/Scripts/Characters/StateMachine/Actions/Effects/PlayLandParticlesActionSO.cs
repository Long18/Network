using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(menuName = "State Machines/Actions/Play Land Particles")]
public class PlayLandParticlesActionSO : StateActionSO<PlayLandParticlesAction>
{
}

public class PlayLandParticlesAction : StateAction
{
    private PlayerEffectController dustController;
    private Transform transform;
    private CharacterController characterController;

    private float coolDown = 0.3f;
    private float time = 0f;

    private float fallStartY = 0f;
    private float fallEndY = 0f;
    private float maxFallDistance = 4f; //Used to adjust particle emission intensity

    public override void Awake(StateMachine.StateMachine stateMachine)
    {
        dustController = stateMachine.GetComponent<PlayerEffectController>();
        transform = stateMachine.transform;
        characterController = stateMachine.GetComponent<CharacterController>();
    }

    public override void OnStateEnter()
    {
        fallStartY = transform.position.y;
    }

    public override void OnStateExit()
    {
        fallEndY = transform.position.y;
        float dY = Mathf.Abs(fallStartY - fallEndY);
        float fallIntensity = Mathf.InverseLerp(0, maxFallDistance, dY);

        if (Time.time >= time + coolDown && characterController.isGrounded)
        {
            dustController.PlayLandParticles(fallIntensity);
            time = Time.time;
        }
    }

    public override void OnUpdate()
    {
    }
}