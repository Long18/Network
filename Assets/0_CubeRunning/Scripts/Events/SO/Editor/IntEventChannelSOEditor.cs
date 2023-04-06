using UnityEditor;
using UnityEngine;

/// <summary>
/// This class is used for Events that have a int argument show in Editor.
/// So you can raise the event from the inspector.
/// </summary>
[CustomEditor(typeof(IntEventChannelSO), editorForChildClasses: true)]
public class IntEventChannelSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUI.enabled = Application.isPlaying;

        var e = target as IntEventChannelSO;
        if (GUILayout.Button($"Raise {e.name}"))
            e.RaiseEvent(e.LastValue);
    }
}