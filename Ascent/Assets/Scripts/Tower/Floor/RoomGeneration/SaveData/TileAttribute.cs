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
