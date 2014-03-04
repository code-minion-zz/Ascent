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
    private Vector3 position = Vector3.zero;
    [NonSerialized]
    private bool wallsPlaced = false;
    [NonSerialized]
    private bool isPreloaded = false;
    [NonSerialized]
    private Room room;
    [NonSerialized]
    private List<Door> doors = new List<Door>();

    // Tiles represent the grid of the room. Every tile has a list of objects it is holding.
    public Tile[,] Tiles { get; set; }

    public bool[] DirectionsFilled { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public string Name { get; set; }
    public FeatureType RoomType { get; set; }

    public const int TileSize = 2;

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
        DirectionsFilled = new bool[4];
    }

    public RoomProperties(Room room)
    {
        DirectionsFilled = new bool[4];
        this.room = room;
    }

    public void InitialiseTiles(int tilesX, int tilesY)
    {
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

                        if (att.Type == TileType.door)
                        {
                            DoorTile doorTile = att as DoorTile;
                            Door door = go.GetComponent<Door>();
                            if (doorTile != null)
                            {
                                door.direction = doorTile.Direction;
                                Doors.Add(door);
                            }
                            else
                            {
                                Debug.Log("For some reason the door tile attribute is null.");
                            }
                        }

                        if (att.Type == TileType.arrowShooter)
                        {
                            // We do not want to be able to place objects on top of an arrow shooter.
                            Tiles[x, y].IsOccupied = true;
                        }
                    }
                }
            }
        }

        room.NumberOfTilesX = Tiles.GetLength(0);
        room.NumberOfTilesY = Tiles.GetLength(1);

        // Apply the new dimensions to the navMesh.
        room.SetNavMeshDimensions(Width, Height);
        SetupCamera();

        IsConstructed = true;
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

    private void SetupCamera()
    {
        if (room == null)
        {
            Debug.LogError("Room object has not been created yet, cannot setup the camera range.");
            return;
        }

        float cameraOffsetX = 1.0f;
        float cameraOffsetMinZ = 1.0f;
        float cameraOffsetMaxZ = 1.0f;

        //if (Game.Singleton.IsWideScreen)
        //{
        //    switch (room.Width)
        //    {
        //        case 10: cameraOffsetX = 0.0f; break;
        //        case 14: cameraOffsetX = 0.0f; break;
        //        case 18: cameraOffsetX = 1.0f; break;
        //        case 22: cameraOffsetX = 3.0f; break;
        //        case 24: cameraOffsetX = 4.0f; break;

        //        default: Debug.LogError("Unhandled case: " + room.Width); break;
        //    }
        //}
        //else
        //{
        switch (Width)
        {
            case 10: cameraOffsetX = 0.0f; break;
            case 14: cameraOffsetX = 1.0f; break;
            case 18: cameraOffsetX = 3.0f; break;
            case 22: cameraOffsetX = 5.0f; break;
            case 24: cameraOffsetX = 4.0f; break;

            default: Debug.LogError("Unhandled case: " + Width); break;
        }

        //}

        switch (Height)
        {
            case 10: cameraOffsetMinZ = -5.0f; cameraOffsetMaxZ = -5.0f; break;
            case 14: cameraOffsetMinZ = -7.25f; cameraOffsetMaxZ = -2.25f; break;
            case 18: cameraOffsetMinZ = -9.25f; cameraOffsetMaxZ = -0.3f; break;
            case 22: cameraOffsetMinZ = -11.1f; cameraOffsetMaxZ = 1.8f; break;
            case 24: cameraOffsetMinZ = -13.1f; cameraOffsetMaxZ = 3.0f; break;

            default: Debug.LogError("Unhandled case: " + Height); break;
        }

        room.minCamera.x = -cameraOffsetX;
        room.maxCamera.x = cameraOffsetX;

        room.minCamera.z = cameraOffsetMinZ;
        room.maxCamera.z = cameraOffsetMaxZ;

        //room.Room.minCamera.x = (room.Width >= 14.0f) ? ((room.Width - 15.0f) * -cameraOffsetX) : 0.0f;
        //room.Room.maxCamera.x = (room.Width >= 14.0f) ? ((room.Width - 15.0f) * cameraOffsetX) : 0.0f;

        //room.Room.minCamera.x = Math.Abs(room.Room.minCamera.x) * -1.0f;
        //room.Room.maxCamera.x = Math.Abs(room.Room.minCamera.x);

        //room.Room.minCamera.z = (room.Height > 10.0f) ? ((-room.Height + (room.Height * 0.48f)) * cameraOffsetZ) : -5.0f;
        //room.Room.maxCamera.z = (room.Height > 10.0f) ? ((room.Height - (room.Height * 1.17f)) * cameraOffsetZ) : -5.0f;

        //room.Room.minCamera.z = Math.Max(room.Room.minCamera.z, -9.25f);
        //room.Room.maxCamera.z = Math.Min(room.Room.maxCamera.z, 0.0f);

        //room.Room.maxCamera.z = room.Room.maxCamera.x < room.Room.minCamera.z ? room.Room.minCamera.z : room.Room.maxCamera.z;
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
