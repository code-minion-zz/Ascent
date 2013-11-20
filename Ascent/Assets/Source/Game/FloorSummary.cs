using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FloorSummary : MonoBehaviour 
{
    int playerCount;
    List<Player> players;
    List<SummaryWindow> summaryWindows;
    Floor floor;

 
	// Use this for initialization
	void Start () 
    {
        // Grab reference to players
        players = Game.Singleton.Players;
        playerCount = players.Count;

        Transform xform = transform.FindChild("UI Root (2D)");
        xform = xform.FindChild("Camera");
        xform = xform.FindChild("Anchor");
        xform = xform.FindChild("Panel");
        xform = xform.FindChild("Background");

        summaryWindows = new List<SummaryWindow>();

        for (int i = 0; i < playerCount; ++i)
        {
            SummaryWindow summaryWindow = new SummaryWindow();

            summaryWindow.Initialise(xform.FindChild("Summary Window " + (i + 1)), players[i]);

            summaryWindows.Add(summaryWindow);
        }
    }
	
	// Update is called once per frame
	void Update () 
    {
        // Update for each player
        for (int i = 0; i < playerCount; ++i )
        {
            // Query this player's input
            InControl.InputDevice inputDevice = players[i].Input;
            Debug.Log("A");
            if (inputDevice.Action1) // TODO: Also check Start
            {
                // On A press go to the next screen
                Debug.Log("A");
                return;
            }


            // Update this player's window
            summaryWindows[i].Process();
        }
	}
}
