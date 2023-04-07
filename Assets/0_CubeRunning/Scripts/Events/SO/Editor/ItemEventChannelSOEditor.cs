using UnityEditor;
using UnityEngine;

/// <summary>
/// This class is used for Events that have one Item argument.
/// So you can raise the event from the inspector.
/// </summary>
[CustomEditor(typeof(ItemEventChannelSO), editorForChildClasses: true)]
public class ItemEventChannelSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var e = (ItemEventChannelSO)target;
        if (GUILayout.Button($"Raise {e.name} "))
        {
            e.RaiseEvent(e.LastValue);
        }
    }
}