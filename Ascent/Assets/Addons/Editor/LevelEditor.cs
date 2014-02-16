using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

namespace Ascent 
{
    [Serializable]
    public class LevelEditor : EditorWindow
	{
        private Grid grid;
        private Vector2 scrollPosition;
        private string selectedRoom;
        private bool objectFoldOut = false;
        private const int buttonSize = 255;

        private string roomName = "New Room";
        private GameObject objectToCreate;
        private GameObject roomToLoad;
        private GameObject currentRoom = null;


        private SaveRooms roomSaver = new SaveRooms();
        private int selectedIndex = 0;
        private List<RoomProperties> roomSaves = new List<RoomProperties>();
        private List<string> roomSaveNames = new List<string>();

        private RoomGeneration roomGen = new RoomGeneration();

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

        public void Start()
        {
            Initialize();
        }

        public void Awake()
        {
            Initialize();
        }

        void OnEnable()
        {
            SceneView.onSceneGUIDelegate = GridUpdate;
        }

        public void Initialize()
        {
            roomSaver = new SaveRooms();
            roomSaver.Initialize();
            roomSaver.LoadRooms();
            roomSaves = roomSaver.RoomSaves.saves;

            roomGen = new RoomGeneration();
        }

        void Update()
        {
            UpdateActiveObject();

            this.Repaint();
        }

        void GridUpdate(SceneView sceneView)
        {
            if (currentRoom == null || grid == null)
            {
                return;
            }

            UpdateActiveObject();

            Event e = Event.current;
            Ray r = Camera.current.ScreenPointToRay(new Vector3(e.mousePosition.x, -e.mousePosition.y + Camera.current.pixelHeight));
            Vector3 mousePos = r.origin;

            if (e.isKey && e.character == 'a')
            {
                if (objectToCreate != null)
                {
                    //Undo.IncrementCurrentEventIndex();
                    // Get the object to create from our grid tool
                    GameObject obj = PrefabUtility.InstantiatePrefab(objectToCreate) as GameObject;

                    // Position it on the grid
                    Vector3 aligned = new Vector3(Mathf.Floor(mousePos.x / grid.width) * grid.width + grid.width / 2.0f,
                                                    obj.transform.position.y,
                                                    Mathf.Floor(mousePos.z / grid.length) * grid.length + grid.length / 2.0f);
                    obj.transform.position = aligned;

                    // Setup the parent object.
                    if (currentRoom != null)
                    {
                        GameObject go = FindParentByLayer(obj.layer);

                        if (go == null)
                        {
                            // We need to add this layer
                            Room room = currentRoom.GetComponent<Room>();
                            go = room.AddNewParentCategory(LayerMask.LayerToName(obj.layer), obj.layer);
                        }

                        obj.transform.parent = go.transform;
                    }

                    Undo.RegisterCreatedObjectUndo(obj, "Create " + obj.name);
                }
                else
                {
                    Debug.Log("Please select an object to place first. To select an object open the grid properties");
                }
            }
        }

        void OnGUI()
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            roomName = EditorGUILayout.TextField("Room Name", roomName);

            // Assign selected game object prefab
            if (GUILayout.Button("Create New Room", GUILayout.Width(buttonSize)))
            {
                CreateNewRoom();
            }

            SelectRoomGUI();

            if (currentRoom != null)
            {
                EditorGUILayout.BeginVertical();

                objectToCreate = EditorGUILayout.ObjectField("Object to place",
                    objectToCreate, typeof(GameObject), true) as GameObject;

                EditorGUILayout.EndVertical();
                EditorGUILayout.Separator();

                GUILayout.Label("Selected Room: " + selectedRoom);

                objectFoldOut = EditorGUILayout.Foldout(objectFoldOut, "Room Options");

                if (objectFoldOut == true)
                {
                    if (GUILayout.Button("Fix Room Nodes", GUILayout.Width(buttonSize)))
                    {
                        Room room = currentRoom.GetComponent<Room>();

                        if (room == null)
                        {
                            Debug.Log("Failed to load Room: Please make sure the Room Script is attached to the room");
                        }
                        else
                        {
                            room.FixTreeStructure();
                        }
                    }

                    if (GUILayout.Button("Group Selected", GUILayout.Width(buttonSize)))
                    {
                        GroupObjects(Selection.gameObjects);
                    }
                }
            }

            EditorGUILayout.EndScrollView();
        }

        #region GUI Helper Functions

        private void SelectRoomGUI()
        {
            selectedIndex = EditorGUILayout.Popup("Select a Room", selectedIndex, roomSaveNames.ToArray(), GUILayout.Width(buttonSize));

            if (GUILayout.Button("Load Rooms", GUILayout.Width(buttonSize)))
            {
                roomSaveNames.Clear();
                roomSaver.Initialize();
                roomSaver.LoadRooms();
                roomSaves = roomSaver.RoomSaves.saves;

                int index = 0;
                foreach (RoomProperties room in roomSaves)
                {
                    roomSaveNames.Add(index + "." + room.Name);
                    index++;
                }
            }

            //EditorUtility.OpenFilePanel(
            if (GUILayout.Button("Load Into Grid", GUILayout.Width(buttonSize)))
            {
                roomGen.ReconstructRoom(roomSaves[selectedIndex]);
            }

            if (roomSaver != null)
            {
                if (GUILayout.Button("Save Room", GUILayout.Width(buttonSize)))
                {
                    roomSaver.SaveAllRooms();
                }

                if (GUILayout.Button("Delete Room", GUILayout.Width(buttonSize)))
                {
                    roomSaveNames.Remove(roomSaveNames[selectedIndex]);
                    roomSaver.RoomSaves.saves.Remove(roomSaver.RoomSaves.saves[selectedIndex]);
                }
            }
        }

        private void SaveCurrentRoom()
        {
            if (currentRoom != null)
            {
                // Create the prefab at this location with the name of the parent.
                UnityEngine.Object prefab = PrefabUtility.CreatePrefab("Assets/Addons/Editor/Prefabs/" + currentRoom.name + ".prefab", currentRoom, ReplacePrefabOptions.ConnectToPrefab);
                Debug.Log("Creating prefab at path (Assets/Addons/Editor/Prefabs/" + currentRoom.name + ".prefab)");

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
            //roomGen.CreateNewRoom(
        }

        private void LoadRoomGUI()
        {
            roomToLoad = EditorGUILayout.ObjectField("Load Room",
                roomToLoad, typeof(GameObject), true) as GameObject;

            if (roomToLoad != null)
            {
                if (GUILayout.Button("Insert into grid", GUILayout.Width(buttonSize)))
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
        public GameObject FindParentByLayer(int layer)
        {
            GameObject go = null;

            foreach (Transform node in currentRoom.transform)
            {
                if (node.gameObject.layer == layer)
                {
                    go = node.gameObject;
                }
            }

            return go;
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

                while (T.parent != null && T.tag != "RoomRoot")
                {
                    T = T.parent;
                }

                currentRoom = T.gameObject;
                selectedRoom = currentRoom.name;

                // Update to the new grid
                Grid otherGrid = currentRoom.GetComponent<Grid>();

                // Since we found the grid we actually want to get the window to it's properties.
                if (otherGrid != null && otherGrid != grid)
                {
                    grid = otherGrid;
                    GridProperties gridProperties = EditorWindow.GetWindow<GridProperties>("Grid Properties");
                    gridProperties.Init(grid);
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
