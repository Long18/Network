using UnityEditor;
using UnityEngine;


/// <summary>
/// This class is used for Events that have one Item stack argument.
/// So you can raise the event from the inspector.
/// </summary>
[CustomEditor(typeof(ItemStackEventChannelSO), editorForChildClasses: true)]
public class ItemStackEventChannelSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var e = (TransformEventChannelSO)target;
        if (GUILayout.Button($"Raise {e.name} "))
        {
            e.RaiseEvent(e.LastValue);
        }
    }
}