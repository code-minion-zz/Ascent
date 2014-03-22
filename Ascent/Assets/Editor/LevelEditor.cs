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

        private SaveRooms roomSaver = new SaveRooms();
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

            // Assign selected game object prefab
            if (GUILayout.Button("Create New Room", GUILayout.Width(buttonSize)))
            {                
                RoomCreationWindow roomCreationWnd = EditorWindow.GetWindow<RoomCreationWindow>("Create Room");
                roomCreationWnd.Initialise(roomGen);
            }
			if (GUILayout.Button("Create New Room From Template", GUILayout.Width(buttonSize)))
			{
				RoomTemplateWindow roomCreationWnd = EditorWindow.GetWindow<RoomTemplateWindow>("Create Room");
				roomCreationWnd.Initialise(roomGen);
			}

            SelectRoomGUI();

            EditorGUILayout.EndScrollView();
        }

        #region GUI Helper Functions

        private void SelectRoomGUI()
        {
            //if (GUILayout.Button("Load Room", GUILayout.Width(buttonSize)))
            //{
            //    directory = EditorUtility.OpenFilePanel("Open file", "Assets/Resources/Maps", "txt");

            //    if (directory != "")
            //    {
            //        // Load and add the room to the list of rooms.
            //        RoomProperties room = roomSaver.LoadRoom(directory, false);

            //        if (room != null)
            //        {
            //            // Construct the newly loaded room.
            //            room.ConstructRoom();
            //        }
            //    }
            //}

            if (roomSaver != null)
            {
                //if (GUILayout.Button("Save Room", GUILayout.Width(buttonSize)))
                //{
                //    SaveSelected();
                //}
            }


            if (selectedTile != null && selectedRoom != null)
            {
                GUILayout.Label("Selected Room: " + selectedRoom.name);
                GUILayout.Label("Selected Tile: " + selectedTile.name);

                TilePlacementGUI();

                if (GUILayout.Button("Rotate 90", GUILayout.Width(buttonSize)))
                {
                    Selection.activeGameObject.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 90.0f, Space.World);
                }
            }

            if (selectedDoors.Count > 1)
            {
                ConnectDoorsGUI();
            }
        }

        private void SaveSelected()
        {
            if (selectedRoom == null)
            {
                return;
            }

            directory = EditorUtility.SaveFilePanel("Save Room", "Assets/Resources/Maps", "NewRoom", "txt");

            if (directory == "")
            {
                return;
            }

            Room room = selectedRoom.GetComponent<Room>();
            room.FindAllNodes();

            RoomProperties roomProperties = new RoomProperties(room);
            roomProperties.InitialiseTiles(room.NumberOfTilesX, room.NumberOfTilesY, 2);
            roomProperties.Name = selectedRoom.name;

            GameObject env = room.GetNodeByLayer("Environment");

            if (env != null)
            {
                foreach (Transform t in env.transform)
                {
                    if (t.tag == "RoomTile")
                    {
                        // Make sure the tiles are correctly label
                        string[] parts = t.name.Split("Tile[], ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        int XCoord = Int32.Parse(parts[0]);
                        int YCoord = Int32.Parse(parts[1]);
                        roomProperties.Tiles[XCoord, YCoord].GameObject = t.gameObject;
                       
                        foreach (Transform child in t)
                        {
                            EnvironmentObj id = child.GetComponent<EnvironmentObj>();
                            if (id != null)
                            {
                                TileAttribute att = new TileAttribute();
                                att.Type = id.TileAttributeType;
                                att.Angle = child.eulerAngles.y;

                                if (att.Type == EnvironmentID.door)
                                {
                                    att = new DoorTile();
                                    att.Type = id.TileAttributeType;
                                    att.Angle = child.eulerAngles.y;
                                    Door door = child.GetComponent<Door>();
                                    DoorTile tile = att as DoorTile;
                                    tile.IsConnected = door.isConnected;
                                    tile.IsEntryDoor = door.isEntryDoor;
                                    tile.Direction = door.direction;
                                }

                                roomProperties.Tiles[XCoord, YCoord].TileAttributes.Add(att);
                            }
                            else
                            {
                                Debug.Log("Unrecognized object attached to tile. Did you forget to add an EnvEdentifier to the object?");
                            }
                        }
                    }
                }
            }

            roomSaver.SaveRoom(roomProperties, directory);
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

            //Transform parent = GetParentByType(selectedRoom, tileType);
            Transform parent = null;

            if (envIdentifier != null)
            {
                parent = GetParentByType(selectedRoom, envIdentifier.TileAttributeType);
            }

            if (parent == null)
            {
                parent = selectedTile.transform;
            }

            if (GUILayout.Button("Insert to " + parent.name, GUILayout.Width(buttonSize)))
            {
                //UnityEngine.Object go = EnvironmentFactory.CreateGameObjectByType(tileType) as UnityEngine.Object;
                UnityEngine.Object go = PrefabUtility.InstantiatePrefab(selectedObject);
                if (go != null)
                {
                    GameObject instantiatedGo = go as GameObject;
                    instantiatedGo.transform.parent = parent;
                    instantiatedGo.transform.position = selectedTile.transform.position;
                    Selection.activeGameObject = instantiatedGo;
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
