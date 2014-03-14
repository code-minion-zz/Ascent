using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

public enum RoomConnectionType
{
    Right,
    Left,
    Plus,
    Empty,
    Straight,
    BothSides,
    LeftUp,
    RightUp
}

/// <summary>
/// Handles generation of a room for use in the floor generation.
/// </summary>
/// 
public class RoomGeneration 
{
    private Rarity miscObjects = Rarity.few;


    /// <summary>
    /// Creates a new room and intializes variables.
    /// TODO: This will be swapped out for the new createroom function.
    /// </summary>
    /// <returns>The new room.</returns>
    /// <param name="width">Width.</param>
    /// <param name="height">Height.</param>
    /// <param name="name">Name.</param>
    public RoomProperties ConstructNewRoom(int width, int height, string name)
	{
		// Initialise and construct the new room.
		RoomProperties newRoom = new RoomProperties();
        newRoom.Name = name;
        newRoom.InitialiseTiles((int)(width * 0.5f), (int)(height * 0.5f), 2);


        newRoom.ConstructRoom();
		PlaceGroundTiles(newRoom);

        newRoom.IsConstructed = false;

		return newRoom;
	}

    /// <summary>
    /// Creates the data structure for a new room. Which can be used to reconstruct a room.
    /// </summary>
    /// <param name="shape"></param>
    /// <param name="tilesX"></param>
    /// <param name="tilesY"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public RoomProperties CreateNewRoom(RoomConnectionType shape, int tilesX, int tilesY, int tileSize)
    {
        RoomProperties newRoom = new RoomProperties();
        newRoom.InitialiseTiles(tilesX, tilesY, tileSize);

        return newRoom;
    }

    public void PopulateMonsters(int dungeonLevel, RoomProperties room, Rarity rarity)
    {
        List<Tile> tempAvailablePosition = new List<Tile>();

        // Find all the available positions that a misc object can be placed.
        for (int i = 0; i < room.Tiles.GetLength(0); ++i)
        {
            for (int j = 0; j < room.Tiles.GetLength(1); ++j)
            {
                // Search for tiles that are available.
                if (room.Tiles[i, j].ContainsAttribute(EnvironmentID.monster) || 
                    room.Tiles[i, j].ContainsAttribute(EnvironmentID.none) &&
                    room.Tiles[i, j].ContainsAttribute(EnvironmentID.groundTile))
                {
                    tempAvailablePosition.Add(room.Tiles[i, j]);
                }
            }
        }

        // Generate number of monsters.
        // TODO: make this better haha.
        int numberOfMonsters = (int)rarity * UnityEngine.Random.Range(1, 5);
		numberOfMonsters = Mathf.Clamp(numberOfMonsters, 1, 7);

        int monstersPlaced = 0;

        for (monstersPlaced = 0; monstersPlaced < numberOfMonsters; ++monstersPlaced)
        {
            // If we have exausted all of our available positions we can finish.
            if (tempAvailablePosition.Count == 0)
                return;

            // Choose type of monster
            Room.EMonsterTypes mobType = (Room.EMonsterTypes)(UnityEngine.Random.Range(0, (int)Room.EMonsterTypes.MAX));

            GameObject go = null;

            switch (mobType)
            {
                case Room.EMonsterTypes.Rat:
                    go = room.Room.InstantiateGameObject(Room.ERoomObjects.Enemy, "Rat");
                    break;

                case Room.EMonsterTypes.Imp:
                    go = room.Room.InstantiateGameObject(Room.ERoomObjects.Enemy, "Imp");
                    break;

                case Room.EMonsterTypes.Slime:
                    go = room.Room.InstantiateGameObject(Room.ERoomObjects.Enemy, "Slime");
                    break;

                case Room.EMonsterTypes.EnchantedStatue:
                    go = room.Room.InstantiateGameObject(Room.ERoomObjects.Enemy, "EnchantedStatue");
					go.transform.rotation = Quaternion.LookRotation(Vector3.back);
                    break;

                    // These should be left for boss rooms.
                case Room.EMonsterTypes.Abomination:
                    //go = room.Room.InstantiateGameObject(Room.ERoomObjects.Enemy, "Abomination");
                    break;

                case Room.EMonsterTypes.Boss:
                    break;
            }

            if (go == null)
            {
                // We may not have created a monster.
                monstersPlaced--;
                continue;
            }

            // Give the monster a random position.
            // Choose a random tile.
            int randomTile = UnityEngine.Random.Range(0, tempAvailablePosition.Count);
            go.transform.localPosition = tempAvailablePosition[randomTile].Position;
            go.transform.parent = room.Room.MonsterParent;

            float angle = 0.0f;

            // Add the attribute of this object.
            TileAttribute att = new TileAttribute();
            att.Angle = angle;
            att.Type = EnvironmentID.monster;

            // Apply configurations to the tile of this room and remove
            // the tile from our temp list so that a monster is not placed here again.
            tempAvailablePosition[randomTile].TileAttributes.Add(att);
            tempAvailablePosition[randomTile].IsOccupied = true;

            tempAvailablePosition.Remove(tempAvailablePosition[randomTile]);
        }
    }

