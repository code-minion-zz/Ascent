using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveRooms
{
    public List<RoomProperties> LoadAllRooms(string directory)
    {
        List<RoomProperties> rooms = new List<RoomProperties>();

        DirectoryInfo info = new DirectoryInfo(directory);
        if (!info.Exists)
        {
            Debug.Log("Path does not exist");
        }

        FileInfo[] fileInfo = info.GetFiles();

        foreach (FileInfo file in fileInfo)
        {
            if (file.Extension == ".txt")
            {
                RoomProperties room = LoadRoom(file.FullName, false);

                if (room != null)
                {
                    rooms.Add(room);
                }
            }
        }

        return rooms;
    }

    /// <summary>
    /// Saves a room to the file path specified.
    /// </summary>
    /// <param name="room">The room properties to save.</param>
    /// <param name="filePath">The file path for the room.</param>
    public void SaveRoom(RoomProperties room, string filePath)
    {
        XMLSerialiser.SerializeObjectBin(filePath, room);
    }

    /// <summary>
    /// Loads a room from the file path specified.
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public RoomProperties LoadRoom(string filePath, bool isResource)
    {
        RoomProperties savedRoom = (RoomProperties)XMLSerialiser.DeserializeObjectBin(filePath, isResource);
        return (savedRoom);
    }
}
