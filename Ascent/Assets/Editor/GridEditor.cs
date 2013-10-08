using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Ascent
{
    [CustomEditor(typeof(Grid))]
    public class GridEditor : Editor
    {
        Grid grid;

        public void OnEnable()
        {
            grid = target as Grid;
        }

        public override void OnInspectorGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(" Grid Width ");
            grid.width = EditorGUILayout.FloatField(grid.width, GUILayout.Width(50));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label(" Grid Height ");
            grid.height = EditorGUILayout.FloatField(grid.height, GUILayout.Width(50));
            GUILayout.EndHorizontal();

            SceneView.RepaintAll();
        }
    }
}
