using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum TilePropertyType
{
	none,
	wallTile,
    cornerWallTile,
	doorTile,
	miscObj,
    brazier,
	trap,
	monster,
	invisibleWall
}

public class TileProperties
{
	private TilePropertyType tileType;
	private Vector3 position;
    private bool isOccupied = false;

	public TilePropertyType TileType
	{
		get { return tileType; }
		set { tileType = value; }
	}

	public Vector3 Position
	{
		get { return position; }
		set { position = value; }
	}

    public bool IsOccupied
    {
        get { return isOccupied; }
        set { isOccupied = value; }
    }

	public TileProperties()
	{
	}
}

