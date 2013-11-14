using UnityEngine;
using System.Collections;

public class HudManager : MonoBehaviour {
	
	public	GameObject 	hudCamera;
	private	Game		gameScript;
	private	int			numPlayers;
	public	StatBar		PlayerHP1;
	public	StatBar		PlayerHP2;
	public	StatBar		PlayerHP3;
	
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

		if (numPlayers > 0)
		{		
			PlayerHP1.Init(StatBar.eStat.HP, gameScript.Players[0].Hero.GetComponent<Character>().CharacterStats);
			if (numPlayers > 1)
			{

			}
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
