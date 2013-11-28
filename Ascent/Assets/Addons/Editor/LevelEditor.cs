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
        static int meshIndex = 0;

        public GameObject objectToCreate;
        public GameObject roomToLoad;
        public GameObject currentRoom = null;
        public string roomName = "New Room";

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

        public void Init(Grid grid)
        {
            this.grid = grid;
        }

        void OnEnable()
        {
            SceneView.onSceneGUIDelegate = GridUpdate;
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
                if (currentRoom != null)
                {
                    grid = currentRoom.GetComponent<Grid>();
                }
                else
                {
                    return;
                }
            }

            UpdateActiveObject();

            Event e = Event.current;
            Ray r = Camera.current.ScreenPointToRay(new Vector3(e.mousePosition.x, -e.mousePosition.y + Camera.current.pixelHeight));
            Vector3 mousePos = r.origin;

            // TODO: Make this instantiated prefab connect to the right category based on 
            // its layer.
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

        void OnGUI()
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            roomName = EditorGUILayout.TextField("Room Name", roomName);

            // Assign selected game object prefab
            if (GUILayout.Button("Create New Room", GUILayout.Width(255)))
            {
                // Don't want to save the room just yet because its creating a prefab
                // this prefab is being nested.
                //SaveCurrentRoom();
                //DeleteCurrentRoom();
                CreateNewRoom();
            }

            if (currentRoom != null)
            {
                EditorGUILayout.BeginVertical();

                objectToCreate = EditorGUILayout.ObjectField("Object to place",
                    objectToCreate, typeof(GameObject), true) as GameObject;

                EditorGUILayout.EndVertical();
                EditorGUILayout.Separator();

                //if (GUILayout.Button("Save Room Layout", GUILayout.Width(255)) && currentRoom != null)
                //{
                //    // Since we have a room to save lets go ahead and save it.
                //    SaveCurrentRoom();
                //}

                GUILayout.Label("Selected Room: " + selectedRoom);

                if (GUILayout.Button("Fix Room Nodes", GUILayout.Width(255)))
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

                if (GUILayout.Button("Combine Selected Meshs", GUILayout.Width(255)))
                {
                    CombineMeshObjects(Selection.activeGameObject);
                }

                if (GUILayout.Button("Group Selected", GUILayout.Width(255)))
                {
                    GameObject go = new GameObject();

                    foreach (GameObject selected in Selection.gameObjects)
                    {
                        go.transform.parent = selected.transform.parent;
                        selected.transform.parent = go.transform;
                        go.layer = selected.layer;
                        go.name = "Group: " + selected.name;
                        go.tag = selected.tag;
                    }
                }
            }

            //LoadRoomGUI();

            EditorGUILayout.EndScrollView();
        }

        #region GUI Helper Functions

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
            currentRoom = new GameObject(roomName);
            Room room = currentRoom.AddComponent<Room>();

            if (room.GetComponent<Grid>() == null)
            {
                grid = room.gameObject.AddComponent<Grid>();
            }

            room.tag = "RoomRoot";
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
            }
        }

        public void CombineMeshObjects(GameObject group)
        {
            MeshFilter[] meshFilters = group.GetComponentsInChildren<MeshFilter>();

            MeshFilter meshF = group.GetComponent<MeshFilter>();
            MeshRenderer meshR = group.GetComponent<MeshRenderer>();

            if (meshF == null)
                meshF = group.AddComponent<MeshFilter>();

            if (meshR == null)
                meshR = group.AddComponent<MeshRenderer>();

            meshF.mesh = CombineMeshObject(meshFilters);
        }

        public Mesh CombineMeshObject(MeshFilter[] meshes)
        {
            Mesh mesh = new Mesh();

            CombineInstance[] combineMeshes = new CombineInstance[meshes.Length];
            int count = 0;
            foreach (MeshFilter mF in meshes)
            {
                combineMeshes[count].mesh = mF.sharedMesh;
                combineMeshes[count].transform = mF.transform.localToWorldMatrix;
                mF.gameObject.SetActive(false);
                count++;
            }

            mesh.CombineMeshes(combineMeshes, true);
            mesh.Optimize();
            mesh.name = "CombinedMesh_" + meshIndex;

            AssetDatabase.CreateAsset(mesh, "Assets/Addons/LevelEditor/Meshes/" + mesh.name + ".asset");
            AssetDatabase.SaveAssets();

            meshIndex++;

            return mesh;
        }

        //public Mesh CombineMeshObject(GameObject[] meshes)
        //{
        //    Mesh mesh = new Mesh();
 
        //    CombineInstance[] combineMeshes = new CombineInstance[meshes.Length];
        //    Debug.Log(meshes.Length);
        //    int count = 0;
        //    foreach (GameObject c in meshes)
        //    {
        //        MeshFilter mF = c.GetComponent<MeshFilter>();
        //        combineMeshes[count].mesh = mF.sharedMesh;
        //        combineMeshes[count].transform = c.transform.localToWorldMatrix;
        //        mF.gameObject.SetActive(false);
        //        count++;
        //    }

        //    mesh.CombineMeshes(combineMeshes);
        //    mesh.Optimize();
        //    mesh.name = "CombinedMesh_" + meshIndex;

        //    AssetDatabase.CreateAsset(mesh, "Assets/Addons/LevelEditor/Meshes/" + mesh.name + ".asset");
        //    AssetDatabase.SaveAssets();

        //    meshIndex++;

        //    return mesh;
        //}

        //public void CombineMeshObject(GameObject[] meshes)
        //{
        //    MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        //    CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        //    int i = 0;
        //    while (i < meshFilters.Length) {
        //        combine[i].mesh = meshFilters[i].sharedMesh;
        //        combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
        //        meshFilters[i].gameObject.active = false;
        //        i++;
        //    }
        //    transform.GetComponent<MeshFilter>().mesh = new Mesh();
        //    transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        //    transform.gameObject.active = true;
        //}
    //}

        #endregion
    }
}
