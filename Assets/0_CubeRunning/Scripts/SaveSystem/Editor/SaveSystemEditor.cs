using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SaveSystem), editorForChildClasses: true)]
public class SaveSystemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var e = target as SaveSystem;
        if (GUILayout.Button($"Reset all data"))
            e.ResetSettings();
    }
}