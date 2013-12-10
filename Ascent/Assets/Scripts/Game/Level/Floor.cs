using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Floor : MonoBehaviour 
{
	private List<Player> players;
	private GameObject[] startPoints;
	private GameObject floorCamera;

	// Camera offset
	//private const float cameraOffset = 15.0f;
	public bool orthographicCamera = false;


	public Camera MainCamera
	{
		get { return floorCamera.camera; }
	}

	public GameObject[] StartPoints
	{
		get { return startPoints; }
	}

	private List<Enemy> enemies;
	public List<Enemy> Enemies
	{
		get { return enemies; }
	}

	//private FloorRecordKeeper recordKeeper;
	//public FloorRecordKeeper Records
	//{
	//    get { return recordKeeper; }
	//}

	public void Initialise()
	{
        //// TODO: Load the prefab for this level
        ////Resources.Load("Prefabs/Level" + Game.Singleton.GetChosenLevel);

		// Create HUD
		GameObject.Instantiate(Resources.Load("Prefabs/UI/HUD"));

		// Initialise the players onto the start points
		startPoints = GameObject.FindGameObjectsWithTag("StartPoint");
		players = Game.Singleton.Players;

		for (int i = 0; i < players.Count; ++i)
		{
			Vector3 pos = startPoints[i].transform.position;
			players[i].Hero.transform.position = pos;
			players[i].Hero.transform.rotation = Quaternion.identity;
			players[i].Hero.transform.localScale = Vector3.one;
			players[i].Hero.SetActive(true);
		}

		// Create the floor's camera
		GameObject go = null;

		if (orthographicCamera)
		{
			go = Resources.Load("Prefabs/floorCameraOrtho") as GameObject;
		}
		else
		{
			go = Resources.Load("Prefabs/floorCamera") as GameObject;
		}

		floorCamera = Instantiate(go) as GameObject;
		floorCamera.name = "floorCamera";

		floorCamera.GetComponent<FloorCamera>().Initialise();

		go = new GameObject();
		go.name = "Cameras";
		go.tag = "Cameras";

		floorCamera.transform.parent = go.transform;

		// Initialise all the enemies
		enemies = new List<Enemy>();

		GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

		for (int i = 0; i < monsters.Length; ++i)
		{
			Enemy thisEnemy = monsters[i].GetComponent<Enemy>();

			if (thisEnemy != null)
			{
				thisEnemy.Initialise();
				enemies.Add(thisEnemy);
			}
		}

        Debug.Log("Enemies: " + enemies.Count);
	}

	public void AddEnemy(Enemy _enemy)
	{
		enemies.Add(_enemy);
	}

	#region Update

	// Update is called once per frame
	void Update()
	{
		HandleDeadHeroes();
        HandleDeadMonsters();

		if (Input.GetKeyUp(KeyCode.F1))
		{
			EndFloor();
		}
	}

    void HandleDeadMonsters()
    {

    }

    // TODO: Make all the players start spawn at the point.
	void HandleDeadHeroes()
	{
		foreach (Player player in players)
		{
			Hero hero = player.Hero.GetComponent<Hero>();

			if (hero.IsDead)
			{
				hero.Respawn(startPoints[0].transform.position);
			}
		}
	}

	void EndFloor()
	{
		// Disable the whole floor( audio listener from the camera )
		enabled = false;
		floorCamera.SetActive(false);

		// Disable input on all heroes
		foreach (Player player in players)
		{
			//player.Hero.GetComponent<Hero>().HeroController.DisableInput();
			player.Hero.SetActive(false);
		}

		// Show summary screen
		//Instantiate(Resources.Load("Prefabs/FloorSummary"));

        Game.Singleton.LoadLevel("Level2", Game.EGameState.Tower);

		// Enable input on summary screen
	}

	#endregion
}
