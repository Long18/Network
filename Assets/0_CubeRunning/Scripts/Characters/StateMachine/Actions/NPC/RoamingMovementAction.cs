using UnityEngine;
using UnityEngine.AI;

public class RoamingMovementAction : NPCMovementAction
{
    private NavMeshAgent agent;
    private bool isActiveAgent;
    private Vector3 startPosition;

    private float roamingSpeed;
    private float roamingDistance;

    private Vector3 roamingTargetPosition;

    public RoamingMovementAction(
        RoamingAroundCenterConfigSO config, NavMeshAgent agent, Vector3 startPosition)
    {
        this.agent = agent;
        isActiveAgent = this.agent != null && this.agent.isActiveAndEnabled && this.agent.isOnNavMesh;
        if (config.FromSpawningPoint)
        {
            this.startPosition = startPosition;
        }
        else
        {
            this.startPosition = config.CustomCenter;
        }

        roamingSpeed = config.Speed;
        roamingDistance = config.Radius;
    }

    public override void OnUpdate()
    {
    }

    public override void OnStateEnter()
    {
        if (isActiveAgent)
        {
            roamingTargetPosition = GetRoamingPositionAroundPosition(startPosition);
            agent.speed = roamingSpeed;
            agent.isStopped = false;
            agent.SetDestination(roamingTargetPosition);
        }
    }

    public override void OnStateExit()
    {
    }

    // Compute a random target position around the starting position.
    private Vector3 GetRoamingPositionAroundPosition(Vector3 position)
    {
        return position + new Vector3(Random.Range(-1, 1), 0.0f, Random.Range(-1, 1)).normalized *
            Random.Range(roamingDistance / 2, roamingDistance);
    }
}