using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    public int roomsToPlace = 30;
    public float roomOffsetValue = 25.0f;

    // Rarity controls
    public Rarity treasureChestSpawn;
    public Rarity trapRoom;
    public Rarity monsterRoom;
    public Rarity specialRoom;

    private List<RoomProperties> rooms = new List<RoomProperties>();
    private List<int> roomDimensions = new List<int>();
    private Vector3 locationVector;
    private int roomsPlaced = 0;

    // Objects used for room creation
    private GameObject floorObject;
    private GameObject wallObject;
    private GameObject wallCorner;
    private GameObject wallWindow;
    private GameObject doorObject;

    public void GenerateFloor()
    {
        floorObject = Resources.Load("Prefabs/RoomWalls/Ground") as GameObject;
        wallObject = Resources.Load("Prefabs/RoomWalls/Wall") as GameObject;
        wallCorner = Resources.Load("Prefabs/RoomWalls/WallCorner") as GameObject;
        wallWindow = Resources.Load("Prefabs/RoomWalls/WallWindow") as GameObject;
        doorObject = Resources.Load("Prefabs/RoomWalls/Door") as GameObject;

        roomDimensions.Add(18);
        roomDimensions.Add(14);
        roomDimensions.Add(10);
        roomDimensions.Add(6);

        //Random.seed = (int)System.DateTime.Today.Millisecond;
        Random.seed = (int)System.DateTime.Now.TimeOfDay.Ticks;
        CreateRooms();
    }

    public void CreateRooms()
    {
        rooms.Clear();
        rooms = new List<RoomProperties>();
        locationVector = Vector3.zero;

        // We have to add the first room, for now we add at pos zero
        Room startRoom = GameObject.Find("StartRoom").GetComponent<Room>();
        startRoom.Initialise();

        RoomProperties firstRoom = new RoomProperties(startRoom);
        firstRoom.Position = startRoom.transform.position;
        firstRoom.Width = 18;
        firstRoom.Height = 14;
        firstRoom.WallsPlaced = true;
        rooms.Add(firstRoom);

        // Go through and place all the floor components based on the number of them we have.
        for (roomsPlaced = 0; roomsPlaced < roomsToPlace; roomsPlaced++)
        {
            // Pick a random room from the pool of rooms that currently exist
            int randomRoom = Random.Range(0, rooms.Count);
            RoomProperties fromRoom = rooms[randomRoom];

            // Choose a random direction to place the room
            int randRoomDir = Random.Range(0, 4);

            // Checks if we can make a room in this direction
            if (fromRoom.directionsFilled[randRoomDir] == false)
            {
                // If we are ready to place the boss room.
                if (roomsPlaced == roomsToPlace - 2)
                {
                    GenerateBossRoom(22, 22, fromRoom, (Floor.TransitionDirection)randRoomDir);
                }
                else
                {
                    // TODO: Generate room size variations
                    int width = roomDimensions[Random.Range(0, roomDimensions.Count)];
                    int height = roomDimensions[Random.Range(0, roomDimensions.Count)];

                    // See if we can add a new room through the chosen direction.
                    GenerateNewRoom(width, height, "Room " + roomsPlaced, fromRoom, (Floor.TransitionDirection)randRoomDir);
                }
            }
            else
            {
                roomsPlaced--;
            }
        }

        GenerateWalls();
    }

    /// <summary>
    /// Calculate the type of room to produce based on rarity. With the number of rooms
    /// placed so far we can use that as a factor.
    /// </summary>
    /// <returns>The type of room to create.</returns>
    private FeatureType ChooseFeatureRoom()
    {
        FeatureType type = FeatureType.monster;

        int randomChance = Random.Range(0, 101);

        // Less than 5 percent chance
        if (randomChance <= 5)
        {

        }

        return type;
    }

    private void GenerateWalls()
    {
        foreach (RoomProperties room in rooms)
        {
            if (room.WallsPlaced == false)
            {
                PlaceWalls(room);
            }
        }
    }

    /// <summary>
    /// Creates a standard room with nothing in it.
    /// </summary>
    /// <param name="width">Width of the room.</param>
    /// <param name="height">Height of the room.</param>
    /// <param name="name">"Name of the room.</param>
    /// <returns>The newly created room Game Object</returns>
    private GameObject CreateRoom(float width, float height, string name)
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

        // Create the floor.
        GameObject floorGo = GameObject.Instantiate(floorObject, Vector3.zero, floorObject.transform.rotation) as GameObject;
        floorGo.transform.localScale = new Vector3(width, height, 1.0f);
        floorGo.transform.parent = room.GetNodeByLayer("Environment").transform;
        floorGo.name = "Ground";

        room.Initialise();

        // Apply the new dimensions to the navMesh.
        room.NavMesh.transform.localScale = new Vector3(width - 1.0f, height - 1.0f, 0.0f);
        room.minCamera.x = -width * 0.15f;
        room.minCamera.z = -height * 0.15f;
        room.maxCamera.z = height * 0.15f;
        room.maxCamera.x = width * 0.15f;
        return roomGo;
    }

    private Door CreateDoor(GameObject doors, RoomProperties fromRoom, Floor.TransitionDirection direction)
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

        switch (direction)
        {
            case Floor.TransitionDirection.North:
                //doorGo.transform.position = new Vector3(doors.transform.position.x, doorGo.transform.position.y, doors.transform.position.z + 8.0f);
                doorGo.transform.position = new Vector3(doors.transform.position.x, doorGo.transform.position.y, doors.transform.position.z + heightOffset);
                doorGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 90.0f);
                doorGo.name = "North Door";
                break;

            case Floor.TransitionDirection.East:
                //doorGo.transform.position = new Vector3(doors.transform.position.x + 8.0f, doorGo.transform.position.y, doors.transform.position.z);
                doorGo.transform.position = new Vector3(doors.transform.position.x + widthOffset, doorGo.transform.position.y, doors.transform.position.z);
                doorGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 180.0f);
                doorGo.name = "East Door";
                break;

            case Floor.TransitionDirection.South:
                //doorGo.transform.position = new Vector3(doors.transform.position.x, doorGo.transform.position.y, doors.transform.position.z - 8.0f);
                doorGo.transform.position = new Vector3(doors.transform.position.x, doorGo.transform.position.y, doors.transform.position.z - heightOffset);
                doorGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 270.0f);
                doorGo.name = "South Door";
                break;

            case Floor.TransitionDirection.West:
                //doorGo.transform.position = new Vector3(doors.transform.position.x - 8, doorGo.transform.position.y, doors.transform.position.z);
                doorGo.transform.position = new Vector3(doors.transform.position.x - widthOffset, doorGo.transform.position.y, doors.transform.position.z);
                doorGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 0.0f);
                doorGo.name = "West Door";
                break;

        }

        return (returnDoor);
    }

    private void GenerateBossRoom(float width, float height, RoomProperties from, Floor.TransitionDirection dir)
    {
        RoomProperties bossRoom = GenerateNewRoom(width, height, "BoosRoom", from, dir);
    }

    public RoomProperties GenerateNewRoom(float width, float height, string name, RoomProperties from, Floor.TransitionDirection dir)
    {
        RoomProperties room = null;

        // Test placing the new room.
        if (TestRoomPlacement(dir, from, width, height) == true)
        {
            // Choose a feature to build based on weights.
            FeatureType roomToMake = ChooseFeatureRoom();

            // Build the new room.
            BuildFeatureRoom(dir, width, height, roomToMake, from);
        }
        else
        {
            roomsPlaced--;
        }

        return room;
    }

    private void BuildFeatureRoom(Floor.TransitionDirection dir, float width, float height, FeatureType roomType, RoomProperties fromRoom)
    {
        // TODO: If we know the feature to create we can choose the right one to create here.
        GameObject roomGO = CreateRoom(width, height, "Room " + roomsPlaced);
        roomGO.transform.position = locationVector;

        fromRoom.FillDirection(dir);
        Transform doors = fromRoom.Room.GetNodeByLayer("Environment").transform.FindChild("Doors");
        Door entryDoor = CreateDoor(doors.gameObject, fromRoom, dir);
        entryDoor.direction = dir;

        // TODO: Find a way to get rid of this switch its too big.
        switch (dir)
        {
            case Floor.TransitionDirection.North:
                {
                    // Create the new room with number of doors.
                    // we also need to create the door that connects the previous room.
                    RoomProperties newRoom = new RoomProperties(roomGO.GetComponent<Room>());
                    newRoom.FillDirection(Floor.TransitionDirection.South); // We set this position to filled because its where the other door came from
                    newRoom.Position = locationVector;
                    newRoom.Width = width;
                    newRoom.Height = height;
                    rooms.Add(newRoom);

                    // Generate the door for this new room and link it to the previous room.
                    Transform newDoor = roomGO.GetComponent<Room>().GetNodeByLayer("Environment").transform.FindChild("Doors");
                    Door exitDoor = CreateDoor(newDoor.gameObject, newRoom, Floor.TransitionDirection.South);
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
                    RoomProperties newRoom = new RoomProperties(roomGO.GetComponent<Room>());
                    newRoom.FillDirection(Floor.TransitionDirection.West); // We set this position to filled because its where the other door came from
                    newRoom.Position = locationVector;
                    newRoom.Width = width;
                    newRoom.Height = height;
                    rooms.Add(newRoom);

                    // Generate the door for this new room and link it to the previous room.
                    Transform newDoor = roomGO.GetComponent<Room>().GetNodeByLayer("Environment").transform.FindChild("Doors");
                    Door exitDoor = CreateDoor(newDoor.gameObject, newRoom, Floor.TransitionDirection.West);
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
                    RoomProperties newRoom = new RoomProperties(roomGO.GetComponent<Room>());
                    newRoom.FillDirection(Floor.TransitionDirection.North); // We set this position to filled because its where the other door came from
                    newRoom.Position = locationVector;
                    newRoom.Width = width;
                    newRoom.Height = height;
                    rooms.Add(newRoom);

                    // Generate the door for this new room and link it to the previous room.
                    Transform newDoor = roomGO.GetComponent<Room>().GetNodeByLayer("Environment").transform.FindChild("Doors");
                    Door exitDoor = CreateDoor(newDoor.gameObject, newRoom, Floor.TransitionDirection.North);
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
                    RoomProperties newRoom = new RoomProperties(roomGO.GetComponent<Room>());
                    newRoom.FillDirection(Floor.TransitionDirection.East); // We set this position to filled because its where the other door came from
                    newRoom.Position = locationVector;
                    newRoom.Width = width;
                    newRoom.Height = height;
                    rooms.Add(newRoom);

                    // Generate the door for this new room and link it to the previous room.
                    Transform newDoor = roomGO.GetComponent<Room>().GetNodeByLayer("Environment").transform.FindChild("Doors");
                    Door exitDoor = CreateDoor(newDoor.gameObject, newRoom, Floor.TransitionDirection.East);
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

    private void PlaceWalls(RoomProperties room)
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
