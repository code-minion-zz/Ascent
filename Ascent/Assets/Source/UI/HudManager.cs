using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Hud manager.
/// </summary>
public class HudManager : MonoBehaviour {
	
	public static 	HudManager Singleton;
	public			Camera		hudCamera;
	private			Game		gameScript;
	private			int			numPlayers;
	public			PlayerHUD	Player1;
	public			PlayerHUD	Player2;
	public			PlayerHUD	Player3;
	protected		List<StatBar> enemyBars;

	public UIAnchor anchor;
	
	public void OnEnable()
	{
		if (Singleton == null)
			Singleton = this;
	}

	void Awake()
	{
		GameObject gameLoop = GameObject.Find("Game");
		if (gameLoop == null)
		{
			Debug.LogError("HudManager : 'Game' GameObject does not exist!", this);
			return;
		}
		gameScript = gameLoop.GetComponent<Game>();		
		
		enemyBars = new List<StatBar>();
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

	public StatBar AddEnemyLifeBar(Vector3 characterScale)
	{
		GameObject go = Resources.Load("Prefabs/EnemyStatBar") as GameObject;
		go = Instantiate(go) as GameObject;
		StatBar statBar = go.GetComponent<StatBar>();
		enemyBars.Add(statBar);

		go.layer = LayerMask.NameToLayer("Character");
		statBar.transform.parent = anchor.transform;
		statBar.transform.localScale = characterScale;

		return statBar;
	}

	public void RemoveEnemyLifeBar(StatBar bar)
	{
		enemyBars.Remove(bar);
		bar.Shutdown();
	}
}
