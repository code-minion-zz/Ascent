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
        public GameObject objectToCreate;
        public GameObject roomToLoad;
        public GameObject currentRoom = null;
        public string roomName = "Room1";

        private Vector2 scrollPosition;

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
            UpdateActiveObject();
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

            roomName = EditorGUILayout.TextField("Room Name", roomName);

            if (currentRoom != null)
                currentRoom.name = roomName;

            if (GUILayout.Button("Create New Room", GUILayout.Width(255)))
            {
                // Don't want to save the room just yet because its creating a prefab
                // this prefab is being nested.
                //SaveCurrentRoom();
                //DeleteCurrentRoom();
                CreateNewRoom();
            }
            
            if (GUILayout.Button("Save Room Layout", GUILayout.Width(255)) && currentRoom != null)
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
            if (currentRoom != null)
            {
                // Create the prefab at this location with the name of the parent.
                UnityEngine.Object prefab = PrefabUtility.CreatePrefab("Assets/Addons/Editor/Prefabs/" + currentRoom.name + ".prefab", currentRoom, ReplacePrefabOptions.ConnectToPrefab);
                Debug.Log("Createing prefab at path (Assets/Addons/Editor/Prefabs/" + currentRoom.name + ".prefab)");

                // Destroy the parent room so that there is no room.
                DestroyImmediate(currentRoom);

                // Instiate the newly created room and assign it back to the parent.
                currentRoom = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            }
        }

        private void DeleteCurrentRoom()
        {
            if (currentRoom != null)
                DestroyImmediate(currentRoom);
        }

        /// <summary>
        /// Creates a new room game object and creates child nodes which hold
        /// other objects to keep them all in a category.
        /// </summary>
        private void CreateNewRoom()
        {
            currentRoom = new GameObject(roomName);
            Room room = currentRoom.AddComponent<Room>();

            room.AddNewParentCategory("FloorTiles", LayerMask.NameToLayer("Floor"));
            room.AddNewParentCategory("Walls", LayerMask.NameToLayer("Wall"));
            room.AddNewParentCategory("Monsters", LayerMask.NameToLayer("Monster"));
            room.AddNewParentCategory("Items", LayerMask.NameToLayer("Items"));
            room.AddNewParentCategory("Lights", LayerMask.NameToLayer("Default"));
        }

        private void LoadRoomGUI()
        {
            roomToLoad = EditorGUILayout.ObjectField("Load Room",
                roomToLoad, typeof(GameObject), true) as GameObject;

            if (roomToLoad != null)
            {
                if (GUILayout.Button("Insert into grid", GUILayout.Width(255)))
                {
                    // Instantiate the prefab of the loaded room
                    GameObject room = PrefabUtility.InstantiatePrefab(roomToLoad) as GameObject;

                    // Now we want to make the parent room which we will edit as the room prefab.
                    currentRoom = room;
                    roomName = room.name;
                    currentRoom.transform.position = grid.gridPosition;
                }
            }
        }

        /// <summary>
        /// Finds a specific node of a room based on the layer name.
        /// </summary>
        /// <param name="layer">The layer of the node</param>
        /// <returns></returns>
        public Transform FindParentByLayer(int layer)
        {
            Transform trans = null;

            foreach (Transform node in currentRoom.transform)
            {
                if (node.gameObject.layer == layer)
                {
                    trans = node;
                }
            }

            return trans;
        }

        /// <summary>
        /// Makes the current room object the top level parent of a selected object.
        /// </summary>
        private void UpdateActiveObject()
        {
            GameObject go = Selection.activeGameObject;

            if (go != null && go != currentRoom)
            {
                Transform T = go.transform;

                while (T.parent != null)
                {
                    T = T.parent;
                }

                currentRoom = T.gameObject;
            }
        }

        #endregion
    }
}
