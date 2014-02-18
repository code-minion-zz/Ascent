using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

[Serializable]
public class RoomSaves
{
    public List<RoomProperties> saves = new List<RoomProperties>();
}
