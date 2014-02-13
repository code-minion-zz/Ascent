using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum TileType
{
    none = 0,
    groundTile = 1,
    door = 2,
    standardWall = 3,
    northWestCorner = 4,
    northEastCorner = 5,
    southEastCorner = 6,
    southWestCorner = 7,
    cornerWallTile,
    chest,
    lockedDoor,
    miscObj,
    brazier,
    trap,
    monster,
    invalid
}

[Serializable]
public class Tile
{
    [NonSerialized]
    private Vector3 position;
    // TODO: Get rid of this as this tile is just a place holder.
    private bool isOccupied;

    // Serialized fields.
    public float PosX { get; set; }
    public float PosY { get; set; }
    public float PosZ { get; set; }

    public Directions FacingDirection { get; set; }
    

    // TODO: Get rid of this as this tile is just a place holder.
    public TileType TileType { get; set; }

    // These attributes could be any objects in the game.
    public List<TileAttribute> TileAttributes { get; set; }

    // Non Serialized
    public Vector3 Position
    {
        get { return new Vector3(PosX, PosY, PosZ); }
        set 
        {
            position = value; 
            PosX = position.x;
            PosY = position.y;
            PosZ = position.z;
        }
    }

    public bool IsOccupied
    {
        get { return isOccupied; }
        set { isOccupied = value; }
    }

    public bool ContainsAttribute(TileType type)
    {
        foreach (TileAttribute att in TileAttributes)
        {
            if (att.Type == type)
            {
                return true;
            }
        }

        return false;
    }
}