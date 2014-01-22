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

	private uint townVotes;
	private uint levelVotes;

	public GameObject SummaryParent;

	public delegate void VoteChanged (SummaryVote from, SummaryVote to);

	// Use this for initialization
	void Awake () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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

		// if vote deadlocked, load no level
		if (townVotes == levelVotes)
		{
			return;
		}
		if (townVotes == 1)
		{
			if (Game.Singleton.NumberOfPlayers < 2)
			{

			}
		}
	}
}
