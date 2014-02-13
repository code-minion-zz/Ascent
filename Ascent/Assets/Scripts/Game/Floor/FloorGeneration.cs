using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public enum Directions
{
    north = 1,
    easth,
    south,
    west
}

public enum Rarity
{
    onlyOne,
    veryRare,
    rare,
    few,
    often,
    many
}

public class FloorGeneration 
{
    // Variables to control the outcome of the level
    public int dungeonLevel = 1;
    public int roomsToPlace = 15;
    public float roomOffsetValue = 25.0f;

    // Rarity controls
    public Rarity monsterRarity;
    public Rarity treasureChestSpawn;
    public Rarity trapRoom;
    public Rarity specialRoom;

	private RoomGeneration roomGeneration = new RoomGeneration();
    private List<RoomProperties> rooms = new List<RoomProperties>();
    private List<int> roomDimensions = new List<int>();
    private Vector3 locationVector;
    private int roomsPlaced = 0;

    public void GenerateFloor()
    {
        roomDimensions.Add(18);
        roomDimensions.Add(14);
        roomDimensions.Add(10);
        //roomDimensions.Add(6);

        //Random.seed = (int)System.DateTime.Today.Millisecond;
        UnityEngine.Random.seed = (int)System.DateTime.Now.TimeOfDay.Ticks;

        CreateRooms();
    }

    private void CreateRooms()
    {
        rooms.Clear();
        rooms = new List<RoomProperties>();
        locationVector = Vector3.zero;

        // Save data from the first room as a test.
        SaveRooms saver = new SaveRooms();
        saver.Initialize();
        saver.LoadRooms();

		// Generate the first room in the game.
        //RoomProperties firstRoom = roomGeneration.CreateNewRoom(18, 14, "Room 0: Start");
		//firstRoom.Position = Vector3.zero;

        RoomProperties firstRoom = saver.RoomSaves.saves[0];
        roomGeneration.ReconstructRoom(firstRoom);

		rooms.Add(firstRoom);

        // Go through and place all the floor components based on the number of them we have.
        for (roomsPlaced = 1; roomsPlaced < roomsToPlace+1; roomsPlaced++)
        {
            // Pick a random room from the pool of rooms that currently exist
            int randomRoom = UnityEngine.Random.Range(0, rooms.Count);
            RoomProperties fromRoom = rooms[randomRoom];

            // Choose a random direction to place the room
            // TODO: Eventually choose a random wall off a room.
            int randRoomDir = UnityEngine.Random.Range(0, 4);

            // Checks if we can make a room in this direction
            if (fromRoom.DirectionsFilled[randRoomDir] == false)
            {
				// Choose a width and height
                int width = roomDimensions[UnityEngine.Random.Range(0, roomDimensions.Count)];
                int height = roomDimensions[UnityEngine.Random.Range(0, roomDimensions.Count)];

				// If we are ready to place the boss room.
				if (roomsPlaced == roomsToPlace - 2)
                {
                    GenerateBossRoom(22, 22, fromRoom, (Floor.TransitionDirection)randRoomDir);
                }
                else
                {
                    // Choose a random feature
                    FeatureType roomToMake = ChooseFeatureRoom();

                    // See if we can add a new room through the chosen direction.
                    GenerateNewRoom(width, height, roomToMake, fromRoom, (Floor.TransitionDirection)randRoomDir);
                }
            }
            else
            {
                roomsPlaced--;
            }
        }

        GenerateWalls();

        //saver.AddNewRoom(firstRoom);

        //Debug.Log(firstRoom.Name);
        //Debug.Log(firstRoom.Width);
        //Debug.Log(firstRoom.Height);
        //Debug.Log(firstRoom.NumberOfTilesX);
        //Debug.Log(firstRoom.NumberOfTilesY);
        //Debug.Log(firstRoom.Position);
        //Debug.Log(firstRoom.RoomType);
        //Debug.Log(firstRoom.WallsPlaced);

        saver.SaveAllRooms();
    }

    /// <summary>
    /// Calculate the type of room to produce based on rarity. With the number of rooms
    /// placed so far we can use that as a factor.
    /// </summary>
    /// <returns>The type of room to create.</returns>
    private FeatureType ChooseFeatureRoom()
    {
        FeatureType type = FeatureType.monster;

        int randomChance = UnityEngine.Random.Range(0, 101);

        // 75% Percent chance region
        if (randomChance >= 10 && randomChance <= 100)
        {
            type = FeatureType.monster;
        }
        // 25% chance region
        else if (randomChance <= 10)
        {
            type = FeatureType.treasure;
        }

        return type;
    }

    private void GenerateWalls()
    {
        foreach (RoomProperties room in rooms)
        {
            if (room.WallsPlaced == false)
            {
				roomGeneration.PlaceWalls(room);
            }
        }
    }

