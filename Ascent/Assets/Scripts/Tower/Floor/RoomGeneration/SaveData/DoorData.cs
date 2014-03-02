using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DoorData
{
    public Floor.TransitionDirection Direction { get; set; }
    public bool IsEntryDoor { get; set; }
    private bool isConnected;

    [NonSerialized]
    private Door doorObject;

    public bool IsConnect
    {
        get { return isConnected; }
        set { isConnected = value; }
    }

    public Door DoorObject
    {
        get { return doorObject; }
        set { doorObject = value; }
    }
}
