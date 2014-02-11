using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

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

    public RoomProperties LoadRoom(Stream fileStream)
    {
        TilePropertyType[,] tiles = LoadTiles(fileStream);

        Room room = CreateRoomObject("Room : Custom");

        // Handle creation of the ground tiles.
        RoomProperties newRoom = new RoomProperties(room);
        newRoom.SetRoomTiles(tiles.GetLength(0), tiles.GetLength(1));

        // Copy over the tiles.
        for (int i = 0; i < tiles.GetLength(0); ++i)
        {
            for (int j = 0; j < tiles.GetLength(1); ++j)
            {
                newRoom.RoomTiles[i, j].TileType = tiles[i, j];
            }
        }

        // Apply the new dimensions to the navMesh.
        room.NavMesh.transform.localScale = new Vector3((tiles.GetLength(0) * 2) - 1.0f, (tiles.GetLength(1) * 2) - 1.0f, 0.0f);

        PlaceGroundTiles(newRoom);
        //PlaceDoorsDisconnected(newRoom);
        SetupCamera(newRoom);
        newRoom.IsPreloaded = true;

        return newRoom;
    }

    /// <summary>
    /// Loads tiles from a specified file.
    /// </summary>
    /// <param name="fileStream">The file path for the room.</param>
    private TilePropertyType[,] LoadTiles(Stream fileStream)
    {
        int width;
        List<string> lines = new List<string>();

        using (StreamReader reader = new StreamReader(fileStream))
        {
            string line = reader.ReadLine();
            width = line.Length;

            while (line != null)
            {
                lines.Add(line);
                if (line.Length != width)
                    throw new Exception(String.Format("The length of line {0} is different from all preceeding lines.", lines.Count));
                line = reader.ReadLine();
            }
        }

        TilePropertyType[,] tiles = new TilePropertyType[width, lines.Count];

        for (int y = 0; y < tiles.GetLength(1); ++y)
        {
            for (int x = 0; x < tiles.GetLength(0); ++x)
            {
                char tileType = lines[y][x];
                tiles[x, y] = LoadTile(tileType, x, y);
            }
        }

        return tiles;
    }

    /// <summary>
    /// Based on the tile type will load the appropriate tile.
    /// </summary>
    /// <param name="tileType"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private TilePropertyType LoadTile(char tileType, int x, int y)
    {
        TilePropertyType tile = TilePropertyType.none;

        switch (tileType)
        {
            case 'B':
                tile = TilePropertyType.brazier;
                break;

            case 'C':
                tile = TilePropertyType.cornerWallTile;
                break;

            case 'D':
                tile = TilePropertyType.doorTile;
                break;

            case 'X':
                tile = TilePropertyType.wallTile;
                break;
        }

        return tile;
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
        List<TileProperties> tempAvailablePosition = new List<TileProperties>();

        // Find all the available positions that a misc object can be placed.
        for (int i = 0; i < room.NumberOfTilesX; ++i)
        {
            for (int j = 0; j < room.NumberOfTilesY; ++j)
            {
                // Search for tiles that are available.
                if (room.RoomTiles[i, j].TileType == TilePropertyType.monster || room.RoomTiles[i, j].TileType == TilePropertyType.none)
                {
                    tempAvailablePosition.Add(room.RoomTiles[i, j]);
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
            tempAvailablePosition[randomTile].TileType = TilePropertyType.monster;
            tempAvailablePosition.Remove(tempAvailablePosition[randomTile]);
        }
    }

    public void PopulateBossRoom(int dungeonLevel, RoomProperties room)
    {
        GameObject go = room.Room.InstantiateGameObject(Room.ERoomObjects.Enemy, "Abomination");
        int centreX = (int)(room.NumberOfTilesX * 0.7f);
        int centreY = (int)(room.NumberOfTilesY * 0.7f);

        room.RoomTiles[centreX, centreY].TileType = TilePropertyType.monster;
        room.RoomTiles[centreX, centreY].IsOccupied = true;
        go.transform.localPosition = room.RoomTiles[centreX, centreY].Position;
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

        List<TileProperties> tempAvailableTiles = new List<TileProperties>();

        // Find all the available positions that a misc object can be placed.
		for (int i = 0; i < room.NumberOfTilesX; ++i)
		{
			for (int j = 0; j < room.NumberOfTilesY; ++j)
			{
                // Populate random misc objects.
                if (room.RoomTiles[i, j].TileType == TilePropertyType.miscObj || 
                    room.RoomTiles[i, j].TileType == TilePropertyType.wallTile || 
                    room.RoomTiles[i, j].TileType == TilePropertyType.cornerWallTile ||
                    room.RoomTiles[i, j].TileType == TilePropertyType.brazier)
                {
                    if (room.RoomTiles[i, j].IsOccupied == false)
                    {
                        tempAvailableTiles.Add(room.RoomTiles[i, j]);
                    }
                }
			}
		}

        GameObject go = null;

        // For each corner place the braziers.
        foreach (TileProperties tile in tempAvailableTiles)
        {
            if (tile.TileType == TilePropertyType.cornerWallTile ||
                tile.TileType == TilePropertyType.brazier)
            {
                go = GameObject.Instantiate(brazierObject, Vector3.zero, brazierObject.transform.rotation) as GameObject;
                go.transform.parent = room.Room.EnvironmentParent;
                go.transform.localPosition = tile.Position;

                tile.IsOccupied = true;
            }
        }

        // Since the room is preloaded we don't need to randomly place things.
        if (room.IsPreloaded == true)
        {
            return;
        }

        // Determine how many barrel objects we will try to place.
        int numberOfMisc = (int)miscObjects * UnityEngine.Random.Range(1, 4);
        int miscPlaced = 0;

        for (miscPlaced = 0; miscPlaced < numberOfMisc; ++miscPlaced)
        {
            // Reset as we do not want to accidentally place more braziers.
            go = null;

            // If we have exausted all of our available positions we can finish.
            if (tempAvailableTiles.Count == 0)
                return;

            // Choose a random tile, and a random misc object.
            int randomTile = UnityEngine.Random.Range(0, tempAvailableTiles.Count);

            // Check to see if the tile type is a wall tile to be sure.
            if (tempAvailableTiles[randomTile].TileType == TilePropertyType.wallTile)
            {
                int random = UnityEngine.Random.Range(0, 2);

                switch (random)
                {
                    case 0:
                        go = GameObject.Instantiate(barrelObject, Vector3.zero, barrelObject.transform.rotation) as GameObject;
                        break;

                    case 1:
                        float rotationY = UnityEngine.Random.Range(0.0f, 270.0f);
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

            tempAvailableTiles[randomTile].TileType = TilePropertyType.miscObj;
            tempAvailableTiles[randomTile].IsOccupied = true;
            tempAvailableTiles.Remove(tempAvailableTiles[randomTile]);
        }
	}

	public void PlaceGroundTiles(RoomProperties room)
	{
		// Create the floor tiles and positions.
		// TODO: If we want to at some point make different shaped rooms we will need to only place tiles
		// where necessary.
		for (int x = 0; x < room.NumberOfTilesX; ++x)
		{
			for (int y = 0; y < room.NumberOfTilesY; ++y)
			{
                if (room.RoomTiles[x, y].TileType == TilePropertyType.none ||
                    room.RoomTiles[x, y].TileType == TilePropertyType.doorTile ||
                    room.RoomTiles[x, y].TileType == TilePropertyType.wallTile ||
                    room.RoomTiles[x, y].TileType == TilePropertyType.brazier ||
                    room.RoomTiles[x, y].TileType == TilePropertyType.cornerWallTile)
                {
                    GameObject groundTile = GameObject.Instantiate(floorObject, Vector3.zero, floorObject.transform.rotation) as GameObject;
                    groundTile.transform.parent = room.Room.GetNodeByLayer("Environment").transform;
                    groundTile.transform.position = room.RoomTiles[x, y].Position;
                    groundTile.name = "GroundTile[" + x + ", " + y + "]";
                }
			}
		}
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
		GameObject doorGo = GameObject.Instantiate(doorObject, Vector3.zero, doorObject.transform.rotation) as GameObject;
		doorGo.transform.parent = doors.transform;
		
		// Attach the doors to their rightful component.
		Doors doorsScript = doors.GetComponent<Doors>();
		
		doorsScript.doors[(int)direction] = doorGo.GetComponent<Door>();
		Door returnDoor = doorsScript.doors[(int)direction];
		
		int lastTileX = fromRoom.NumberOfTilesX - 1;
		int lastTileY = fromRoom.NumberOfTilesY - 1;

        int midXTile = (int)(lastTileX * 0.5f);
        int midYTile = (int)(lastTileY * 0.5f);

		switch (direction)
		{
            case Floor.TransitionDirection.North:
                {
                    doorGo.transform.localPosition = fromRoom.RoomTiles[midXTile, lastTileY].Position;
                    doorGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 90.0f);
                    doorGo.name = "North Door";
                    fromRoom.RoomTiles[midXTile, lastTileY].TileType = TilePropertyType.doorTile;
                    fromRoom.RoomTiles[midXTile, lastTileY].IsOccupied = true;
                }
                break;

            case Floor.TransitionDirection.East:
                {
                    doorGo.transform.localPosition = fromRoom.RoomTiles[lastTileX, midYTile].Position;
                    doorGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 180.0f);
                    doorGo.name = "East Door";
                    fromRoom.RoomTiles[lastTileX, midYTile].TileType = TilePropertyType.doorTile;
                    fromRoom.RoomTiles[lastTileX, midYTile].IsOccupied = true;
                }
                break;

            case Floor.TransitionDirection.South:
                {
                    doorGo.transform.localPosition = fromRoom.RoomTiles[midXTile, 0].Position;
                    doorGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 270.0f);
                    doorGo.name = "South Door";
                    fromRoom.RoomTiles[midXTile, 0].TileType = TilePropertyType.doorTile;
                    fromRoom.RoomTiles[midXTile, 0].IsOccupied = true;
                }
                break;

            case Floor.TransitionDirection.West:
                {
                    doorGo.transform.localPosition = fromRoom.RoomTiles[0, midYTile].Position;
                    doorGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 0.0f);
                    doorGo.name = "West Door";
                    fromRoom.RoomTiles[0, midYTile].TileType = TilePropertyType.doorTile;
                    fromRoom.RoomTiles[0, midYTile].IsOccupied = true;
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
		wallCornerGo = GameObject.Instantiate(wallCorner, Vector3.zero, wallCorner.transform.rotation) as GameObject;
		wallCornerGo.transform.position = new Vector3(room.Position.x + (room.Width * 0.5f) - 1.0f, wallCornerGo.transform.position.y, room.Position.z + (room.Height * 0.5f) - 1.0f);
		wallCornerGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 270.0f);
		wallCornerGo.name = "CornerNE";
		wallCornerGo.transform.parent = walls.transform;

        // Make this tile a wall tile.
        room.RoomTiles[room.NumberOfTilesX-1, room.NumberOfTilesY-1].TileType = TilePropertyType.cornerWallTile;
		
		// South east corner
		wallCornerGo = GameObject.Instantiate(wallCorner, Vector3.zero, wallCorner.transform.rotation) as GameObject;
		wallCornerGo.transform.position = new Vector3(room.Position.x + (room.Width * 0.5f) - 1.0f, wallCornerGo.transform.position.y, room.Position.z - (room.Height * 0.5f) + 1.0f);
		wallCornerGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 0.0f);
		wallCornerGo.name = "CornerSE";
		wallCornerGo.transform.parent = walls.transform;

        // Make this tile a wall tile.
        room.RoomTiles[room.NumberOfTilesX - 1, 0].TileType = TilePropertyType.cornerWallTile;
		
		// North west corner
		wallCornerGo = GameObject.Instantiate(wallCorner, Vector3.zero, wallCorner.transform.rotation) as GameObject;
		wallCornerGo.transform.position = new Vector3(room.Position.x - (room.Width * 0.5f) + 1.0f, wallCornerGo.transform.position.y, room.Position.z + (room.Height * 0.5f) - 1.0f);
		wallCornerGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 180.0f);
		wallCornerGo.name = "CornerNW";
		wallCornerGo.transform.parent = walls.transform;

        // Make this tile a wall tile.
        room.RoomTiles[0, (int)room.NumberOfTilesY - 1].TileType = TilePropertyType.cornerWallTile;
		
		// South west corner
		wallCornerGo = GameObject.Instantiate(wallCorner, Vector3.zero, wallCorner.transform.rotation) as GameObject;
		wallCornerGo.transform.position = new Vector3(room.Position.x - (room.Width * 0.5f) + 1.0f, wallCornerGo.transform.position.y, room.Position.z - (room.Height * 0.5f) + 1.0f);
		wallCornerGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 90.0f);
		wallCornerGo.name = "CornerSW";
		wallCornerGo.transform.parent = walls.transform;

        // Make this tile a wall tile.
        room.RoomTiles[0, 0].TileType = TilePropertyType.cornerWallTile;
		
		// Place north walls
		int numberOfWalls = (int)(room.Width * 0.5f) - 2;
		
		bool[] horizontalWalls = new bool[numberOfWalls];
		
		if (room.directionsFilled[0] == true)
		{
			int doorPos = (int)(numberOfWalls * 0.5f);
			horizontalWalls[doorPos] = true;
		}
		
		for (int i = 0; i < numberOfWalls; ++i)
		{
			if (horizontalWalls[i] == false)
			{
				float xPos = (room.Position.x - (room.Width * 0.5f)) + 3.0f + (i * 2.0f);
				GameObject wallGo = GameObject.Instantiate(wallObject, Vector3.zero, wallObject.transform.rotation) as GameObject;
				wallGo.transform.position = new Vector3(xPos, wallCornerGo.transform.position.y, room.Position.z + (room.Height * 0.5f) + 1.0f);
				wallGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 90.0f);
				wallGo.name = "Wall";
				wallGo.transform.parent = walls.transform;

                // Make this tile a wall tile.
                if (room.RoomTiles[i, room.NumberOfTilesY-1].TileType != TilePropertyType.doorTile &&
                    room.RoomTiles[i, room.NumberOfTilesY-1].TileType != TilePropertyType.cornerWallTile)
                {
                    room.RoomTiles[i, room.NumberOfTilesY - 1].TileType = TilePropertyType.wallTile;
                }
				
				horizontalWalls[i] = true;
			}
		}
		
		// Place south walls
		numberOfWalls = (int)(room.Width * 0.5f) - 2;
		
		horizontalWalls = new bool[numberOfWalls];
		
		if (room.directionsFilled[2] == true)
		{
			int doorPos = (int)(numberOfWalls * 0.5f);
			horizontalWalls[doorPos] = true;
		}
		
		for (int i = 0; i < numberOfWalls; ++i)
		{
			if (horizontalWalls[i] == false)
			{
				float xPos = (room.Position.x - (room.Width * 0.5f)) + 3.0f + (i * 2.0f);
				GameObject wallGo = GameObject.Instantiate(wallObject, Vector3.zero, wallObject.transform.rotation) as GameObject;
				wallGo.transform.position = new Vector3(xPos, wallCornerGo.transform.position.y, room.Position.z - (room.Height * 0.5f) - 1.0f);
				wallGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 270.0f);
				wallGo.name = "Wall";
				wallGo.transform.parent = walls.transform;

                // Make this tile a wall tile.
                if (room.RoomTiles[i, 0].TileType != TilePropertyType.doorTile &&
                    room.RoomTiles[i, 0].TileType != TilePropertyType.cornerWallTile)
                {
                    room.RoomTiles[i, 0].TileType = TilePropertyType.wallTile;
                }
				
				horizontalWalls[i] = true;
			}
		}
		
		// Place east walls
		numberOfWalls = (int)(room.Height * 0.5f) - 2;
		
		horizontalWalls = new bool[numberOfWalls];
		
		if (room.directionsFilled[1] == true)
		{
			int doorPos = (int)(numberOfWalls * 0.5f);
			horizontalWalls[doorPos] = true;
		}
		
		for (int i = 0; i < numberOfWalls; ++i)
		{
			if (horizontalWalls[i] == false)
			{
				float zPos = (room.Position.z - (room.Height * 0.5f)) + 3.0f + (i * 2.0f);
				GameObject wallGo = GameObject.Instantiate(wallObject, Vector3.zero, wallObject.transform.rotation) as GameObject;
				wallGo.transform.position = new Vector3(room.Position.x + (room.Width * 0.5f) + 1.0f, wallCornerGo.transform.position.y, zPos);
				wallGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 180.0f);
				wallGo.name = "Wall";
				wallGo.transform.parent = walls.transform;

                // Make this tile a wall tile.
                if (room.RoomTiles[room.NumberOfTilesX-1, i].TileType != TilePropertyType.doorTile &&
                    room.RoomTiles[room.NumberOfTilesX-1, i].TileType != TilePropertyType.cornerWallTile)
                {
                    room.RoomTiles[room.NumberOfTilesX-1, i].TileType = TilePropertyType.wallTile;
                }
				
				horizontalWalls[i] = true;
			}
		}
		
		// Place west walls
		numberOfWalls = (int)(room.Height * 0.5f) - 2;
		
		horizontalWalls = new bool[numberOfWalls];
		
		if (room.directionsFilled[3] == true)
		{
			int doorPos = (int)(numberOfWalls * 0.5f);
			horizontalWalls[doorPos] = true;
		}
		
		for (int i = 0; i < numberOfWalls; ++i)
		{
			if (horizontalWalls[i] == false)
			{
				float zPos = (room.Position.z - (room.Height * 0.5f)) + 3.0f + (i * 2.0f);
				GameObject wallGo = GameObject.Instantiate(wallObject, Vector3.zero, wallObject.transform.rotation) as GameObject;
				wallGo.transform.position = new Vector3(room.Position.x - (room.Width * 0.5f) - 1.0f, wallCornerGo.transform.position.y, zPos);
				wallGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 0.0f);
				wallGo.name = "Wall";
				wallGo.transform.parent = walls.transform;

                // Make this tile a wall tile.
                if (room.RoomTiles[0, i].TileType != TilePropertyType.doorTile &&
                    room.RoomTiles[0, i].TileType != TilePropertyType.cornerWallTile)
                {
                    room.RoomTiles[0, i].TileType = TilePropertyType.wallTile;
                }
				
				horizontalWalls[i] = true;
			}
		}
		
		room.WallsPlaced = true;
	}
}