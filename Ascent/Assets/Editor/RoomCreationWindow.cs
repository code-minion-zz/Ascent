#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

public class RoomCreationWindow : EditorWindow
{
    private string roomName;
    private int numberOfTilesX;
    private int numberOfTilesY;
    private RoomGeneration roomGenRef;

    public void Initialise(RoomGeneration generator)
    {
        roomGenRef = generator;
    }

    void OnGUI()
    {
        roomName = EditorGUILayout.TextField("Room Name", roomName);
        numberOfTilesX = EditorGUILayout.IntField("Number Of Tiles X", numberOfTilesX);
        numberOfTilesY = EditorGUILayout.IntField("Number Of Tiles Y", numberOfTilesY);

        if (GUILayout.Button("Create"))
        {
            RoomProperties roomProperties = roomGenRef.CreateNewRoom(numberOfTilesX * 2, numberOfTilesY * 2, roomName);
            this.Close();
        }
    }
}
#endif