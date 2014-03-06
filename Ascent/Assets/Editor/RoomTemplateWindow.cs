#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

public class RoomTemplateWindow : EditorWindow
{
	private string roomName;
	private int numberOfTilesX;
	private int numberOfTilesY;
	private RoomGeneration roomGenRef;

    bool rotated;

	public void Initialise(RoomGeneration generator)
	{
		roomGenRef = generator;
	}

	void OnGUI()
	{
		roomName = EditorGUILayout.TextField("Room Name", roomName);
        rotated = EditorGUILayout.Toggle(rotated);

        bool createRoom = false;
        int width = 0;
        int height = 0;

        if (GUILayout.Button("11x11 (22x22)"))
        {
            width = 11;
            height = 11;
            createRoom = true;
        }

        if (GUILayout.Button("11x9 (22x18)"))
        {
            width = 11;
            height = 9;
            createRoom = true;
        }

        if (GUILayout.Button("11x7 (22x14)"))
        {
            width = 11;
            height = 7;
            createRoom = true;
        }

        if (GUILayout.Button("11x5 (22x10)"))
        {
            width = 11;
            height = 5;
            createRoom = true;
        }

        if (GUILayout.Button("11x3 (22x6)"))
        {
            width = 11;
            height = 3;
            createRoom = true;
        }

        if (GUILayout.Button("9x9 (18x18)"))
        {
            width = 9;
            height = 9;
            createRoom = true;
        }

        if (GUILayout.Button("9x7 (18x14) Standard"))
        {
            width = 9;
            height = 7;
            createRoom = true;
        }

        if (GUILayout.Button("9x5 (18x10)"))
        {
            width = 9;
            height = 5;
            createRoom = true;
        }

        if (GUILayout.Button("9x3 (18x6)"))
        {
            width = 9;
            height = 3;
            createRoom = true;
        }

        if (GUILayout.Button("7x7 (14x14)"))
        {
            width = 7;
            height = 7;
            createRoom = true;
        }

        if (GUILayout.Button("7x5 (14x10)"))
        {
            width = 7;
            height = 5;
            createRoom = true;
        }

        if (GUILayout.Button("7x3 (14x6)"))
        {
            width = 7;
            height = 3;
            createRoom = true;
        }

        if (GUILayout.Button("5x5 (10x10)"))
        {
            width = 5;
            height = 5;
            createRoom = true;
        }

        if (GUILayout.Button("5x3 (10x6)"))
        {
            width = 5;
            height = 3;
            createRoom = true;
        }

        if (GUILayout.Button("3x3 (6x6)"))
        {
            width = 3;
            height = 3;
            createRoom = true;
        }

        if (createRoom)
        {
            if (rotated)
            {
                int temp = height;
                height = width;
                width = temp;
            }

            CreateRoom(width, height);
        }
	}

	private void CreateRoom(int numberOfTilesX, int numberOfTilesY)
	{
		//RoomProperties roomProperties = roomGenRef.CreateNewRoom(numberOfTilesX * 2, numberOfTilesY * 2, roomName);
        RoomProperties room = roomGenRef.CreateNewRoom(RoomConnectionType.Empty, numberOfTilesX, numberOfTilesY);
        room.ConstructRoom();
        roomGenRef.PlaceGroundTiles(room);
        roomGenRef.PlaceWalls(room);
		this.Close();
	}
}
#endif