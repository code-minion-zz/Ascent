using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DungeonGenerator : MonoBehaviour 
{
    public GameObject floor;
    public GameObject wall;
    public GameObject torch;
    List<Vector3> panels = new List<Vector3>();
    bool positionFilled;
    public int panelsToPlace = 50;

    int tempRandom;
    int panelsPlaced;
    float previousX = 0.0f;
    float previousZ = 0.0f;
    Vector3 locationVector;
    int torchPlace;

    Vector3 positionToCheck;
    Vector3 positionToCheck2;
    Vector3 positionToCheck3;
    Vector3 positionToCheck4;

    public bool buildCeiling = false;
    public float groundTileOffset = 20.0f;
    public float wallXZOffset = 4.86f;
    public float wallYOffset = 0.0f;

    private GameObject parent;

    void Awake()
    {

    }

    public void GenerateDungeon()
    {
        panels.Clear();
        panels = new List<Vector3>();

        positionFilled = false;

        tempRandom = 0;
        previousX = 0.0f;
        previousZ = 0.0f;
        locationVector = Vector3.zero;
        torchPlace = 0;

        positionToCheck = Vector3.zero;
        positionToCheck2 = Vector3.zero;
        positionToCheck3 = Vector3.zero;
        positionToCheck4 = Vector3.zero;


        parent = new GameObject("GeneratedDungeon");

        GameObject go = Instantiate(floor, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity) as GameObject;
        go.name = floor.name;
        go.transform.parent = parent.transform;

        if (buildCeiling)
        {
            GameObject ceiling = Instantiate(floor, new Vector3(0.0f, 10.0f, 0.0f), Quaternion.identity) as GameObject;
            ceiling.name = "Ceiling";
            ceiling.transform.parent = parent.transform;
        }

        panels.Add(new Vector3(0.0f, 0.0f, 0.0f));
        positionFilled = false;

        // Go through and place all the floor components based on the number of them we have.
        for (panelsPlaced = 0; panelsPlaced < panelsToPlace; panelsPlaced++)
        {
            placePanel();
        }

        placeWalls();
    }
 
    void placePanel()
    {
        tempRandom = Random.Range(1, 5);

		int tempRot = Random.Range(1, 4);
		float rotation = 0.0f;
		
		switch (tempRot)
		{
		case 1:
			rotation = 90.0f;
			break;
			
		case 2:
			rotation = 180.0f;
			break;
			
		case 3:
			rotation = 270.0f;
			break;
			
		case 4:
			rotation = 360.0f;
			break;
			
		default:
			Debug.Log("Unknown rotation rand: " + tempRot);
			break;
		}

        if (tempRandom == 1)
        {
            locationVector = new Vector3((previousX + groundTileOffset), 0.0f, previousZ);

            for (int a = 0; a < panels.Count; a++)
            {
                if (locationVector == panels[a])
                {
                    positionFilled = true;
                    break;
                }
            }

            if (positionFilled == true)
            {
                panelsPlaced = panelsPlaced - 1;
                previousX = previousX + groundTileOffset;
            }
            else
            {
                GameObject go = Instantiate(floor, locationVector, Quaternion.identity) as GameObject;
                go.name = floor.name;
                go.transform.parent = parent.transform;
				go.transform.Rotate(Vector3.up, rotation);

                if (buildCeiling)
                {
                    GameObject ceiling = Instantiate(floor, new Vector3(locationVector.x, (locationVector.y + 10), locationVector.z), Quaternion.identity) as GameObject;
                    ceiling.name = "Ceiling";
                    ceiling.transform.parent = parent.transform;
                }

                panels.Add(locationVector);
                previousX = previousX + groundTileOffset;
            }

            positionFilled = false;
        }

        if (tempRandom == 2)
        {
            locationVector = new Vector3((previousX - groundTileOffset), 0, previousZ);

            for (int b = 0; b < panels.Count; b++)
            {
                if (locationVector == panels[b])
                {
                    positionFilled = true;
                    break;
                }
            }

            if (positionFilled == true)
            {
                panelsPlaced = panelsPlaced - 1;
                previousX = previousX - groundTileOffset;
            }
            else
            {
                GameObject go = Instantiate(floor, locationVector, Quaternion.identity) as GameObject;
                go.name = floor.name;
                go.transform.parent = parent.transform;
				go.transform.Rotate(Vector3.up, rotation);

                if (buildCeiling)
                {
                    GameObject ceiling = Instantiate(floor, new Vector3(locationVector.x, (locationVector.y + 10), locationVector.z), Quaternion.identity) as GameObject;
                    ceiling.name = "Ceiling";
                    ceiling.transform.parent = parent.transform;
                }

                panels.Add(locationVector);
                previousX = previousX - groundTileOffset;
            }

            positionFilled = false;
        }

        if (tempRandom == 3)
        {
            locationVector = new Vector3((previousX), 0, (previousZ + groundTileOffset));

            for (int c = 0; c < panels.Count; c++)
            {
                if (locationVector == panels[c])
                {
                    positionFilled = true;
                    break;
                }
            }

            if (positionFilled == true)
            {
                panelsPlaced = panelsPlaced - 1;
                previousZ = previousZ + groundTileOffset;
            }
            else
            {
                GameObject go = Instantiate(floor, locationVector, Quaternion.identity) as GameObject;
                go.name = floor.name;
                go.transform.parent = parent.transform;
				go.transform.Rotate(Vector3.up, rotation);

                if (buildCeiling)
                {
                    GameObject ceiling = Instantiate(floor, new Vector3(locationVector.x, (locationVector.y + 10), locationVector.z), Quaternion.identity) as GameObject;
                    ceiling.name = "Ceiling";
                    ceiling.transform.parent = parent.transform;
                } 
                
                panels.Add(locationVector);
                previousZ = previousZ + groundTileOffset;

            }

            positionFilled = false;
        }

        if (tempRandom == 4)
        {
            locationVector = new Vector3((previousX), 0, (previousZ - groundTileOffset));

            for (int d = 0; d < panels.Count; d++)
            {
                if (locationVector == panels[d])
                {
                    positionFilled = true;
                    break;
                }
            }

            if (positionFilled == true)
            {
                panelsPlaced = panelsPlaced - 1;
                previousZ = previousZ - groundTileOffset;
            }
            else
            {
                GameObject go = Instantiate(floor, locationVector, Quaternion.identity) as GameObject;
                go.name = floor.name;
                go.transform.parent = parent.transform;
				go.transform.Rotate(Vector3.up, rotation);

                if (buildCeiling)
                {
                    GameObject ceiling = Instantiate(floor, new Vector3(locationVector.x, (locationVector.y + 10), locationVector.z), Quaternion.identity) as GameObject;
                    ceiling.name = "Ceiling";
                    ceiling.transform.parent = parent.transform;
                }
                panels.Add(locationVector);
                previousZ = previousZ - groundTileOffset;
            }

            positionFilled = false;
        }
    }

    void placeWalls()
    {
        List<Vector3> floorArray = panels;
        bool buildWall1 = true;
        bool buildWall2 = true;
        bool buildWall3 = true;
        bool buildWall4 = true;

        for (int e = 0; e < floorArray.Count; e++)
        {
            buildWall1 = true;
            buildWall2 = true;
            buildWall3 = true;
            buildWall4 = true;

            for (int f = 0; f < floorArray.Count; f++)
            {
                positionToCheck = new Vector3((floorArray[e].x + groundTileOffset), floorArray[e].y, (floorArray[e].z));
                positionToCheck2 = new Vector3((floorArray[e].x - groundTileOffset), floorArray[e].y, (floorArray[e].z));
                positionToCheck3 = new Vector3((floorArray[e].x), floorArray[e].y, (floorArray[e].z + groundTileOffset));
                positionToCheck4 = new Vector3((floorArray[e].x), floorArray[e].y, (floorArray[e].z - groundTileOffset));

                if (floorArray[f] == positionToCheck)
                {
                    buildWall1 = false;
                }

                if (floorArray[f] == positionToCheck2)
                {
                    buildWall2 = false;
                }           

                if (floorArray[f] == positionToCheck3)
                {
                    buildWall3 = false;
                }           

                if (floorArray[f] == positionToCheck4)
                {
                    buildWall4 = false;
                }                       
            }

            if (buildWall1 == true)
            {

                GameObject go = Instantiate(wall, new Vector3((floorArray[e].x + wallXZOffset), (floorArray[e].y + wallYOffset), floorArray[e].z), Quaternion.identity) as GameObject; 
                go.transform.parent = parent.transform;
                torchPlace = Random.Range(1, 6);

                if (torchPlace == 5)
                {
                    GameObject torchObj = Instantiate(torch, new Vector3((floorArray[e].x + 4.7f), (floorArray[e].y + 3.0f), floorArray[e].z), Quaternion.identity) as GameObject;
                    torchObj.transform.parent = parent.transform;
                    torchObj.transform.parent = parent.transform;
                }
            }

            if (buildWall2 == true)
            {
                GameObject go = Instantiate(wall, new Vector3((floorArray[e].x - wallXZOffset), (floorArray[e].y + wallYOffset), floorArray[e].z), Quaternion.identity) as GameObject;
                go.transform.parent = parent.transform;
                torchPlace = Random.Range(1, 6); 

                if (torchPlace == 5)
                {
                    GameObject torchObj = Instantiate(torch, new Vector3((floorArray[e].x - 4.7f), (floorArray[e].y + 3), floorArray[e].z), Quaternion.identity) as GameObject;
                    torchObj.transform.Rotate(Vector3.up, 180);
                    torchObj.transform.parent = parent.transform;
                }
            }

            if (buildWall3 == true)
            {
                GameObject go = Instantiate(wall, new Vector3((floorArray[e].x), (floorArray[e].y + wallYOffset), (floorArray[e].z + wallXZOffset)), Quaternion.identity) as GameObject;
                go.transform.Rotate(Vector3.up, 90);
                go.transform.parent = parent.transform;
                torchPlace = Random.Range(1, 6);

                if (torchPlace == 5)
                {
                    GameObject torchObj = Instantiate(torch, new Vector3((floorArray[e].x), (floorArray[e].y + 3), (floorArray[e].z + 4.7f)), Quaternion.identity) as GameObject;
                    torchObj.transform.Rotate(Vector3.up, -90);
                    torchObj.transform.parent = parent.transform;
                }
            }

            if (buildWall4 == true)
            {
                GameObject go = Instantiate(wall, new Vector3((floorArray[e].x), (floorArray[e].y + wallYOffset), (floorArray[e].z - wallXZOffset)), Quaternion.identity) as GameObject;
                go.transform.Rotate(Vector3.up, 90);
                go.transform.parent = parent.transform;
                torchPlace = Random.Range(1, 6);

                if (torchPlace == 5)
                {
                    GameObject torchGo = Instantiate(torch, new Vector3((floorArray[e].x), (floorArray[e].y + 3), (floorArray[e].z - 4.7f)), Quaternion.identity) as GameObject;
                    torchGo.transform.Rotate(Vector3.up, 90);
                    torchGo.transform.parent = parent.transform;
                }
            }
        }       
    }
}