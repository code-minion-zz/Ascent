using UnityEngine;
using System.Collections;

/// <summary>
/// Hud manager.
/// </summary>
public class HudManager : MonoBehaviour {
	
	public	GameObject 	hudCamera;
	private	Game		gameScript;
	private	int			numPlayers;
	public	PlayerHUD	Player1;
	public	PlayerHUD	Player2;
	public	PlayerHUD	Player3;
	
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
			Player1.Init(gameScript.Players[0].Hero.GetComponent<Character>());

			if (numPlayers > 1)
			{
				Player2.Init(gameScript.Players[1].Hero.GetComponent<Character>());

				if (numPlayers > 2)
				{				
					Player3.Init(gameScript.Players[2].Hero.GetComponent<Character>());
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
