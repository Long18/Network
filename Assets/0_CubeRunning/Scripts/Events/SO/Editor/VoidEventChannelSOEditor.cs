using UnityEditor;
using UnityEngine;

/// <summary>
/// This class is used for Events that have no arguments show in Editor.
/// So you can raise the event from the inspector.
/// </summary>
[CustomEditor(typeof(VoidEventChannelSO), editorForChildClasses: true)]
public class VoidEventChannelSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUI.enabled = Application.isPlaying;

        var e = target as VoidEventChannelSO;
        if (GUILayout.Button($"Raise {e.name}"))
            e.RaiseEvent();
    }
}