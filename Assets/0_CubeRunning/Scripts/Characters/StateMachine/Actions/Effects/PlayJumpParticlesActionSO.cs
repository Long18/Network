using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(menuName = "State Machines/Actions/Play Jump Particles")]
public class PlayJumpParticlesActionSO : StateActionSO<PlayJumpParticlesAction>
{
}

public class PlayJumpParticlesAction : StateAction
{
    private PlayerEffectController dustController;

    public override void Awake(StateMachine.StateMachine stateMachine)
    {
        dustController = stateMachine.GetComponent<PlayerEffectController>();
    }

    public override void OnStateEnter()
    {
        dustController.PlayJumpParticles();
    }

    public override void OnUpdate()
    {
    }
}