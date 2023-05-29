using UnityEngine;
using UnityEngine.AI;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "StopAgent", menuName = "State Machines/Actions/Stop NavMesh Agent")]
public class StopAgentSO : StateActionSO
{
    protected override StateAction CreateAction() => new StopAgent();
}

public class StopAgent : StateAction
{
    private NavMeshAgent agent;
    private bool agentDefined;

    public override void Awake(StateMachine.StateMachine stateMachine)
    {
        agent = stateMachine.gameObject.GetComponent<NavMeshAgent>();
        agentDefined = agent != null;
    }

    public override void OnUpdate()
    {
    }

    public override void OnStateEnter()
    {
        if (agentDefined && agent.isActiveAndEnabled && agent.isOnNavMesh)
        {
            agent.isStopped = true;
        }
    }
}