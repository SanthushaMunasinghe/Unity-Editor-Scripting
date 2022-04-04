using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(PatrolRoute)), CanEditMultipleObjects]
public class PatrolRouteEditor : Editor
{
    private void OnEnable()
    {
        // Register method for drawing to and interacting with the SceneView
        SceneView.duringSceneGui += DuringSceneGUI;
        // Hide the default gizmos
        Tools.hidden = true;
    }

    private void OnDisable()
    {
        // Unregister the scenegui method
        SceneView.duringSceneGui -= DuringSceneGUI;
        // Restore the default gizmo
        Tools.hidden = false;
    }

    // This is the UIElements method of drawing the inspector
    public override VisualElement CreateInspectorGUI()
    {
        var inspector = new VisualElement();
        inspector.Add(new HelpBox("This is the Patrol Route Inspector\r\n[UIElements]", HelpBoxMessageType.Info));
        inspector.Add(new IMGUIContainer(() => DrawDefaultInspector()));
        return inspector;
    }

    // This is the [legacy] IMGUI version of the inspector
    // Note that implementing CreateInspectorGUI means the editor will *ignore* OnInspectorGUI
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("This is the Patrol Route Inspector\r\n[IMGUI]", MessageType.Info);
        base.OnInspectorGUI();
    }

    private void DuringSceneGUI(SceneView view)
    {
        //DrawWaypoints(target as PatrolRoute, view);
        //DrawWaypoints(serializedObject, view);

        foreach (var o in serializedObject.targetObjects)
        {
            var so = new SerializedObject(o);
            DrawWaypoints(so, view);
        }
    }

    private void DrawWaypoints (SerializedObject so, SceneView view)
    {
        // We use SerializedObject and SerializedProperties because the editor implements a ton of helper functionality
        // Undo/redo, multi-object editing (at least for the inspector)
        var waypoints = so.FindProperty("waypoints");
        for (int i = 0; i < waypoints.arraySize; ++i)
        {
            waypoints.GetArrayElementAtIndex(i).vector3Value = Handles.DoPositionHandle(waypoints.GetArrayElementAtIndex(i).vector3Value, Quaternion.identity);
            //var pos = waypoints.GetArrayElementAtIndex(i).vector3Value;
            // Handles (and HandleUtility) is an entire class with useful functions to draw and interact with the SceneView
            //pos = Handles.Slider2D(pos, Vector3.up, Vector3.right, Vector3.forward, HandleUtility.GetHandleSize(pos) * 0.25f, Handles.CircleHandleCap, Vector2.zero);
            //waypoints.GetArrayElementAtIndex(i).vector3Value = pos;
            //var offset = view.camera.transform.right + view.camera.transform.up;
            //Handles.Label(pos + offset * HandleUtility.GetHandleSize(pos) * 0.25f, i.ToString());
        }
        // If we forget this, the data never gets written
        so.ApplyModifiedProperties();
    }

    private void DrawWaypoints(PatrolRoute route, SceneView view)
    {
        for (int i = 0; i < route.waypoints.Length; ++i)
        {
            var hotControl = GUIUtility.hotControl;
            route.waypoints[i] = Handles.DoPositionHandle(route.waypoints[i], Quaternion.identity);
            // var pos = route.Waypoints[i];
            // pos = Handles.Slider2D(pos, Vector3.up, Vector3.right, Vector3.forward, HandleUtility.GetHandleSize(pos) * 0.25f, Handles.CircleHandleCap, Vector2.zero);
            if (GUIUtility.hotControl != hotControl)
            {
                Undo.RegisterCompleteObjectUndo(route, "Modify Waypoints");
            }
            // route.Waypoints[i] = pos;
            // var offset = view.camera.transform.right + view.camera.transform.up;
            // Handles.Label(pos + offset * HandleUtility.GetHandleSize(pos) * 0.25f, i.ToString());
        }
        // If we forget this, the data never gets written
        EditorUtility.SetDirty(route);
    }
}
