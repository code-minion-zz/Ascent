using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Handles generation of a room for use in the floor generation.
/// </summary>
/// 
public class RoomGeneration 
{
	private GameObject floorObject;
	private GameObject wallObject;
	private GameObject wallCorner;
	private GameObject wallWindow;
	private GameObject doorObject;

	private GameObject barrelObject;
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
        brazierObject = Resources.Load("Prefabs/RoomPieces/Brazier") as GameObject;
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
		
		// Handle creation of the ground tiles.
		RoomProperties newRoom = new RoomProperties(room);
		newRoom.SetRoomTiles(width, height);
		PlaceGroundTiles(newRoom);
		
		// Apply the new dimensions to the navMesh.
		room.NavMesh.transform.localScale = new Vector3(width - 1.0f, height - 1.0f, 0.0f);

		// TODO: Fix the camera setup for this room.
		room.minCamera.x = -width * 0.25f;
		room.minCamera.z = -height * 0.25f;
		room.maxCamera.z = height * 0.25f;
		room.maxCamera.x = width * 0.25f;
		
		return newRoom;
	}

    public void PopulateMonsters(int dungeonLevel, RoomProperties room, Rarity rarity)
    {
        int numberOfTilesX = (int)(room.Width * 0.5f);
        int numberOfTilesY = (int)(room.Height * 0.5f);

        List<TileProperties> tempAvailablePosition = new List<TileProperties>();

        // Find all the available positions that a misc object can be placed.
        for (int i = 0; i < numberOfTilesX; ++i)
        {
            for (int j = 0; j < numberOfTilesY; ++j)
            {
                // Search for tiles that are available.
                if (room.RoomTiles[i, j].TileType == TilePropertyType.monster || room.RoomTiles[i, j].TileType == TilePropertyType.none)
                {
                    tempAvailablePosition.Add(room.RoomTiles[i, j]);
                }
            }
        }

        Debug.Log(tempAvailablePosition.Count);

        // Generate number of monsters.
        // TODO: make this better haha.
        int numberOfMonsters = (int)rarity * Random.Range(1, 5);
        int monstersPlaced = 0;

        for (monstersPlaced = 0; monstersPlaced < numberOfMonsters; ++monstersPlaced)
        {
            // If we have exausted all of our available positions we can finish.
            if (tempAvailablePosition.Count == 0)
                return;

            // Choose type of monster
            Room.EMonsterTypes mobType = (Room.EMonsterTypes)(Random.Range(0, (int)Room.EMonsterTypes.MAX));

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
                    //go = InstantiateGameObject(ERoomObjects.Enemy, "Slime");
                    break;

                case Room.EMonsterTypes.EnchantedStatue:
                    go = room.Room.InstantiateGameObject(Room.ERoomObjects.Enemy, "EnchantedStatue");
                    break;

                // Abominations are crashing
                case Room.EMonsterTypes.Abomination:
                    //go = InstantiateGameObject(ERoomObjects.Enemy, "Abomination");
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
            int randomTile = Random.Range(0, tempAvailablePosition.Count);
            go.transform.localPosition = tempAvailablePosition[randomTile].Position;
            go.transform.parent = room.Room.MonsterParent;

            // Apply configurations to the tile of this room and remove
            // the tile from our temp list so that a monster is not placed here again.
            tempAvailablePosition[randomTile].TileType = TilePropertyType.monster;
            tempAvailablePosition.Remove(tempAvailablePosition[randomTile]);
        }
    }

	/// <summary>
	/// Populates the the room with misc objects.
	/// </summary>
	/// <param name="room">Room.</param>
	public void PopulateMiscObjects(RoomProperties room)
	{
		int numberOfTilesX = (int)(room.Width * 0.5f);
		int numberOfTilesY = (int)(room.Height * 0.5f);

        List<TileProperties> tempAvailablePosition = new List<TileProperties>();

        // Find all the available positions that a misc object can be placed.
		for (int i = 0; i < numberOfTilesX; ++i)
		{
			for (int j = 0; j < numberOfTilesY; ++j)
			{
                // Populate random misc objects.
                if (room.RoomTiles[i, j].TileType == TilePropertyType.miscObj || room.RoomTiles[i, j].TileType == TilePropertyType.none)
                {
                    tempAvailablePosition.Add(room.RoomTiles[i, j]);
                }
			}
		}

        // Determine how many objects we will try to place.
        int numberOfMisc = (int)miscObjects * Random.Range(1, 3);
        int miscPlaced = 0;

        for (miscPlaced = 0; miscPlaced < numberOfMisc; ++miscPlaced)
        {
            // If we have exausted all of our available positions we can finish.
            if (tempAvailablePosition.Count == 0)
                return;

            // Choose a random tile, and a random misc object.
            int randomTile = Random.Range(0, tempAvailablePosition.Count);
            int random = Random.Range(0, 2);

            switch (random)
            {
                case 0:
                    GameObject barrelGo = GameObject.Instantiate(barrelObject, Vector3.zero, barrelObject.transform.rotation) as GameObject;
                    barrelGo.transform.parent = room.Room.GetNodeByLayer("Environment").transform;
                    barrelGo.transform.localPosition = tempAvailablePosition[randomTile].Position;

                    tempAvailablePosition[randomTile].TileType = TilePropertyType.miscObj;
                    tempAvailablePosition.Remove(tempAvailablePosition[randomTile]);
                    break;

                case 1:
                    GameObject brazierGo = GameObject.Instantiate(brazierObject, Vector3.zero, brazierObject.transform.rotation) as GameObject;
                    brazierGo.transform.parent = room.Room.GetNodeByLayer("Environment").transform;
                    brazierGo.transform.localPosition = tempAvailablePosition[randomTile].Position;

                    tempAvailablePosition[randomTile].TileType = TilePropertyType.miscObj;
                    tempAvailablePosition.Remove(tempAvailablePosition[randomTile]);
                    break;
            }
        }
	}

	public void PlaceGroundTiles(RoomProperties room)
	{
		int numberOfTilesX = (int)(room.Width * 0.5f);
		int numberOfTilesY = (int)(room.Height * 0.5f);
		
		// Create the floor tiles and positions.
		// TODO: If we want to at some point make different shaped rooms we will need to only place tiles
		// where necessary.
		for (int i = 0; i < numberOfTilesX; ++i)
		{
			for (int j = 0; j < numberOfTilesY; ++j)
			{
				// Create the floor.
				GameObject floorGo = GameObject.Instantiate(floorObject, Vector3.zero, floorObject.transform.rotation) as GameObject;
				float tileSizeX = floorObject.transform.localScale.x;
				float tileSizeY = floorObject.transform.localScale.y;
				float halfTileX = tileSizeX * 0.5f;
				float halfTileY = tileSizeY * 0.5f;
				floorGo.transform.parent = room.Room.GetNodeByLayer("Environment").transform;
				Vector3 tilePosition = new Vector3(-(room.Width * 0.5f) + halfTileX + (i * tileSizeX), 0.0f, -(room.Height * 0.5f) + halfTileY + (j * tileSizeY));
				floorGo.transform.position = tilePosition;
				floorGo.name = "GroundTile[" + i + ", " + j + "]";
				
				room.RoomTiles[i,j].Position = tilePosition;
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
		Door returnDoor = null;
		
		doorsScript.doors[(int)direction] = doorGo.GetComponent<Door>();
		returnDoor = doorsScript.doors[(int)direction];
		
		float widthOffset = (fromRoom.Width * 0.5f) + 1.0f;
		float heightOffset = (fromRoom.Height * 0.5f) + 1.0f;

        int tileX = (int)((widthOffset * 0.5f) - 1.0f);
        int tileY = (int)((heightOffset * 0.5f) - 1.0f);
		
		switch (direction)
		{
		case Floor.TransitionDirection.North:
			doorGo.transform.position = new Vector3(doors.transform.position.x, doorGo.transform.position.y, doors.transform.position.z + heightOffset);
			doorGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 90.0f);
			doorGo.name = "North Door";
            fromRoom.RoomTiles[tileX, (int)(heightOffset-2.0f)].TileType = TilePropertyType.doorTile;
			break;
			
		case Floor.TransitionDirection.East:
			doorGo.transform.position = new Vector3(doors.transform.position.x + widthOffset, doorGo.transform.position.y, doors.transform.position.z);
			doorGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 180.0f);
			doorGo.name = "East Door";
            fromRoom.RoomTiles[(int)(widthOffset - 2.0f), tileY].TileType = TilePropertyType.doorTile;
			break;
			
		case Floor.TransitionDirection.South:
			doorGo.transform.position = new Vector3(doors.transform.position.x, doorGo.transform.position.y, doors.transform.position.z - heightOffset);
			doorGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 270.0f);
			doorGo.name = "South Door";
            fromRoom.RoomTiles[tileX, 0].TileType = TilePropertyType.doorTile;
			break;
			
		case Floor.TransitionDirection.West:
			doorGo.transform.position = new Vector3(doors.transform.position.x - widthOffset, doorGo.transform.position.y, doors.transform.position.z);
			doorGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 0.0f);
			doorGo.name = "West Door";
            fromRoom.RoomTiles[0, tileY].TileType = TilePropertyType.doorTile;
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
		
		// South east corner
		wallCornerGo = GameObject.Instantiate(wallCorner, Vector3.zero, wallCorner.transform.rotation) as GameObject;
		wallCornerGo.transform.position = new Vector3(room.Position.x + (room.Width * 0.5f) - 1.0f, wallCornerGo.transform.position.y, room.Position.z - (room.Height * 0.5f) + 1.0f);
		wallCornerGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 0.0f);
		wallCornerGo.name = "CornerSE";
		wallCornerGo.transform.parent = walls.transform;
		
		// North west corner
		wallCornerGo = GameObject.Instantiate(wallCorner, Vector3.zero, wallCorner.transform.rotation) as GameObject;
		wallCornerGo.transform.position = new Vector3(room.Position.x - (room.Width * 0.5f) + 1.0f, wallCornerGo.transform.position.y, room.Position.z + (room.Height * 0.5f) - 1.0f);
		wallCornerGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 180.0f);
		wallCornerGo.name = "CornerNW";
		wallCornerGo.transform.parent = walls.transform;
		
		// North east corner
		wallCornerGo = GameObject.Instantiate(wallCorner, Vector3.zero, wallCorner.transform.rotation) as GameObject;
		wallCornerGo.transform.position = new Vector3(room.Position.x - (room.Width * 0.5f) + 1.0f, wallCornerGo.transform.position.y, room.Position.z - (room.Height * 0.5f) + 1.0f);
		wallCornerGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 90.0f);
		wallCornerGo.name = "CornerSW";
		wallCornerGo.transform.parent = walls.transform;
		
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
				
				horizontalWalls[i] = true;
			}
		}
		
		room.WallsPlaced = true;
	}
}