using UnityEngine;
using System.Collections;

public class HudManager : MonoBehaviour {
	
	GameObject 	hudCamera;
	Game		gameScript;
	int			numPlayers;
	public StatBar		PlayerHP1;
	
	void Awake()
	{
		GameObject gameLoop = GameObject.Find("Game");
		if (gameLoop == null)
		{
			Debug.LogError("HudManager : 'Game' GameObject does not exist!", this);
			return;
		}
		gameScript = gameLoop.GetComponent<Game>();	
		
		
	}
	
	// Use this for initialization
	void Start () 
	{
		int numPlayers = gameScript.NumberOfPlayers;
		
		switch (numPlayers)
		{
			case 1 :
			{
				
			}
			break;
			case 2 :
			{
				
			}
			break;
			case 3 :
			{
				
			}
			break;
			default :
				Debug.LogError("HudManager : Unexpected number of players", this);
			break;
		}
		PlayerHP1.Init(StatBar.eStat.HP, gameScript.Players[0].Hero.GetComponent<Character>().CharacterStats);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
