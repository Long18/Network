using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathwayMovementAction : NPCMovementAction
{
    private NavMeshAgent agent;
    private bool isActiveAgent;
    private List<WaypointData> wayppoints;
    private int wayPointIndex;
    private float roamingSpeed;

    public PathwayMovementAction(
        PathwayConfigSO config, NavMeshAgent agent)
    {
        this.agent = agent;
        isActiveAgent = this.agent != null && this.agent.isActiveAndEnabled && this.agent.isOnNavMesh;
        wayPointIndex = -1; //Initialized to -1 so we don't skip the first element from the waypoint list
        roamingSpeed = config.Speed;
        wayppoints = config.Waypoints;
    }

    public override void OnUpdate()
    {
    }

    public override void OnStateEnter()
    {
        if (isActiveAgent)
        {
            agent.speed = roamingSpeed;
            agent.isStopped = false;
            agent.SetDestination(GetNextDestination());
        }
    }

    public override void OnStateExit()
    {
    }

    private Vector3 GetNextDestination()
    {
        Vector3 nextDestination = agent.transform.position;
        if (wayppoints.Count > 0)
        {
            //We check the modulo so when we reach the end of the array we go back to the first element
            wayPointIndex = (wayPointIndex + 1) % wayppoints.Count;
            nextDestination = wayppoints[wayPointIndex].waypoint;
        }

        //Debug.Log("the next destination index = " +_wayPointIndex + "value = " + nextDestination);
        return nextDestination;
    }
}