using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Floor : MonoBehaviour 
{

	public enum TransitionDirection
	{
		North = 0,
		South,
		East,
		West
	}

	private List<Player> players;
	private GameObject[] startPoints;
	private GameObject floorCamera;
	private Room currentRoom;
	private Room targetRoom;
	private FadePlane fadePlane;
    private Room[] allRooms;

	// Camera offset
	//private const float cameraOffset = 15.0f;
	public bool orthographicCamera = false;


	public Camera MainCamera
	{
		get { return floorCamera.camera; }
	}

	public FloorCamera FloorCamera
	{
		get { return floorCamera.GetComponent<FloorCamera>(); }
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

		currentRoom = GameObject.Find("StartRoom").GetComponent<Room>();

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

		go = Instantiate(Resources.Load("Prefabs/FadePlane")) as GameObject;
		fadePlane = go.GetComponent<FadePlane>();
		go.SetActive(false);

        Debug.Log("Enemies: " + enemies.Count);

        allRooms = GameObject.FindObjectsOfType<Room>() as Room[];

        foreach (Room r in allRooms)
        {
            r.gameObject.SetActive(false);
        }

        currentRoom.gameObject.SetActive(true);
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
                if (hero.DerivedStats.Lives > 0)
                {
				    hero.Respawn(currentRoom.EntryDoor.transform.position);
                    --hero.DerivedStats.Lives;
                }
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


	public void TransitionToRoom(TransitionDirection direction, Door targetDoor)
	{
		// Set old remove inactive and new one active
		fadePlane.StartFade(1.5f, currentRoom.transform.position);

		StartCoroutine(CoTransitionToRoom());

		targetRoom = targetDoor.transform.parent.parent.GetComponent<Room>();
		targetRoom.gameObject.SetActive(true);

		// Move camera over
		FloorCamera.TransitionToRoom(direction);

		// Move heroes to the new room
		foreach(Player p in players)
		{
			p.Hero.transform.position = targetDoor.transform.position;
		}

		targetDoor.SetAsStartDoor();

	}

	public IEnumerator CoTransitionToRoom()
	{
		yield return new WaitForSeconds(1.5f);

		currentRoom.gameObject.SetActive(false);

		currentRoom = targetRoom;

		fadePlane.gameObject.SetActive(false);
	}

	#endregion
}
