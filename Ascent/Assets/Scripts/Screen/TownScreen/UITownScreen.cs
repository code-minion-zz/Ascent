using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UITownScreen : UIPlayerMenuScreen 
{
	public GameObject WindowPrefab;
	//public GameObject PlayerGrid;
	public delegate void ReadyHandler();

	public const int maxPlayers = 3;
	private List<Player> players = new List<Player>();
	private List<Player> playersToRemove = new List<Player>();
	private int nextEmptyPlayerSlot = 0;
#pragma warning disable 0649
    private	int readyPlayers = 0;

	List<InputDevice> devices;
	bool allReady = false;

	void OnDestroy()
	{
		InputManager.OnDeviceAttached -= OnDeviceAttached;
		InputManager.OnDeviceDetached -= OnDeviceDetached;
	}

	public override void Start()
	{
		base.Start();


		InputManager.OnDeviceAttached += OnDeviceAttached;
		InputManager.OnDeviceDetached += OnDeviceDetached;

		players = Game.Singleton.Players;


		// Activate windows for number of players
		for (int i = 0; i < maxPlayers; ++i)
		{
			windows[i].gameObject.SetActive(true);
			//windows[i].SetPlayer(players[i]);
			windows[i].Initialise();
			windows[i].OnEnable();
		}

		devices = InputManager.Devices;
	}

	public void Update () 
	{
		// Remove players that wish to leave
		if (playersToRemove.Count > 0)
		{
			foreach (Player p in playersToRemove)
			{
				RemovePlayer(p);
			}
			
			playersToRemove.Clear();
		}
		
		// Check if all players are ready
		int activePlayers = 0;
		int readiedPlayers = 0;
		foreach (UIPlayerMenuWindow win in windows)
		{
			if (win.gameObject.activeSelf)
			{
				++activePlayers;
				if (win.Ready)
				{
					++readiedPlayers;
				}
			}
		}
		if (activePlayers > 0)
		{
			if (activePlayers == readiedPlayers)
			{
				allReady = true;
			}
			else
			{
				allReady = false;
			}
		}
		else
		{
			allReady = false;
		}

		// Check if any players want to enter the game
		if (players.Count < 3)
		{
			AddNewPlayers();
		}
	}

	public void AddNewPlayers()
	{
		// Check if any players want to enter the game
		if (players.Count < maxPlayers)
		{
			foreach (InputDevice device in devices)
			{
				if (!device.InUse)
				{
					if (device.A.WasPressed)
					//if (device.Start.WasPressed || device.A.WasPressed)
					{
						device.InUse = true;
						int i;
						for (i = 0; i < windows.Count; ++i)
						{
							if (windows[i].HasPlayer)
							{
								continue;
							}
							
							GameObject go = Instantiate(Resources.Load("Prefabs/Player")) as GameObject;
							go.transform.parent = Game.Singleton.transform;

							Player newPlayer = go.GetComponent<Player>() as Player;
							newPlayer.PlayerID = i;
							newPlayer.name = "Player" + i;
							//windows[i].
							newPlayer.BindInputDevice(device);
							windows[i].SetPlayer(newPlayer);
							windows[i].ActivateWindow();
							players.Add(newPlayer);
							break;
						}
					}
				}
			}
		}
	}

	public void OnDeviceAttached(InputDevice device)
	{
	}

	public void OnDeviceDetached(InputDevice device)
	{
		// Remove player from game if their device was in use
		if (device.InUse)
		{
			foreach (Player p in players)
			{
				if (p.Input == device)
				{
                    foreach (UIPlayerMenuWindow win in windows)
                    {
                        if(win.Player == p)
                        {
                            //win.CloseWindow();
                        }
                    }
					continue;
				}
			}
		}

	}

	public void RemovePlayer(Player p)
	{
		nextEmptyPlayerSlot = Mathf.Min(nextEmptyPlayerSlot, p.PlayerID);

		players.Remove(p);
		Destroy(p.gameObject);
	}

//	public void CloseWindow(UIPlayerMenuWindow window)
//	{
//		window.TransitionToPanel((int)UIHeroSelect_Window.EHeroSelectPanels.Main);
//		window.ReadyWindow(false);
//		window.Player.Input.InUse = false;
//		window.Player.UnbindInputDevice();
//		playersToRemove.Add(window.Player);
//	}

	public void Ready(bool state)
	{
		//Debug.Log(readyPlayers);
		readyPlayers += state ? 1 : -1;
		//Debug.Log(readyPlayers);
		ReadyTracker();
	}

	public void ReadyTracker()
	{
		Debug.Log(readyPlayers + " players ready out of " + Game.Singleton.NumberOfPlayers);
		if (readyPlayers == Game.Singleton.NumberOfPlayers)
		{
			// load next level
			Game.Singleton.LoadLevel(Game.EGameState.Tower);
		}
	}

	public void RequestQuit()
	{ // If 'back' is pressed with no active players, return to main menu
		if (Game.Singleton.NumberOfPlayers == 0)
		{ 
			Game.Singleton.LoadLevel(Game.EGameState.MainMenu);
		}
	}
}
