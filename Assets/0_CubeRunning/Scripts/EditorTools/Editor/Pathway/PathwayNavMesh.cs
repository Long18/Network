using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Linq;

public class PathwayNavMesh
{
    private PathwayConfigSO pathway;

    public PathwayNavMesh(PathwayConfigSO pathway)
    {
        this.pathway = pathway;
        this.pathway.Hits = new List<bool>();
    }

    public bool HasNavMeshAt(int index)
    {
        NavMeshHit hit;
        bool hasHit = true;

        if (pathway.Waypoints.Count >= pathway.Hits.Count)
        {
            hasHit = NavMesh.SamplePosition(pathway.Waypoints[index].waypoint, out hit, pathway.ProbeRadius,
                NavMesh.AllAreas);

            if (index > pathway.Hits.Count - 1)
            {
                index = pathway.Hits.Count;
                pathway.Hits.Add(hasHit);
            }
            else
            {
                pathway.Hits[index] = hasHit;
            }

            if (hasHit)
            {
                pathway.Waypoints[index].waypoint = hit.position;
            }
        }
        else
        {
            pathway.Hits.RemoveAt(index);
        }

        return hasHit;
    }

    private List<Vector3> GetPathCorners(int startIndex, int endIndex)
    {
        NavMeshPath navMeshPath = new NavMeshPath();

        if (NavMesh.CalculatePath(pathway.Waypoints[startIndex].waypoint, pathway.Waypoints[endIndex].waypoint,
                NavMesh.AllAreas, navMeshPath))
        {
            return navMeshPath.corners.ToList();
        }

        else
            return null;
    }

    private bool CopyCorners(int startIndex, int endIndex)
    {
        List<Vector3> result;

        if ((result = GetPathCorners(startIndex, endIndex)) != null)
        {
            pathway.Waypoints[startIndex].corners = result;
        }

        return result != null;
    }

    public bool UpdateCornersAt(int index)
    {
        bool canUpdate = true;

        if (pathway.Waypoints.Count > 1 && index < pathway.Waypoints.Count)
        {
            if (index == 0)
            {
                canUpdate = CopyCorners(index, index + 1);
                canUpdate &= CopyCorners(pathway.Waypoints.Count - 1, index);
            }
            else if (index == pathway.Waypoints.Count - 1)
            {
                canUpdate = CopyCorners(index - 1, index);
                canUpdate &= CopyCorners(index, 0);
            }
            else
            {
                canUpdate = CopyCorners(index - 1, index);
                canUpdate &= CopyCorners(index, index + 1);
            }
        }

        return canUpdate;
    }

    public void UpdatePath()
    {
        if (pathway.Waypoints.Count > 1)
        {
            pathway.Path = pathway.Waypoints.Aggregate(new List<Vector3>(), (acc, wpd) =>
            {
                wpd.corners.ForEach(c => acc.Add(c));
                return acc;
            });
        }
        else
        {
            pathway.Path.Clear();
        }
    }
}