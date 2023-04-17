using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InputReaderSO), editorForChildClasses: true)]
public class InputReaderSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var e = target as InputReaderSO;

        EditorGUILayout.Space();
        EditorGUILayout.TextField("Gameplay Input Enabled: ", e.StatusInput);
        EditorGUILayout.Space();


        if (GUILayout.Button($"Enable Gameplay Input"))
            e.EnableGameplayInput();
        else if (GUILayout.Button($"Enable Menu Input"))
            e.EnableMenuInput();
        else if (GUILayout.Button($"Disable All Input"))
            e.DisableAllInput();
    }
}