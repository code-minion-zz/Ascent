using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

namespace Ascent 
{
    [Serializable]
    public class GridWindow : EditorWindow
	{
        Grid grid; 
        public UnityEngine.Object objectToCreate;
        public UnityEngine.Object roomToLoad;
        public GameObject parentRoom = null;
        public string prefabName = "Room1";

        Vector2 scrollPosition;

        public void Init()
        {
            grid = FindObjectOfType(typeof(Grid)) as Grid;
        }

        void OnEnable()
        {
            grid = FindObjectOfType(typeof(Grid)) as Grid;
        }

        void Update()
        {

        }

        void OnGUI()
        {
            if (grid == null)
                return;

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            grid.color = EditorGUILayout.ColorField(grid.color, GUILayout.Width(200));

            GUILayout.BeginHorizontal();
            GUILayout.Label(" Spacing width ");
            grid.width = EditorGUILayout.FloatField(grid.width, GUILayout.Width(50));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label(" Spacing length ");
            grid.length = EditorGUILayout.FloatField(grid.length, GUILayout.Width(50));
            GUILayout.EndHorizontal();

            grid.width = Mathf.Clamp(grid.width, 0.1f, Mathf.Infinity);
            grid.length = Mathf.Clamp(grid.length, 0.1f, Mathf.Infinity);

            GUILayout.BeginHorizontal();
            GUILayout.Label(" Grid Width ");
            grid.gridWidth = EditorGUILayout.FloatField(grid.gridWidth, GUILayout.Width(50));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label(" Grid Length ");
            grid.gridLength = EditorGUILayout.FloatField(grid.gridLength, GUILayout.Width(50));
            GUILayout.EndHorizontal();

            // Assign selected game object prefab
            EditorGUILayout.BeginVertical();
            objectToCreate = EditorGUILayout.ObjectField("Object to place",
                objectToCreate, typeof(GameObject), true) as GameObject;
            EditorGUILayout.EndVertical();

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            prefabName = EditorGUILayout.TextField("Room Name", prefabName);

            if (parentRoom != null)
                parentRoom.name = prefabName;

            if (GUILayout.Button("Create New Room", GUILayout.Width(255)))
            {
                // Save the room and delete it so we can start again.
                SaveCurrentRoom();
                DeleteCurrentRoom();
                CreateNewRoom();
            }
            else if (GUILayout.Button("Add New Room", GUILayout.Width(255)))
            {
                SaveCurrentRoom();
                CreateNewRoom();
            }
            else if (GUILayout.Button("Save Room Layout", GUILayout.Width(255)) && parentRoom != null)
            {
                // Since we have a room to save lets go ahead and save it.
                SaveCurrentRoom();
            }

            LoadRoomGUI();

            EditorGUILayout.EndScrollView();
        }

        #region GUI Helper Functions

        private void SaveCurrentRoom()
        {
            if (parentRoom != null)
            {
                // Create the prefab at this location with the name of the parent.
                UnityEngine.Object prefab = PrefabUtility.CreatePrefab("Assets/Editor/Prefabs/" + parentRoom.name + ".prefab", parentRoom, ReplacePrefabOptions.ConnectToPrefab);
                Debug.Log("Createing prefab at path (Assets/Editor/Prefabs/" + parentRoom.name + ".prefab)");

                // Destroy the parent room so that there is no room.
                DestroyImmediate(parentRoom);

                // Instiate the newly created room and assign it back to the parent.
                parentRoom = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            }
        }

        private void DeleteCurrentRoom()
        {
            if (parentRoom != null)
                DestroyImmediate(parentRoom);
        }

        private void CreateNewRoom()
        {
            parentRoom = new GameObject(prefabName);
        }

        private void LoadRoomGUI()
        {
            roomToLoad = EditorGUILayout.ObjectField("Load Room",
                roomToLoad, typeof(GameObject), true) as GameObject;

            if (roomToLoad != null)
            {
                if (GUILayout.Button("Insert into grid", GUILayout.Width(255)))
                {
                    // Save and delete this room
                    //if (parentRoom != null)
                    //{
                    //    DeleteCurrentRoom();
                    //}

                    // Instantiate the prefab of the loaded room
                    GameObject room = PrefabUtility.InstantiatePrefab(roomToLoad) as GameObject;
                    // Now we want to make the parent room which we will edit as the room prefab.
                    parentRoom = room;
                    prefabName = room.name;
                    parentRoom.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
                }
            }
        }

        #endregion
    }
}
