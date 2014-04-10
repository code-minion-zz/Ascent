using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

namespace Ascent 
{
    public class LevelEditor : EditorWindow
	{
        private Vector2 scrollPosition;
        private const int buttonSize = 255;

        private GameObject selectedRoom = null;
        private GameObject selectedTile = null;
        private List<Door> selectedDoors = new List<Door>();
        private bool multipleSelected;

        private RoomGeneration roomGen = new RoomGeneration();
        private EnvironmentID tileType;
        private GameObject selectedObject;
        private string directory;

        [MenuItem("Ascent/Level Editor %h")]
        private static void showEditor()
        {
            EditorWindow.GetWindow<LevelEditor>(false, "Level Editor");
        }

        [MenuItem("Ascent/Level Editor %h", true)]
        private static bool showEditorValidtator()
        {
            return true;
        }

        void OnEnable()
        {

        }

        void Update()
        {
            UpdateSelectedRoom();
            UpdateSelectedTile();
            UpdateSelectedDoors();

            this.Repaint();
        }

        void OnGUI()
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

			if (GUILayout.Button("Create New Room From Template", GUILayout.Width(buttonSize)))
			{
				RoomTemplateWindow roomCreationWnd = EditorWindow.GetWindow<RoomTemplateWindow>("Create Room");
				roomCreationWnd.Initialise(roomGen);
			}

            if (GUILayout.Button("Enter tile edit mode", GUILayout.Width(buttonSize)))
            {
                EditorWindow.GetWindow<TileEditorWindow>("Tile edit mode");
            }

            SelectRoomGUI();

            EditorGUILayout.EndScrollView();
        }

        #region GUI Helper Functions

        private void SelectRoomGUI()
        {
            if (selectedTile != null && selectedRoom != null)
            {
                GUILayout.Label("Selected Room: " + selectedRoom.name);
                GUILayout.Label("Selected Tile: " + selectedTile.name);

                TilePlacementGUI();

                if (GUILayout.Button("Rotate 90", GUILayout.Width(buttonSize)))
                {
                    foreach (GameObject selection in Selection.gameObjects)
                    {
                        selection.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 90.0f, Space.World);
                    }
                }
            }

