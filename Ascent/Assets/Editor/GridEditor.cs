using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

namespace Ascent
{
    [CustomEditor(typeof(Grid)), Serializable]
    public class GridEditor : Editor
    {
        [SerializeField]
        private Grid grid;
        private LevelEditor levelEditor;

        public void OnEnable()
        {
            grid = target as Grid;
        }

        public void Awake()
        {
            grid = target as Grid;
        }

        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open Grid Properties", GUILayout.Width(255)))
            {
                GridProperties gridProperties = EditorWindow.GetWindow<GridProperties>("Grid Properties");
                gridProperties.Init(grid);
            }

            SceneView.RepaintAll();
        }
    }
}
