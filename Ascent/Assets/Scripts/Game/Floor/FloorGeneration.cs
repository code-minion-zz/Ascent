using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct RoomInfo
{
    public Vector3 position;
    public int numberOfDoors;
    public bool[] directionsFilled;
    public Room room;

    public RoomInfo(int doorCount, Room room)
    {
        position = Vector3.zero;
        numberOfDoors = doorCount;
        directionsFilled = new bool[4];
        this.room = room;

        for (int i = 0; i < 4; ++i)
        {
            directionsFilled[i] = false;
        }
    }
}

public class FloorGeneration : MonoBehaviour 
{
    public enum Directions
    {
        north = 1,
        easth,
        south,
        west
    }

    private float floorWidth;
    private float floorHeight;

    private GameObject floorObject;
    private GameObject wallObject;
    private GameObject wallCorner;
    private GameObject wallWindow;
    private GameObject doorObject;

    // The rooms need to be seperated from each other to a degree.
    private float roomOffsetMultiplier = 1.5f;

    // Number of rooms to place.
    private int roomsToPlace = 50;
    private int roomsPlaced = 0;
    private List<RoomInfo> rooms = new List<RoomInfo>();

    private float previousX = 0.0f;
    private float previousZ = 0.0f;
    private Vector3 locationVector;
    private bool positionFilled;
    private int randDoorCount = 0;

    public void GenerateFloor()
    {
        floorObject = Resources.Load("Prefabs/RoomWalls/Ground") as GameObject;
        wallObject = Resources.Load("Prefabs/RoomWalls/Wall") as GameObject;
        wallCorner = Resources.Load("Prefabs/RoomWalls/WallCorner") as GameObject;
        wallWindow = Resources.Load("Prefabs/RoomWalls/WallWindow") as GameObject;
        doorObject = Resources.Load("Prefabs/RoomWalls/Door") as GameObject;

        //Random.seed = System.DateTime.Today.Millisecond;
        Random.seed = (int)Time.time;
        CreateRooms();
    }

    public void CreateRooms()
    {
        rooms.Clear();
        rooms = new List<RoomInfo>();
        positionFilled = false;
        previousX = 0.0f;
        previousZ = 0.0f;
        locationVector = Vector3.zero;

        // We have to add the first room, for now we add at pos zero
        Room startRoom = GameObject.Find("StartRoom").GetComponent<Room>();
        startRoom.Initialise();

        RoomInfo firstRoom = new RoomInfo(3, startRoom);
        firstRoom.position = startRoom.transform.position;
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
                // See if we can add a new room through the chosen direction.
                GenerateNewRoom(18, 14, "Room " + roomsPlaced, fromRoom, randRoomDir);
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
        GameObject floorGo = Instantiate(floorObject, Vector3.zero, floorObject.transform.rotation) as GameObject;
        floorGo.transform.localScale = new Vector3(width, height, 1.0f);
        floorGo.transform.parent = room.GetNodeByLayer("Environment").transform;
        floorGo.name = "Ground";

        room.Initialise();
        room.gameObject.SetActive(false);
        return roomGo;
    }

    private Door CreateDoor(GameObject doors, float roomWidth, float roomHeight, int direction)
    {
        GameObject doorGo = Instantiate(doorObject, Vector3.zero, doorObject.transform.rotation) as GameObject;
        doorGo.transform.parent = doors.transform;

        // Attach the doors to their rightful component.
        Doors doorsScript = doors.GetComponent<Doors>();
        Door returnDoor = null;

        Debug.Log(doorsScript);

        if (doorsScript.doors[direction] == null)
        {
            doorsScript.doors[direction] = doorGo.GetComponent<Door>();
            returnDoor = doorsScript.doors[direction];
        }
        else
        {
            doorsScript.doors[direction] = doorGo.GetComponent<Door>();
            returnDoor = doorsScript.doors[direction];
        }

        switch (direction)
        {
            case 0:
                doorGo.transform.position = new Vector3(doors.transform.position.x, doorGo.transform.position.y, doors.transform.position.z + 8.0f);
                doorGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 90.0f);
                doorGo.name = "North Door";
                break;

            case 1:
                doorGo.transform.position = new Vector3(doors.transform.position.x + 10, doorGo.transform.position.y, doors.transform.position.z);
                doorGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 180.0f);
                doorGo.name = "East Door";
                break;

