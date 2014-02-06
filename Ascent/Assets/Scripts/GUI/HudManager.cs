using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Hud manager.
/// </summary>
public class HudManager : MonoBehaviour 
{
	
	public static 	HudManager singleton;
	public			Camera		hudCamera;
	public			TextDriver  TextDriver;
	private			Game		gameScript;
	private			int			numPlayers;
	public			PlayerHUD[]	playerHUDs;
	protected		List<StatBar> enemyBars;
	
	public UIAnchor anchor;

    public static HudManager Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = GameObject.FindObjectOfType<HudManager>(); 
            }

            return singleton;
        }

    }

	public void OnEnable()
	{
        if (singleton == null)
        {
            singleton = this;
        }
	}

    public void Initialise()
    {
        GameObject gameLoop = Game.Singleton.gameObject;
        if (gameLoop == null)
        {
            Debug.LogError("HudManager : 'Game' GameObject does not exist!", this);
            return;
        }
        gameScript = gameLoop.GetComponent<Game>();

        enemyBars = new List<StatBar>();

        int numPlayers = gameScript.NumberOfPlayers;

		//if (numPlayers > 0)
		//{
		//    Player1.gameObject.SetActive(true);
		//    Player1.Init(gameScript.Players[0].Hero.GetComponent<Hero>());
		//    Player1.transform.position = new Vector3(Screen.width * 0.2f, Player1.transform.position.y, Player1.transform.position.z);

		//    if (numPlayers > 1)
		//    {
		//        Player2.gameObject.SetActive(true);
		//        Player2.Init(gameScript.Players[1].Hero.GetComponent<Hero>());
		//        Player2.transform.position = new Vector3(Screen.width * 0.3f, Player2.transform.position.y, Player2.transform.position.z);

		//        if (numPlayers > 2)
		//        {
		//            Player3.gameObject.SetActive(true);
		//            Player3.Init(gameScript.Players[2].Hero.GetComponent<Hero>());
		//            Player3.transform.position = new Vector3(Screen.width * 0.8f, Player3.transform.position.y, Player3.transform.position.z);
		//        }
		//    }
		//}
		for (int i = 0; i < playerHUDs.Length; ++i)
		{
			if(i >= gameScript.Players.Count)
			{
				Destroy(playerHUDs[i].gameObject);
				continue;
			}

			playerHUDs[i].gameObject.SetActive(true);
			playerHUDs[i].Init(gameScript.Players[i].Hero);
		}
		GetComponentInChildren<UIGrid>().Reposition();
    }
	
	
	public StatBar AddEnemyLifeBar(Vector3 characterScale)
	{
		GameObject go = Resources.Load("Prefabs/UI/EnemyStatBar") as GameObject;
		go = NGUITools.AddChild (anchor.gameObject, go);
		//go = Instantiate(go) as GameObject;
		StatBar statBar = go.GetComponent<StatBar>();
		enemyBars.Add(statBar);
		
		//go.layer = LayerMask.NameToLayer("Character");
		//statBar.transform.parent = anchor.transform;
		statBar.transform.localScale = characterScale;
		
		return statBar;
	}
	
	public void RemoveEnemyLifeBar(StatBar bar)
	{
        bar.Shutdown();
        enemyBars.Remove(bar);
	}
}
