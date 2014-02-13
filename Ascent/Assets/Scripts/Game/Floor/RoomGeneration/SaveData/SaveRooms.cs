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
    public string FilePath { get; set; }

    public RoomSaves RoomSaves
    {
        get { return roomSaves; }
    }

    public void Initialize()
    {
        FilePath = string.Format("Assets/Resources/Maps/RoomSaves.bin");
        //LoadRooms();
    }

    [ContextMenu("SaveRooms")]
    public void SaveAllRooms()
    {
        XMLSerialiser.SerializeObjectBin(FilePath, roomSaves);
    }

    public void AddNewRoom(RoomProperties data)
    {
        roomSaves.saves.Add(data);
    }

    public RoomSaves LoadRooms()
    {
        roomSaves = (RoomSaves)XMLSerialiser.DeserializeObjectBin(FilePath);

        return roomSaves;
    }
}
