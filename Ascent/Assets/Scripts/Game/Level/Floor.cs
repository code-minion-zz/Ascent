using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Floor : MonoBehaviour 
{
	private List<Player> players;
	private GameObject[] startPoints;
	private GameObject gameCamera;

	// Camera offset
	//private const float cameraOffset = 15.0f;
	public bool orthographicCamera = false;


	public Camera MainCamera
	{
		get { return gameCamera.camera; }
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

	// Use this for initialization
	void Awake()
	{
		enemies = new List<Enemy>();
	}

	void Start()
	{

		//Resources.Load("Prefabs/Level" + Game.Singleton.GetChosenLevel);
		// Create the camera

		GameObject go = null;

		if (orthographicCamera)
		{
			go = Resources.Load("Prefabs/GameCameraOrtho") as GameObject;
		}
		else
		{
			go = Resources.Load("Prefabs/GameCamera") as GameObject;
		}

		startPoints = GameObject.FindGameObjectsWithTag("StartPoint");
		players = Game.Singleton.Players;

        for (int i = 0; i < players.Count; ++i)
        {
            Vector3 pos = startPoints[i].transform.position;
            players[i].Hero.transform.position = pos;
            players[i].Hero.transform.rotation = Quaternion.identity;
            players[i].Hero.transform.localScale = Vector3.one;
        }

		gameCamera = Instantiate(go) as GameObject;
        gameCamera.name = "GameCamera";
        gameCamera.transform.parent = GameObject.FindGameObjectWithTag("Cameras").transform;
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

		if (Input.GetKeyUp(KeyCode.F1))
		{
			EndFloor();
		}
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
		gameCamera.SetActive(false);

		// Disable input on all heroes
		foreach (Player player in players)
		{
			player.Hero.GetComponent<Hero>().HeroController.DisableInput();
			player.Hero.SetActive(false);
		}

		// Show summary screen
		Instantiate(Resources.Load("Prefabs/FloorSummary"));

		// Enable input on summary screen
	}

	#endregion
}