    public void PopulateBossRoom(int dungeonLevel, RoomProperties room)
    {
        GameObject go = room.Room.InstantiateGameObject(Room.ERoomObjects.Enemy, "Abomination");
        int centreX = (int)(room.Tiles.GetLength(0) * 0.7f);
        int centreY = (int)(room.Tiles.GetLength(1) * 0.7f);

        TileAttribute att = new TileAttribute();
        att.Angle = 0.0f;
        att.Type = EnvironmentID.monster;

        room.Tiles[centreX, centreY].TileAttributes.Add(att);
        room.Tiles[centreX, centreY].IsOccupied = true;
        go.transform.localPosition = room.Tiles[centreX, centreY].Position;
        go.transform.parent = room.Room.MonsterParent;
		go.transform.position =  new Vector3(go.transform.position.x, 0.1f, go.transform.position.z);


		Game.Singleton.Tower.CurrentFloor.floorBoss = go.GetComponent<Enemy>();
    }

	/// <summary>
	/// Populates the the room with misc objects.
	/// </summary>
	/// <param name="room">Room.</param>
	public void PopulateMiscObjects(RoomProperties room)
	{
        // Since the room is preloaded we don't need to randomly place things.
        //if (room.IsPreloaded == true)
        //{
        //    return;
        //}

        List<Tile> tempAvailableTiles = new List<Tile>();

        // Find all the available positions that a misc object can be placed.
		for (int i = 0; i < room.Tiles.GetLength(0); ++i)
		{
			for (int j = 0; j < room.Tiles.GetLength(1); ++j)
			{
                // Populate random misc objects.
                if (room.Tiles[i, j].ContainsAttribute(EnvironmentID.randMisc) ||
                    room.Tiles[i, j].ContainsAttribute(EnvironmentID.standardWall) ||
                    room.Tiles[i, j].ContainsAttribute(EnvironmentID.cornerWallTile) ||
                    room.Tiles[i, j].ContainsAttribute(EnvironmentID.brazier))
                {
                    // There must be a ground tile here otherwise it does not make sense.
                    if (room.Tiles[i, j].IsOccupied == false && room.Tiles[i, j].ContainsAttribute(EnvironmentID.groundTile))
                    {
                        tempAvailableTiles.Add(room.Tiles[i, j]);
                    }
                }
			}
		}

        GameObject go = null;

        // For each corner place the braziers.
        foreach (Tile tile in tempAvailableTiles)
        { 
            if (tile.ContainsAttribute(EnvironmentID.cornerWallTile))
            {
                go = EnvironmentFactory.CreateGameObjectByType(EnvironmentID.brazier);
                go.transform.parent = room.Room.EnvironmentParent;
                go.transform.localPosition = tile.Position;

                tile.IsOccupied = true;
            }
        }

        // Determine how many barrel objects we will try to place.
        int numberOfMisc = (int)miscObjects * UnityEngine.Random.Range(1, 4);
        int miscPlaced = 0;

        for (miscPlaced = 0; miscPlaced < numberOfMisc; ++miscPlaced)
        {
            // Reset as we do not want to accidentally place more braziers.
            go = null;
            float angle = 0.0f;

            // If we have exausted all of our available positions we can finish.
            if (tempAvailableTiles.Count == 0)
                return;

            // Choose a random tile, and a random misc object.
            int randomTile = UnityEngine.Random.Range(0, tempAvailableTiles.Count);

            // Check to see if the tile type is a wall tile to be sure.
            if (tempAvailableTiles[randomTile].ContainsAttribute(EnvironmentID.standardWall))
            {
                int random = UnityEngine.Random.Range(0, 2);

                switch (random)
                {
                    case 0:
                        go = EnvironmentFactory.CreateMiscObject(MiscObjectType.barrel);
                        break;

                    case 1:
                        float rotationY = UnityEngine.Random.Range(0.0f, 270.0f);
                        angle = rotationY;
                        go = EnvironmentFactory.CreateMiscObject(MiscObjectType.barrelCluster);
                        go.transform.eulerAngles = new Vector3(go.transform.eulerAngles.x, rotationY, go.transform.eulerAngles.z);
                        break;
                }
            }

            if (go == null)
            {
                // We may not have created an object.
                continue;
            }

            go.transform.parent = room.Room.EnvironmentParent;
            go.transform.localPosition = tempAvailableTiles[randomTile].Position;

            // Add the attribute of this object.
            TileAttribute att = new TileAttribute();
            att.Angle = angle;
            att.Type = EnvironmentID.randMisc;

            tempAvailableTiles[randomTile].TileAttributes.Add(att);
            tempAvailableTiles[randomTile].IsOccupied = true;
            tempAvailableTiles.Remove(tempAvailableTiles[randomTile]);
        }
	}

