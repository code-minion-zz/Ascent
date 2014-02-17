using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveRooms
{
    private RoomSaves roomSaves = new RoomSaves();
    private string filePath;

    public string FilePath 
    {
        get { return filePath; }
        set { filePath = value; }
    }

    public RoomSaves RoomSaves
    {
        get { return roomSaves; }
    }

    public void Awake()
    {
        Initialize();
    }

    public void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        roomSaves = new RoomSaves();
        //FilePath = string.Format("Assets/Resources/Maps/RoomSaves.bin");
        FilePath = string.Format("Maps/RoomSaves");
    }

    [ContextMenu("SaveRooms")]
    public void SaveAllRooms()
    {
        XMLSerialiser.SerializeObjectBin(FilePath, roomSaves);
        XMLSerialiser.SerializeToString("Assets/Resources/Maps/RoomSaves.txt", roomSaves);
    }

    public void AddNewRoom(RoomProperties data)
    {
        roomSaves.saves.Add(data);
    }

    public RoomSaves LoadRooms()
    {
        roomSaves = (RoomSaves)XMLSerialiser.DeserializeObjectBin(FilePath, true);

        return roomSaves;
    }

    [ContextMenu("Load First Room")]
    public void LoadFirstRoom()
    {
        Initialize();
        roomSaves = (RoomSaves)XMLSerialiser.DeserializeObjectBin(filePath, true);

        if (roomSaves != null)
        {
            Debug.Log(roomSaves.saves.Count);
            // Reconstruct the first room.
            RoomGeneration roomGen = new RoomGeneration();
            RoomProperties firstRoom = roomSaves.saves[1];
            roomGen.ReconstructRoom(firstRoom);
        }
    }
}
