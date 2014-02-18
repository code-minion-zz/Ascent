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
    private RoomProperties roomProperties;
    private Ascent.LevelEditor roomEditorRef;

    public RoomProperties RoomProperties
    {
        get { return roomProperties; }
    }

    public void Initialise(Ascent.LevelEditor roomEditor, RoomGeneration generator)
    {
        roomGenRef = generator;
        roomEditorRef = roomEditor;
    }

    void OnGUI()
    {
        roomName = EditorGUILayout.TextField("Room Name", roomName);
        numberOfTilesX = EditorGUILayout.IntField("Number Of Tiles X", numberOfTilesX);
        numberOfTilesY = EditorGUILayout.IntField("Number Of Tiles Y", numberOfTilesY);

        if (GUILayout.Button("Create"))
        {
            roomProperties = roomGenRef.CreateNewRoom(numberOfTilesX * 2, numberOfTilesY * 2, roomName);

            if (roomProperties != null)
            {
                roomEditorRef.AddRoom(roomProperties);
            }

            EditorWindow.DestroyImmediate(this);
        }
    }
}
#endif