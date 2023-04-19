using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(fileName = "ControlWalkingParticlesAction",
    menuName = "State Machines/Actions/Control Walking Particles")]
public class PlayWalkingParticlesActionSO : StateActionSO<PlayWalkingParticlesAction>
{
}

public class PlayWalkingParticlesAction : StateAction
{
    private PlayerEffectController dustController;

    public override void Awake(StateMachine.StateMachine stateMachine)
    {
        dustController = stateMachine.GetComponent<PlayerEffectController>();
    }

    public override void OnStateEnter()
    {
        dustController.EnableWalkParticles();
    }

    public override void OnStateExit()
    {
        dustController.DisableWalkParticles();
    }

    public override void OnUpdate()
    {
    }
}