    public void PopulateRooms()
    {
        // Add barrels and misc objects.
        PopulateRoomsMisc();

        // Add monsters / treasure chests.
        foreach (RoomProperties room in rooms)
        {
            FeatureType type = room.RoomType;

            switch (type)
            {
				case FeatureType.none:
					break;
                case FeatureType.monster:
                    // Populate room with monsters!
                    //room.Room.GenerateMonsterSpawnLoc(dungeonLevel, room, monsterRarity);
                    roomGeneration.PopulateMonsters(dungeonLevel, room, monsterRarity);
                    break;

                case FeatureType.treasure:
                    GameObject go = room.Room.InstantiateGameObject(Room.ERoomObjects.Chest, "Chest");
					go.transform.rotation = Quaternion.LookRotation(Vector3.back);
                    // Place treasure in this room.
                    break;

                case FeatureType.boss:
                    roomGeneration.PopulateBossRoom(dungeonLevel, room);
                    //roomGeneration.PopulateMonsters(dungeonLevel, room, monsterRarity);
                    break;

                case FeatureType.trap:
                    // Place a trap here.
                    break;
            }
        }
    }


    /// <summary>
    /// Populates the room with random decoration.
    /// </summary>
    private void PopulateRoomsMisc()
    {
        foreach (RoomProperties room in rooms)
        {
            roomGeneration.PopulateMiscObjects(room);
        }
    }

    /// <summary>
    /// Generates a boss room that is the furtherest away from the start room.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="from"></param>
    /// <param name="dir"></param>
    private void GenerateBossRoom(float width, float height, RoomProperties from, Floor.TransitionDirection dir)
    {
        RoomProperties furtherestRoom = null;
        RoomProperties startRoom = rooms[0];
        float distance = 0.0f;

        // Place this room furtherest away from the start room.
        foreach (RoomProperties room in rooms)
        {
            float tempDist = Vector3.Distance(startRoom.Position, room.Position);

            if (tempDist >= distance)
            {
                distance = tempDist;
                furtherestRoom = room;
            }
        }

        if (furtherestRoom != null)
        {
            for (int i = 0; i < 4; ++i)
            {
                if (furtherestRoom.DirectionsFilled[i] != true)
                {
                    // Choose a feature to build based on weights.
                    GenerateNewRoom(width, height, FeatureType.boss, furtherestRoom, (Floor.TransitionDirection)i);
                    return;
                }
            }

            Debug.Log("Could not place the boss room off this room");
        }
        else
        {
            Debug.Log("Could not find furtherest room");
        }
    }

    public RoomProperties GenerateNewRoom(float width, float height, FeatureType feature, RoomProperties from, Floor.TransitionDirection dir)
    {
        RoomProperties room = null;

        // Test placing the new room.
        if (TestRoomPlacement(dir, from, width, height) == true)
        {
            // Build the new room.
            BuildFeatureRoom(dir, width, height, feature, from);
        }
        else
        {
            roomsPlaced--;
        }

        return room;
    }

    public void GenerateNewRoom(RoomProperties roomToMake, RoomProperties from, Floor.TransitionDirection dir)
    {
        // Test placing the new room.
        if (TestRoomPlacement(dir, from, roomToMake.Width, roomToMake.Height) == true)
        {
            // Build the new room.
            BuildFeatureRoom(dir, roomToMake.Width, roomToMake.Height, roomToMake.RoomType, from);
        }
        else
        {
            roomsPlaced--;
        }
    }

