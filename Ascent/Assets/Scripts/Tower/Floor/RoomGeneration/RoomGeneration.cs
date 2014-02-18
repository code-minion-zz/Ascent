using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

/// <summary>
/// Handles generation of a room for use in the floor generation.
/// </summary>
/// 
public class RoomGeneration 
{
#pragma warning disable 0414 // Room.Generation.wallWindow is assigned but never used

	private GameObject floorObject;
	private GameObject wallObject;
	private GameObject wallCorner;
	private GameObject wallWindow;
	private GameObject doorObject;

	private GameObject barrelObject;
    private GameObject barrelCluster;
    private GameObject brazierObject;

    private Rarity miscObjects = Rarity.few;

	public RoomGeneration()
	{
		floorObject = Resources.Load("Prefabs/RoomWalls/GroundTile_2x2") as GameObject;
		wallObject = Resources.Load("Prefabs/RoomWalls/Wall") as GameObject;
		wallCorner = Resources.Load("Prefabs/RoomWalls/WallCorner") as GameObject;
		wallWindow = Resources.Load("Prefabs/RoomWalls/WallWindow") as GameObject;
		doorObject = Resources.Load("Prefabs/RoomWalls/Door") as GameObject;

		barrelObject = Resources.Load("Prefabs/RoomPieces/Barrel") as GameObject;
        barrelCluster = Resources.Load("Prefabs/RoomPieces/BarrelCluster") as GameObject;
        brazierObject = Resources.Load("Prefabs/RoomPieces/Brazier") as GameObject;
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

        room.AddNewParentCategory("Monsters", LayerMask.NameToLayer("Monster"));
        room.AddNewParentCategory("Items", LayerMask.NameToLayer("Items"));
        room.AddNewParentCategory("Lights", LayerMask.NameToLayer("Default"));

        room.Initialise();

        return room;
    }

    /// <summary>
    /// Creates a new room and intializes variables.
    /// </summary>
    /// <returns>The new room.</returns>
    /// <param name="width">Width.</param>
    /// <param name="height">Height.</param>
    /// <param name="name">Name.</param>
    public RoomProperties CreateNewRoom(int width, int height, string name)
	{
        Room room = CreateRoomObject(name);
		
		// Handle creation of the ground tiles.
		RoomProperties newRoom = new RoomProperties(room);
        newRoom.Name = name;
		newRoom.SetRoomTiles((int)(width * 0.5f), (int)(height * 0.5f));
		PlaceGroundTiles(newRoom);
        SetupCamera(newRoom);
		
		// Apply the new dimensions to the navMesh.
		room.NavMesh.transform.localScale = new Vector3(width - 1.0f, height - 1.0f, 0.0f);

		return newRoom;
	}

    private void SetupCamera(RoomProperties room)
    {
        float cameraOffsetX = 0.175f;
        float cameraOffsetZ = 0.57f;

        // A standard room is 18 wide(X) by 14 high(Z)
        float highestDimension = room.Width > room.Height ? room.Height : room.Width;
        if (highestDimension <= 6.0f)
        {
            room.Room.cameraHeight = 17.0f;
            room.Room.cameraOffsetZ = -0.3f;
            cameraOffsetX = 0.20f;
            cameraOffsetZ = 0.75f;

        }
        else if (highestDimension <= 10.0f)
        {
            room.Room.cameraHeight = 18.0f;
            room.Room.cameraOffsetZ = -0.318f;
            cameraOffsetX = 0.20f;
            cameraOffsetZ = 0.75f;
        }
        else if (highestDimension <= 14.0f)
        {
            room.Room.cameraHeight = 19.0f;
            room.Room.cameraOffsetZ = -0.336f;
            cameraOffsetX = 0.175f;
            cameraOffsetZ = 0.57f;
        }
        else
        {
            room.Room.cameraHeight = 20.0f;
            room.Room.cameraOffsetZ = -0.35f;
            cameraOffsetX = 0.175f;
            cameraOffsetZ = 0.57f;
        }


        // TODO: Fix the camera setup for this room.
        room.Room.minCamera.x = -room.Width * cameraOffsetX;
        room.Room.maxCamera.x = room.Width * cameraOffsetX;
        room.Room.minCamera.z = -room.Height * cameraOffsetZ;

        // Min is the bottom. Max is TOp

        // Assuming a base room height is 14 (ie. a room with height 14 will have max as 0), increase the max by the difference from the base height.
        room.Room.maxCamera.z = (room.Height - 14.0f) * cameraOffsetZ;

        // If the max is less than the min set the max to the min.
        room.Room.maxCamera.z = room.Room.maxCamera.z < room.Room.minCamera.z ? room.Room.minCamera.z : room.Room.maxCamera.z;
    }

