using UnityEngine;
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

[Serializable]
public class RoomProperties
{
    [NonSerialized]
    private Vector3 position;
    [NonSerialized]
    private bool wallsPlaced;
    [NonSerialized]
    private bool isPreloaded;
    [NonSerialized]
    private Room room;
    [NonSerialized]
    private List<Door> doors = new List<Door>();

    // Tiles represent the grid of the room. Every tile has a list of objects it is holding.
    public Tile[,] Tiles { get; set; }

    public bool[] DirectionsFilled { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public int NumberOfTilesX { get; set; }
    public int NumberOfTilesY { get; set; }
    public string Name { get; set; }
    public FeatureType RoomType { get; set; }

    public const int TileSize = 2;

    public Vector3 Position
    {
        get { return position; }
        set { position = value; }
    }

    public bool WallsPlaced
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
        DirectionsFilled = new bool[4];
    }

    public RoomProperties(Room room)
    {
        position = Vector3.zero;
        DirectionsFilled = new bool[4];
        this.room = room;
        wallsPlaced = false;

        for (int i = 0; i < 4; ++i)
        {
            DirectionsFilled[i] = false;
        }
    }

    public void InitialiseTiles(int tilesX, int tilesY)
    {
        Width = (tilesX * TileSize);
        Height = (tilesY * TileSize);

        NumberOfTilesX = tilesX;
        NumberOfTilesY = tilesY;

        Tiles = new Tile[tilesX, tilesY];

        for (int i = 0; i < NumberOfTilesX; ++i)
        {
            for (int j = 0; j < NumberOfTilesY; ++j)
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
                att.Type = TileType.none;
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

        Transform envNode = room.GetNodeByLayer("Environment").transform;

        for (int x = 0; x < Tiles.GetLength(0); ++x)
        {
            for (int y = 0; y < Tiles.GetLength(1); ++y)
            {
                // Create the parent tile.
                CreateTileNodeObject(x, y, envNode);
            }
        }
    }

    private Room CreateRoomObject(string name)
    {
        GameObject roomGo = new GameObject(name);
        Room room = roomGo.AddComponent<Room>();

        // Add necessary nodes.
        room.tag = "RoomRoot";
        GameObject envGo = room.AddNewParentCategory("Environment", LayerMask.NameToLayer("Environment"));
        GameObject doorGo = room.AddSubParent("Doors", envGo, LayerMask.NameToLayer("Environment")) as GameObject;
        doorGo.AddComponent<Doors>();
        room.AddSubParent("Walls", envGo, LayerMask.NameToLayer("Environment"));

        room.AddNewParentCategory("Monsters", (int)Layer.Monster);
        room.AddNewParentCategory("Items", (int)Layer.Item);
        room.AddNewParentCategory("Lights", (int)Layer.Default);

        room.Initialise();

        return room;
    }

    /// <summary>
    /// Creates the parent node transform for this tile.
    /// </summary>
    /// <param name="tile"></param>
    private GameObject CreateTileNodeObject(int x, int y, Transform parent)
    {
        GameObject go = new GameObject();
        go.transform.parent = parent;
        go.transform.localPosition = Tiles[x, y].Position;
        go.name = "Tile[" + x + ", " + y + "]";
        go.tag = "RoomTile";
        Tiles[x, y].GameObject = go;

        return go;
    }

    public void FillDirection(Floor.TransitionDirection direction)
    {
        switch (direction)
        {
            case Floor.TransitionDirection.North:
                DirectionsFilled[0] = true;
                break;

            case Floor.TransitionDirection.East:
                DirectionsFilled[1] = true;
                break;

            case Floor.TransitionDirection.South:
                DirectionsFilled[2] = true;
                break;

            case Floor.TransitionDirection.West:
                DirectionsFilled[3] = true;
                break;
        }
    }
}
