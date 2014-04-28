using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

public enum FeatureType
{
    none,
    monster,
    trap,
    treasure,
    boss
}

public class RoomProperties
{
    private Vector3 position = Vector3.zero;
    private bool wallsPlaced = false;
    private bool isPreloaded = false;
    private Room room;
    private List<Door> doors = new List<Door>();

    // Tiles represent the grid of the room. Every tile has a list of objects it is holding.
    public Tile[,] Tiles { get; set; }

    public int Width { get; set; }
    public int Height { get; set; }
    public string Name { get; set; }
    public FeatureType RoomType { get; set; }

    private int tileSize = 5;

    public int TileSize
    {
        get { return tileSize; }
    }

    public Vector3 Position
    {
        get { return position; }
        set { position = value; }
    }

    public bool IsConstructed
    {
        get { return wallsPlaced; }
        set { wallsPlaced = value; }
    }

    public bool IsPreloaded
    {
        get { return isPreloaded; }
        set { isPreloaded = value; }
    }

    /// <summary>
    /// Gets the bounds of the room.
    /// </summary>
    public Bounds Bounds
    {
        get
        {
            return new Bounds(position, new Vector3(Width, 1.0f, Height));
        }
    }

    public Room Room
    {
        get { return room; }
        set { room = value; }
    }

    public List<Door> Doors
    {
        get { return doors; }
        set { doors = value; }
    }

    public RoomProperties()
    {
    }

    public RoomProperties(Room room)
    {
        this.room = room;
    }

    public void InitialiseTiles(int tilesX, int tilesY, int tileSize)
    {
        this.tileSize = tileSize;

        Width = (tilesX * TileSize);
        Height = (tilesY * TileSize);

        Tiles = new Tile[tilesX, tilesY];

        for (int i = 0; i < Tiles.GetLength(0); ++i)
        {
            for (int j = 0; j < Tiles.GetLength(1); ++j)
            {
                // Create and assign the position of the tile.
                Tiles[i, j] = new Tile();
                float xPos = -(tilesX) + (TileSize * 0.5f) + (i * TileSize);
                float zPos = -(tilesY) + (TileSize * 0.5f) + (j * TileSize);

                Tiles[i, j].Position = new Vector3(xPos, 0.0f, zPos);

                // Assign each tile with a list of attributes.
                Tiles[i, j].TileAttributes = new List<TileAttribute>();
                // Add a default none tile attribute.
                TileAttribute att = new TileAttribute();
                att.Angle = 0.0f;
                att.Type = EnvironmentID.none;
                Tiles[i, j].TileAttributes.Add(att);
            }
        }
    }

    /// <summary>
    /// Construct the room from the data.
    /// </summary>
    public void ConstructRoom()
    {
        doors = new List<Door>();
        room = CreateRoomObject(Name);

        if (room == null)
        {
            Debug.LogError("The room object has not been created or found, Cannot construct room.");
            return;
        }

        Transform envNode = room.transform.FindChild("Environment");
        Transform tilesNode = envNode.FindChild("Tiles");

        for (int x = 0; x < Tiles.GetLength(0); ++x)
        {
            for (int y = 0; y < Tiles.GetLength(1); ++y)
            {
                // Create the parent tile.
                Tiles[x, y].GameObject = CreateTileNodeObject(x, y, tilesNode);

                // Go through and create / hook up the tile objects.
                foreach (TileAttribute att in Tiles[x, y].TileAttributes)
                {
                    // Place each attribute on the tile position.
                    GameObject go = EnvironmentFactory.CreateGameObjectByType(att.Type);

                    if (go != null)
                    {
                        go.transform.parent = Tiles[x, y].GameObject.transform;
                        go.transform.position = Tiles[x, y].Position;
                        go.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), att.Angle);
                        go.name = "[" + x + ", " + y + "]" + go.name;

                        if (att.Type == EnvironmentID.door)
                        {
                            DoorTile doorTile = att as DoorTile;
                            Door door = go.GetComponent<Door>();
                            if (doorTile != null)
                            {
                                Doors.Add(door);
                            }
                            else
                            {
                                Debug.Log("For some reason the door tile attribute is null.");
                            }
                        }

                        if (att.Type == EnvironmentID.arrowShooter)
                        {
                            // We do not want to be able to place objects on top of an arrow shooter.
                            Tiles[x, y].IsOccupied = true;
                        }
                    }
                }
            }
        }

		#if UNITY_EDITOR
        EditorUtility.SetDirty(room);
        #endif

        room.Initialise();
        // Apply the new dimensions to the navMesh.
        room.SetNavMeshDimensions(Width, Height);

        IsConstructed = true;
    }

    private Room CreateRoomObject(string name)
    {
        #if UNITY_EDITOR
        GameObject roomGo = PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Environment/Room/Room")) as GameObject;
        #else
        GameObject roomGo = GameObject.Instantiate(Resources.Load("Prefabs/Environment/Room/Room")) as GameObject;
        #endif
        roomGo.name = name;
        Room room = roomGo.GetComponent<Room>();
        room.Initialise();

        return room;
    }

    /// <summary>
    /// Creates the parent node transform for this tile.
    /// </summary>
    /// <param name="tile"></param>
    private GameObject CreateTileNodeObject(int x, int y, Transform parent)
    {
        #if UNITY_EDITOR
        GameObject go = PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Environment/Room/Tile")) as GameObject;
        #else
        GameObject go = GameObject.Instantiate(Resources.Load("Prefabs/Environment/Room/Tile")) as GameObject;
        #endif
        go.transform.parent = parent;
        go.transform.localPosition = Tiles[x, y].Position;
        go.name = "Tile[" + x + ", " + y + "]";

        return go;
    }
}