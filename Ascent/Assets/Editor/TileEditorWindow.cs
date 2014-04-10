#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[InitializeOnLoad]
public class TileEditorWindow : EditorWindow
{
    public static SceneView sceneV;
    private static GUISkin editorSkin;
    public static bool enabled = true;

    static TileEditorWindow()
    {
        editorSkin = Resources.Load<GUISkin>("GUISkins/editorGUISkin");
        SceneView.onSceneGUIDelegate += OnSceneGUI;
    }

    static void OnSceneGUI(SceneView sceneView)
    {
        GUI.skin = editorSkin;

        // Begin drawing inside the scene view
        Handles.BeginGUI();

        enabled = GUI.Toggle(new Rect(0, 0, 100, 25), enabled, "Enable Editor");


        if (enabled == false)
        {
            sceneV = sceneView;

            Handles.BeginGUI();

            GUI.Window(0, new Rect(200, sceneView.position.height - 50, sceneView.position.width - 400, 50), TileEditorGUITools.EditorTools, "Tools");
            Handles.EndGUI();
        }

        Handles.EndGUI();
    }
}
#endif