	public void PlaceGroundTiles(RoomProperties room)
	{
		// Create the floor tiles and positions.
		// where necessary.
		for (int x = 0; x < room.Tiles.GetLength(0); ++x)
		{
			for (int y = 0; y < room.Tiles.GetLength(1); ++y)
			{
                // Populate the whole room with a ground tile attribute.
                TileAttribute att = new TileAttribute();
                att.Type = EnvironmentID.groundTile;
                att.Angle = 0.0f;
                room.Tiles[x, y].TileAttributes.Add(att);

                // Create the ground tile.
                GameObject groundTile = EnvironmentFactory.CreateGameObjectByType(EnvironmentID.groundTile);
                groundTile.transform.parent = room.Tiles[x, y].GameObject.transform;
                groundTile.transform.position = room.Tiles[x, y].Position;
                groundTile.transform.localScale= new Vector3(room.TileSize * 0.5f, 1.0f, room.TileSize * 0.5f);
                groundTile.name = "GroundTile[" + x + ", " + y + "]";
                room.Tiles[x, y].GameObject = groundTile;
			}
		}
	}

    private Floor.TransitionDirection GetDirectionFromRot(float angle)
    {
        Floor.TransitionDirection dir = Floor.TransitionDirection.North;

        if (angle >= 0.0f && angle < 90.0f)
        {
            dir = Floor.TransitionDirection.West;
        }
        else if (angle >= 90.0f && angle < 180.0f)
        {
            dir = Floor.TransitionDirection.North;
        }
        else if (angle >= 180.0f && angle < 270.0f)
        {
            dir = Floor.TransitionDirection.East;
        }
        else if (angle >= 270.0f)
        {
            dir = Floor.TransitionDirection.South;
        }

        return dir;
    }

	/// <summary>
	/// Creates a door in the direction specified at the room specified.
	/// </summary>
	/// <returns>The door.</returns>
	/// <param name="doors">Doors.</param>
	/// <param name="fromRoom">From room.</param>
	/// <param name="direction">Direction.</param>
	public Door CreateDoor(GameObject doors, RoomProperties fromRoom, Floor.TransitionDirection direction)
	{
        GameObject doorGo = EnvironmentFactory.CreateGameObjectByType(EnvironmentID.door);
		doorGo.transform.parent = doors.transform;
		
		// Attach the doors to their rightful component.
		Doors doorsScript = doors.GetComponent<Doors>();
		
		doorsScript.RoomDoors[(int)direction] = doorGo.GetComponent<Door>();
		Door returnDoor = doorsScript.RoomDoors[(int)direction];

        int lastTileX = fromRoom.Tiles.GetLength(0)- 1;
		int lastTileY = fromRoom.Tiles.GetLength(1)-1;

        int midXTile = (int)(lastTileX * 0.5f);
        int midYTile = (int)(lastTileY * 0.5f);

        TileAttribute att;

		switch (direction)
		{
            case Floor.TransitionDirection.North:
                {
                    doorGo.transform.localPosition = fromRoom.Tiles[midXTile, lastTileY].Position;
                    doorGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 90.0f);
                    doorGo.name = "North Door";

                    att = new TileAttribute();
                    att.Angle = 90.0f;
                    att.Type = EnvironmentID.door;
                    fromRoom.Tiles[midXTile, lastTileY].TileAttributes.Add(att);
                    fromRoom.Tiles[midXTile, lastTileY].IsOccupied = true;
                }
                break;

            case Floor.TransitionDirection.East:
                {
                    doorGo.transform.localPosition = fromRoom.Tiles[lastTileX, midYTile].Position;
                    doorGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 180.0f);
                    doorGo.name = "East Door";

                    att = new TileAttribute();
                    att.Angle = 180.0f;
                    att.Type = EnvironmentID.door;
                    fromRoom.Tiles[lastTileX, midYTile].TileAttributes.Add(att);
                    fromRoom.Tiles[lastTileX, midYTile].IsOccupied = true;
                }
                break;

