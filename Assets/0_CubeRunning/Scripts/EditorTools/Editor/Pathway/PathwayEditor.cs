using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Linq;

[CustomEditor(typeof(PathwayConfigSO))]
public class PathwayEditor : Editor
{
    private ReorderableList reorderableList;
    private PathwayConfigSO pathway;
    private PathwayHandles pathwayHandles;
    private PathWayNavMeshUI pathWayNavMeshUI;

    private enum LIST_MODIFICATION
    {
        ADD,
        SUPP,
        DRAG,
        OTHER
    };

    private LIST_MODIFICATION currentListModification;
    private int indexCurrentModification;

    public void OnSceneGUI(SceneView sceneView)
    {
        int index = pathwayHandles.DisplayHandles();
        pathWayNavMeshUI.RealTime(index);
        PathwayGizmos.DrawGizmosSelected(pathway);
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        serializedObject.Update();
        reorderableList.DoLayoutList();
        pathWayNavMeshUI.OnInspectorGUI();
        serializedObject.ApplyModifiedProperties();
    }

    private void OnEnable()
    {
        Undo.undoRedoPerformed += DoUndo;
        reorderableList = new ReorderableList(serializedObject, serializedObject.FindProperty("Waypoints"), true, true,
            true, true);
        reorderableList.drawHeaderCallback += DrawHeader;
        reorderableList.drawElementCallback += DrawElement;
        reorderableList.onAddCallback += AddItem;
        reorderableList.onRemoveCallback += RemoveItem;
        reorderableList.onChangedCallback += ListModified;
        reorderableList.onMouseDragCallback += DragItem;
        pathway = (target as PathwayConfigSO);
        pathWayNavMeshUI = new PathWayNavMeshUI(pathway);
        pathwayHandles = new PathwayHandles(pathway);
        currentListModification = LIST_MODIFICATION.OTHER;
        SceneView.duringSceneGui += this.OnSceneGUI;
    }

    private void OnDisable()
    {
        Undo.undoRedoPerformed -= DoUndo;
        reorderableList.drawHeaderCallback -= DrawHeader;
        reorderableList.drawElementCallback -= DrawElement;
        reorderableList.onAddCallback -= AddItem;
        reorderableList.onRemoveCallback -= RemoveItem;
        reorderableList.onChangedCallback -= ListModified;
        reorderableList.onMouseDragCallback -= DragItem;
        SceneView.duringSceneGui -= this.OnSceneGUI;
    }

    private void DrawHeader(Rect rect)
    {
        GUI.Label(rect, PathwayConfigSO.TITLE_LABEL);
    }

    private void DrawElement(Rect rect, int index, bool active, bool focused)
    {
        SerializedProperty item = reorderableList.serializedProperty.GetArrayElementAtIndex(index)
            .FindPropertyRelative("waypoint");
        item.vector3Value = EditorGUI.Vector3Field(rect, PathwayConfigSO.FIELD_LABEL + index, item.vector3Value);
    }

    private void AddItem(ReorderableList list)
    {
        int index = list.index;

        if (index > -1 && list.serializedProperty.arraySize >= 1)
        {
            list.serializedProperty.InsertArrayElementAtIndex(index + 1);
            Vector3 previous = list.serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("waypoint")
                .vector3Value;
            list.serializedProperty.GetArrayElementAtIndex(index + 1).FindPropertyRelative("waypoint").vector3Value =
                new Vector3(previous.x + 2, previous.y, previous.z + 2);
            indexCurrentModification = index + 1;
        }
        else
        {
            list.serializedProperty.InsertArrayElementAtIndex(list.serializedProperty.arraySize);
            Vector3 previous = Vector3.zero;
            list.serializedProperty.GetArrayElementAtIndex(list.serializedProperty.arraySize - 1)
                    .FindPropertyRelative("waypoint").vector3Value =
                new Vector3(previous.x + 2, previous.y, previous.z + 2);
            indexCurrentModification = list.serializedProperty.arraySize - 1;
        }

        currentListModification = LIST_MODIFICATION.ADD;
        list.index++;
    }

    private void RemoveItem(ReorderableList list)
    {
        int index = list.index;

        list.serializedProperty.DeleteArrayElementAtIndex(index);

        if (list.index == list.serializedProperty.arraySize)
        {
            list.index--;
        }

        indexCurrentModification = index - 1;
        currentListModification = LIST_MODIFICATION.SUPP;
    }

    private void DragItem(ReorderableList list)
    {
        indexCurrentModification = list.index;
        currentListModification = LIST_MODIFICATION.DRAG;
    }

    private void ListModified(ReorderableList list)
    {
        list.serializedProperty.serializedObject.ApplyModifiedProperties();

        switch (currentListModification)
        {
            case LIST_MODIFICATION.ADD:
                pathWayNavMeshUI.UpdatePathAt(indexCurrentModification);
                break;

            case LIST_MODIFICATION.SUPP:
                if (list.serializedProperty.arraySize > 1)
                {
                    pathWayNavMeshUI.UpdatePathAt((list.serializedProperty.arraySize + indexCurrentModification) %
                                                  list.serializedProperty.arraySize);
                }

                break;
            case LIST_MODIFICATION.DRAG:
                pathWayNavMeshUI.UpdatePathAt(list.index);
                pathWayNavMeshUI.UpdatePathAt(indexCurrentModification);
                break;
            default:
                break;
        }

        currentListModification = LIST_MODIFICATION.OTHER;
    }

    private void DoUndo()
    {
        serializedObject.UpdateIfRequiredOrScript();

        if (reorderableList.index >= reorderableList.serializedProperty.arraySize)
        {
            reorderableList.index = reorderableList.serializedProperty.arraySize - 1;
        }

        pathWayNavMeshUI.GeneratePath();
    }
}