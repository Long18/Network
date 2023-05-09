using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "NPCMoveToNextDestination",
    menuName = "State Machines/Actions/NPC Move To Next Destination")]
public class NPCMoveToNextDestinationSO : StateActionSO
{
    protected override StateAction CreateAction() => new NPCMoveToNextDestination();
}

public class NPCMoveToNextDestination : StateAction
{
    private NPCMovement npcMovement;
    private NPCMovementConfigSO config;
    private NPCMovementAction action;
    private NavMeshAgent agent;

    public override void Awake(StateMachine.StateMachine stateMachine)
    {
        agent = stateMachine.GetComponent<NavMeshAgent>();
        npcMovement = stateMachine.GetComponent<NPCMovement>();
        InitMovementStrategy(npcMovement.npcMovementConfig);
    }

    public override void OnStateEnter()
    {
        if (config != npcMovement.npcMovementConfig)
        {
            InitMovementStrategy(npcMovement.npcMovementConfig);
        }

        action.OnStateEnter();
    }

    public override void OnUpdate()
    {
        action.OnUpdate();
    }

    public override void OnStateExit()
    {
        action.OnStateExit();
    }

    private void InitMovementStrategy(NPCMovementConfigSO config)
    {
        this.config = config;
        if (npcMovement.npcMovementConfig is RoamingAroundCenterConfigSO)
        {
            action = new RoamingMovementAction(
                (RoamingAroundCenterConfigSO)npcMovement.npcMovementConfig,
                agent,
                npcMovement.transform.position);
        }
        else if (npcMovement.npcMovementConfig is PathwayConfigSO)
        {
            action = new PathwayMovementAction(
                (PathwayConfigSO)npcMovement.npcMovementConfig,
                agent);
        }
    }
}