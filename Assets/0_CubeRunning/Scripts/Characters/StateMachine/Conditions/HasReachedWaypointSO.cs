using UnityEngine;
using UnityEngine.AI;
using StateMachine;
using StateMachine.ScriptableObjects;


[CreateAssetMenu(fileName = "HasReachedRoamingDestination",
    menuName = "State Machines/Conditions/Has Reached Waypoint")]
public class HasReachedWaypointSO : StateConditionSO
{
    protected override Condition CreateCondition() => new HasReachedWaypoint();
}

public class HasReachedWaypoint : Condition
{
    private NavMeshAgent agent;

    public override void Awake(StateMachine.StateMachine stateMachine)
    {
        agent = stateMachine.gameObject.GetComponent<NavMeshAgent>();
    }

    protected override bool Statement()
    {
        if (!agent.pathPending)
        {
            //set the stop distance to 0.1 if it is set to 0 in the inspector 
            if (agent.stoppingDistance == 0) agent.stoppingDistance = 0.1f;
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }

        return false;
    }
}