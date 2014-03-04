using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum MiscObjectType
{
    barrelCluster,
    barrel
}

public static class EnvironmentFactory
{
    private static GameObject floorObject = Resources.Load("Prefabs/RoomWalls/GroundTile_2x2") as GameObject;
    private static GameObject wallObject = Resources.Load("Prefabs/RoomWalls/Wall") as GameObject;
    private static GameObject wallCorner = Resources.Load("Prefabs/RoomWalls/WallCorner") as GameObject;
    private static GameObject wallWindow = Resources.Load("Prefabs/RoomWalls/WallWindow") as GameObject;
    private static GameObject doorObject = Resources.Load("Prefabs/RoomWalls/Door") as GameObject;

    private static GameObject barrelObject = Resources.Load("Prefabs/RoomPieces/Barrel") as GameObject;
    private static GameObject barrelCluster = Resources.Load("Prefabs/RoomPieces/BarrelCluster") as GameObject;
    private static GameObject brazierObject = Resources.Load("Prefabs/RoomPieces/Brazier") as GameObject;
    private static GameObject pillarObject = Resources.Load("Prefabs/RoomPieces/Pillar") as GameObject;
    private static GameObject arrowShooter = Resources.Load("Prefabs/Hazards/ArrowShooter") as GameObject;
    private static GameObject spinningBlade = Resources.Load("Prefabs/Hazards/SpinningBlade") as GameObject;

    public static GameObject CreateMiscObject(MiscObjectType type)
    {
        GameObject go = null;

        switch (type)
        {
            case MiscObjectType.barrel:
                go = GameObject.Instantiate(barrelObject, Vector3.zero, barrelObject.transform.rotation) as GameObject;
                go.name = barrelObject.name;
                break;

            case MiscObjectType.barrelCluster:
                go = GameObject.Instantiate(barrelCluster, Vector3.zero, barrelCluster.transform.rotation) as GameObject;
                go.name = barrelCluster.name;
                break;
        }

        return go;
    }

    /// <summary>
    /// Returns a newly created object based on the type.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static GameObject CreateGameObjectByType(TileType type)
    {
        GameObject go = null;

        switch (type)
        {
            case TileType.groundTile:
                go = GameObject.Instantiate(floorObject, Vector3.zero, floorObject.transform.rotation) as GameObject;
                go.name = floorObject.name;
                break;

            case TileType.standardWall:
                go = GameObject.Instantiate(wallObject, Vector3.zero, wallObject.transform.rotation) as GameObject;
                go.name = wallObject.name;
                break;

            case TileType.cornerWallTile:
                go = GameObject.Instantiate(wallCorner, Vector3.zero, wallCorner.transform.rotation) as GameObject;
                go.name = wallCorner.name;
                break;

            case TileType.brazier:
                go = GameObject.Instantiate(brazierObject, Vector3.zero, brazierObject.transform.rotation) as GameObject;
                go.name = brazierObject.name;
                break;

            case TileType.pillar:
                go = GameObject.Instantiate(pillarObject, Vector3.zero, pillarObject.transform.rotation) as GameObject;
                go.name = pillarObject.name;
                break;

            case TileType.door:
                go = GameObject.Instantiate(doorObject, Vector3.zero, doorObject.transform.rotation) as GameObject;
                go.name = doorObject.name;
                break;

            case TileType.arrowShooter:
                go = GameObject.Instantiate(arrowShooter, Vector3.zero, arrowShooter.transform.rotation) as GameObject;
                go.name = arrowShooter.name;
                break;

            case TileType.spinningBlade:
                go = GameObject.Instantiate(spinningBlade, Vector3.zero, spinningBlade.transform.rotation) as GameObject;
                go.name = spinningBlade.name;
                break;
        }

        return go;
    }
}
