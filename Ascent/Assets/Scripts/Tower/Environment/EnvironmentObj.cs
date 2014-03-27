using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum EnvironmentID
{
    none = 0,
    groundTile = 1,
    door = 2,
    standardWall = 3,
    cornerWallTile = 8,
    pillar = 9,
    chest = 10,
    randMisc = 11,
    brazier = 12,
    arrowShooter = 13,
    monster = 14,
    spinningBlade = 15,
}

public class EnvironmentObj : MonoBehaviour
{
    public EnvironmentID environmentID;

    public EnvironmentID TileAttributeType
    {
        get { return environmentID; }
        set { environmentID = value; }
    }

    public virtual void Update()
    {

    }
}
