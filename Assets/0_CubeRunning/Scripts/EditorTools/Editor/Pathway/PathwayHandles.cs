using UnityEngine;
using UnityEditor;


public class PathwayHandles
{
	private PathwayConfigSO pathway;
	private Vector3 tmp;

	public PathwayHandles(PathwayConfigSO pathway)
	{
		this.pathway = pathway;
	}

	public int DisplayHandles()
	{
		for (int i = 0; i < pathway.Waypoints.Count; i++)
		{
			EditorGUI.BeginChangeCheck();

			tmp = Handles.PositionHandle(pathway.Waypoints[i].waypoint, Quaternion.identity);

			if (EditorGUI.EndChangeCheck())
			{
				pathway.Waypoints[i].waypoint = tmp;
				return i;
			}
		}
		return -1;
	}
}
