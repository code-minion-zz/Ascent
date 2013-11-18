using UnityEngine;
using System.Collections;

public class HudManager : MonoBehaviour {
	
	GameObject 	hudCamera;
	Game		gameScript;
	int			numPlayers;
	//public StatBar		PlayerHP1;

    public StatBar[] playerStatBars;
	
	void Awake()
	{
        //GameObject gameLoop = GameObject.Find("Game");
        //if (gameLoop == null)
        //{
        //    Debug.LogError("HudManager : 'Game' GameObject does not exist!", this);
        //    return;
        //}
        //gameScript = gameLoop.GetComponent<Game>();

        //foreach (StatBar statBar in playerStatBars)
        //{
        //    statBar.gameObject.SetActive(false);
        //}
	}
	
	// Use this for initialization
	void Start () 
	{
        //int numPlayers = gameScript.NumberOfPlayers;
        gameScript = Game.Singleton;

        foreach (StatBar statBar in playerStatBars)
        {
            statBar.gameObject.SetActive(false);
        }

        //switch (numPlayers)
        //{
        //    case 1 :
        //    {
        //        playerStatBars[0].Init(StatBar.eStat.HP, gameScript.Players[0].Hero.GetComponent<Character>().CharacterStats);
        //    }
        //    break;
        //    case 2 :
        //    {
        //        playerStatBars[1].Init(StatBar.eStat.HP, gameScript.Players[1].Hero.GetComponent<Character>().CharacterStats);
        //    }
        //    break;
        //    case 3 :
        //    {
				
        //    }
        //    break;
        //    default :
        //        Debug.LogError("HudManager : Unexpected number of players", this);
        //    break;
        //}
        //PlayerHP1.Init(StatBar.eStat.HP, gameScript.Players[0].Hero.GetComponent<Character>().CharacterStats);

        numPlayers = gameScript.NumberOfPlayers;

        for (int i = 0; i < numPlayers; ++i)
        {
            Character stats = gameScript.Players[i].Hero.GetComponent<Character>();

            if (stats != null)
            {
                playerStatBars[i].gameObject.SetActive(true);
                playerStatBars[i].Init(StatBar.eStat.HP, stats.CharacterStats);
            }
        }
	}

    // Update is called once per frame
    void Update() 
	{
	
	}
}
