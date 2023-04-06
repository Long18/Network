using UnityEditor;
using UnityEngine;

/// <summary>
/// This class is used for Events that have a bool argument show in Editor.
/// So you can raise the event from the inspector
/// </summary>
[CustomEditor(typeof(FloatEventChannelSO), editorForChildClasses: true)]
public class FloatEventChannelSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUI.enabled = Application.isPlaying;

        var e = target as FloatEventChannelSO;
        if (GUILayout.Button($"Raise {e.name}"))
            e.RaiseEvent(e.LastValue);
    }
}