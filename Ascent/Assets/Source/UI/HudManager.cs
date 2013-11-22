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
        //GameObject gameLoop = ;
        //if (gameLoop == null)
        //{
        //    Debug.LogError("HudManager : 'Game' GameObject does not exist!", this);
        //    return;
        //}
		//gameScript = Game.Singleton;		
		
	}
	
	// Use this for initialization
	void Start () 
	{
        gameScript = Game.Singleton;		
		int numPlayers = gameScript.NumberOfPlayers;

		if (numPlayers > 0)
		{		
			Player1.Init(gameScript.Players[0].Hero.GetComponent<Character>());

			if (numPlayers > 1)
			{
				Player2.gameObject.SetActive(true);
				Player2.Init(gameScript.Players[1].Hero.GetComponent<Character>());

				if (numPlayers > 2)
				{				
					Player3.Init(gameScript.Players[2].Hero.GetComponent<Character>());
					Player3.gameObject.SetActive(true);
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
