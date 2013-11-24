using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterSelect : MonoBehaviour 
{
    private const int numberPlayers = 3;

    private Transform background;
    private Transform[] playerPanels = new Transform[numberPlayers];


	void Start () 
    {
        //// Grab ref to all the player panels
        //Transform xform = transform.FindChild("UI Root (2D)");
        //xform = xform.FindChild("Camera");
        //xform = xform.FindChild("Anchor");
        //xform = xform.FindChild("Panel");
        //xform = xform.FindChild("Background");

        //summaryWindows = new List<SummaryWindow>();

        //for (int i = 0; i < playerCount; ++i)
        //{
        //    SummaryWindow summaryWindow = new SummaryWindow();

        //    summaryWindow.Initialise(xform.FindChild("Summary Window " + (i + 1)), players[i]);

        //    summaryWindows.Add(summaryWindow);
        //}

        Transform xform = transform.FindChild("UI Root (2D)");
        xform = xform.FindChild("Camera");
        xform = xform.FindChild("Anchor");
        xform = xform.FindChild("Panel");
        background = xform.FindChild("Background");

        for (int i = 0; i < numberPlayers; ++i)
        {
            playerPanels[i] = background.FindChild("Player " + (i + 1) + " Panel");
            
			playerPanels[i].gameObject.SetActive(false);
        }
	}
	
	void Update () 
    {
		//// We want to keep grabbing this list incase it changes
		//List<InControl.InputDevice> devices = Game.Singleton.InputHandler.GetAllInputDevices();

		//// Check if other player's want to play
		//int deviceCount = devices.Count;

		//for (int i = 0; i < deviceCount; ++i) // Start as 1 because player 1 will be on 0
		//{
		//    InControl.InputDevice input = devices[i];

		//    if (input.Start.IsPressed)
		//    {
		//        Debug.Log("Start / Enter");
		//        // Bind it to the next player
		//        playerPanels[i].gameObject.SetActive(true);
		//    }
		//}
	}

	public void OnDeviceAttached(InControl.InputDevice device)
	{
		// Request player/s press start/enter on the device
	}

	public void OnDeviceDetached(InControl.InputDevice device)
	{
		// If on floor or in menus - pause game and request player replug in the controller. 
		// Has a timer that will expire so other players may continue.
		// Unbind it
	}
}