    public void PopulateMonsters(int dungeonLevel, RoomProperties room, Rarity rarity)
    {
        List<Tile> tempAvailablePosition = new List<Tile>();

        // Find all the available positions that a misc object can be placed.
        for (int i = 0; i < room.NumberOfTilesX; ++i)
        {
            for (int j = 0; j < room.NumberOfTilesY; ++j)
            {
                // Search for tiles that are available.
                if (room.Tiles[i, j].TileType == TileType.monster || room.Tiles[i, j].TileType == TileType.none)
                {
                    tempAvailablePosition.Add(room.Tiles[i, j]);
                }
            }
        }

        // Generate number of monsters.
        // TODO: make this better haha.
        int numberOfMonsters = (int)rarity * UnityEngine.Random.Range(1, 5);
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

            // Apply configurations to the tile of this room and remove
            // the tile from our temp list so that a monster is not placed here again.
            tempAvailablePosition[randomTile].TileType = TileType.monster;
            tempAvailablePosition.Remove(tempAvailablePosition[randomTile]);
        }
    }

    public void PopulateBossRoom(int dungeonLevel, RoomProperties room)
    {
        GameObject go = room.Room.InstantiateGameObject(Room.ERoomObjects.Enemy, "Abomination");
        int centreX = (int)(room.NumberOfTilesX * 0.7f);
        int centreY = (int)(room.NumberOfTilesY * 0.7f);

        room.Tiles[centreX, centreY].TileType = TileType.monster;
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
        if (room.IsPreloaded == true)
        {
            return;
        }

        List<Tile> tempAvailableTiles = new List<Tile>();

        // Find all the available positions that a misc object can be placed.
		for (int i = 0; i < room.NumberOfTilesX; ++i)
		{
			for (int j = 0; j < room.NumberOfTilesY; ++j)
			{
                // Populate random misc objects.
                if (room.Tiles[i, j].ContainsAttribute(TileType.miscObj) ||
                    room.Tiles[i, j].ContainsAttribute(TileType.standardWall) ||
                    room.Tiles[i, j].ContainsAttribute(TileType.cornerWallTile) ||
                    room.Tiles[i, j].ContainsAttribute(TileType.brazier))
                {
                    if (room.Tiles[i, j].IsOccupied == false)
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
            if (tile.ContainsAttribute(TileType.cornerWallTile))
            {
                go = GetGameObjectByType(TileType.brazier);
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
            if (tempAvailableTiles[randomTile].ContainsAttribute(TileType.standardWall))
            {
                int random = UnityEngine.Random.Range(0, 2);

                switch (random)
                {
                    case 0:
                        go = GameObject.Instantiate(barrelObject, Vector3.zero, barrelObject.transform.rotation) as GameObject;
                        break;

                    case 1:
                        float rotationY = UnityEngine.Random.Range(0.0f, 270.0f);
                        angle = rotationY;
                        go = GameObject.Instantiate(barrelCluster, Vector3.zero, barrelObject.transform.rotation) as GameObject;
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
            att.Type = TileType.miscObj;

            tempAvailableTiles[randomTile].TileAttributes.Add(att);
            tempAvailableTiles[randomTile].IsOccupied = true;
            tempAvailableTiles.Remove(tempAvailableTiles[randomTile]);
        }
	}

	public void PlaceGroundTiles(RoomProperties room)
	{
		// Create the floor tiles and positions.

		// where necessary.
		for (int x = 0; x < room.NumberOfTilesX; ++x)
		{
			for (int y = 0; y < room.NumberOfTilesY; ++y)
			{
                // Populate the whole room with a ground tile attribute.
                TileAttribute att = new TileAttribute();
                att.Type = TileType.groundTile;
                att.Angle = 0.0f;
                room.Tiles[x, y].TileAttributes.Add(att);

                // Create the ground tile.
                GameObject groundTile = GameObject.Instantiate(floorObject, Vector3.zero, floorObject.transform.rotation) as GameObject;
                //groundTile.transform.parent = room.Room.GetNodeByLayer("Environment").transform;
                groundTile.transform.parent = room.Tiles[x, y].GameObject.transform;
                groundTile.transform.position = room.Tiles[x, y].Position;
                groundTile.name = "GroundTile[" + x + ", " + y + "]";
                room.Tiles[x, y].GameObject = groundTile;
			}
		}
	}

    /// <summary>
    /// Reconstructs a room.
    /// </summary>
    /// <param name="room">The room data.</param>
    public void ReconstructRoom(RoomProperties room)
    {
        // Reconstruct the object.
        room.Room = CreateRoomObject(room.Name);
        room.CreateBaseTiles();

        // Construct all the tiles.
        for (int x = 0; x < room.NumberOfTilesX; ++x)
        {
            for (int y = 0; y < room.NumberOfTilesY; ++y)
            {
                ConstructBaseTiles(room, room.Tiles[x, y], x, y);
            }
        }

        SetupCamera(room);

        room.IsPreloaded = true;
        room.WallsPlaced = true;
    }

    private void ConstructBaseTiles(RoomProperties room, Tile tile, int x, int y)
	{
        // Loop through the individual attributes for this tile.
        foreach (TileAttribute att in tile.TileAttributes)
        {
            // Place each attribute on the tile position.
            GameObject go = GetGameObjectByType(att.Type);

            if (go != null)
            {
                //go.transform.parent = room.Room.GetNodeByLayer("Environment").transform;
                go.transform.parent = room.Tiles[x, y].GameObject.transform;
                go.transform.position = room.Tiles[x, y].Position;
                go.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), att.Angle);
                go.name = "[" + x + ", " + y + "]" + go.name;
            }
        }
    }

    /// <summary>
    /// Returns a newly created object based on the type.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private GameObject GetGameObjectByType(TileType type)
    {
        GameObject go = null;

        switch (type)
        {
            case TileType.groundTile:
                go = GameObject.Instantiate(floorObject, Vector3.zero, floorObject.transform.rotation) as GameObject;
                go.name = floorObject.name;
                break;

            case TileType.standardWall:
                go = GameObject.Instantiate(wallObject, Vector3.zero, wallObject.transform.rotation) as GameObject;
                go.name = wallObject.name;
                break;

            case TileType.cornerWallTile:
                go = GameObject.Instantiate(wallCorner, Vector3.zero, wallCorner.transform.rotation) as GameObject;
                go.name = wallCorner.name;
                break;

            case TileType.brazier:
                go = GameObject.Instantiate(brazierObject, Vector3.zero, brazierObject.transform.rotation) as GameObject;
                go.name = brazierObject.name;
                break;

            case TileType.door:
                go = GameObject.Instantiate(doorObject, Vector3.zero, doorObject.transform.rotation) as GameObject;
                go.name = doorObject.name;
                break;
        }

        return go;
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
        GameObject doorGo = GetGameObjectByType(TileType.door);
		doorGo.transform.parent = doors.transform;
		
		// Attach the doors to their rightful component.
		Doors doorsScript = doors.GetComponent<Doors>();
		
		doorsScript.doors[(int)direction] = doorGo.GetComponent<Door>();
		Door returnDoor = doorsScript.doors[(int)direction];
		
		int lastTileX = fromRoom.NumberOfTilesX-1;
		int lastTileY = fromRoom.NumberOfTilesY-1;

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
                    fromRoom.Tiles[midXTile, lastTileY].TileType = TileType.door;
                    fromRoom.Tiles[midXTile, lastTileY].IsOccupied = true;

                    att = new TileAttribute();
                    att.Angle = 90.0f;
                    att.Type = TileType.door;
                    fromRoom.Tiles[midXTile, lastTileY].TileAttributes.Add(att);
                }
                break;

            case Floor.TransitionDirection.East:
                {
                    doorGo.transform.localPosition = fromRoom.Tiles[lastTileX, midYTile].Position;
                    doorGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 180.0f);
                    doorGo.name = "East Door";
                    fromRoom.Tiles[lastTileX, midYTile].TileType = TileType.door;
                    fromRoom.Tiles[lastTileX, midYTile].IsOccupied = true;

                    att = new TileAttribute();
                    att.Angle = 180.0f;
                    att.Type = TileType.door;
                    fromRoom.Tiles[lastTileX, midYTile].TileAttributes.Add(att);
                }
                break;

            case Floor.TransitionDirection.South:
                {
                    doorGo.transform.localPosition = fromRoom.Tiles[midXTile, 0].Position;
                    doorGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 270.0f);
                    doorGo.name = "South Door";
                    fromRoom.Tiles[midXTile, 0].TileType = TileType.door;
                    fromRoom.Tiles[midXTile, 0].IsOccupied = true;

                    att = new TileAttribute();
                    att.Angle = 270.0f;
                    att.Type = TileType.door;
                    fromRoom.Tiles[midXTile, 0].TileAttributes.Add(att);
                }
                break;

            case Floor.TransitionDirection.West:
                {
                    doorGo.transform.localPosition = fromRoom.Tiles[0, midYTile].Position;
                    doorGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 0.0f);
                    doorGo.name = "West Door";
                    fromRoom.Tiles[0, midYTile].TileType = TileType.door;
                    fromRoom.Tiles[0, midYTile].IsOccupied = true;

                    att = new TileAttribute();
                    att.Angle = 0.0f;
                    att.Type = TileType.door;
                    fromRoom.Tiles[0, midYTile].TileAttributes.Add(att);
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
        att.Type = TileType.cornerWallTile;
        att.Angle = 270.0f;
        room.Tiles[room.NumberOfTilesX - 1, room.NumberOfTilesY - 1].TileAttributes.Add(att);

        // Create the corner piece.
		wallCornerGo = GameObject.Instantiate(wallCorner, Vector3.zero, wallCorner.transform.rotation) as GameObject;
		wallCornerGo.transform.position = new Vector3(room.Position.x + (room.Width * 0.5f) - 1.0f, wallCornerGo.transform.position.y, room.Position.z + (room.Height * 0.5f) - 1.0f);
		wallCornerGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 270.0f);
		wallCornerGo.name = "CornerNE";
		wallCornerGo.transform.parent = walls.transform;
        // Make this tile a wall tile. 
        // TODO: Remove this later.
        room.Tiles[room.NumberOfTilesX-1, room.NumberOfTilesY-1].TileType = TileType.cornerWallTile;
		
		// South east corner
        // Assign the attribute.
        att = new TileAttribute();
        att.Type = TileType.cornerWallTile;
        att.Angle = 0.0f;
        room.Tiles[room.NumberOfTilesX - 1, 0].TileAttributes.Add(att);

		wallCornerGo = GameObject.Instantiate(wallCorner, Vector3.zero, wallCorner.transform.rotation) as GameObject;
		wallCornerGo.transform.position = new Vector3(room.Position.x + (room.Width * 0.5f) - 1.0f, wallCornerGo.transform.position.y, room.Position.z - (room.Height * 0.5f) + 1.0f);
		wallCornerGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 0.0f);
		wallCornerGo.name = "CornerSE";
		wallCornerGo.transform.parent = walls.transform;
        // Make this tile a wall tile.
        room.Tiles[room.NumberOfTilesX - 1, 0].TileType = TileType.cornerWallTile;
		
        // North west corner
        // Assign the attribute.
        att = new TileAttribute();
        att.Type = TileType.cornerWallTile;
        att.Angle = 180.0f;
        room.Tiles[0, (int)room.NumberOfTilesY - 1].TileAttributes.Add(att);

		wallCornerGo = GameObject.Instantiate(wallCorner, Vector3.zero, wallCorner.transform.rotation) as GameObject;
		wallCornerGo.transform.position = new Vector3(room.Position.x - (room.Width * 0.5f) + 1.0f, wallCornerGo.transform.position.y, room.Position.z + (room.Height * 0.5f) - 1.0f);
		wallCornerGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 180.0f);
		wallCornerGo.name = "CornerNW";
		wallCornerGo.transform.parent = walls.transform;
        // Make this tile a wall tile.
        room.Tiles[0, (int)room.NumberOfTilesY - 1].TileType = TileType.cornerWallTile;
		
		// South west corner
        // Assign the attribute.
        att = new TileAttribute();
        att.Type = TileType.cornerWallTile;
        att.Angle = 90.0f;
        room.Tiles[0, 0].TileAttributes.Add(att);

		wallCornerGo = GameObject.Instantiate(wallCorner, Vector3.zero, wallCorner.transform.rotation) as GameObject;
		wallCornerGo.transform.position = new Vector3(room.Position.x - (room.Width * 0.5f) + 1.0f, wallCornerGo.transform.position.y, room.Position.z - (room.Height * 0.5f) + 1.0f);
		wallCornerGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 90.0f);
		wallCornerGo.name = "CornerSW";
		wallCornerGo.transform.parent = walls.transform;
        // Make this tile a wall tile.
        room.Tiles[0, 0].TileType = TileType.cornerWallTile;
        
        // Variables used for placing walls.
        int lastTileX = room.NumberOfTilesX;
		int lastTileY = room.NumberOfTilesY;

        // Place north walls
		for (int i = 0; i < lastTileX; ++i)
		{
            // If there is no door or corner piece here.
            if (!room.Tiles[i, lastTileY-1].ContainsAttribute(TileType.door) &&
                !room.Tiles[i, lastTileY-1].ContainsAttribute(TileType.cornerWallTile))
            {
                GameObject wallGo = GetGameObjectByType(TileType.standardWall);
                wallGo.transform.parent = walls.transform;
                wallGo.transform.localPosition = room.Tiles[i, lastTileY-1].Position;
                wallGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 90.0f);
                wallGo.name = "Wall";

                att = new TileAttribute();
                att.Type = TileType.standardWall;
                att.Angle = 90.0f;

                room.Tiles[i, lastTileY-1].TileAttributes.Add(att);
			}
		}
		
		// Place south walls
        for (int i = 0; i < lastTileX; ++i)
        {
            if (!room.Tiles[i, 0].ContainsAttribute(TileType.door) &&
                !room.Tiles[i, 0].ContainsAttribute(TileType.cornerWallTile))
            {
                GameObject wallGo = GetGameObjectByType(TileType.standardWall);
                wallGo.transform.parent = walls.transform;
                wallGo.transform.localPosition = room.Tiles[i, 0].Position;
                wallGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 270.0f);
                wallGo.name = "Wall";

                att = new TileAttribute();
                att.Type = TileType.standardWall;
                att.Angle = 270.0f;

                room.Tiles[i, 0].TileAttributes.Add(att);
            }
        }
		
        // Place east walls
        for (int i = 0; i < lastTileY; ++i)
        {
            if (!room.Tiles[lastTileX-1, i].ContainsAttribute(TileType.door) &&
                !room.Tiles[lastTileX-1, i].ContainsAttribute(TileType.cornerWallTile))
            {
                GameObject wallGo = GetGameObjectByType(TileType.standardWall);
                wallGo.transform.parent = walls.transform;
                wallGo.transform.localPosition = room.Tiles[lastTileX-1, i].Position;
                wallGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 180.0f);
                wallGo.name = "Wall";

                att = new TileAttribute();
                att.Type = TileType.standardWall;
                att.Angle = 180.0f;

                room.Tiles[lastTileX-1, i].TileAttributes.Add(att);
            }
        }
		
        // Place west walls
        for (int i = 0; i < lastTileY; ++i)
        {
            if (!room.Tiles[0, i].ContainsAttribute(TileType.door) &&
                !room.Tiles[0, i].ContainsAttribute(TileType.cornerWallTile))
            {
                GameObject wallGo = GetGameObjectByType(TileType.standardWall);
                wallGo.transform.parent = walls.transform;
                wallGo.transform.localPosition = room.Tiles[0, i].Position;
                wallGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 0.0f);
                wallGo.name = "Wall";

                att = new TileAttribute();
                att.Type = TileType.standardWall;
                att.Angle = 0.0f;

                room.Tiles[0, i].TileAttributes.Add(att);
            }
        }
		
        room.WallsPlaced = true;
	}
}