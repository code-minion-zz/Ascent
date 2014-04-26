using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Responsible for containing the data used save and load room configurations.
/// </summary>
public class RoomCreationData
{
    public Tile[,] Tiles { get; set; }
    public string Name { get; set; }
}
