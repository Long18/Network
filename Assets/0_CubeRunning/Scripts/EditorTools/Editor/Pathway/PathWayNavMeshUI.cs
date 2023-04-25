using UnityEngine;
using UnityEditorInternal;


public class PathWayNavMeshUI
{
    private PathwayConfigSO pathway;
    private PathwayNavMesh pathwayNavMesh;

    public PathWayNavMeshUI(PathwayConfigSO pathway)
    {
        this.pathway = pathway;
        pathwayNavMesh = new PathwayNavMesh(pathway);
        RestorePath();
    }

    public void OnInspectorGUI()
    {
        if (!pathway.ToggledNavMeshDisplay)
        {
            if (GUILayout.Button("NavMesh Path"))
            {
                pathway.ToggledNavMeshDisplay = true;
                GeneratePath();
                InternalEditorUtility.RepaintAllViews();
            }
        }
        else
        {
            if (GUILayout.Button("Handles Path"))
            {
                pathway.ToggledNavMeshDisplay = false;
                InternalEditorUtility.RepaintAllViews();
            }
        }
    }

    public void UpdatePath()
    {
        if (!pathway.DisplayProbes)
        {
            pathwayNavMesh.UpdatePath();
        }
    }

    public void UpdatePathAt(int index)
    {
        if (index >= 0)
        {
            pathway.DisplayProbes = !pathwayNavMesh.HasNavMeshAt(index);

            if (!pathway.DisplayProbes && pathway.ToggledNavMeshDisplay)
            {
                pathway.DisplayProbes = !pathwayNavMesh.UpdateCornersAt(index);
            }
        }
    }

    public void RealTime(int index)
    {
        if (pathway.RealTimeEnabled)
        {
            UpdatePathAt(index);

            if (pathway.ToggledNavMeshDisplay)
            {
                UpdatePath();
            }
        }
    }

    private void RestorePath()
    {
        bool existsPath = true;

        pathway.Hits.Clear();
        pathway.DisplayProbes = false;

        if (pathway.Waypoints.Count > 1)
        {
            for (int i = 0; i < pathway.Waypoints.Count; i++)
            {
                existsPath &= pathwayNavMesh.HasNavMeshAt(i);
                existsPath &= pathwayNavMesh.UpdateCornersAt(i);
            }

            if (existsPath)
            {
                pathwayNavMesh.UpdatePath();
            }
        }

        pathway.DisplayProbes = !existsPath;
    }

    public void GeneratePath()
    {
        if (pathway.ToggledNavMeshDisplay)
        {
            RestorePath();
            pathway.ToggledNavMeshDisplay = !pathway.DisplayProbes;
        }
    }
}