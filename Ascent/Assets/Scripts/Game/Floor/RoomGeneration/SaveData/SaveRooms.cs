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
    private string filePath = "Assets/Resources/Maps/RoomSaves.bin";

    public string FilePath 
    {
        get { return filePath; }
        set { filePath = value; }
    }

    public RoomSaves RoomSaves
    {
        get { return roomSaves; }
    }

    public RoomSaves LoadRooms()
    {
        roomSaves = (RoomSaves)XMLSerialiser.DeserializeObjectBin(FilePath);

        return roomSaves;
    }

    /// <summary>
    /// Saves a room to the file path specified.
    /// </summary>
    /// <param name="room">The room properties to save.</param>
    /// <param name="filePath">The file path for the room.</param>
    public void SaveRoom(RoomProperties room, string filePath)
    {
        RoomSaves newRoom = new RoomSaves();
        newRoom.saves.Add(room);
        XMLSerialiser.SerializeObjectBin(filePath, newRoom);
    }

    /// <summary>
    /// Loads a room from the file path specified.
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public RoomProperties LoadRoom(string filePath)
    {
        RoomSaves savedRoom = (RoomSaves)XMLSerialiser.DeserializeObjectBin(filePath);
        return (savedRoom.saves[0]);
    }
}
