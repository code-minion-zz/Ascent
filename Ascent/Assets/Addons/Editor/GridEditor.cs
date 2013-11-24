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
        Grid grid;

        GridWindow window;

        public void OnEnable()
        {
            grid = target as Grid;
            window = EditorWindow.GetWindow<GridWindow>("Grid Properties Window");
            SceneView.onSceneGUIDelegate = GridUpdate;
        }

        void GridUpdate(SceneView sceneView)
        {
            Event e = Event.current;

            Ray r = Camera.current.ScreenPointToRay(new Vector3(e.mousePosition.x, -e.mousePosition.y + Camera.current.pixelHeight));
            Vector3 mousePos = r.origin;

            // TODO: Make this instantiated prefab connect to the right category based on 
            // its layer.
            if (e.isKey && e.character == 'a')
            {
                if (window.objectToCreate)
                {
                    //Undo.IncrementCurrentEventIndex();
                    // Get the object to create from our grid tool
                    GameObject obj = PrefabUtility.InstantiatePrefab(window.objectToCreate) as GameObject;

                    // Position it on the grid
                    Vector3 aligned = new Vector3(Mathf.Floor(mousePos.x / grid.width) * grid.width + grid.width / 2.0f,
                                                    obj.transform.position.y,
                                                    Mathf.Floor(mousePos.z / grid.length) * grid.length + grid.length / 2.0f);
                    obj.transform.position = aligned;

                    // Setup the parent object.
                    if (window.currentRoom != null)
                    {
                        obj.transform.parent = window.FindParentByLayer(obj.layer);
                    }

                    Undo.RegisterCreatedObjectUndo(obj, "Create " + obj.name);
                }
                else
                {
                    Debug.Log("Please select an object to place first. To select an object open the grid properties");
                }
            }
            else if (e.isKey && e.character == 'd')
            {
                //Undo.IncrementCurrentEventIndex();
                //Undo.RegisterSceneUndo("Delete Selected Objects");

                foreach (GameObject obj in Selection.gameObjects)
                {
                    DestroyImmediate(obj);
                }
            }
        }

        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open Grid Properties", GUILayout.Width(255)))
            {
                window = EditorWindow.GetWindow<GridWindow>("Grid Properties Window");
                window.Init();
            }

            if (GUILayout.Button("Open Level Designer", GUILayout.Width(255)))
            {
                LevelDesigner levelDesigner = EditorWindow.GetWindow<LevelDesigner>("LevelDesigner");
                levelDesigner.Init(grid);

            }

            SceneView.RepaintAll();
        }

        void HandleObjectSelectGUI()
        {

        }
    }
}
