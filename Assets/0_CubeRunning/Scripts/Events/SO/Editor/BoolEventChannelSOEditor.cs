using UnityEditor;
using UnityEngine;

/// <summary>
/// This class is used for Events that have a bool argument show in Editor.
/// So you can raise the event from the inspector.
/// </summary>

[CustomEditor(typeof(BoolEventChannelSO), editorForChildClasses: true)]
public class BoolEventChannelSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUI.enabled = Application.isPlaying;

        var e = target as BoolEventChannelSO;
        if (GUILayout.Button($"Raise {e.name}"))
            e.RaiseEvent(e.LastValue);
    }
}