using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FloorSummary : MonoBehaviour
{
	public enum SummaryVote
	{
			INVALID = -1,
			VOTELESS,
			TOWN,
			NEXTLEVEL,
			MAX
	}

	private SummaryVote myVote = 0;
	public SummaryVote MyVote
	{
		get {
			return myVote;
		}
		set {
			myVote = value;
		}
	}

	Player myPlayer;
	FloorStats fs;
	int expReward = 0;
	int goldReward = 0;

	public GameObject[] PanelElements;

	/// <summary>
	/// Script assumes that uiElements is populated in the inspector
	/// </summary>
	void Init (Player player)
	{
		//myPlayer = Game.Singleton.Players [0]; // TODO : comment this line out - testing only!
		myPlayer = player;
		fs = myPlayer.Hero.GetComponent<Hero> ().FloorStatistics;
		expReward = fs.ExperienceGained;
		goldReward = fs.TotalCoinsLooted;
		string bonusNames = PanelElements [0].GetComponent<UILabel> ().text;
		string rewardValues = PanelElements [1].GetComponent<UILabel> ().text;

		if (fs.FloorCompletionTime > 0) {
				bonusNames += "Time Taken" + "\n";
				//rewardValues += 561 + "s" + "\n";
				rewardValues += fs.FloorCompletionTime + "s\n";
		}

		if (fs.BossCompletionTime > 0) {
				bonusNames += "Boss Kill Time" + "\n";
				//rewardValues += 13 + "s" + '\n';
				rewardValues += fs.BossCompletionTime + "s\n";
		}

		if (fs.TotalDamageDealt > 0) {
				bonusNames += "Damage Dealt" + "\n";
				//rewardValues += 43561 + "\n";
				rewardValues += fs.TotalDamageDealt + "\n";
		}

		if (fs.DamageTaken > 0) {
				bonusNames += "Damage Taken" + "\n";
				//rewardValues += 43561 + "\n";
				rewardValues += fs.DamageTaken + "\n";
		}

		if (fs.NumberOfDeaths > 0) {
				bonusNames += "Lives Lost" + "\n";
				//rewardValues += 2 + "\n";
				rewardValues += fs.NumberOfDeaths + "\n";
		}

		if (fs.NumberOfMonstersKilled > 0) {
				bonusNames += "Monsters Killed" + "\n";
				//rewardValues += 30 + "\n";
				rewardValues += fs.NumberOfMonstersKilled + "\n";
		}


		PanelElements [0].GetComponent<UILabel> ().text = bonusNames;
		PanelElements [1].GetComponent<UILabel> ().text = rewardValues;
		PanelElements [2].GetComponent<UILabel> ().text = "Gold: " + (goldReward + myPlayer.Hero.GetComponent<Hero> ().CharacterStats.Currency);

		int newExp = expReward + myPlayer.Hero.GetComponent<Hero> ().CharacterStats.CurrentExperience;
		if (newExp > 999) {
			newExp = newExp - 1000;
		}

		PanelElements [3].GetComponent<UILabel> ().text = "Exp: " + newExp;
		PanelElements [4].GetComponent<UISlider> ().value = (float)newExp/1000;
	} 

	//// Update is called once per frame
	void Update ()
	{
			#region Voting Controls
			// Query this player's input
			InputDevice inputDevice = myPlayer.Input;

			// vote for next Level
			if (inputDevice.Action1.WasPressed) {
					if (myVote != SummaryVote.NEXTLEVEL) {
							myVote = SummaryVote.NEXTLEVEL;
							PanelElements [5].GetComponent<UILabel> ().color = Color.green;
					} else {
							myVote = SummaryVote.VOTELESS;
							PanelElements [5].GetComponent<UILabel> ().color = Color.white;
					}

					// inform scene controller of player's vote
					return;
			}

			// vote for town
			if (inputDevice.Action2.WasPressed) {
					if (myVote != SummaryVote.TOWN) {
							myVote = SummaryVote.TOWN;
							PanelElements [6].GetComponent<UILabel> ().color = Color.red;
					} else {
							myVote = SummaryVote.VOTELESS;
							PanelElements [6].GetComponent<UILabel> ().color = Color.white;
					}

					// inform scene controller of player's vote
					return;
			}		
			#endregion Voting Controls
	}
}
