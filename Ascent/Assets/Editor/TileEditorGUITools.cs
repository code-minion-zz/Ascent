#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections.Generic;

[InitializeOnLoad]
public class TileEditorGUITools : EditorWindow
{
    public static string NewLayerString = "";

    public static void NewLayerWindow(int windowId)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Name");
        NewLayerString = GUILayout.TextField(NewLayerString);
        GUILayout.EndHorizontal();
    }

    public static void EditorTools(int windowId)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Name");
        NewLayerString = GUILayout.TextField(NewLayerString);
        GUILayout.EndHorizontal();
    }
}
#endif