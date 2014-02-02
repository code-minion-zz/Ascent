using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UITownScreen : UIPlayerMenuScreen 
{
	private const int maxPlayers = 3;
	private List<Player> players = new List<Player>();
	//List<Player> playersToRemove = new List<Player>();

	public GameObject WindowPrefab;
	public GameObject PlayerGrid;

	private int nextEmptyPlayerSlot = 0;

	List<InputDevice> devices;

//    bool allReady = false;
//    public bool AllReady
//    {
//        get { return allReady; }
//    }

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

		players = Game.Singleton.Players;


		// Activate windows for number of players
		for (int i = 0; i < players.Count; ++i)
		{
			windows[i].gameObject.SetActive(true);
			windows[i].SetPlayer(players[i]);
			windows[i].Initialise();
		}

		// Spawn a panel for each player

//		for (int i = 0; i < players.Count; ++i)
//		{
//			GameObject go = NGUITools.AddChild(PlayerGrid, WindowPrefab);
//			UIPlayerMenuWindow uipmw =  go.GetComponent<UIPlayerMenuWindow>();
//			windows.Add(uipmw);
//			//uipmw.Initialise();
//			uipmw.SetPlayer(players[i]);
//		}
//		PlayerGrid.GetComponent<UIGrid>().repositionNow = true;
//
//		for (int i = 0; i < windows.Count; ++i)
//		{
//			windows[i].Initialise();
//		}
	}

	public void Update () 
	{
//		// Remove players that wish to leave
//		if (playersToRemove.Count > 0)
//		{
//			foreach (Player p in playersToRemove)
//			{
//				RemovePlayer(p);
//			}
//
//			playersToRemove.Clear();
//		}

        // Check if all players are ready
//        int activePlayers = 0;
//        int readiedPlayers = 0;
//        foreach (UIPlayerMenuWindow win in windows)
//        {
//            if (win.gameObject.activeSelf)
//            {
//                ++activePlayers;
//                if (win.Ready)
//                {
//                    ++readiedPlayers;
//                }
//            }
//        }
//        if (activePlayers > 0)
//        {
//            if (activePlayers == readiedPlayers)
//            {
//                allReady = true;
//            }
//            else
//            {
//                allReady = false;
//            }
//        }
//        else
//        {
//            allReady = false;
//        }

		// Check if any players want to enter the game
//		AddNewPlayers();
		//Debug.Log(devices[0].OnLSti);
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
			foreach (Player p in players)
			{
				if (p.Input == device)
				{
                    foreach (UIPlayerMenuWindow win in windows)
                    {
                        if(win.Player == p)
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

//	public void CloseWindow(UITown_Backpack_Window window)
//	{
//		//if (window.ActivePanel is UITown_Backpack_InventoryPanel)
//        window.TransitionToPanel((int)UITown_Backpack_Window.EBackpackPanels.);
//        window.ReadyWindow(false);
//		window.Player.Input.InUse = false;
//		window.Player.UnbindInputDevice();
//		playersToRemove.Add(window.Player);
//	}
}
