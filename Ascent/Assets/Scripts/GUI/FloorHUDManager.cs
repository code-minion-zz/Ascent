using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Hud manager.
/// </summary>
public class FloorHUDManager : MonoBehaviour 
{
	public static 	FloorHUDManager singleton;

    private PlayerHUD[] playerHUDs;
    private List<StatBar> enemyBars;

    // These are to be set through the editor
    public bool testHUD;
	public			Camera		hudCamera;
	public			TextDriver  TextDriver;
    public          PlayerHUD   playerHUD;
    public          StatBar     enemyStatBar;
    public          GameObject  enemHealthBars;


    public static FloorHUDManager Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = GameObject.FindObjectOfType<FloorHUDManager>(); 
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

        if (testHUD)
        {
            gameObject.SetActive(false);
        }
	}

    public void OnDestroy()
    {
        singleton = null;
    }

    public void Initialise()
    {
        // Grab the game script
        if (Game.Singleton.gameObject == null)
        {
            Debug.LogError("HudManager : 'Game' GameObject does not exist!", this);
            return;
        }
        Game game = Game.Singleton;

        // Create HUD container for number of players
        int iPlayers = game.Players.Count;
        playerHUDs = new PlayerHUD[iPlayers];

        // If Player1 is missing, the default PlayerHUD needs to be removed.
        if (iPlayers == 0)
        {
            Debug.LogError("No players, no point instantiating a PlayerHUD.");
            Destroy(playerHUD.gameObject);
        }

        // Create HUDs foreach player.
        for (int i = 0; i < iPlayers; ++i)
        {
            // There is a Player in this slot so create a HUD for him
            // Player1 doesn't need to have one created because there is one by default
            if (i == 0)
            {
                // Set the first element to the default PlayerHUD
                playerHUDs[i] = playerHUD;
            }
            else
            {
                // Get Grid
                UIGrid grid = playerHUD.transform.parent.GetComponent<UIGrid>();

               // Instantiate as child of the Grid
               PlayerHUD newPlayerHUD = NGUITools.AddChild(grid.gameObject, playerHUD.gameObject).GetComponent<PlayerHUD>();
               playerHUDs[i] = newPlayerHUD;
            }

            // Set name so that it is findable in the editor
            playerHUDs[i].name = "PlayerHUD " + i;
            
            // Initialise the PlayerHUD with the PlayerHero
            playerHUDs[i].Initialise(game.Players[i].Hero);
        }

        // Reposition all the PlayerHUDs using the Grid
		GetComponentInChildren<UIGrid>().Reposition();

        // Create a list of health bars for the enemies.
        enemyBars = new List<StatBar>();
    }
	
	public StatBar AddEnemyLifeBar(Vector3 characterScale)
	{
        // Instantiate the new Health bars as child of the anchor
        GameObject newStatBarGO = NGUITools.AddChild(enemHealthBars, enemyStatBar.gameObject);
        StatBar newStatBar = newStatBarGO.GetComponent<StatBar>();
        enemyBars.Add(newStatBar);
		
        // Set scale similar to the character size
        newStatBar.transform.localScale = characterScale;

        return newStatBar;
	}
	
	public void RemoveEnemyLifeBar(StatBar bar)
	{
        bar.Shutdown();
        enemyBars.Remove(bar);
	}
}
