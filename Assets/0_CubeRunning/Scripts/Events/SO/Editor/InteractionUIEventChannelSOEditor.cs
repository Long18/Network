using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InteractionUIEventChannelSO), editorForChildClasses: true)]
public class InteractionUIEventChannelSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUI.enabled = Application.isPlaying;

        var e = target as InteractionUIEventChannelSO;
        if (GUILayout.Button($"Raise {e.name}"))
            e.RaiseEvent(e.LastBoolValue, e.LastTypeValue);
    }
}