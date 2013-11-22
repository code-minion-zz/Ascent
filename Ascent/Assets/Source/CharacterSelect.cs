using UnityEngine;
using System.Collections;

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

            if (i > 0)
            {
                playerPanels[i].gameObject.SetActive(false);
            }
        }
	}
	
	void Update () 
    {
        // Check if other player's want to play
        int deviceCount = Game.Singleton.InputHandler.NumberOfDevices;

        for (int i = 1; i < deviceCount; ++i) // Start as 1 because player 1 will be on 0
        {
            InControl.InputDevice input = Game.Singleton.InputHandler.GetDevice(i);

            if (input.Start.IsPressed)
            {
                playerPanels[i].gameObject.SetActive(true);
            }
        }
	}
}
