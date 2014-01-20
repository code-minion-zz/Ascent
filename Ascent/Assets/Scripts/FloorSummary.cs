using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FloorSummary : MonoBehaviour 
{
	//int playerCount;
	//List<Player> players;
	//List<SummaryWindow> summaryWindows;
	//Floor floor;
	Player myPlayer;
	FloorStats fs;
	int expReward = 0;
	int goldReward = 0;
	public GameObject[] uiElements;

	/// <summary>
	/// Script assumes that uiElements is populated in the inspector
	/// </summary>
	void Start()
	{
		myPlayer = Game.Singleton.Players [0]; // TODO : comment this line out - testing only!
		fs = myPlayer.Hero.GetComponent<Hero>().FloorStatistics;
		expReward = fs.ExperienceGained;
		goldReward = fs.TotalCoinsLooted;
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

		uiElements [2].GetComponent<UILabel> ().text = "Gold: " + (goldReward + myPlayer.Hero.GetComponent<Hero> ().CharacterStats.Currency);
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
	void Update () 
	{
        // Query this player's input
        InputDevice inputDevice = myPlayer.Input;
		
		// vote for next Level
		if (inputDevice.Action1.WasPressed)
		{
			uiElements[6].GetComponent<UILabel>().color = Color.green;

			// inform scene controller of player's vote
			return;
		}

		// vote for town
		if (inputDevice.Action2.WasPressed) 
		{
			uiElements[6].GetComponent<UILabel>().color = Color.red;

			// inform scene controller of player's vote
			return;
		}		
	}
}
