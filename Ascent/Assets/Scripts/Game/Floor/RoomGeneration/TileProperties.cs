using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum TilePropertyType
{
	none,
	wallTile,
	doorTile,
	miscObj,
	trap,
	monster,
	invisibleWall
}

public class TileProperties
{
	private TilePropertyType tileType;
	private Vector3 position;

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

	public TileProperties()
	{
	}
}

