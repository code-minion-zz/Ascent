using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

[Serializable]
public class TileAttribute
{
    public TileType Type { get; set; }
    public float Angle { get; set; }
}

[Serializable]
public class DoorTile : TileAttribute
{
    public bool IsConnected { get; set; }
    public bool IsEntryDoor { get; set; }
    public Floor.TransitionDirection Direction { get; set; }
}