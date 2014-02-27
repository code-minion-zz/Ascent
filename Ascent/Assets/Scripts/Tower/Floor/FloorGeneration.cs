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
    private List<RoomProperties> loadedRooms = new List<RoomProperties>();
    private List<RoomProperties> placedRooms = new List<RoomProperties>();
    private List<int> roomDimensions = new List<int>();
    private Vector3 locationVector = Vector3.zero;
    private int roomsPlaced = 0;

    public List<RoomProperties> Rooms
    {
        get { return placedRooms; }
    }

    public void GenerateFloor()
    {
        roomDimensions.Add(18);
        roomDimensions.Add(14);
        roomDimensions.Add(10);

        //Random.seed = (int)System.DateTime.Today.Millisecond;
        UnityEngine.Random.seed = (int)System.DateTime.Now.TimeOfDay.Ticks;

        LoadCustomRooms();
        CreateAndPlaceRooms();
    }

    private void LoadCustomRooms()
    {
        // Save data from the first room as a test.
        SaveRooms saver = new SaveRooms();

        // Generate the first room in the game.
        RoomProperties firstRoom = saver.LoadRoom("Maps/ArrowShooter", true);
        roomGeneration.ReconstructRoom(firstRoom);
        placedRooms.Add(firstRoom);
        loadedRooms = saver.LoadAllRooms("Assets/Resources/Maps");
    }

    private void CreateAndPlaceRooms()
    {
        // Go through and place all the floor components based on the number of them we have.
        for (roomsPlaced = 1; roomsPlaced < roomsToPlace+1; roomsPlaced++)
        {
            RoomProperties room = SelectRandomRoom();

            // Check if this room is a custom loaded room. 
            // If so we will handle placement so that all the doors are handled.
            if (room.IsPreloaded)
            {
                HandleCustomRoomPlacement(room);
            }
            else
            {
                HandleRandomRoomPlacement(room);
            }
        }

        GenerateWalls();
    }

    private Door ChooseDoor(RoomProperties room)
    {
        if (room.IsPreloaded)
        {
            foreach (Door door in room.Doors)
            {
                if (!door.isConnected)
                {
                    return door;
                }
            }
        }

        return null;
    }

    private RoomProperties SelectRandomRoom()
    {
        // Pick a random room from the pool of rooms that currently exist
        int randomRoom = UnityEngine.Random.Range(0, placedRooms.Count);
        RoomProperties fromRoom = placedRooms[randomRoom];

        return fromRoom;
    }

    private void HandleCustomRoomPlacement(RoomProperties fromRoom)
    {
        // Check the room for a door that is required.
        Door door = ChooseDoor(fromRoom);

        if (door != null)
        {

            // Choose a width and height for the new room to build off.
            int width = roomDimensions[UnityEngine.Random.Range(0, roomDimensions.Count)];
            int height = roomDimensions[UnityEngine.Random.Range(0, roomDimensions.Count)];

            // Choose a random feature
            FeatureType roomToMake = ChooseFeatureRoom();

            // See if we can add a new room through the chosen door.
            GenerateNewRoom(width, height, roomToMake, fromRoom, door);
        }
        else
        {
            roomsPlaced--;
        }
    }

    private void HandleRandomRoomPlacement(RoomProperties fromRoom)
    {
        // Choose a random direction to place the room
        int randRoomDir = UnityEngine.Random.Range(0, 4);

        // Checks if we can make a room in this direction
        if (fromRoom.DirectionsFilled[randRoomDir] == false)
        {
            // Choose to either load a room or create a new one.
            int randomChance = UnityEngine.Random.Range(0, 101);

            // 50% Percent chance region
            if (randomChance >= 50 && randomChance <= 100)
            {
                // Pick a random room from the pool of rooms that currently exist
                int randomRoom = UnityEngine.Random.Range(0, loadedRooms.Count);
                RoomProperties room = loadedRooms[randomRoom];

                PlaceLoadedRoom(room, fromRoom, (Floor.TransitionDirection)randRoomDir);
            }
            else
            {

                // Choose a width and height
                int width = roomDimensions[UnityEngine.Random.Range(0, roomDimensions.Count)];
                int height = roomDimensions[UnityEngine.Random.Range(0, roomDimensions.Count)];

                // If we are ready to place the boss room.
                if (roomsPlaced == roomsToPlace)
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
        }
        else
        {
            roomsPlaced--;
        }
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
        foreach (RoomProperties room in placedRooms)
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
        foreach (RoomProperties room in placedRooms)
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
        foreach (RoomProperties room in placedRooms)
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
        RoomProperties startRoom = placedRooms[0];
        float distance = 0.0f;

        // Place this room furtherest away from the start room.
        foreach (RoomProperties room in placedRooms)
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

    public void PlaceLoadedRoom(RoomProperties room, RoomProperties from, Floor.TransitionDirection dir)
    {
        if (TestRoomPlacement(dir, from, room.Width, room.Height) == true)
        {
            LinkRooms(room, from, dir);
        }
        else
        {
            roomsPlaced--;
        }
    }

    public void GenerateNewRoom(float width, float height, FeatureType feature, RoomProperties from, Floor.TransitionDirection dir)
    {
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
    }

    public void GenerateNewRoom(float width, float height, FeatureType feature, RoomProperties fromRoom, Door fromDoor)
    {
        if (TestRoomPlacement(fromDoor, width, height) == true)
        {
            BuildFeatureRoom(fromDoor, width, height, feature, fromRoom);
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
				name = "Room " + roomsPlaced + ": " + "Monster" + " [" + width + ", " + height + "]";
                break;

            case FeatureType.boss:
				name = "Room " + roomsPlaced + ": " + "Boss" + " [" + width + ", " + height + "]";
                break;

            case FeatureType.trap:
				name = "Room " + roomsPlaced + ": " + "Trap" + " [" + width + ", " + height + "]";
                break;

            case FeatureType.treasure:
				name = "Room " + roomsPlaced + ": " + "Treasure" + " [" + width + ", " + height + "]";
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
        entryDoor.isConnected = true;

        // TODO: Find a way to get rid of this switch its too big.
        switch (dir)
        {
            case Floor.TransitionDirection.North:
                {
                    // Create the new room with number of doors.
                    // we also need to create the door that connects the previous room.
                    newRoom.FillDirection(Floor.TransitionDirection.South); // We set this position to filled because its where the other door came from
                    newRoom.Position = locationVector;
                    placedRooms.Add(newRoom);

                    // Generate the door for this new room and link it to the previous room.
                    Transform newDoor = newRoom.Room.GetNodeByLayer("Environment").transform.FindChild("Doors");
					Door exitDoor = roomGeneration.CreateDoor(newDoor.gameObject, newRoom, Floor.TransitionDirection.South);
                    exitDoor.direction = Floor.TransitionDirection.South;

                    // Link the doors
                    entryDoor.targetDoor = exitDoor;
                    exitDoor.targetDoor = entryDoor;
                    exitDoor.isConnected = true;
                }
                break;

            case Floor.TransitionDirection.East:
                {
                    // Create the new room with number of doors.
                    // we also need to create the door that connects the previous room.
                    newRoom.FillDirection(Floor.TransitionDirection.West); // We set this position to filled because its where the other door came from
                    newRoom.Position = locationVector;
                    placedRooms.Add(newRoom);

                    // Generate the door for this new room and link it to the previous room.
                    Transform newDoor = newRoom.Room.GetNodeByLayer("Environment").transform.FindChild("Doors");
					Door exitDoor = roomGeneration.CreateDoor(newDoor.gameObject, newRoom, Floor.TransitionDirection.West);
                    exitDoor.direction = Floor.TransitionDirection.West;

                    // Link the doors
                    entryDoor.targetDoor = exitDoor;
                    exitDoor.targetDoor = entryDoor;
                    exitDoor.isConnected = true;
                }
                break;

            case Floor.TransitionDirection.South:
                {
                    // Create the new room with number of doors.
                    // we also need to create the door that connects the previous room.
                    newRoom.FillDirection(Floor.TransitionDirection.North); // We set this position to filled because its where the other door came from
                    newRoom.Position = locationVector;
                    placedRooms.Add(newRoom);

                    // Generate the door for this new room and link it to the previous room.
                    Transform newDoor = newRoom.Room.GetNodeByLayer("Environment").transform.FindChild("Doors");
					Door exitDoor = roomGeneration.CreateDoor(newDoor.gameObject, newRoom, Floor.TransitionDirection.North);
                    exitDoor.direction = Floor.TransitionDirection.North;

                    // Link the doors
                    entryDoor.targetDoor = exitDoor;
                    exitDoor.targetDoor = entryDoor;
                    exitDoor.isConnected = true;
                }
                break;

            case Floor.TransitionDirection.West:
                {
                    // Create the new room with number of doors.
                    // we also need to create the door that connects the previous room.
                    newRoom.FillDirection(Floor.TransitionDirection.East); // We set this position to filled because its where the other door came from
                    newRoom.Position = locationVector;
                    placedRooms.Add(newRoom);

                    // Generate the door for this new room and link it to the previous room.
                    Transform newDoor = newRoom.Room.GetNodeByLayer("Environment").transform.FindChild("Doors");
					Door exitDoor = roomGeneration.CreateDoor(newDoor.gameObject, newRoom, Floor.TransitionDirection.East);
                    exitDoor.direction = Floor.TransitionDirection.East;

                    // Link the doors
                    entryDoor.targetDoor = exitDoor;
                    exitDoor.targetDoor = entryDoor;
                    exitDoor.isConnected = true;
                    break;
                }
        }
    }

    private void BuildFeatureRoom(Door fromDoor, float width, float height, FeatureType roomType, RoomProperties fromRoom)
    {
        string name = "room";
        switch (roomType)
        {
            case FeatureType.monster:
				name = "Room " + roomsPlaced + "[F]: " + "Monster" + " [" + width + ", " + height + "]";
                break;

            case FeatureType.boss:
				name = "Room " + roomsPlaced + "[F]: " + "Boss" + " [" + width + ", " + height + "]";
                break;

            case FeatureType.trap:
				name = "Room " + roomsPlaced + "[F]: " + "Trap" + " [" + width + ", " + height + "]";
                break;

            case FeatureType.treasure:
				name = "Room " + roomsPlaced + "[F]: " + "Treasure" + " [" + width + ", " + height + "]";
                break;

        }

        // TODO: If we know the feature to create we can choose the right one to create here.
        RoomProperties newRoom = roomGeneration.CreateNewRoom((int)width, (int)height, name);
        newRoom.Room.transform.position = locationVector;
        newRoom.RoomType = roomType;

        Floor.TransitionDirection dir = fromDoor.direction;

        Transform doors = fromRoom.Room.GetNodeByLayer("Environment").transform.FindChild("Doors");
        Door entryDoor = fromDoor;
        entryDoor.transform.parent = doors;

        Doors doorsScript = doors.GetComponent<Doors>();
        doorsScript.doors[(int)dir] = entryDoor;

        entryDoor.direction = dir;
        entryDoor.isConnected = true;

        // TODO: Find a way to get rid of this switch its too big.
        switch (dir)
        {
            case Floor.TransitionDirection.North:
                {
                    // Create the new room with number of doors.
                    // we also need to create the door that connects the previous room.
                    newRoom.FillDirection(Floor.TransitionDirection.South); // We set this position to filled because its where the other door came from
                    newRoom.Position = locationVector;
                    placedRooms.Add(newRoom);

                    // Generate the door for this new room and link it to the previous room.
                    Transform newDoor = newRoom.Room.GetNodeByLayer("Environment").transform.FindChild("Doors");
                    Door exitDoor = roomGeneration.CreateDoor(newDoor.gameObject, newRoom, Floor.TransitionDirection.South);
                    exitDoor.direction = Floor.TransitionDirection.South;

                    // Link the doors
                    entryDoor.targetDoor = exitDoor;
                    exitDoor.targetDoor = entryDoor;
                    exitDoor.isConnected = true;
                }
                break;

            case Floor.TransitionDirection.East:
                {
                    // Create the new room with number of doors.
                    // we also need to create the door that connects the previous room.
                    newRoom.FillDirection(Floor.TransitionDirection.West); // We set this position to filled because its where the other door came from
                    newRoom.Position = locationVector;
                    placedRooms.Add(newRoom);

                    // Generate the door for this new room and link it to the previous room.
                    Transform newDoor = newRoom.Room.GetNodeByLayer("Environment").transform.FindChild("Doors");
                    Door exitDoor = roomGeneration.CreateDoor(newDoor.gameObject, newRoom, Floor.TransitionDirection.West);
                    exitDoor.direction = Floor.TransitionDirection.West;

                    // Link the doors
                    entryDoor.targetDoor = exitDoor;
                    exitDoor.targetDoor = entryDoor;
                    exitDoor.isConnected = true;
                }
                break;

            case Floor.TransitionDirection.South:
                {
                    // Create the new room with number of doors.
                    // we also need to create the door that connects the previous room.
                    newRoom.FillDirection(Floor.TransitionDirection.North); // We set this position to filled because its where the other door came from
                    newRoom.Position = locationVector;
                    placedRooms.Add(newRoom);

                    // Generate the door for this new room and link it to the previous room.
                    Transform newDoor = newRoom.Room.GetNodeByLayer("Environment").transform.FindChild("Doors");
                    Door exitDoor = roomGeneration.CreateDoor(newDoor.gameObject, newRoom, Floor.TransitionDirection.North);
                    exitDoor.direction = Floor.TransitionDirection.North;

                    // Link the doors
                    entryDoor.targetDoor = exitDoor;
                    exitDoor.targetDoor = entryDoor;
                    exitDoor.isConnected = true;
                }
                break;

            case Floor.TransitionDirection.West:
                {
                    // Create the new room with number of doors.
                    // we also need to create the door that connects the previous room.
                    newRoom.FillDirection(Floor.TransitionDirection.East); // We set this position to filled because its where the other door came from
                    newRoom.Position = locationVector;
                    placedRooms.Add(newRoom);

                    // Generate the door for this new room and link it to the previous room.
                    Transform newDoor = newRoom.Room.GetNodeByLayer("Environment").transform.FindChild("Doors");
                    Door exitDoor = roomGeneration.CreateDoor(newDoor.gameObject, newRoom, Floor.TransitionDirection.East);
                    exitDoor.direction = Floor.TransitionDirection.East;

                    // Link the doors
                    entryDoor.targetDoor = exitDoor;
                    exitDoor.targetDoor = entryDoor;
                    exitDoor.isConnected = true;
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
                locationVector = new Vector3(from.Position.x, 0.0f, ((from.Position.z + from.Height * 0.5f) + roomOffsetValue));
                break;

            // East
            case Floor.TransitionDirection.East:
                locationVector = new Vector3(((from.Position.x + from.Width * 0.5f) + roomOffsetValue), 0.0f, from.Position.z);
                break;

            // South
            case Floor.TransitionDirection.South:
                locationVector = new Vector3(from.Position.x, 0.0f, ((from.Position.z - from.Height * 0.5f) - roomOffsetValue));
                break;

            // West
            case Floor.TransitionDirection.West:
                locationVector = new Vector3(((from.Position.x - from.Width * 0.5f) - roomOffsetValue), 0.0f, from.Position.z);
                break;
        }

        // Test to see if the room intersects anywhere.
        testBounds = new Bounds(locationVector, new Vector3(width, 1.0f, height));

        // Check to see if the position is filled by another room.
        for (int a = 0; a < placedRooms.Count; a++)
        {
            if (testBounds.Intersects(placedRooms[a].Bounds))
            {
                return (false);
            }
        }

        return (true);
    }

    private void LinkRooms(RoomProperties room, RoomProperties from, Floor.TransitionDirection dir)
    {
        roomGeneration.ReconstructRoom(room);
        room.Room.transform.position = locationVector;

        string name = "Room " + roomsPlaced + "[F]: " + room.Name + " [" + room.Width + ", " + room.Height + "]";
        room.Room.name = name;


        from.FillDirection(dir);
        Transform doors = from.Room.GetNodeByLayer("Environment").transform.FindChild("Doors");
        Door entryDoor = roomGeneration.CreateDoor(doors.gameObject, from, dir);
        entryDoor.direction = dir;

        Door exitDoor = ChooseDoor(room);

        switch (dir)
        {
            case Floor.TransitionDirection.North:
                {
                    switch (exitDoor.direction)
                    {
                        case Floor.TransitionDirection.North:
                            room.Room.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 180.0f);
                            exitDoor.direction = Floor.TransitionDirection.South;
                            break;

                        case Floor.TransitionDirection.East:
                            room.Room.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 90.0f);
                            exitDoor.direction = Floor.TransitionDirection.South;
                            break;

                        case Floor.TransitionDirection.South:
                            // Do nothing
                            break;

                        case Floor.TransitionDirection.West:
                            room.Room.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), -90.0f);
                            exitDoor.direction = Floor.TransitionDirection.South;
                            break;
                    }
                }
                break;

            case Floor.TransitionDirection.East:
                {
                    switch (exitDoor.direction)
                    {
                        case Floor.TransitionDirection.North:
                            room.Room.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), -90.0f);
                            exitDoor.direction = Floor.TransitionDirection.West;
                            break;

                        case Floor.TransitionDirection.East:
                            room.Room.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), -180.0f);
                            exitDoor.direction = Floor.TransitionDirection.West;
                            break;

                        case Floor.TransitionDirection.South:
                            room.Room.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 90.0f);
                            exitDoor.direction = Floor.TransitionDirection.West;
                            break;

                        case Floor.TransitionDirection.West:
                            // Do nothing
                            break;
                    }
                }
                break;

            case Floor.TransitionDirection.South:
                {
                    switch (exitDoor.direction)
                    {
                        case Floor.TransitionDirection.North:
                            // No need to rotate.
                            break;

                        case Floor.TransitionDirection.East:
                            room.Room.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), -90.0f);
                            exitDoor.direction = Floor.TransitionDirection.North;
                            break;

                        case Floor.TransitionDirection.South:
                            room.Room.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), -180.0f);
                            exitDoor.direction = Floor.TransitionDirection.North;
                            break;

                        case Floor.TransitionDirection.West:
                            room.Room.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 90.0f);
                            exitDoor.direction = Floor.TransitionDirection.North;
                            break;
                    }
                }
                break;

            case Floor.TransitionDirection.West:
                {
                    switch (exitDoor.direction)
                    {
                        case Floor.TransitionDirection.North:
                            room.Room.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 90.0f);
                            exitDoor.direction = Floor.TransitionDirection.East;
                            break;

                        case Floor.TransitionDirection.East:
                            // No need to rotate.
                            break;

                        case Floor.TransitionDirection.South:
                            room.Room.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 90.0f);
                            exitDoor.direction = Floor.TransitionDirection.East;
                            break;

                        case Floor.TransitionDirection.West:
                            room.Room.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 180.0f);
                            exitDoor.direction = Floor.TransitionDirection.East;
                            break;
                    }
                }
                break;
        }

        // Link the doors
        entryDoor.targetDoor = exitDoor;
        exitDoor.targetDoor = entryDoor;
        entryDoor.isConnected = true;
        exitDoor.isConnected = true;

        placedRooms.Add(room);
    }

    private bool TestRoomPlacement(Door fromDoor, float width, float height)
    {
        Bounds testBounds = new Bounds();
        
        Floor.TransitionDirection dir = fromDoor.direction;
        Transform from = fromDoor.transform;

        // Find the new location of the room.
        switch (dir)
        {
            // North
            case Floor.TransitionDirection.North:
                // Vector to test new location. 
                locationVector = new Vector3(from.position.x, 0.0f, from.position.z + roomOffsetValue);
                break;

            // East
            case Floor.TransitionDirection.East:
                locationVector = new Vector3(from.position.x + roomOffsetValue, 0.0f, from.position.z);
                break;

            // South
            case Floor.TransitionDirection.South:
                locationVector = new Vector3(from.position.x, 0.0f, from.position.z - roomOffsetValue);
                break;

            // West
            case Floor.TransitionDirection.West:
                locationVector = new Vector3(from.position.x - roomOffsetValue, 0.0f, from.position.z);
                break;
        }

        // Test to see if the room intersects anywhere.
        testBounds = new Bounds(locationVector, new Vector3(width, 1.0f, height));

        // Check to see if the position is filled by another room.
        for (int a = 0; a < placedRooms.Count; a++)
        {
            if (testBounds.Intersects(placedRooms[a].Bounds))
            {
                return (false);
            }
        }

        return (true);
    }
}
