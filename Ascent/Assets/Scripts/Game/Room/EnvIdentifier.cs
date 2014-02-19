using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnvIdentifier : MonoBehaviour
{
    public TileType tileAttributeType;

    public TileType TileAttributeType
    {
        get { return tileAttributeType; }
        set { tileAttributeType = value; }
    }
}
