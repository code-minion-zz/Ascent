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
		West,
		MAX
	}

	private List<Hero> heroes;

	private GameObject[] startPoints;
	private FloorCamera floorCamera;
    private Room startRoom;
	private Room currentRoom;
	private Room targetRoom;
	private FadePlane fadePlane;
    private FloorInstanceReward floorInstanceReward;
	private bool randomFloor;

	public Enemy floorBoss;
	private bool bossKilled = false;

	private float roomTransitionTime = 0.5f;
	public float RoomTransitionTime
	{
		get { return roomTransitionTime; }
		set { roomTransitionTime = value; }
	}

	public float gameOverClock = 0f;
	public float gameOverDelay = 3f;
	public bool	gameOver = false;
    public bool initialised;

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
		get { return floorCamera; }
	}

	public GameObject[] StartPoints
	{
		get { return startPoints; }
	}

    public FloorInstanceReward FloorInstanceReward
    {
        get { return floorInstanceReward; }
    }

    public List<Hero> Heroes
    {
        get { return heroes; }
    }

	public void InitialiseTestFloor()
    {
        // Create HUD
        GameObject hudManagerGO = GameObject.Instantiate(Resources.Load("Prefabs/UI/FloorHUD")) as GameObject;
        hudManagerGO.GetComponent<FloorHUDManager>().Initialise();

        Initialise();
    }

	public void InitialiseRandomFloor()
	{
        // Create HUD
        GameObject hudManagerGO = GameObject.Instantiate(Resources.Load("Prefabs/UI/FloorHUD")) as GameObject;
        hudManagerGO.GetComponent<FloorHUDManager>().Initialise();

		randomFloor = true;

		Initialise();
	}


	private void Initialise()
	{
        initialised = false;
		// Initialise the heroes onto the start points

		startPoints = GameObject.FindGameObjectsWithTag("StartPoint");

		if (startPoints == null)
        {
            Debug.Log("Could not find StartPoints please make sure there is an object with tag StartPoint");
        }
		else if (startPoints != null && startPoints.Length == 0)
		{
			Debug.Log("Could not find StartPoints please make sure there is an object with tag StartPoint");
		}

        // Construct Hero list from player list
        heroes = new List<Hero>();

		List<Player> players = Game.Singleton.Players;
        for (int i = 0; i < players.Count; ++i)
        {
            heroes.Add(players[i].Hero);
        }

        // Initialise the Hero in a default state
		for (int i = 0; i < heroes.Count; ++i)
		{
			Vector3 pos = startPoints[i].transform.position;
			heroes[i].transform.position = pos;
			heroes[i].transform.rotation = Quaternion.identity;
			heroes[i].transform.localScale = Vector3.one;
			heroes[i].gameObject.SetActive(true);

            heroes[i].onDeath += OnPlayerDeath;
            heroes[i].onSpawn += OnPlayerSpawn;

            // Reset individual hero floor records.
            heroes[i].FloorStatistics = new FloorStats();
		}

        // Position the camera into a default state
        // Create the floor's camera
        GameObject go = Resources.Load("Prefabs/Tower/FloorCamera") as GameObject;
        floorCamera = (Instantiate(go) as GameObject).GetComponent<FloorCamera>();
        floorCamera.name = "FloorCamera";
        floorCamera.Initialise();

		//go = Resources.Load("Prefabs/Tower/FloorDirectionalLight") as GameObject;

		Vector3 camPos = FloorCamera.CalculateAverageHeroPosition();
        camPos.z -= 5.25f;
        floorCamera.transform.position = camPos;
        FloorCamera.UpdateCameraPosition();

        // Finds all the rooms
		Room[] allRooms = GameObject.FindObjectsOfType<Room>() as Room[];

		if (!randomFloor)
		{
			currentRoom = allRooms[0];
            GameObject startRoomGO = GameObject.Find("StartRoom") as GameObject;
            if(startRoomGO == null)
            {
                Debug.LogError("StartRoom does not exist. Call the starting room StartRoom");
            }

            startRoom = startRoomGO.GetComponent<Room>();
            currentRoom = startRoom;
		}
		else
		{
			currentRoom = allRooms[0];
		}


		// Put start rooms in first room so that it is tidy
		foreach (GameObject startPoint in startPoints)
		{
			startPoint.transform.parent = currentRoom.transform;
		}

		// The floor generator will initilise the rooms. 
		// If the generator was not used then we must do it here.
        if (!randomFloor)
        {
            currentRoom.Initialise();

            foreach (Room r in allRooms)
            {
                if (currentRoom == r)
                {
                    continue;
                }
                r.Initialise();
            }
        }

		// Initialise all the enemies (Must occur after rooms are initialised)
        if (!randomFloor)
        {
            GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
            for (int i = 0; i < monsters.Length; ++i)
            {
                Enemy thisEnemy = monsters[i].GetComponent<Enemy>();

                if (thisEnemy != null)
                {
					thisEnemy.ContainedRoom = thisEnemy.transform.parent.parent.GetComponent<Room>();
                    thisEnemy.Initialise();
                    thisEnemy.InitiliseHealthbar();
                    thisEnemy.onDeath += OnEnemyDeath;
                }
            }
        }

		// Init the fade plane
		go = Instantiate(Resources.Load("Prefabs/Tower/FadePlane")) as GameObject;
		fadePlane = go.GetComponent<FadePlane>();
		go.SetActive(false);

        // Create the floor record keeper.
        floorInstanceReward = new FloorInstanceReward(this);

		// Disable all the rooms.
		foreach (Room r in allRooms)
		{
			r.gameObject.SetActive(false);
		}


		// Put player indicators on the heroes.
		for (int i = 0; i < players.Count; ++i)
		{
			heroes[i].GetComponent<PlayerIndicator>().Initialise(Player.GetPlayerColor(i)); 
		}

        initialised = true;

		// Activate the start room
		currentRoom.gameObject.SetActive(true);
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
            // Remove hero lives.
            hero.FloorStatistics.NumberOfDeaths++;
            hero.Lives--;
			gameOverClock = 0f;

            //if (hero.Lives > 0)
            //{
            //    if (currentRoom == startRoom)
            //    {
            //        hero.Respawn(startPoints[0].transform.position);
            //    }
            //    else
            //    {
            //        // Respawn the hero here.
            //        hero.Respawn(currentRoom.EntryDoor.transform.position);
            //    }
            //}
            //else
            {
                // Check if all the players are dead as well.
                if (IsAllHeroesDead() == false)
                {
                    // Otherwise make the hero innactive.
                    //hero.gameObject.SetActive(false);
                    //hero.HeroController.enabled = false;
                    hero.onDeath -= OnPlayerDeath;

                    // We want the animation death to play and the character to remain dead on the floor.
                    //((HeroAnimator)hero.Animator).PlayReactionAction(HeroAnimator.EReactionAnimation.Dying, 1.0f);
                }
            }
        }
    }

    private void OnPlayerSpawn(Character character)
    {
        Debug.Log("Spawning player");
    }

    private bool IsAllHeroesDead()
    {
        foreach (Hero hero in heroes)
        {
            if (hero.IsDead == false)
            {
                return false;
            }
        }

        return true;
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
            if (character is Hero)
            {
                Hero hero = character.LastDamagedBy as Hero;
                hero.FloorStatistics.NumberOfMonstersKilled++;
            }
        }

        // Give all heroes in the room the bounty.
        foreach (Hero hero in heroes)
        {
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

	void Update()
	{
		ProcessDebugKeys();
	
		if (!gameOver)
		{
			if (IsAllHeroesDead())
			{
				FloorHUDManager.Singleton.GameOverScreen();
				gameOver = true;
			}
		}

		// Check if all the players are dead as well.
		if (gameOver)
		{
			// Take to summary screen
			gameOverClock += Time.deltaTime;
			if (gameOverClock > gameOverDelay)
			{
				EndFloor();
			}
			// Restart the floor
		}
	}

    private void ProcessDebugKeys()
    {
        if (bossKilled == true || Input.GetKeyUp(KeyCode.F1))
        {
            EndFloor();
        }

        if (currentRoom.Doors == null)
        {
            return;
        }

        // Travel North, South, East or West

        List<Door> roomDoors = currentRoom.Doors.RoomDoors;

        // North
        if (Input.GetKeyUp(KeyCode.Keypad8))
        {
            foreach (Door d in roomDoors)
            {
                if (d == null) continue;
                if (d.targetDoor != null && d.direction == TransitionDirection.North)
                {
                    TransitionToRoomImmediately(d.targetDoor);
                }
            }
        }

        // South
        else if (Input.GetKeyUp(KeyCode.Keypad2))
        {
            foreach (Door d in roomDoors)
            {
                if (d == null) continue;
                if (d.targetDoor != null && d.direction == TransitionDirection.South)
                {
                    TransitionToRoomImmediately(d.targetDoor);
                }
            };
        }

        // West
        else if (Input.GetKeyUp(KeyCode.Keypad4))
        {
            foreach (Door d in roomDoors)
            {
                if (d == null) continue;
                if (d.targetDoor != null && d.direction == TransitionDirection.West)
                {
                    TransitionToRoomImmediately(d.targetDoor);
                }
            }
        }

        // East
        else if (Input.GetKeyUp(KeyCode.Keypad6))
        {
            foreach (Door d in roomDoors)
            {
                if (d == null) continue;
                if (d.targetDoor != null && d.direction == TransitionDirection.East)
                {
                    TransitionToRoomImmediately(d.targetDoor);
                }
            }
        }
    }

	void EndFloor()
	{
		// Disable the whole floor( audio listener from the camera )
		enabled = false;
		floorCamera.gameObject.SetActive(false);

		// Disable input on all heroes
		foreach (Hero hero in heroes)
		{
			//player.Hero.GetComponent<Hero>().HeroController.DisableInput();
            hero.gameObject.SetActive(false);
		}

		// Show summary screen
		//Instantiate(Resources.Load("Prefabs/FloorSummary"));

        floorInstanceReward.ApplyFloorInstanceRewards();

		Game.Singleton.LoadLevel(Game.EGameState.MainMenu);

		// Enable input on summary screen
	}


    public void TransitionToRoom(Door targetDoor)
	{
		// Set old remove inactive and new one active
		fadePlane.StartFade(roomTransitionTime * 0.5f, currentRoom.transform.position);

		StartCoroutine(CoTransitionToRoom());

		targetRoom = targetDoor.transform.parent.parent.parent.GetComponent<Room>();


		Room prevRoom = currentRoom;
		currentRoom = targetRoom;
		targetRoom = prevRoom;

		currentRoom.gameObject.SetActive(true);

		// Disable all the enemies
		if (prevRoom.Enemies != null)
		{
			foreach (Enemy e in prevRoom.Enemies)
			{
				e.enabled = false;
				e.HPBar.enabled = false;
			}
		}
		if (currentRoom.Enemies != null)
		{
			foreach (Enemy e in currentRoom.Enemies)
			{
				e.enabled = false;
			}
		}

		// Move heroes to the new room also disable the controller
		foreach (Hero hero in heroes)
		{
            if (targetDoor.spawnLocation != null)
            {
                hero.transform.position = targetDoor.spawnLocation.position;
            }
            else
            {
                hero.transform.position = targetDoor.transform.position;
            }

			hero.Loadout.StopAbility();
			hero.Motor.StopMotion();
			hero.HeroController.enabled = false;
		}

		// Move camera over
        FloorCamera.TransitionToRoom(roomTransitionTime);

		currentRoom.EntryDoor = targetDoor;
		targetDoor.SetAsStartDoor();
	}

	public IEnumerator CoTransitionToRoom()
	{
		yield return new WaitForSeconds(roomTransitionTime);

		targetRoom.gameObject.SetActive(false);

		// Move heroes to the new room also disable the controller
		foreach (Hero hero in heroes)
		{
			hero.HeroController.enabled = true;
		}

		yield return new WaitForSeconds(0.05f);

		// Enable all new enemies
		if (currentRoom.Enemies != null)
		{
			foreach (Enemy e in currentRoom.Enemies)
			{
				e.enabled = true;
				e.HPBar.enabled = true;
			}
		}

	}

    public void TransitionToRoomImmediately(Door targetDoor)
	{
		targetRoom = targetDoor.transform.parent.parent.parent.GetComponent<Room>();

		currentRoom.gameObject.SetActive(false);
		currentRoom = targetRoom;
		currentRoom.gameObject.SetActive(true);

		// Move heroes to the new room
		foreach (Hero hero in heroes)
		{
            if (targetDoor.spawnLocation != null)
            {
                hero.transform.position = targetDoor.spawnLocation.position;
            }
            else
            {
                hero.transform.position = targetDoor.transform.position;
            }
		}

		// Move camera over
        FloorCamera.TransitionToRoom(roomTransitionTime);

		currentRoom.EntryDoor = targetDoor;
		targetDoor.SetAsStartDoor();

		targetRoom.gameObject.SetActive(true);
	}

	#endregion
}