            case Floor.TransitionDirection.South:
                {
                    doorGo.transform.localPosition = fromRoom.Tiles[midXTile, 0].Position;
                    doorGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 270.0f);
                    doorGo.name = "South Door";

                    att = new TileAttribute();
                    att.Angle = 270.0f;
                    att.Type = EnvironmentID.door;
                    fromRoom.Tiles[midXTile, 0].TileAttributes.Add(att);
                    fromRoom.Tiles[midXTile, 0].IsOccupied = true;
                }
                break;

            case Floor.TransitionDirection.West:
                {
                    doorGo.transform.localPosition = fromRoom.Tiles[0, midYTile].Position;
                    doorGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 0.0f);
                    doorGo.name = "West Door";

                    att = new TileAttribute();
                    att.Angle = 0.0f;
                    att.Type = EnvironmentID.door;
                    fromRoom.Tiles[0, midYTile].TileAttributes.Add(att);
                    fromRoom.Tiles[0, midYTile].IsOccupied = true;
                }
                break;
		}
		
		return (returnDoor);
	}
	
	/// <summary>
	/// Place walls for the following room.
	/// </summary>
	/// <param name="room">Room.</param>
	public void PlaceWalls(RoomProperties room)
	{
		// Place wall corners
		GameObject walls = room.Room.GetNodeByLayer("Environment").transform.Find("Walls").gameObject;
		GameObject wallCornerGo = null;
		
		// North east corner
        // Assign the attribute.
        TileAttribute att = new TileAttribute();
        att.Type = EnvironmentID.cornerWallTile;
        att.Angle = 270.0f;
        room.Tiles[room.Tiles.GetLength(0) - 1, room.Tiles.GetLength(1) - 1].TileAttributes.Add(att);

        // Create the corner piece.
        wallCornerGo = EnvironmentFactory.CreateGameObjectByType(EnvironmentID.cornerWallTile);
        wallCornerGo.transform.parent = walls.transform;
        wallCornerGo.transform.localPosition = room.Tiles[room.Tiles.GetLength(0) - 1, room.Tiles.GetLength(1) - 1].Position;
		wallCornerGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 270.0f);
		wallCornerGo.name = "CornerNE";
		
		// South east corner
        // Assign the attribute.
        att = new TileAttribute();
        att.Type = EnvironmentID.cornerWallTile;
        att.Angle = 0.0f;
        room.Tiles[room.Tiles.GetLength(0) - 1, 0].TileAttributes.Add(att);

        wallCornerGo = EnvironmentFactory.CreateGameObjectByType(EnvironmentID.cornerWallTile);
        wallCornerGo.transform.parent = walls.transform;
        wallCornerGo.transform.localPosition = room.Tiles[room.Tiles.GetLength(0) - 1, 0].Position;
		wallCornerGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 0.0f);
		wallCornerGo.name = "CornerSE";
		
        // North west corner
        // Assign the attribute.
        att = new TileAttribute();
        att.Type = EnvironmentID.cornerWallTile;
        att.Angle = 180.0f;
        room.Tiles[0, room.Tiles.GetLength(1) - 1].TileAttributes.Add(att);

        wallCornerGo = EnvironmentFactory.CreateGameObjectByType(EnvironmentID.cornerWallTile);
        wallCornerGo.transform.parent = walls.transform;
        wallCornerGo.transform.localPosition = room.Tiles[0, room.Tiles.GetLength(1) - 1].Position;
		wallCornerGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 180.0f);
		wallCornerGo.name = "CornerNW";
		
		// South west corner
        // Assign the attribute.
        att = new TileAttribute();
        att.Type = EnvironmentID.cornerWallTile;
        att.Angle = 90.0f;
        room.Tiles[0, 0].TileAttributes.Add(att);

        wallCornerGo = EnvironmentFactory.CreateGameObjectByType(EnvironmentID.cornerWallTile);
        wallCornerGo.transform.parent = walls.transform;
        wallCornerGo.transform.localPosition = room.Tiles[0, 0].Position;
		wallCornerGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 90.0f);
		wallCornerGo.name = "CornerSW";
        
        // Variables used for placing walls.
        int lastTileX = room.Tiles.GetLength(0);
        int lastTileY = room.Tiles.GetLength(1);

        // Place north walls
		for (int i = 0; i < lastTileX; ++i)
		{
            // If there is no door or corner piece here.
            if (!room.Tiles[i, lastTileY-1].ContainsAttribute(EnvironmentID.door) &&
                !room.Tiles[i, lastTileY-1].ContainsAttribute(EnvironmentID.cornerWallTile))
            {
                GameObject wallGo = EnvironmentFactory.CreateGameObjectByType(EnvironmentID.standardWall);
                wallGo.transform.parent = walls.transform;
                wallGo.transform.localPosition = room.Tiles[i, lastTileY-1].Position;
                wallGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 90.0f);
                wallGo.name = "Wall";

                att = new TileAttribute();
                att.Type = EnvironmentID.standardWall;
                att.Angle = 90.0f;

                room.Tiles[i, lastTileY-1].TileAttributes.Add(att);
			}
		}
		
		// Place south walls
        for (int i = 0; i < lastTileX; ++i)
        {
            if (!room.Tiles[i, 0].ContainsAttribute(EnvironmentID.door) &&
                !room.Tiles[i, 0].ContainsAttribute(EnvironmentID.cornerWallTile))
            {
                GameObject wallGo = EnvironmentFactory.CreateGameObjectByType(EnvironmentID.standardWall);
                wallGo.transform.parent = walls.transform;
                wallGo.transform.localPosition = room.Tiles[i, 0].Position;
                wallGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 270.0f);
                wallGo.name = "Wall";

                att = new TileAttribute();
                att.Type = EnvironmentID.standardWall;
                att.Angle = 270.0f;

                room.Tiles[i, 0].TileAttributes.Add(att);
            }
        }
		
        // Place east walls
        for (int i = 0; i < lastTileY; ++i)
        {
            if (!room.Tiles[lastTileX-1, i].ContainsAttribute(EnvironmentID.door) &&
                !room.Tiles[lastTileX-1, i].ContainsAttribute(EnvironmentID.cornerWallTile))
            {
                GameObject wallGo = EnvironmentFactory.CreateGameObjectByType(EnvironmentID.standardWall);
                wallGo.transform.parent = walls.transform;
                wallGo.transform.localPosition = room.Tiles[lastTileX-1, i].Position;
                wallGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 180.0f);
                wallGo.name = "Wall";

                att = new TileAttribute();
                att.Type = EnvironmentID.standardWall;
                att.Angle = 180.0f;

                room.Tiles[lastTileX-1, i].TileAttributes.Add(att);
            }
        }
		
        // Place west walls
        for (int i = 0; i < lastTileY; ++i)
        {
            if (!room.Tiles[0, i].ContainsAttribute(EnvironmentID.door) &&
                !room.Tiles[0, i].ContainsAttribute(EnvironmentID.cornerWallTile))
            {
                GameObject wallGo = EnvironmentFactory.CreateGameObjectByType(EnvironmentID.standardWall);
                wallGo.transform.parent = walls.transform;
                wallGo.transform.localPosition = room.Tiles[0, i].Position;
                wallGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 0.0f);
                wallGo.name = "Wall";

                att = new TileAttribute();
                att.Type = EnvironmentID.standardWall;
                att.Angle = 0.0f;

                room.Tiles[0, i].TileAttributes.Add(att);
            }
        }
		
        room.IsConstructed = true;
	}
}