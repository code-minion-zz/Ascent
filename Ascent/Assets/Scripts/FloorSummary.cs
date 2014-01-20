using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FloorSummary : MonoBehaviour 
{
	//int playerCount;
	//List<Player> players;
	//List<SummaryWindow> summaryWindows;
	//Floor floor;
	FloorStats fs;
	int expReward = 0;
	int goldReward = 0;
	public GameObject[] uiElements;

	/// <summary>
	/// Script assumes that uiElements is populated in the inspector
	/// </summary>
	void Start()
	{
		//Game.Singleton.Players [0].Hero.GetComponent<Hero> ().ResetFloorStatistics ();
		fs = Game.Singleton.Players[0].Hero.GetComponent<Hero>().FloorStatistics;
		//expReward = fs.ExperienceGained;
		//goldReward = fs.TotalCoinsLooted;
		string bonusNames = uiElements[0].GetComponent<UILabel>().text;
		string rewardValues = uiElements [1].GetComponent<UILabel> ().text;
		
		if (fs.FloorCompletionTime > 0)
		{
			bonusNames += "Time Taken" + "\n";
			//rewardValues += 561 + "s" + "\n";
			rewardValues += fs.FloorCompletionTime + "\n";
		}

		if (fs.BossCompletionTime > 0) 
		{
			bonusNames += "Boss Kill Time" + "\n";
			//rewardValues += 13 + "s" + '\n';
			rewardValues += fs.BossCompletionTime + "\n";
		}
		
		if (fs.TotalDamageDealt > 0)
		{
			bonusNames += "Damage Dealt" + "\n";
			//rewardValues += 43561 + "\n";
			rewardValues += fs.TotalDamageDealt + "\n";
		}
		
		if (fs.DamageTaken > 0)
		{
			bonusNames += "Damage Taken" + "\n";
			//rewardValues += 43561 + "\n";
			rewardValues += fs.DamageTaken + "\n";
		}

		if (fs.NumberOfDeaths > 0)
		{
			bonusNames += "Lives Lost" + "\n";
			//rewardValues += 2 + "\n";
			rewardValues += fs.NumberOfDeaths + "\n";
		}

		if (fs.NumberOfMonstersKilled > 0)
		{
			bonusNames += "Monsters Killed" + "\n";
			//rewardValues += 30 + "\n";
			rewardValues += fs.NumberOfMonstersKilled + "\n";
		}

		
		uiElements [0].GetComponent<UILabel> ().text = bonusNames;
		uiElements [1].GetComponent<UILabel> ().text = rewardValues;
	} 
	//// Use this for initialization
	//void Start () 
	//{
	//    // Grab reference to players
	//    players = Game.Singleton.Players;
	//    playerCount = players.Count;

	//    Transform xform = transform.FindChild("UI Root (2D)");
	//    xform = xform.FindChild("Camera");
	//    xform = xform.FindChild("Anchor");
	//    xform = xform.FindChild("Panel");
	//    xform = xform.FindChild("Background");

	//    summaryWindows = new List<SummaryWindow>();

	//    for (int i = 0; i < playerCount; ++i)
	//    {
	//        SummaryWindow summaryWindow = new SummaryWindow();

	//        summaryWindow.Initialise(xform.FindChild("Summary Window " + (i + 1)), players[i]);

	//        summaryWindows.Add(summaryWindow);
	//    }
	//}
	
	//// Update is called once per frame
	//void Update () 
	//{
	//    //// Update for each player
	//    //for (int i = 0; i < playerCount; ++i )
	//    //{
	//    //    // Query this player's input
	//    //    InputDevice inputDevice = players[i].Input;
	//    //    if (inputDevice.Action1.WasPressed) // TODO: Also check Start
	//    //    {
	//    //        // On A press go to the next screen
	//    //        Application.LoadLevel(0);
	//    //        return;
	//    //    }


	//    //    // Update this player's window
	//    //    summaryWindows[i].Process();
	//    //}
	//}
}
