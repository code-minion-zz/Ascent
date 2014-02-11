using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    public bool[] directionsFilled;
    private Vector3 position;
    private float width;
    private float height;
    private int numberOfTilesX;
    private int numberOfTilesY;
    private bool wallsPlaced;
    private bool isPreloaded;

    private Room room;
    private FeatureType roomType;
    private int weight;
    private TileProperties[,] tiles;

    public const int tileSize = 2;

    /// <summary>
    /// Gets the bounds of the room.
    /// </summary>
    public Bounds Bounds
    {
        get
        {
            return new Bounds(position, new Vector3(width, 1.0f, height));
        }
    }

    /// <summary>
    /// Gets the tiles of the room. Each tile represents 2x2 world space units.
    /// </summary>
    public TileProperties[,] RoomTiles
    {
        get
        {
            if (tiles == null)
            {
                tiles = new TileProperties[(int)width, (int)height];
                return tiles;
            }
            else
            {
                return tiles;
            }
        }
    }
    
    public int Weight
    {
        get { return weight; }
        set { weight = value; }
    }

    public Vector3 Position
    {
        get { return position; }
        set { position = value; }
    }

    public float Width
    {
        get { return width; }
        set { width = value; }
    }

    public float Height
    {
        get { return height; }
        set { height = value; }
    }

    public int NumberOfTilesX
    {
        get { return numberOfTilesX; }
    }

    public int NumberOfTilesY
    {
        get { return numberOfTilesY; }
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

    public int TileSize
    {
        get { return tileSize; }
    }

    public Room Room
    {
        get { return room; }
        set { room = value; }
    }

    public FeatureType RoomType
    {
        get { return roomType; }
        set { roomType = value; }
    }

    public RoomProperties(Room room)
    {
        position = Vector3.zero;
        directionsFilled = new bool[4];
        this.room = room;
        wallsPlaced = false;

        for (int i = 0; i < 4; ++i)
        {
            directionsFilled[i] = false;
        }
    }

	/// <summary>
	/// Sets and initializes the room tiles.
	/// </summary>
	/// <param name="width">Width.</param>
	/// <param name="height">Height.</param>
	/// 
    public void SetRoomTiles(int width, int height)
    {
        this.width = (width * TileSize);
        this.height = (height * TileSize);

        numberOfTilesX = width;
        numberOfTilesY = height;

        tiles = new TileProperties[width, height];

        for (int i = 0; i < numberOfTilesX; ++i)
        {
            for (int j = 0; j < numberOfTilesY; ++j)
            {
                // Create and assign the position of the tile.
                tiles[i, j] = new TileProperties();
                float xPos = -(width) + (tileSize * 0.5f) + (i * tileSize);
                float zPos = -(height) + (tileSize * 0.5f) + (j * tileSize);
                tiles[i, j].Position = new Vector3(xPos, 0.0f, zPos);
                tiles[i, j].TileType = TilePropertyType.none;
            }
        }
    }

    /// <summary>
    /// Eventually this function will allow for loading a custom room from a file.
    /// </summary>
    public void LoadFromFile(string filename)
    {
#pragma warning disable 0219 // Disable unassigned field warning. Remove once filePath is used.
        string filePath = "Resources/Level/Rooms/" + filename;
    }

    public void FillDirection(Floor.TransitionDirection direction)
    {
        switch (direction)
        {
            case Floor.TransitionDirection.North:
                directionsFilled[0] = true;
                break;

            case Floor.TransitionDirection.East:
                directionsFilled[1] = true;
                break;

            case Floor.TransitionDirection.South:
                directionsFilled[2] = true;
                break;

            case Floor.TransitionDirection.West:
                directionsFilled[3] = true;
                break;
        }
    }
}
