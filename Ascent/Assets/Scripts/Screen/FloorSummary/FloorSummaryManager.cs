using UnityEngine;
using System.Collections;

public class FloorSummaryManager : MonoBehaviour {
	
	public enum SummaryVote
	{
		INVALID = -1,
		VOTELESS,
		TOWN,
		NEXTLEVEL,
		MAX
	}

	private	uint townVotes;
	private	uint levelVotes;

	public	GameObject SummaryParent;
	public	GameObject PanelPrefab; 

	private	float counter;
	private bool voteTimer;

	public delegate void VoteHandler (SummaryVote from, SummaryVote to);

	// Use this for initialization
	void Start ()
	{
		// blah blah spawn panels
		int numPlayers = Game.Singleton.NumberOfPlayers;
		int i;
		for (i = 0; i < numPlayers; ++i)
		{
			GameObject myPanel = NGUITools.AddChild(SummaryParent, PanelPrefab);
			FloorSummaryPanel fsp = myPanel.GetComponent<FloorSummaryPanel>();
			fsp.Init(Game.Singleton.Players[i]);
			fsp.VoteChanged += TrackVote;

            AscentGameSaver.SaveHero(Game.Singleton.Players[i].Hero, false);
		}

		Destroy( Game.Singleton.Tower.CurrentFloor);
        
        AscentGameSaver.SaveGame();
	}
	
	// Update is called once per frame
	void Update () {
		if (voteTimer)
		{
			counter -= Time.deltaTime;

			if (counter < 0f)
			{
				if (townVotes > levelVotes)
				{
					Game.Singleton.LoadLevel(Game.EGameState.City);
				}
				else
				{
					Game.Singleton.LoadLevel(Game.EGameState.Tower);
				}
			}
		}
	}

	private void TrackVote(SummaryVote from, SummaryVote to)
    {
		switch (from)
		{
		case SummaryVote.NEXTLEVEL:
			--levelVotes;
			break;
		case SummaryVote.TOWN:
			--townVotes;
			break;
		}
		switch (to)
		{
		case SummaryVote.NEXTLEVEL:
			++levelVotes;
			break;
		case SummaryVote.TOWN:
			++townVotes;
			break;
		}

		if (townVotes == levelVotes)
		{
			// if vote deadlocked, do nothing
			voteTimer = false;
			return;
		}
		else if (townVotes == Game.Singleton.NumberOfPlayers)
		{
			// skip timer and just transition
			Game.Singleton.LoadLevel(Game.EGameState.City);
		}
		else if (levelVotes == Game.Singleton.NumberOfPlayers)
		{
			// skip timer and just transition
			Game.Singleton.LoadLevel(Game.EGameState.Tower);
		}
		else // in all other cases
		{
			// Start countdown timer
			if (!voteTimer)
			{
				voteTimer = true;
				counter = 10f;
			}
			else 
			{
				// already started? this new vote will deduct from the timer
				counter -= 5f;
			}
		}

	}
}
