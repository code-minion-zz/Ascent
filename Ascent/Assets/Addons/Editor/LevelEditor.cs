using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

namespace Ascent 
{
    [Serializable]
    public class LevelEditor : EditorWindow
	{
        private Vector2 scrollPosition;
        private const int buttonSize = 255;

        private GameObject selectedRoom = null;
        private GameObject selectedTile = null;

        private SaveRooms roomSaver = new SaveRooms();
        private RoomGeneration roomGen = new RoomGeneration();
        private TileType tileType;
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
            SceneView.onSceneGUIDelegate = GridUpdate;
        }

        void Update()
        {
            UpdateSelectedRoom();
            UpdateSelectedTile();

            this.Repaint();
        }

        void GridUpdate(SceneView sceneView)
        {
            UpdateSelectedRoom();
            UpdateSelectedTile();

            Event e = Event.current;
            Ray r = Camera.current.ScreenPointToRay(new Vector3(e.mousePosition.x, -e.mousePosition.y + Camera.current.pixelHeight));
            Vector3 mousePos = r.origin;
        }

        void OnGUI()
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            // Assign selected game object prefab
            if (GUILayout.Button("Create New Room", GUILayout.Width(buttonSize)))
            {                
                RoomCreationWindow roomCreationWnd = EditorWindow.GetWindow<RoomCreationWindow>("Create Room");
                roomCreationWnd.Initialise(this, roomGen);
            }

            SelectRoomGUI();

            EditorGUILayout.EndScrollView();
        }

        #region GUI Helper Functions

        private void SelectRoomGUI()
        {
            if (GUILayout.Button("Load Room", GUILayout.Width(buttonSize)))
            {
                directory = EditorUtility.OpenFilePanel("Open file", "Assets/Resources/Maps", "txt");

                if (directory != "")
                {
                    // Load and add the room to the list of rooms.
                    RoomProperties room = roomSaver.LoadRoom(directory, false);

                    if (room != null)
                    {
                        roomGen.ReconstructRoom(room);
                    }
                }
            }

            if (roomSaver != null)
            {
                if (GUILayout.Button("Save Room", GUILayout.Width(buttonSize)))
                {
                    SaveSelected();
                }
            }

            if (selectedRoom != null)
            {
                GUILayout.Label("Selected Room: " + selectedRoom.name);
            }

            if (selectedTile != null)
            {
                GUILayout.Label("Selected Tile: " + selectedTile.name);
                TilePlacementGUI();
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
            roomProperties.InitialiseTiles(7, 7);
            roomProperties.Name = selectedRoom.name;

            GameObject env = room.GetNodeByLayer("Environment");

            if (env != null)
            {
                foreach (Transform t in env.transform)
                {
                    if (t.tag == "RoomTile")
                    {
                        // Bit ugly but extracts the x,y component from the name of the tile.
                        int x = Int32.Parse(t.name[5].ToString());
                        int y = Int32.Parse(t.name[8].ToString());
                        roomProperties.Tiles[x, y].GameObject = t.gameObject;
                       
                        foreach (Transform child in t)
                        {
                            EnvIdentifier id = child.GetComponent<EnvIdentifier>();
                            if (id != null)
                            {
                                TileAttribute att = new TileAttribute();
                                att.Type = id.TileAttributeType;
                                att.Angle = child.eulerAngles.y;
                                roomProperties.Tiles[x, y].TileAttributes.Add(att);

                                if (att.Type == TileType.door)
                                {

                                }
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

        private void TilePlacementGUI()
        {
            tileType = (TileType)EditorGUILayout.EnumPopup("Choose Environment Piece", (Enum)tileType);

            if (GUILayout.Button("Insert at tile", GUILayout.Width(buttonSize)))
            {
                GameObject go = roomGen.GetGameObjectByType(tileType);

                if (go != null)
                {
                    go.transform.parent = selectedTile.transform;
                    go.transform.position = selectedTile.transform.position;
                }
            }
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

        private void UpdateSelectedTile()
        {
            GameObject go = Selection.activeGameObject;

            if (go != null && go != selectedTile)
            {
                Transform T = go.transform;

                while (T.parent != null && T.tag != "RoomTile")
                {
                    T = T.parent;
                }

                if (T.tag != "RoomTile")
                {
                    selectedTile = null;
                }
                else
                {
                    selectedTile = T.gameObject;
                }
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
