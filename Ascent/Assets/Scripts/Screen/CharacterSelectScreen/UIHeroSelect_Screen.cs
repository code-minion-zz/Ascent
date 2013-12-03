using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIHeroSelect_Screen : UIPlayerMenuScreen 
{
	private enum EPlayerState
	{
		Main_NoHero,
		Main_HasHero,
		Main_Ready,
		LoadHero,
		NewHero,
	}

	private const int maxPlayers = 3;
	private Dictionary<Player, EPlayerState> players = new Dictionary<Player, EPlayerState>();
	List<Player> playersToRemove = new List<Player>();

	private int nextEmptyPlayerSlot = 0;

	List<InputDevice> devices;

    bool allReady = false;
    public bool AllReady
    {
        get { return allReady; }
    }

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
		AddNewPlayers();

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
					if (device.Start.WasPressed || device.A.WasPressed)
					{
						device.InUse = true;

						GameObject go = Instantiate(Resources.Load("Prefabs/Player")) as GameObject;
						go.transform.parent = Game.Singleton.transform;
						Player newPlayer = go.GetComponent<Player>() as Player;

						players[newPlayer] = EPlayerState.Main_HasHero;

						newPlayer.PlayerID = nextEmptyPlayerSlot;
						newPlayer.name = newPlayer.name + nextEmptyPlayerSlot;

						windows[nextEmptyPlayerSlot].gameObject.SetActive(true);

						newPlayer.BindInputDevice(device);

						windows[nextEmptyPlayerSlot].SetPlayer(newPlayer);

						++nextEmptyPlayerSlot;
					}
				}
			}
		}
	}

	public void OnDeviceAttached(InputDevice device)
	{
		// Repoll all the devices
		devices = InputManager.Devices;
	}

	public void OnDeviceDetached(InputDevice device)
	{
		// Remove player from game if their device was in use
		if (device.InUse)
		{
			foreach (KeyValuePair<Player, EPlayerState> p in players)
			{
				if (p.Key.Input == device)
				{
                    foreach (UIPlayerMenuWindow win in windows)
                    {
                        if(win.Player == p.Key)
                        {
                            win.CloseWindow();
                        }
                    }
					continue;
				}
			}
		}

		// Repoll all the devices
		devices = InputManager.Devices;
	}

	public void RemovePlayer(Player p)
	{
		nextEmptyPlayerSlot = Mathf.Min(nextEmptyPlayerSlot, p.PlayerID);

		players.Remove(p);
		Destroy(p.gameObject);
	}

	public void CloseWindow(UIHeroSelect_Window window)
	{
        window.TransitionToPanel((int)UIHeroSelect_Window.EHeroSelectPanels.Main);
        window.ReadyWindow(false);
		window.Player.Input.InUse = false;
		window.Player.UnbindInputDevice();
		playersToRemove.Add(window.Player);
	}

    public void StartGame()
    {
        Game.Singleton.LoadLevel("Level");
    }
}
