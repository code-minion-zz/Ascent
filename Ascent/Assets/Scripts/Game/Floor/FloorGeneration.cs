using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomInfo
{
    public Vector3 position;
    public bool[] directionsFilled;
    public Room room;
    public float width;
    public float height;
    public bool wallsPlaced;

    private int weight;

    public int Weight
    {
        get { return weight; }
        set { weight = value; }
    }

    public RoomInfo(Room room)
    {
        position = Vector3.zero;
        directionsFilled = new bool[4];
        this.room = room;
        wallsPlaced = false;

        for (int i = 0; i < 4; ++i)
        {
            directionsFilled[i] = false;
        }
    }
}

public class FloorGeneration 
{
    public enum Directions
    {
        north = 1,
        easth,
        south,
        west
    }

    private GameObject floorObject;
    private GameObject wallObject;
    private GameObject wallCorner;
    private GameObject wallWindow;
    private GameObject doorObject;

    private List<int> roomDimensions = new List<int>();

    // The rooms need to be seperated from each other to a degree.
    private float roomOffsetMultiplier = 1.0f;

    // Variables to control the outcome of the level
    public int dungeonLevel = 1;
    public int roomsToPlace = 20;
    public int numberOfChests = 5;
    public float roomOffsetValue = 25.0f;


    private int roomsPlaced = 0;
    private List<RoomInfo> rooms = new List<RoomInfo>();

    private Vector3 locationVector;
    private bool positionFilled;

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
        rooms = new List<RoomInfo>();
        positionFilled = false;
        locationVector = Vector3.zero;

        // We have to add the first room, for now we add at pos zero
        Room startRoom = GameObject.Find("StartRoom").GetComponent<Room>();
        startRoom.Initialise();

        RoomInfo firstRoom = new RoomInfo(startRoom);
        firstRoom.position = startRoom.transform.position;
        firstRoom.width = 18;
        firstRoom.height = 14;
        firstRoom.wallsPlaced = true;
        rooms.Add(firstRoom);

        // Go through and place all the floor components based on the number of them we have.
        for (roomsPlaced = 0; roomsPlaced < roomsToPlace; roomsPlaced++)
        {
            // Pick a random room from the pool of rooms that currently exist
            int randomRoom = Random.Range(0, rooms.Count);
            RoomInfo fromRoom = rooms[randomRoom];

            // Choose a random direction to place the room
            int randRoomDir = Random.Range(0, 4);

            // Checks if we can make a room in this direction
            if (fromRoom.directionsFilled[randRoomDir] == false)
            {

                // TODO: Generate room size variations
                int width = roomDimensions[Random.Range(0, roomDimensions.Count)];
                int height = roomDimensions[Random.Range(0, roomDimensions.Count)];

                // See if we can add a new room through the chosen direction.
                GenerateNewRoom(width, height, "Room " + roomsPlaced, fromRoom, randRoomDir);
            }
            else
            {
                roomsPlaced--;
            }
        }