            if (selectedDoors.Count > 1)
            {
                ConnectDoorsGUI();
            }
        }

        private void ConnectDoorsGUI()
        {
            if (GUILayout.Button("Connect Doors", GUILayout.Width(buttonSize)))
            {
                Door firstDoor = selectedDoors[0];

                // This will link the doors to the first door if there are more than one doors.
                foreach (Door door in selectedDoors)
                {
                    firstDoor.targetDoor = door;
                    door.targetDoor = firstDoor;

                    door.targetDoor.isConnected = true;
                    firstDoor.targetDoor.isConnected = true;

                    EditorUtility.SetDirty(firstDoor.targetDoor);
                    EditorUtility.SetDirty(door.targetDoor);
                }
            }
        }

        private void TilePlacementGUI()
        {
            selectedObject = EditorGUILayout.ObjectField("Select Object", selectedObject, typeof(GameObject), false) as GameObject;

            if (selectedObject == null)
            {
                return;
            }

            EnvironmentObj envIdentifier = selectedObject.GetComponent<EnvironmentObj>();
            Transform parent = null;
            List<GameObject> newSelection = new List<GameObject>();

            if (envIdentifier != null)
            {
                parent = GetParentByType(selectedRoom, envIdentifier.TileAttributeType);
            }

            if (parent == null)
            {
                parent = selectedTile.transform;
            }

            if (multipleSelected == true)
            {
                if (GUILayout.Button("Insert to selected objects", GUILayout.Width(buttonSize)))
                {
                    foreach (GameObject selection in Selection.gameObjects)
                    {
                        if (envIdentifier == null)
                        {
                            parent = selection.transform;
                        }

                        UnityEngine.Object go = PrefabUtility.InstantiatePrefab(selectedObject);

                        if (go != null)
                        {
                            GameObject instantiatedGo = go as GameObject;
                            instantiatedGo.transform.parent = parent;
                            instantiatedGo.transform.position = new Vector3(0.0f, selectedObject.transform.position.y, 0.0f) + selection.transform.position;
                            newSelection.Add(instantiatedGo);
                        }
                    }

                    Selection.objects = newSelection.ToArray();
                }

                if (GUILayout.Button("Replace objects with " + selectedObject.name, GUILayout.Width(buttonSize)))
                {
                    foreach (GameObject selection in Selection.gameObjects)
                    {
                        parent = selection.transform.parent;

                        UnityEngine.Object go = PrefabUtility.InstantiatePrefab(selectedObject);

                        if (go != null)
                        {
                            GameObject instantiatedGo = go as GameObject;
                            instantiatedGo.transform.parent = parent;
                            instantiatedGo.name = selection.name;
                            instantiatedGo.transform.position = selection.transform.position;
                            instantiatedGo.transform.rotation = selection.transform.rotation;
                            newSelection.Add(instantiatedGo);

                            foreach (Transform t in selection.GetComponentInChildren<Transform>())
                            {
                                t.parent = instantiatedGo.transform;
                            }
                        }

                        GameObject.DestroyImmediate(selection);
                    }

                    Selection.objects = newSelection.ToArray();
                }
            }
            else
            {
                if (GUILayout.Button("Insert to " + parent.name, GUILayout.Width(buttonSize)))
                {
                    //UnityEngine.Object go = EnvironmentFactory.CreateGameObjectByType(tileType) as UnityEngine.Object;
                    UnityEngine.Object go = PrefabUtility.InstantiatePrefab(selectedObject);
                    if (go != null)
                    {
                        GameObject instantiatedGo = go as GameObject;
                        instantiatedGo.transform.parent = parent;
                        instantiatedGo.transform.position = new Vector3(0.0f, selectedObject.transform.position.y, 0.0f) + selectedTile.transform.position;
                        Selection.activeGameObject = instantiatedGo;
                    }
                }

                if (GUILayout.Button("Replace object with " + selectedObject.name, GUILayout.Width(buttonSize)))
                {
                    Transform activeTransform = Selection.activeTransform;
                    parent = activeTransform.parent;

                    UnityEngine.Object go = PrefabUtility.InstantiatePrefab(selectedObject);

                    if (go != null)
                    {
                        GameObject instantiatedGo = go as GameObject;
                        instantiatedGo.transform.parent = parent;
                        instantiatedGo.name = activeTransform.name;
                        instantiatedGo.transform.position = activeTransform.position;
                        instantiatedGo.transform.rotation = activeTransform.rotation;
                        newSelection.Add(instantiatedGo);

                        // For every child of the object move it to the new object.
                        foreach (Transform t in activeTransform.GetComponentInChildren<Transform>())
                        {
                            t.parent = instantiatedGo.transform;
                        }
                    }

                    GameObject.DestroyImmediate(activeTransform.gameObject);
                    Selection.objects = newSelection.ToArray();
                }
            }
        }

        private Transform GetParentByType(GameObject roomObject, EnvironmentID type)
        {
            Transform parent = null;

            Room room = roomObject.GetComponent<Room>();

            if (room != null)
            {
                switch (type)
                {
                    case EnvironmentID.door:
                        {
                            GameObject t = room.GetNodeByLayer("Environment");

                            if (t == null)
                            {
                                room.FindAllNodes();
                                t = room.EnvironmentParent.gameObject;
                            }

                            parent = t.transform.FindChild("Doors");
                        }
                        break;

                    case EnvironmentID.monster:
                        {
                            GameObject t = room.GetNodeByLayer("Monster");

                            if (t == null)
                            {
                                room.FindAllNodes();
                                t = room.MonsterParent.gameObject;
                            }

                            parent = t.transform;
                        }
                        break;
                }

                return parent;
            }

            return parent;
        }

        /// <summary>
        /// Finds a specific node of a room based on the layer name.
        /// </summary>
        /// <param name="layer">The layer of the node</param>
        /// <returns></returns>
        public GameObject FindParentByLayer(int layer)
        {
            GameObject go = null;

            foreach (Transform node in selectedRoom.transform)
            {
                if (node.gameObject.layer == layer)
                {
                    go = node.gameObject;
                }
            }

            return go;
        }

        private void UpdateSelectedDoors()
        {
            GameObject[] selectedObjects = Selection.gameObjects;

            selectedDoors.Clear();

            foreach (GameObject go in selectedObjects)
            {
                if (go != null)
                {
                    Transform T = go.transform;

                    Door door = T.GetComponent<Door>();
                    if (door != null)
                    {
                        selectedDoors.Add(door);
                    }
                }
            }
        }

        private void UpdateSelectedTile()
        {
            if (Selection.gameObjects.Length > 1)
            {
                multipleSelected = true;
            }
            else
            {
                multipleSelected = false;
            }

            GameObject go = Selection.activeGameObject;

            if (go != null && go != selectedTile)
            {
                Transform T = go.transform;

                // Get the top heirachy
                while (T.parent != null && T.tag != "RoomTile")
                {
                    T = T.parent;
                }

                // Check if the object is an environment piece.
                if (T.tag == "RoomTile")
                {
                    selectedTile = T.gameObject;
                }
                else
                {
                    selectedTile = go;
                }
            }
            else if (go == selectedTile)
            {
                return;
            }
            else
            {
                selectedTile = null;
            }
        }

        /// <summary>
        /// Makes the current room object the top level parent of a selected object.
        /// </summary>
        private void UpdateSelectedRoom()
        {
            GameObject go = Selection.activeGameObject;

            if (go != null && go != selectedRoom)
            {
                Transform T = go.transform;

                while (T.parent != null && T.tag != "RoomRoot")
                {
                    T = T.parent;
                }

                if (T.tag != "RoomRoot")
                {
                    selectedRoom = null;
                }
                else
                {
                    selectedRoom = T.gameObject;
                }
            }
            else if (go == selectedRoom)
            {
                return;
            }
            else
            {
                selectedRoom = null;
            }
        }

        /// <summary>
        /// Groups the gameObjects and returns a new parent object
        /// with the newly grouped objects as children
        /// </summary>
        /// <param name="gameObjects">The game objects to group</param>
        /// <returns></returns>
        public GameObject GroupObjects(GameObject[] gameObjects)
        {
            GameObject go = new GameObject();

            foreach (GameObject g in gameObjects)
            {
                go.transform.parent = g.transform.parent;
                go.transform.localPosition = Vector3.zero;
                go.transform.localRotation = Quaternion.identity;
                go.transform.localScale = Vector3.one;

                g.transform.parent = go.transform;
                go.layer = g.layer;
                go.name = "Group: " + g.name;
                go.tag = g.tag;
            }

            return go;
        }

        #endregion
    }
}