            case 2:
                doorGo.transform.position = new Vector3(doors.transform.position.x, doorGo.transform.position.y, doors.transform.position.z - 8.0f);
                doorGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 270.0f);
                doorGo.name = "South Door";
                break;

            case 3:
                doorGo.transform.position = new Vector3(doors.transform.position.x - 10, doorGo.transform.position.y, doors.transform.position.z);
                doorGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 0.0f);
                doorGo.name = "West Door";
                break;

        }

        return (returnDoor);
    }

    public void GenerateNewRoom(float width, float height, string name, RoomInfo from, int dir)
    {
        GameObject roomGo = null;

        // Random door count for the new room.
        randDoorCount = Random.Range(1, 5);

        // Place a room in a random direction, if room cannot be placed in the location it will decrease the number of rooms placed
        // and the loop which called this function will repeat itself;
        switch (dir)
        {
            // North
            case 0:
                Debug.Log("North");

                // Vector to test new location.
                locationVector = new Vector3(from.position.x, 0.0f, (from.position.z + (height * roomOffsetMultiplier)));

                // Check to see if the position is filled by another room.
                for (int a = 0; a < rooms.Count; a++)
                {
                    if (locationVector == rooms[a].position)
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
                    previousZ += (height * roomOffsetMultiplier);
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
                    Door entryDoor = CreateDoor(doors.gameObject, width, height, 0);
                    entryDoor.direction = Floor.TransitionDirection.North;

                    // Create the new room with number of doors.
                    // we also need to create the door that connects the previous room.
                    RoomInfo newRoom = new RoomInfo(randDoorCount, roomGo.GetComponent<Room>());
                    newRoom.directionsFilled[2] = true; // We set this position to filled because its where the other door came from
                    newRoom.position = locationVector;
                    rooms.Add(newRoom);

                    // Generate the door for this new room and link it to the previous room.
                    Transform newDoor = roomGo.GetComponent<Room>().GetNodeByLayer("Environment").transform.FindChild("Doors");
                    Door exitDoor = CreateDoor(newDoor.gameObject, width, height, 2);
                    exitDoor.direction = Floor.TransitionDirection.South;

                    // Link the doors
                    entryDoor.targetDoor = exitDoor;
                    exitDoor.targetDoor = entryDoor;
                }

                positionFilled = false;
                break;

            // East
            case 1:
                Debug.Log("East");

                locationVector = new Vector3((from.position.x + (width * roomOffsetMultiplier)), 0.0f, from.position.z);

                for (int a = 0; a < rooms.Count; a++)
                {
                    if (locationVector == rooms[a].position)
                    {
                        positionFilled = true;
                        break;
                    }
                }

                if (positionFilled == true)
                {
                    roomsPlaced--;
                    previousX += (width * roomOffsetMultiplier);
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
                    Door entryDoor = CreateDoor(doors.gameObject, width, height, 1);
                    entryDoor.direction = Floor.TransitionDirection.East;

                    // Create the new room with number of doors.
                    // we also need to create the door that connects the previous room.
                    RoomInfo newRoom = new RoomInfo(randDoorCount, roomGo.GetComponent<Room>());
                    newRoom.directionsFilled[3] = true; // We set this position to filled because its where the other door came from
                    newRoom.position = locationVector;
                    rooms.Add(newRoom);

                    // Generate the door for this new room and link it to the previous room.
                    Transform newDoor = roomGo.GetComponent<Room>().GetNodeByLayer("Environment").transform.FindChild("Doors");
                    Door exitDoor = CreateDoor(newDoor.gameObject, width, height, 3);
                    exitDoor.direction = Floor.TransitionDirection.West;

                    // Link the doors
                    entryDoor.targetDoor = exitDoor;
                    exitDoor.targetDoor = entryDoor;
                }

                positionFilled = false;
                break;

            // South
            case 2:
                Debug.Log("South");

                locationVector = new Vector3(from.position.x, 0.0f, (from.position.z - (height * roomOffsetMultiplier)));

                for (int a = 0; a < rooms.Count; a++)
                {
                    if (locationVector == rooms[a].position)
                    {
                        positionFilled = true;
                        break;
                    }
                }

                if (positionFilled == true)
                {
                    roomsPlaced--;
                    previousZ -= (height * roomOffsetMultiplier);
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
                    Debug.Log(from);
                    Debug.Log(from.room);
                    Debug.Log(from.room.GetNodeByLayer("Environment"));
                    Debug.Log(from.room.GetNodeByLayer("Environment").transform.FindChild("Doors"));
                    Transform doors = from.room.GetNodeByLayer("Environment").transform.FindChild("Doors");
                    Door entryDoor = CreateDoor(doors.gameObject, width, height, 2);
                    entryDoor.direction = Floor.TransitionDirection.South;

                    // Create the new room with number of doors.
                    // we also need to create the door that connects the previous room.
                    RoomInfo newRoom = new RoomInfo(randDoorCount, roomGo.GetComponent<Room>());
                    newRoom.directionsFilled[0] = true; // We set this position to filled because its where the other door came from
                    newRoom.position = locationVector;
                    rooms.Add(newRoom);

                    // Generate the door for this new room and link it to the previous room.
                    Transform newDoor = roomGo.GetComponent<Room>().GetNodeByLayer("Environment").transform.FindChild("Doors");
                    Door exitDoor = CreateDoor(newDoor.gameObject, width, height, 0);
                    exitDoor.direction = Floor.TransitionDirection.North;

                    // Link the doors
                    entryDoor.targetDoor = exitDoor;
                    exitDoor.targetDoor = entryDoor;
                }

                positionFilled = false;
                break;

            // West
            case 3:
                Debug.Log("West");

                locationVector = new Vector3((from.position.x - (width * roomOffsetMultiplier)), 0.0f, from.position.z);

                for (int a = 0; a < rooms.Count; a++)
                {
                    if (locationVector == rooms[a].position)
                    {
                        positionFilled = true;
                        break;
                    }
                }

                if (positionFilled == true)
                {
                    roomsPlaced--;
                    previousX -= (width * roomOffsetMultiplier);
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
                    Door entryDoor = CreateDoor(doors.gameObject, width, height, 3);
                    entryDoor.direction = Floor.TransitionDirection.West;

                    // Create the new room with number of doors.
                    // we also need to create the door that connects the previous room.
                    RoomInfo newRoom = new RoomInfo(randDoorCount, roomGo.GetComponent<Room>());
                    newRoom.directionsFilled[1] = true; // We set this position to filled because its where the other door came from
                    newRoom.position = locationVector;
                    rooms.Add(newRoom);

                    // Generate the door for this new room and link it to the previous room.
                    Transform newDoor = roomGo.GetComponent<Room>().GetNodeByLayer("Environment").transform.FindChild("Doors");
                    Door exitDoor = CreateDoor(newDoor.gameObject, width, height, 1);
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
