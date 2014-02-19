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

	public GameObject groundPrefab = Resources.Load("Prefabs/RoomWalls/Ground") as GameObject;
	public GameObject wallPrefab = Resources.Load("Prefabs/RoomWalls/Wall") as GameObject;
	public GameObject wallWindowPrefab = Resources.Load("Prefabs/RoomWalls/WallWindow") as GameObject;
	public GameObject doorPrefab = Resources.Load("Prefabs/RoomWalls/Door") as GameObject;
	public GameObject cornerPrefab = Resources.Load("Prefabs/RoomWalls/WallCorner") as GameObject;
	public GameObject archPrefab = Resources.Load("Prefabs/RoomWalls/Archway") as GameObject;

	private List<Player> players;
    private List<Enemy> enemies;
	private GameObject[] startPoints;
	private GameObject floorCamera;
	private Room currentRoom;
	private Room targetRoom;
	private FadePlane fadePlane;
    private Room[] allRooms;
    private FloorInstanceReward floorInstanceReward;
	private bool randomFloor;
    //private FloorGeneration floorGenerator;

	public Enemy floorBoss;
	private bool bossKilled = false;

	// Camera offset
	//private const float cameraOffset = 15.0f;
	public bool orthographicCamera = false;

    public Room CurrentRoom
    {
        get { return currentRoom; }
    }

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

    public FloorInstanceReward FloorInstanceReward
    {
        get { return floorInstanceReward; }
    }

	public List<Enemy> Enemies
	{
		get { return enemies; }
	}

    public List<Player> Players
    {
        get { return players; }
    }

	public void InitialiseFloor()
    {
        InitialiseCamera();

        // Create HUD
        GameObject hudManagerGO = GameObject.Instantiate(Resources.Load("Prefabs/UI/FloorHUD")) as GameObject;
        hudManagerGO.GetComponent<FloorHUDManager>().Initialise();

        Initialise();
    }

	public void InitialiseRandomFloor()
	{
		InitialiseCamera();

        // Create HUD
        GameObject hudManagerGO = GameObject.Instantiate(Resources.Load("Prefabs/UI/FloorHUD")) as GameObject;
        hudManagerGO.GetComponent<FloorHUDManager>().Initialise();

		FloorGeneration floorGenerator = new FloorGeneration();
        floorGenerator.dungeonLevel = 1;
        floorGenerator.monsterRarity = Rarity.many;

		floorGenerator.GenerateFloor();
        floorGenerator.PopulateRooms();

		randomFloor = true;

		Initialise();
	}

    // Initialize the camera first.
    private void InitialiseCamera()
    {
       
        // Create the floor's camera
        GameObject go = null;

        if (orthographicCamera)
        {
            go = Resources.Load("Prefabs/Floors/floorCameraOrtho") as GameObject;
        }
        else
        {
            go = Resources.Load("Prefabs/Floors/floorCamera") as GameObject;
        }

        floorCamera = Instantiate(go) as GameObject;
        floorCamera.name = "FloorCamera";

        floorCamera.GetComponent<FloorCamera>().Initialise();

		//go = new GameObject();
		//go.name = "Cameras";
		//go.tag = "Cameras";

		//floorCamera.transform.parent = go.transform;
    }

	private void Initialise()
	{
		// Initialise the players onto the start points

		startPoints = GameObject.FindGameObjectsWithTag("StartPoint");

        if (startPoints == null)
        {
            Debug.Log("Could not find StartPoints please make sure there is an object with tag StartPoint");
        }

		players = Game.Singleton.Players;
		for (int i = 0; i < players.Count; ++i)
		{
			Vector3 pos = startPoints[i].transform.position;
			players[i].Hero.transform.position = pos;
			players[i].Hero.transform.rotation = Quaternion.identity;
			players[i].Hero.transform.localScale = Vector3.one;
			players[i].Hero.gameObject.SetActive(true);

            players[i].Hero.GetComponent<Hero>().onDeath += OnPlayerDeath;

            // Reset individual hero floor records.
            players[i].Hero.GetComponent<Hero>().FloorStatistics = new FloorStats();
		}

        // Finds all the rooms
		allRooms = GameObject.FindObjectsOfType<Room>() as Room[];
		currentRoom = GameObject.Find("Room 0: Start").GetComponent<Room>();

		if (!randomFloor)
		{
			currentRoom.Initialise();
		}

		// The floor generator will initilise the rooms. 
		// If the generator was not used then we must do it here.
		if (!randomFloor)
		{
			foreach (Room r in allRooms)
			{
				if(currentRoom == r)
				{
					continue;
				}
				r.Initialise();
			}
		}

		// Initialise all the enemies (Must occur after rooms are initialised)
        if (!randomFloor)
        {
            enemies = new List<Enemy>();
            GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
            for (int i = 0; i < monsters.Length; ++i)
            {
                Enemy thisEnemy = monsters[i].GetComponent<Enemy>();

                if (thisEnemy != null)
                {
                    thisEnemy.Initialise();
                    thisEnemy.InitiliseHealthbar();
                    thisEnemy.onDeath += OnEnemyDeath;
                    enemies.Add(thisEnemy);
                }
            }
        }

		// Init the fade plane
		GameObject go = Instantiate(Resources.Load("Prefabs/Floors/FadePlane")) as GameObject;
		fadePlane = go.GetComponent<FadePlane>();
		go.SetActive(false);

        // Create the floor record keeper.
        floorInstanceReward = new FloorInstanceReward(this);

		// Disable all the rooms.
		foreach (Room r in allRooms)
		{
			r.gameObject.SetActive(false);
		}

		// Activate the start room
		currentRoom.gameObject.SetActive(true);
	}

	public void AddEnemy(Enemy _enemy)
	{
		enemies.Add(_enemy);
	}

	#region Update

    /// <summary>
    /// Event called when a player has died on this floor.
    /// </summary>
    /// <param name="character">The character / hero that has died.</param>
    private void OnPlayerDeath(Character character)
    {
        Hero hero = character as Hero;

        if (hero != null)
        {
            if (hero.Lives > 0)
            {
                if (currentRoom.startRoom)
                {
                    //hero.Respawn(startPoints[0].transform.position);
                }
                else
                {
                    //hero.Respawn(currentRoom.EntryDoor.transform.position);
                    //hero.Respawn(startPoints[0].transform.position);
                }

                //--hero.stats.Lives;
                hero.FloorStatistics.NumberOfDeaths++;
            }
            else
            {
                hero.gameObject.SetActive(false);
                hero.onDeath -= OnPlayerDeath;
            }
        }
    }

    /// <summary>
    /// Event called when an enemy has died on the floor.
    /// </summary>
    /// <param name="character">The enemy that has died.</param>
    private void OnEnemyDeath(Character character)
    {
		Enemy enemy = character as Enemy;

		if (enemy.LastDamagedBy != null)
        {
            // This may break if the enemy was killed by something else such as a trap with no owner maybe?
            Hero hero = character.LastDamagedBy as Hero;
            hero.FloorStatistics.NumberOfMonstersKilled++;
        }

        // Give all players in the room the bounty.
        foreach (Player player in players)
        {
            Hero hero = player.Hero.GetComponent<Hero>();
            float expGain = enemy.EnemyStats.ExperienceBounty * hero.HeroStats.ExperienceGainBonus;
            hero.FloorStatistics.ExperienceGained += (int)expGain;
        }

		if(enemy == floorBoss)
		{
			bossKilled = true;
		}

        // Unsubscribe from listening to events from this enemy.
        enemy.onDeath -= OnEnemyDeath;
    }

	// Update is called once per frame
	void Update()
	{
		if(bossKilled == true || Input.GetKeyUp(KeyCode.F1))
		{
			EndFloor();
		}
		////DEBUG
		//if (Input.GetKeyUp(KeyCode.F1))
		//{
		//    EndFloor();
		//}
		if (currentRoom.Doors == null)
		{
			return;
		}

		Door[] roomDoors = currentRoom.Doors.doors;
		if (Input.GetKeyUp(KeyCode.Keypad8)) // UP
		{
			foreach (Door d in roomDoors)
			{
				if (d == null) continue;
				if (d.targetDoor != null && d.direction == TransitionDirection.North)
				{
					TransitionToRoomImmediately(Floor.TransitionDirection.North, d.targetDoor);
				}
			}
		}
		else if (Input.GetKeyUp(KeyCode.Keypad2))
		{
			foreach (Door d in roomDoors)
			{
				if (d == null) continue;
				if (d.targetDoor != null && d.direction == TransitionDirection.South)
				{
					TransitionToRoomImmediately(Floor.TransitionDirection.South, d.targetDoor);
				}
			};
		}
		else if (Input.GetKeyUp(KeyCode.Keypad4))
		{
			foreach (Door d in roomDoors)
			{
				if (d == null) continue;
				if (d.targetDoor != null && d.direction == TransitionDirection.West)
				{
					TransitionToRoomImmediately(Floor.TransitionDirection.West, d.targetDoor);
				}
			}
		}
		else if (Input.GetKeyUp(KeyCode.Keypad6))
		{
			foreach (Door d in roomDoors)
			{
				if (d == null) continue;
				if (d.targetDoor != null && d.direction == TransitionDirection.East)
				{
					TransitionToRoomImmediately(Floor.TransitionDirection.East, d.targetDoor);
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
			player.Hero.gameObject.SetActive(false);
		}

		// Show summary screen
		//Instantiate(Resources.Load("Prefabs/FloorSummary"));

        floorInstanceReward.ApplyFloorInstanceRewards();

        Game.Singleton.LoadLevel("FloorSummary", Game.EGameState.Town);

		// Enable input on summary screen
	}


	public void TransitionToRoom(TransitionDirection direction, Door targetDoor)
	{
		// Set old remove inactive and new one active
		fadePlane.StartFade(0.5f, currentRoom.transform.position);

		StartCoroutine(CoTransitionToRoom());

		targetRoom = targetDoor.transform.parent.parent.parent.GetComponent<Room>();


		Room prevRoom = currentRoom;
		currentRoom = targetRoom;
		targetRoom = prevRoom;

		currentRoom.gameObject.SetActive(true);

		// Move heroes to the new room also disable the controller
		foreach (Player p in players)
		{
			p.Hero.transform.position = targetDoor.transform.position;
			p.Hero.Loadout.StopAbility();
			p.Hero.Motor.StopMotion();
			p.Hero.HeroController.enabled = false;
		}

		// Move camera over
		FloorCamera.TransitionToRoom(direction);

		currentRoom.EntryDoor = targetDoor;
		targetDoor.SetAsStartDoor();
	}

	public IEnumerator CoTransitionToRoom()
	{
		yield return new WaitForSeconds(1.5f);

		targetRoom.gameObject.SetActive(false);

		// Move heroes to the new room also disable the controller
		foreach (Player p in players)
		{
			p.Hero.HeroController.enabled = true;
		}

		//fadePlane.gameObject.SetActive(false);
	}

	public void TransitionToRoomImmediately(TransitionDirection direction, Door targetDoor)
	{
		targetRoom = targetDoor.transform.parent.parent.parent.GetComponent<Room>();

		currentRoom.gameObject.SetActive(false);
		currentRoom = targetRoom;
		currentRoom.gameObject.SetActive(true);

		// Move heroes to the new room
		foreach (Player p in players)
		{
			p.Hero.transform.position = targetDoor.transform.position;
		}

		// Move camera over
        FloorCamera.TransitionToRoom(direction);

		currentRoom.EntryDoor = targetDoor;
		targetDoor.SetAsStartDoor();

		targetRoom.gameObject.SetActive(true);
	}

	#endregion
}