    private void BuildFeatureRoom(Floor.TransitionDirection dir, float width, float height, FeatureType roomType, RoomProperties fromRoom)
    {
        string name = "room";
        switch (roomType)
        {
            case FeatureType.monster:
                name = "Room " + roomsPlaced + ": " + "Monster";
                break;

            case FeatureType.boss:
                name = "Room " + roomsPlaced + ": " + "Boss";
                break;

            case FeatureType.trap:
                name = "Room " + roomsPlaced + ": " + "Trap";
                break;

            case FeatureType.treasure:
                name = "Room " + roomsPlaced + ": " + "Treasure";
                break;

        }

        // TODO: If we know the feature to create we can choose the right one to create here.
        RoomProperties newRoom = roomGeneration.CreateNewRoom((int)width, (int)height, name);
        newRoom.Room.transform.position = locationVector;
        newRoom.RoomType = roomType;

        fromRoom.FillDirection(dir);
        Transform doors = fromRoom.Room.GetNodeByLayer("Environment").transform.FindChild("Doors");
        Door entryDoor = roomGeneration.CreateDoor(doors.gameObject, fromRoom, dir);
        entryDoor.direction = dir;

        // TODO: Find a way to get rid of this switch its too big.
        switch (dir)
        {
            case Floor.TransitionDirection.North:
                {
                    // Create the new room with number of doors.
                    // we also need to create the door that connects the previous room.
                    newRoom.FillDirection(Floor.TransitionDirection.South); // We set this position to filled because its where the other door came from
                    newRoom.Position = locationVector;
                    rooms.Add(newRoom);

                    // Generate the door for this new room and link it to the previous room.
                    Transform newDoor = newRoom.Room.GetNodeByLayer("Environment").transform.FindChild("Doors");
					Door exitDoor = roomGeneration.CreateDoor(newDoor.gameObject, newRoom, Floor.TransitionDirection.South);
                    exitDoor.direction = Floor.TransitionDirection.South;

                    // Link the doors
                    entryDoor.targetDoor = exitDoor;
                    exitDoor.targetDoor = entryDoor;
                }
                break;

            case Floor.TransitionDirection.East:
                {
                    // Create the new room with number of doors.
                    // we also need to create the door that connects the previous room.
                    newRoom.FillDirection(Floor.TransitionDirection.West); // We set this position to filled because its where the other door came from
                    newRoom.Position = locationVector;
                    rooms.Add(newRoom);

                    // Generate the door for this new room and link it to the previous room.
                    Transform newDoor = newRoom.Room.GetNodeByLayer("Environment").transform.FindChild("Doors");
					Door exitDoor = roomGeneration.CreateDoor(newDoor.gameObject, newRoom, Floor.TransitionDirection.West);
                    exitDoor.direction = Floor.TransitionDirection.West;

                    // Link the doors
                    entryDoor.targetDoor = exitDoor;
                    exitDoor.targetDoor = entryDoor;
                }
                break;

            case Floor.TransitionDirection.South:
                {
                    // Create the new room with number of doors.
                    // we also need to create the door that connects the previous room.
                    newRoom.FillDirection(Floor.TransitionDirection.North); // We set this position to filled because its where the other door came from
                    newRoom.Position = locationVector;
                    rooms.Add(newRoom);

                    // Generate the door for this new room and link it to the previous room.
                    Transform newDoor = newRoom.Room.GetNodeByLayer("Environment").transform.FindChild("Doors");
					Door exitDoor = roomGeneration.CreateDoor(newDoor.gameObject, newRoom, Floor.TransitionDirection.North);
                    exitDoor.direction = Floor.TransitionDirection.North;

                    // Link the doors
                    entryDoor.targetDoor = exitDoor;
                    exitDoor.targetDoor = entryDoor;
                }
                break;

            case Floor.TransitionDirection.West:
                {
                    // Create the new room with number of doors.
                    // we also need to create the door that connects the previous room.
                    newRoom.FillDirection(Floor.TransitionDirection.East); // We set this position to filled because its where the other door came from
                    newRoom.Position = locationVector;
                    rooms.Add(newRoom);

                    // Generate the door for this new room and link it to the previous room.
                    Transform newDoor = newRoom.Room.GetNodeByLayer("Environment").transform.FindChild("Doors");
					Door exitDoor = roomGeneration.CreateDoor(newDoor.gameObject, newRoom, Floor.TransitionDirection.East);
                    exitDoor.direction = Floor.TransitionDirection.East;

                    // Link the doors
                    entryDoor.targetDoor = exitDoor;
                    exitDoor.targetDoor = entryDoor;
                    break;
                }
        }
    }

    /// <summary>
    /// Tests to see if a room can be placed off of another room in the set direction, offset, width and height.
    /// </summary>
    /// <param name="dir">The direction to build the room off another.</param>
    /// <param name="from">The room that we will test building off of.</param>
    /// <param name="width">The width of the new room.</param>
    /// <param name="height">The height of the new room.</param>
    /// <returns>Returns true if a room can be placed here.</returns>
    private bool TestRoomPlacement(Floor.TransitionDirection dir, RoomProperties from, float width, float height)
    {
        Bounds testBounds = new Bounds();

        // Find the new location of the room.
        switch (dir)
        {
            // North
            case Floor.TransitionDirection.North:
                // Vector to test new location. 
                //locationVector = new Vector3(from.position.x, 0.0f, ((from.position.z + from.height * 0.5f) + (height * roomOffsetMultiplier)));
                locationVector = new Vector3(from.Position.x, 0.0f, ((from.Position.z + from.Height * 0.5f) + roomOffsetValue));
                break;

            // East
            case Floor.TransitionDirection.East:

                //locationVector = new Vector3(((from.position.x + from.width * 0.5f) + (width * roomOffsetMultiplier)), 0.0f, from.position.z);
                locationVector = new Vector3(((from.Position.x + from.Width * 0.5f) + roomOffsetValue), 0.0f, from.Position.z);
                break;

            // South
            case Floor.TransitionDirection.South:
                //locationVector = new Vector3(from.position.x, 0.0f, ((from.position.z - from.height * 0.5f) - (height * roomOffsetMultiplier)));
                locationVector = new Vector3(from.Position.x, 0.0f, ((from.Position.z - from.Height * 0.5f) - roomOffsetValue));
                break;

            // West
            case Floor.TransitionDirection.West:
                //locationVector = new Vector3(((from.position.x - from.width * 0.5f) - (width * roomOffsetMultiplier)), 0.0f, from.position.z);
                locationVector = new Vector3(((from.Position.x - from.Width * 0.5f) - roomOffsetValue), 0.0f, from.Position.z);
                break;
        }

        // Test to see if the room intersects anywhere.
        testBounds = new Bounds(locationVector, new Vector3(width, 1.0f, height));

        // Check to see if the position is filled by another room.
        for (int a = 0; a < rooms.Count; a++)
        {
            if (testBounds.Intersects(rooms[a].Bounds))
            {
                return (false);
            }
        }

        return (true);
    }
}