        GenerateWalls();
    }

    private void GenerateWalls()
    {
        foreach (RoomInfo room in rooms)
        {
            if (room.wallsPlaced == false)
            {
                PlaceWalls(room);
            }
        }
    }

    private void PlaceWalls(RoomInfo room)
    {
        // Place wall corners
        GameObject walls = room.room.GetNodeByLayer("Environment").transform.Find("Walls").gameObject;

        GameObject wallCornerGo = null;
        
        // North east corner
        wallCornerGo = GameObject.Instantiate(wallCorner, Vector3.zero, wallCorner.transform.rotation) as GameObject;
        wallCornerGo.transform.position = new Vector3(room.position.x + (room.width * 0.5f) - 1.0f, wallCornerGo.transform.position.y, room.position.z + (room.height * 0.5f) - 1.0f);
        wallCornerGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 270.0f);
        wallCornerGo.name = "CornerNE";
        wallCornerGo.transform.parent = walls.transform;

        // South east corner
        wallCornerGo = GameObject.Instantiate(wallCorner, Vector3.zero, wallCorner.transform.rotation) as GameObject;
        wallCornerGo.transform.position = new Vector3(room.position.x + (room.width * 0.5f) - 1.0f, wallCornerGo.transform.position.y, room.position.z - (room.height * 0.5f) + 1.0f);
        wallCornerGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 0.0f);
        wallCornerGo.name = "CornerSE";
        wallCornerGo.transform.parent = walls.transform;

        // North west corner
        wallCornerGo = GameObject.Instantiate(wallCorner, Vector3.zero, wallCorner.transform.rotation) as GameObject;
        wallCornerGo.transform.position = new Vector3(room.position.x - (room.width * 0.5f) + 1.0f, wallCornerGo.transform.position.y, room.position.z + (room.height * 0.5f) - 1.0f);
        wallCornerGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 180.0f);
        wallCornerGo.name = "CornerNW";
        wallCornerGo.transform.parent = walls.transform;

        // North east corner
        wallCornerGo = GameObject.Instantiate(wallCorner, Vector3.zero, wallCorner.transform.rotation) as GameObject;
        wallCornerGo.transform.position = new Vector3(room.position.x - (room.width * 0.5f) + 1.0f, wallCornerGo.transform.position.y, room.position.z - (room.height * 0.5f) + 1.0f);
        wallCornerGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 90.0f);
        wallCornerGo.name = "CornerSW";
        wallCornerGo.transform.parent = walls.transform;

        // Place north walls
        int numberOfWalls = (int)(room.width * 0.5f) - 2;

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
                float xPos = (room.position.x - (room.width * 0.5f)) + 3.0f + (i * 2.0f);
                GameObject wallGo = GameObject.Instantiate(wallObject, Vector3.zero, wallObject.transform.rotation) as GameObject;
                wallGo.transform.position = new Vector3(xPos, wallCornerGo.transform.position.y, room.position.z + (room.height * 0.5f) + 1.0f);
                wallGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 90.0f);
                wallGo.name = "Wall";
                wallGo.transform.parent = walls.transform;

                horizontalWalls[i] = true;
            }
        }

        // Place south walls
        numberOfWalls = (int)(room.width * 0.5f) - 2;

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
                float xPos = (room.position.x - (room.width * 0.5f)) + 3.0f + (i * 2.0f);
                GameObject wallGo = GameObject.Instantiate(wallObject, Vector3.zero, wallObject.transform.rotation) as GameObject;
                wallGo.transform.position = new Vector3(xPos, wallCornerGo.transform.position.y, room.position.z - (room.height * 0.5f) - 1.0f);
                wallGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 270.0f);
                wallGo.name = "Wall";
                wallGo.transform.parent = walls.transform;

                horizontalWalls[i] = true;
            }
        }

        // Place east walls
        numberOfWalls = (int)(room.height * 0.5f) - 2;

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
                float zPos = (room.position.z - (room.height * 0.5f)) + 3.0f + (i * 2.0f);
                GameObject wallGo = GameObject.Instantiate(wallObject, Vector3.zero, wallObject.transform.rotation) as GameObject;
                wallGo.transform.position = new Vector3(room.position.x + (room.width * 0.5f) + 1.0f, wallCornerGo.transform.position.y, zPos);
                wallGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 180.0f);
                wallGo.name = "Wall";
                wallGo.transform.parent = walls.transform;

                horizontalWalls[i] = true;
            }
        }

        // Place west walls
        numberOfWalls = (int)(room.height * 0.5f) - 2;

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
                float zPos = (room.position.z - (room.height * 0.5f)) + 3.0f + (i * 2.0f);
                GameObject wallGo = GameObject.Instantiate(wallObject, Vector3.zero, wallObject.transform.rotation) as GameObject;
                wallGo.transform.position = new Vector3(room.position.x - (room.width * 0.5f) - 1.0f, wallCornerGo.transform.position.y, zPos);
                wallGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 0.0f);
                wallGo.name = "Wall";
                wallGo.transform.parent = walls.transform;

                horizontalWalls[i] = true;
            }
        }

        room.wallsPlaced = true;
    }

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

    private Door CreateDoor(GameObject doors, RoomInfo fromRoom, int direction)
    {
		GameObject doorGo = GameObject.Instantiate(doorObject, Vector3.zero, doorObject.transform.rotation) as GameObject;
        doorGo.transform.parent = doors.transform;

        // Attach the doors to their rightful component.
        Doors doorsScript = doors.GetComponent<Doors>();
        Door returnDoor = null;

        doorsScript.doors[direction] = doorGo.GetComponent<Door>();
        returnDoor = doorsScript.doors[direction];

        float widthOffset = (fromRoom.width * 0.5f) + 1.0f;
        float heightOffset = (fromRoom.height * 0.5f) + 1.0f;

        switch (direction)
        {
            case 0:
                //doorGo.transform.position = new Vector3(doors.transform.position.x, doorGo.transform.position.y, doors.transform.position.z + 8.0f);
                doorGo.transform.position = new Vector3(doors.transform.position.x, doorGo.transform.position.y, doors.transform.position.z + heightOffset);
                doorGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 90.0f);
                doorGo.name = "North Door";
                break;

            case 1:
                //doorGo.transform.position = new Vector3(doors.transform.position.x + 8.0f, doorGo.transform.position.y, doors.transform.position.z);
                doorGo.transform.position = new Vector3(doors.transform.position.x + widthOffset, doorGo.transform.position.y, doors.transform.position.z);
                doorGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 180.0f);
                doorGo.name = "East Door";
                break;

            case 2:
                //doorGo.transform.position = new Vector3(doors.transform.position.x, doorGo.transform.position.y, doors.transform.position.z - 8.0f);
                doorGo.transform.position = new Vector3(doors.transform.position.x, doorGo.transform.position.y, doors.transform.position.z - heightOffset);
                doorGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 270.0f);
                doorGo.name = "South Door";
                break;

            case 3:
                //doorGo.transform.position = new Vector3(doors.transform.position.x - 8, doorGo.transform.position.y, doors.transform.position.z);
                doorGo.transform.position = new Vector3(doors.transform.position.x - widthOffset, doorGo.transform.position.y, doors.transform.position.z);
                doorGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 0.0f);
                doorGo.name = "West Door";
                break;

        }

        return (returnDoor);
    }

    public void GenerateNewRoom(float width, float height, string name, RoomInfo from, int dir)
    {
        GameObject roomGo = null;
        Bounds testBounds = new Bounds();

        // Place a room in a random direction, if room cannot be placed in the location it will decrease the number of rooms placed
        // and the loop which called this function will repeat itself;
        switch (dir)
        {
            // North
            case 0:
                // Vector to test new location. 
                //locationVector = new Vector3(from.position.x, 0.0f, ((from.position.z + from.height * 0.5f) + (height * roomOffsetMultiplier)));
                locationVector = new Vector3(from.position.x, 0.0f, ((from.position.z + from.height * 0.5f) + roomOffsetValue));
                testBounds = new Bounds(locationVector, new Vector3(width, 1.0f, height));

                // Check to see if the position is filled by another room.
                for (int a = 0; a < rooms.Count; a++)
                {
                    Bounds roomBounds = new Bounds(rooms[a].position, new Vector3(rooms[a].width, 1.0f, rooms[a].height));

                    if (testBounds.Intersects(roomBounds))
                    {
                        positionFilled = true;
                        break;
                    }
                }

                // If the position is filled
                if (positionFilled == true)
                {
                    // Decrease the counter so we can go through and try and place again.
                    roomsPlaced--;
                }
                else
                {
                    // Create the room at this position
                    roomGo = CreateRoom(width, height, name);
                    roomGo.transform.position = locationVector;

                    // Do rotation soon
                    //roomGo.transform.Rotate(Vector3.up, rotation);

                    // This door came from the north direction, we need to make a door here if it does not exist for the coming from
                    // room
                    from.directionsFilled[0] = true;
                    Transform doors = from.room.GetNodeByLayer("Environment").transform.FindChild("Doors");
                    Door entryDoor = CreateDoor(doors.gameObject, from, 0);
                    entryDoor.direction = Floor.TransitionDirection.North;

                    // Create the new room with number of doors.
                    // we also need to create the door that connects the previous room.
                    RoomInfo newRoom = new RoomInfo(roomGo.GetComponent<Room>());
                    newRoom.directionsFilled[2] = true; // We set this position to filled because its where the other door came from
                    newRoom.position = locationVector;
                    newRoom.width = width;
                    newRoom.height = height;
                    rooms.Add(newRoom);

                    // Generate the door for this new room and link it to the previous room.
                    Transform newDoor = roomGo.GetComponent<Room>().GetNodeByLayer("Environment").transform.FindChild("Doors");
                    Door exitDoor = CreateDoor(newDoor.gameObject, newRoom, 2);
                    exitDoor.direction = Floor.TransitionDirection.South;

                    // Link the doors
                    entryDoor.targetDoor = exitDoor;
                    exitDoor.targetDoor = entryDoor;
                }

                positionFilled = false;
                break;

            // East
            case 1:

                //locationVector = new Vector3(((from.position.x + from.width * 0.5f) + (width * roomOffsetMultiplier)), 0.0f, from.position.z);
                locationVector = new Vector3(((from.position.x + from.width * 0.5f) + roomOffsetValue), 0.0f, from.position.z);

                testBounds = new Bounds(locationVector, new Vector3(width, 1.0f, height));

                // Check to see if the position is filled by another room.
                for (int a = 0; a < rooms.Count; a++)
                {
                    Bounds roomBounds = new Bounds(rooms[a].position, new Vector3(rooms[a].width, 1.0f, rooms[a].height));

                    if (testBounds.Intersects(roomBounds))
                    {
                        positionFilled = true;
                        break;
                    }
                }

                if (positionFilled == true)
                {
                    roomsPlaced--;
                }
                else
                {
                    // Create the room at this position
                    roomGo = CreateRoom(width, height, name);
                    roomGo.transform.position = locationVector;

                    // Do rotation soon
                    //roomGo.transform.Rotate(Vector3.up, rotation);

                    // This door came from the east direction, we need to make a door here if it does not exist for the coming from
                    // room
                    from.directionsFilled[1] = true;
                    Transform doors = from.room.GetNodeByLayer("Environment").transform.FindChild("Doors");
                    Door entryDoor = CreateDoor(doors.gameObject, from, 1);
                    entryDoor.direction = Floor.TransitionDirection.East;

                    // Create the new room with number of doors.
                    // we also need to create the door that connects the previous room.
                    RoomInfo newRoom = new RoomInfo(roomGo.GetComponent<Room>());
                    newRoom.directionsFilled[3] = true; // We set this position to filled because its where the other door came from
                    newRoom.position = locationVector;
                    newRoom.width = width;
                    newRoom.height = height;
                    rooms.Add(newRoom);

                    // Generate the door for this new room and link it to the previous room.
                    Transform newDoor = roomGo.GetComponent<Room>().GetNodeByLayer("Environment").transform.FindChild("Doors");
                    Door exitDoor = CreateDoor(newDoor.gameObject, newRoom, 3);
                    exitDoor.direction = Floor.TransitionDirection.West;

                    // Link the doors
                    entryDoor.targetDoor = exitDoor;
                    exitDoor.targetDoor = entryDoor;
                }

                positionFilled = false;
                break;

            // South
            case 2:
                //locationVector = new Vector3(from.position.x, 0.0f, ((from.position.z - from.height * 0.5f) - (height * roomOffsetMultiplier)));
                locationVector = new Vector3(from.position.x, 0.0f, ((from.position.z - from.height * 0.5f) - roomOffsetValue));

                testBounds = new Bounds(locationVector, new Vector3(width, 1.0f, height));

                // Check to see if the position is filled by another room.
                for (int a = 0; a < rooms.Count; a++)
                {
                    Bounds roomBounds = new Bounds(rooms[a].position, new Vector3(rooms[a].width, 1.0f, rooms[a].height));

                    if (testBounds.Intersects(roomBounds))
                    {
                        positionFilled = true;
                        break;
                    }
                }

                if (positionFilled == true)
                {
                    roomsPlaced--;
                }
                else
                {
                    // Create the room at this position
                    roomGo = CreateRoom(width, height, name);
                    roomGo.transform.position = locationVector;

                    // Do rotation soon
                    //roomGo.transform.Rotate(Vector3.up, rotation);

                    // This door came from the south direction, we need to make a door here if it does not exist for the coming from
                    // room
                    from.directionsFilled[2] = true;
                    Transform doors = from.room.GetNodeByLayer("Environment").transform.FindChild("Doors");
                    Door entryDoor = CreateDoor(doors.gameObject, from, 2);
                    entryDoor.direction = Floor.TransitionDirection.South;

                    // Create the new room with number of doors.
                    // we also need to create the door that connects the previous room.
                    RoomInfo newRoom = new RoomInfo(roomGo.GetComponent<Room>());
                    newRoom.directionsFilled[0] = true; // We set this position to filled because its where the other door came from
                    newRoom.position = locationVector;
                    newRoom.width = width;
                    newRoom.height = height;
                    rooms.Add(newRoom);

                    // Generate the door for this new room and link it to the previous room.
                    Transform newDoor = roomGo.GetComponent<Room>().GetNodeByLayer("Environment").transform.FindChild("Doors");
                    Door exitDoor = CreateDoor(newDoor.gameObject, newRoom, 0);
                    exitDoor.direction = Floor.TransitionDirection.North;

                    // Link the doors
                    entryDoor.targetDoor = exitDoor;
                    exitDoor.targetDoor = entryDoor;
                }

                positionFilled = false;
                break;

            // West
            case 3:
                //locationVector = new Vector3(((from.position.x - from.width * 0.5f) - (width * roomOffsetMultiplier)), 0.0f, from.position.z);
                locationVector = new Vector3(((from.position.x - from.width * 0.5f) - roomOffsetValue), 0.0f, from.position.z);

                testBounds = new Bounds(locationVector, new Vector3(width, 1.0f, height));

                // Check to see if the position is filled by another room.
                for (int a = 0; a < rooms.Count; a++)
                {
                    Bounds roomBounds = new Bounds(rooms[a].position, new Vector3(rooms[a].width, 1.0f, rooms[a].height));

                    if (testBounds.Intersects(roomBounds))
                    {
                        positionFilled = true;
                        break;
                    }
                }

                if (positionFilled == true)
                {
                    roomsPlaced--;
                }
                else
                {
                    // Create the room at this position
                    roomGo = CreateRoom(width, height, name);
                    roomGo.transform.position = locationVector;

                    // Do rotation soon
                    //roomGo.transform.Rotate(Vector3.up, rotation);

                    from.directionsFilled[3] = true;
                    Transform doors = from.room.GetNodeByLayer("Environment").transform.FindChild("Doors");
                    Door entryDoor = CreateDoor(doors.gameObject, from, 3);
                    entryDoor.direction = Floor.TransitionDirection.West;

                    // Create the new room with number of doors.
                    // we also need to create the door that connects the previous room.
                    RoomInfo newRoom = new RoomInfo(roomGo.GetComponent<Room>());
                    newRoom.directionsFilled[1] = true; // We set this position to filled because its where the other door came from
                    newRoom.position = locationVector;
                    newRoom.width = width;
                    newRoom.height = height;
                    rooms.Add(newRoom);

                    // Generate the door for this new room and link it to the previous room.
                    Transform newDoor = roomGo.GetComponent<Room>().GetNodeByLayer("Environment").transform.FindChild("Doors");
                    Door exitDoor = CreateDoor(newDoor.gameObject, newRoom, 1);
                    exitDoor.direction = Floor.TransitionDirection.East;

                    // Link the doors
                    entryDoor.targetDoor = exitDoor;
                    exitDoor.targetDoor = entryDoor;
                }

                positionFilled = false;
                break;
        }
    }
}
