using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayLiftoffJumpParticlesAction",
    menuName = "State Machines/Actions/Play Liftoff Jump Particles Action")]
public class PlayLiftoffJumpParticlesActionSO : StateActionSO<PlayLiftoffJumpParticlesAction>
{
}

public class PlayLiftoffJumpParticlesAction : StateAction
{
    private PlayerEffectController dustController;

    public override void Awake(StateMachine.StateMachine stateMachine)
    {
        dustController = stateMachine.GetComponent<PlayerEffectController>();
    }

    public override void OnStateEnter()
    {
        dustController.PlayLandParticles(1f); //Same particles as the landing, but with full power
    }

    public override void OnUpdate()
    {
    }
}