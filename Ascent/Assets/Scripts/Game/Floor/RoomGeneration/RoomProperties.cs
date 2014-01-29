using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum FeatureType
{
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
    private bool wallsPlaced;

    private Room room;
    private FeatureType roomType;
    private int weight;

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

    public bool WallsPlaced
    {
        get { return wallsPlaced; }
        set { wallsPlaced = value; }
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
