using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct RoomInfo
{
    public Vector3 position;
    public int numberOfDoors;
    public bool[] directionsFilled;

    public RoomInfo(int doorCount)
    {
        position = Vector3.zero;
        numberOfDoors = doorCount;
        directionsFilled = new bool[4];

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
    private int roomsToPlace = 10;
    private int roomsPlaced = 0;
    private List<RoomInfo> rooms = new List<RoomInfo>();

    private float previousX = 0.0f;
    private float previousZ = 0.0f;
    private Vector3 locationVector;
    private bool positionFilled;
    private int randDir = 0;
    private int randDoorCount = 0;

    void Awake()
    {
        floorObject = Resources.Load("Prefabs/RoomWalls/Ground") as GameObject;
        wallObject = Resources.Load("Prefabs/RoomWalls/Wall") as GameObject;
        wallCorner = Resources.Load("Prefabs/RoomWalls/WallCorner") as GameObject;
        wallWindow = Resources.Load("Prefabs/RoomWalls/WallWindow") as GameObject;
        doorObject = Resources.Load("Prefabs/RoomWalls/Door") as GameObject;

        GenerateFloors();
    }

    void Start()
    {
        Random.seed = System.DateTime.Today.Millisecond;
    }

    public void GenerateFloors()
    {
        rooms.Clear();
        rooms = new List<RoomInfo>();
        positionFilled = false;
        randDir = 0;
        previousX = 0.0f;
        previousZ = 0.0f;
        locationVector = Vector3.zero;

        // We have to add the first room, for now we add at pos zero
        RoomInfo firstRoom = new RoomInfo(3);
        firstRoom.position = Vector3.zero;
        rooms.Add(firstRoom);

        // Go through and place all the floor components based on the number of them we have.
        for (roomsPlaced = 0; roomsPlaced < roomsToPlace; roomsPlaced++)
        {
            //// Work out a random door count for the next room.
            //// If the door count is 1 then it only has an entrance.
            //int randDoorCount = Random.Range(1, 5);

            //// Work out if the room can support this number of rooms to place
            //if (randDoorCount > 1 && randDoorCount <= (roomsToPlace - roomsPlaced))
            //{

            //}

            // Pick a wall from any room
            int randomRoom = Random.Range(0, rooms.Count);
            RoomInfo fromRoom = rooms[randomRoom];

            int randRoomDir = Random.Range(0, fromRoom.numberOfDoors);

            if (fromRoom.directionsFilled[randRoomDir] == false)
            {
                // See if we can add a new room through the chosen direction.
                GenerateNewRoom(18, 14, "Room " + roomsPlaced, fromRoom, randRoomDir);
            }
        }
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

        return roomGo;
    }

    private GameObject CreateDoor(GameObject doors, float roomWidth, float roomHeight, int direction)
    {
        GameObject doorGo = Instantiate(doorObject, Vector3.zero, doorObject.transform.rotation) as GameObject;
        doorGo.transform.parent = doors.transform;

        switch (direction)
        {
            case 0:
                doorGo.transform.position = new Vector3(doors.transform.position.x, doorGo.transform.position.y, doors.transform.position.z + 8.0f);
                doorGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 90.0f);
                break;

            case 1:
                doorGo.transform.position = new Vector3(doors.transform.position.x + 10, doorGo.transform.position.y, doors.transform.position.z);
                doorGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 180.0f);
                break;

            case 2:
                doorGo.transform.position = new Vector3(doors.transform.position.x, doorGo.transform.position.y, doors.transform.position.z - 8.0f);
                doorGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 270.0f);
                break;

            case 3:
                doorGo.transform.position = new Vector3(doors.transform.position.x - 10, doorGo.transform.position.y, doors.transform.position.z);
                doorGo.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 270.0f);
                break;

        }

        return (doorGo);
    }

    //public void GenerateDoors()
    //{
    //    foreach (RoomInfo room in rooms)
    //    {
    //        for (int i = 0; i < room.numberOfDoors; ++i)
    //        {
    //        }
    //    }
    //}

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

                    // Tell the previous room they have now filled up this one.
                    from.directionsFilled[0] = true;

                    // Create the new room with number of doors.
                    RoomInfo newRoom = new RoomInfo(randDoorCount);
                    newRoom.directionsFilled[0] = true;
                    newRoom.position = locationVector;
                    rooms.Add(newRoom);

                    // Generate the door for this new room and link it to the previous room.
                    CreateDoor(roomGo, width, height, 0);

                    previousZ = previousZ + (height * roomOffsetMultiplier);
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

                    from.directionsFilled[1] = true;

                    // Create the new room with number of doors.
                    RoomInfo newRoom = new RoomInfo(randDoorCount);
                    newRoom.directionsFilled[1] = true;
                    newRoom.position = locationVector;
                    rooms.Add(newRoom);

                    // Generate the door for this new room and link it to the previous room.
                    CreateDoor(roomGo, width, height, 1);
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

                    from.directionsFilled[2] = true;

                    // Create the new room with number of doors.
                    RoomInfo newRoom = new RoomInfo(randDoorCount);
                    newRoom.directionsFilled[2] = true;
                    newRoom.position = locationVector;
                    rooms.Add(newRoom);

                    // Generate the door for this new room and link it to the previous room.
                    CreateDoor(roomGo, width, height, 2);
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

                    // Create the new room with number of doors.
                    RoomInfo newRoom = new RoomInfo(randDoorCount);
                    newRoom.directionsFilled[3] = true;
                    newRoom.position = locationVector;
                    rooms.Add(newRoom);

                    // Generate the door for this new room and link it to the previous room.
                    CreateDoor(roomGo, width, height, 3);
                }

                positionFilled = false;
                break;
        }

    //public void GenerateNewRoom(float width, float height, string name, RoomInfo from)
    //{
    //    GameObject roomGo = null;

    //    // Random direction to choose from when going from the selected room
    //    randDir = Random.Range(1, 5);
    //    // Random door count for the new door.
    //    randDoorCount = Random.Range(1, 5);

    //    // Place a room in a random direction, if room cannot be placed in the location it will decrease the number of rooms placed
    //    // and the loop which called this function will repeat itself;
    //    switch (randDir)
    //    {
    //        // North
    //        case 1:
    //            Debug.Log("North");

    //            // Vector to test new location.
    //            locationVector = new Vector3(from.position.x, 0.0f, (from.position.z + (height * roomOffsetMultiplier)));

    //            // Check to see if the position is filled by another room.
    //            for (int a = 0; a < rooms.Count; a++)
    //            {
    //                if (locationVector == rooms[a].position)
    //                {
    //                    positionFilled = true;
    //                    break;
    //                }
    //            }

    //            // If the position is filled
    //            if (positionFilled == true)
    //            {
    //                // Decrease the counter so we can go through and try and place again.
    //                roomsPlaced--;
    //                previousZ += (height * roomOffsetMultiplier);
    //            }
    //            else
    //            {
    //                // Create the room at this position
    //                roomGo = CreateRoom(width, height, name);
    //                roomGo.transform.position = locationVector;

    //                // Do rotation soon
    //                //roomGo.transform.Rotate(Vector3.up, rotation);

    //                // Tell the previous room they have now filled up this one.
    //                from.directionsFilled[1] = true;

    //                // Create the new room with number of doors.
    //                RoomInfo newRoom = new RoomInfo(randDoorCount);
    //                newRoom.directionsFilled[1] = true;
    //                newRoom.position = locationVector;
    //                rooms.Add(newRoom);

    //                // Generate the door for this new room and link it to the previous room.
    //                CreateDoor(roomGo, width, height, 1);

    //                previousZ = previousZ + (height * roomOffsetMultiplier);
    //            }

    //            positionFilled = false;
    //            break;

    //        // East
    //        case 2:
    //            Debug.Log("East");

    //            locationVector = new Vector3((from.position.x + (width * roomOffsetMultiplier)), 0.0f, from.position.z);

    //            for (int a = 0; a < rooms.Count; a++)
    //            {
    //                if (locationVector == rooms[a].position)
    //                {
    //                    positionFilled = true;
    //                    break;
    //                }
    //            }

    //            if (positionFilled == true)
    //            {
    //                roomsPlaced--;
    //                previousX += (width * roomOffsetMultiplier);
    //            }
    //            else
    //            {
    //                // Create the room at this position
    //                roomGo = CreateRoom(width, height, name);
    //                roomGo.transform.position = locationVector;

    //                // Do rotation soon
    //                //roomGo.transform.Rotate(Vector3.up, rotation);

    //                from.directionsFilled[2] = true;

    //                // Create the new room with number of doors.
    //                RoomInfo newRoom = new RoomInfo(randDoorCount);
    //                newRoom.directionsFilled[2] = true;
    //                newRoom.position = locationVector;
    //                rooms.Add(newRoom);

    //                // Generate the door for this new room and link it to the previous room.
    //                CreateDoor(roomGo, width, height, 2);
    //            }

    //            positionFilled = false;
    //            break;

    //        // South
    //        case 3:
    //            Debug.Log("South");

    //            locationVector = new Vector3(from.position.x, 0.0f, (from.position.z - (height * roomOffsetMultiplier)));

    //            for (int a = 0; a < rooms.Count; a++)
    //            {
    //                if (locationVector == rooms[a].position)
    //                {
    //                    positionFilled = true;
    //                    break;
    //                }
    //            }

    //            if (positionFilled == true)
    //            {
    //                roomsPlaced--;
    //                previousZ -= (height * roomOffsetMultiplier);
    //            }
    //            else
    //            {
    //                // Create the room at this position
    //                roomGo = CreateRoom(width, height, name);
    //                roomGo.transform.position = locationVector;

    //                // Do rotation soon
    //                //roomGo.transform.Rotate(Vector3.up, rotation);

    //                from.directionsFilled[3] = true;

    //                // Create the new room with number of doors.
    //                RoomInfo newRoom = new RoomInfo(randDoorCount);
    //                newRoom.directionsFilled[3] = true;
    //                newRoom.position = locationVector;
    //                rooms.Add(newRoom);

    //                // Generate the door for this new room and link it to the previous room.
    //                CreateDoor(roomGo, width, height, 3);
    //            }

    //            positionFilled = false;
    //            break;

    //        // West
    //        case 4:
    //            Debug.Log("West");

    //            locationVector = new Vector3((from.position.x - (width * roomOffsetMultiplier)), 0.0f, from.position.z);

    //            for (int a = 0; a < rooms.Count; a++)
    //            {
    //                if (locationVector == rooms[a].position)
    //                {
    //                    positionFilled = true;
    //                    break;
    //                }
    //            }

    //            if (positionFilled == true)
    //            {
    //                roomsPlaced--;
    //                previousX -= (width * roomOffsetMultiplier);
    //            }
    //            else
    //            {
    //                // Create the room at this position
    //                roomGo = CreateRoom(width, height, name);
    //                roomGo.transform.position = locationVector;

    //                // Do rotation soon
    //                //roomGo.transform.Rotate(Vector3.up, rotation);

    //                from.directionsFilled[4] = true;

    //                // Create the new room with number of doors.
    //                RoomInfo newRoom = new RoomInfo(randDoorCount);
    //                newRoom.directionsFilled[4] = true;
    //                newRoom.position = locationVector;
    //                rooms.Add(newRoom);

    //                // Generate the door for this new room and link it to the previous room.
    //                CreateDoor(roomGo, width, height, 4);
    //            }

    //            positionFilled = false;
    //            break;
    //    }

        //switch (randDir)
        //{
        //        // North
        //    case 1:
        //        Debug.Log("North");

        //        // Vector to test new location.
        //        locationVector = new Vector3(previousX, 0.0f, (previousZ + (height * roomOffsetMultiplier)));

        //        // Check to see if the position is filled by another room.
        //        for (int a = 0; a < rooms.Count; a++)
        //        {
        //            if (locationVector == rooms[a].position)
        //            {
        //                positionFilled = true;
        //                break;
        //            }
        //        }

        //        // If the position is filled
        //        if (positionFilled == true)
        //        {
        //            // Decrease the counter so we can go through and try and place again.
        //            roomsPlaced--;
        //            previousZ = previousZ + (height * roomOffsetMultiplier);
        //        }
        //        else
        //        {
        //            // Create the room at this position
        //            roomGo = CreateRoom(width, height, name);
        //            roomGo.transform.position = locationVector;

        //            // Do rotation soon
        //            //roomGo.transform.Rotate(Vector3.up, rotation);
        //            RoomInfo newRoom = new RoomInfo();
        //            newRoom.position = locationVector;
        //            newRoom.numberOfDoors++;
        //            rooms.Add(newRoom);
        //            previousZ = previousZ + (height * roomOffsetMultiplier);
        //        }

        //        positionFilled = false;
        //        break;

        //        // East
        //    case 2:
        //        Debug.Log("East");

        //        locationVector = new Vector3((previousX + (width * roomOffsetMultiplier)), 0.0f, previousZ);

        //        for (int a = 0; a < rooms.Count; a++)
        //        {
        //            if (locationVector == rooms[a].position)
        //            {
        //                positionFilled = true;
        //                break;
        //            }
        //        }

        //        if (positionFilled == true)
        //        {
        //            roomsPlaced--;
        //            previousX = previousX + (width * roomOffsetMultiplier);
        //        }
        //        else
        //        {
        //            // Create the room at this position
        //            roomGo = CreateRoom(width, height, name);
        //            roomGo.transform.position = locationVector;

        //            // Do rotation soon
        //            //roomGo.transform.Rotate(Vector3.up, rotation);

        //            RoomInfo newRoom = new RoomInfo();
        //            newRoom.position = locationVector;
        //            newRoom.numberOfDoors++;
        //            rooms.Add(newRoom);
        //            previousX = previousX + (width * roomOffsetMultiplier);
        //        }

        //        positionFilled = false;
        //        break;

        //        // South
        //    case 3:
        //        Debug.Log("South");

        //        locationVector = new Vector3(previousX, 0.0f, (previousZ - (height * roomOffsetMultiplier)));

        //        for (int a = 0; a < rooms.Count; a++)
        //        {
        //            if (locationVector == rooms[a].position)
        //            {
        //                positionFilled = true;
        //                break;
        //            }
        //        }

        //        if (positionFilled == true)
        //        {
        //            roomsPlaced--;
        //            previousZ = previousZ - (height * roomOffsetMultiplier);
        //        }
        //        else
        //        {
        //            // Create the room at this position
        //            roomGo = CreateRoom(width, height, name);
        //            roomGo.transform.position = locationVector;

        //            // Do rotation soon
        //            //roomGo.transform.Rotate(Vector3.up, rotation);

        //            RoomInfo newRoom = new RoomInfo();
        //            newRoom.position = locationVector;
        //            newRoom.numberOfDoors++;
        //            rooms.Add(newRoom);
        //            previousZ = previousZ - (height * roomOffsetMultiplier);
        //        }

        //        positionFilled = false;
        //        break;

        //        // West
        //    case 4:
        //        Debug.Log("West");

        //        locationVector = new Vector3((previousX - (width * roomOffsetMultiplier)), 0.0f, previousZ);

        //        for (int a = 0; a < rooms.Count; a++)
        //        {
        //            if (locationVector == rooms[a].position)
        //            {
        //                positionFilled = true;
        //                break;
        //            }
        //        }

        //        if (positionFilled == true)
        //        {
        //            roomsPlaced--;
        //            previousX = previousX - (width * roomOffsetMultiplier);
        //        }
        //        else
        //        {
        //            // Create the room at this position
        //            roomGo = CreateRoom(width, height, name);
        //            roomGo.transform.position = locationVector;

        //            // Do rotation soon
        //            //roomGo.transform.Rotate(Vector3.up, rotation);

        //            RoomInfo newRoom = new RoomInfo();
        //            newRoom.position = locationVector;
        //            newRoom.numberOfDoors++;
        //            rooms.Add(newRoom);
        //            previousX = previousX - (width * roomOffsetMultiplier);
        //        }

        //        positionFilled = false;
        //        break;
        //}

        // Create the walls

        // Create the doors

        // Add to the list.
    }
}
