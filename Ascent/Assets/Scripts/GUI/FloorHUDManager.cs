using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Hud manager.
/// </summary>
public class FloorHUDManager : MonoBehaviour 
{
	private static 	FloorHUDManager singleton;

    private PlayerHUD[] playerHUDs;
    private List<StatBar> enemyBars;

    // These are to be set through the editor
	public			UIRoot root;
	public			Camera		hudCamera;
	public			TextDriver  TextDriver;
    public          PlayerHUD   playerHUD;
	public          StatBar     enemyStatBar;
	public          GameObject  enemHealthBars;
	public          Transform  	pausePanel;
	public          Transform  	transitionPanel;
	public			UILabel		pauseLabel;
	public			UILabel		transitionLabel;
	public			UITexture	transitionTexture;
	private			float		transitionTimer = 0f;
    private         float      transitionTime = 3.0f;
	private			bool		paused = true;

	public SceneFadeInFadeOut fader;

	public UIPanel mainPanel;


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

	public bool canPause
	{
		get { return transitionTimer == 0.0f; }
	}

	public void OnEnable()
	{
        if (singleton == null)
        {
            singleton = this;
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
			playerHUDs[i].playerLabel.text = "P" + (i + 1);
			playerHUDs[i].playerLabel.GetComponent<UIWidget>().color = Player.GetPlayerColor(i);
            
            // Initialise the PlayerHUD with the PlayerHero
            playerHUDs[i].Initialise(game.Players[i].Hero);
        }

        // Reposition all the PlayerHUDs using the Grid
		UIGrid playerGrid = GetComponentInChildren<UIGrid>();
		//playerGrid.cellWidth = (Screen.width / 2);
		//playerGrid.Reposition();

		if (Game.Singleton.IsWideScreen)
		{
			playerGrid.GetComponent<UIWidget>().width = 1600;
		}
		else
		{
			playerGrid.GetComponent<UIWidget>().width = 1200;
		}
		playerGrid.transform.localPosition = new Vector3(playerGrid.GetComponent<UIWidget>().width * -0.5f, 450.0f, playerGrid.transform.localPosition.z);


		float playerHUDWidth = 350.0f;

		if(playerHUDs.Length == 1)
		{
			// Just 1 player. Place it top left.
			playerHUDs[0].transform.localPosition = new Vector3(0.0f, 0.0f, playerGrid.transform.localPosition.z);
		}
		else if(playerHUDs.Length == 2)
		{
			// Place P1 on top left. Place P2 on top right.
			playerHUDs[0].transform.localPosition = new Vector3(0.0f, 0.0f, playerGrid.transform.localPosition.z);

			playerHUDs[1].GetComponent<UIWidget>().pivot = UIWidget.Pivot.Right;
			playerHUDs[1].GetComponent<UIWidget>().rightAnchor.SetHorizontal(playerHUDs[1].GetComponent<UIWidget>().rightAnchor.target, -playerHUDWidth);// = -350.0f;
			playerHUDs[1].transform.localRotation = playerHUDs[0].transform.localRotation;
		}
		else if(playerHUDs.Length == 3)
		{
			// Place P1 on top left. P2  topcentre and P3 top Right.
			playerHUDs[0].transform.localPosition = new Vector3(0.0f, 0.0f, playerGrid.transform.localPosition.z);

			playerHUDs[1].GetComponent<UIWidget>().pivot = UIWidget.Pivot.Center;
			playerHUDs[1].GetComponent<UIWidget>().rightAnchor.SetHorizontal(playerHUDs[1].GetComponent<UIWidget>().rightAnchor.target, -playerHUDWidth);// = -350.0f;
			playerHUDs[1].transform.localRotation = playerHUDs[0].transform.localRotation;

			playerHUDs[2].GetComponent<UIWidget>().pivot = UIWidget.Pivot.Right;
			playerHUDs[2].GetComponent<UIWidget>().rightAnchor.SetHorizontal(playerHUDs[2].GetComponent<UIWidget>().rightAnchor.target, -playerHUDWidth);// = -350.0f;
			playerHUDs[2].transform.localRotation = playerHUDs[0].transform.localRotation;
		}

        // Create a list of health bars for the enemies.
        enemyBars = new List<StatBar>();

		foreach (Player p in game.Players)
		{
			if (p != null)
			{
				p.Hero.HeroController.InitialiseControllerIndicators();
			}
		}
	
		
		ShowPauseScreen(false);

		LevelStartScreen();

		fader.gameObject.SetActive(true);
		fader.onTransitionEnd += OnFadeInEnd;
		fader.onReverseTransitionEnd += OnFadeOutEnd;
		fader.transitionTime = Game.Singleton.fadeIntoLevelTime;
		fader.Transition();
    }

	void Update()
	{
		if (fader.Fading)
			return;

		if (transitionTimer < 0) // level start transition
		{
			transitionTimer += Time.deltaTime;

			if (transitionTimer > 0)
			{
				transitionTimer = 0;
			}

		}
		
		if (transitionTimer == 0)
		{
			if (transitionPanel.gameObject.activeInHierarchy)
			{
				ToggleTransition(false);
			}
		}
		else
		{
			transitionPanel.GetComponent<UIPanel>().alpha = (Mathf.Abs(transitionTimer)/transitionTime);
			
		}
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

	public void PauseGame()
	{
		Time.timeScale = paused ? 0f : 1f;
		
		Game.Singleton.Players.ForEach(p => p.Hero.HeroController.ToggleInput(paused));
	}

	public void ShowPauseScreen(bool pause)
	{
		paused = pause;
		NGUITools.SetActive(pausePanel.gameObject, paused);

		PauseGame();
	}

	public void LevelStartScreen()
	{
		if(Game.Singleton.showLevelStartMessageTimer == 0.0f)
			return;

		
		SetTransitionText((Game.Singleton.Tower.currentFloorNumber >= Game.Singleton.maxFloor ? "Final Floor" : "Floor " + Game.Singleton.Tower.currentFloorNumber));
		ToggleTransition(true);
		transitionPanel.GetComponent<UIPanel>().alpha = 1;
        transitionTimer = -Game.Singleton.showLevelStartMessageTimer;
		transitionTime = Mathf.Abs (transitionTimer);
	}
	
	public void LevelCompleteScreen()
	{
		ToggleTransition(true);
		SetTransitionText((Game.Singleton.Tower.currentFloorNumber >= Game.Singleton.maxFloor ? "Congratulations Final Floor" : "Floor " + Game.Singleton.Tower.currentFloorNumber) + " Completed");
		transitionPanel.GetComponent<UIPanel>().alpha = 1;
		transitionTimer = Game.Singleton.showLevelCompleteMessageTimer;
		transitionTime = transitionTimer;
		InputManager.DisableInputForTime(transitionTime + fader.waitTimeOut + fader.transitionTime + 0.5f);

		fader.waitTimeOut = transitionTimer;
		fader.transitionTime = Game.Singleton.fadeOutOfLevelTime;
		fader.ReverseTransition();
	}
	
	public void GameOverScreen()
	{
		ToggleTransition(true);
		SetTransitionText("GAME OVER");
		transitionPanel.GetComponent<UIPanel>().alpha = 1;
		transitionTimer = Game.Singleton.showGameOvermessageTimer;
		transitionTime = transitionTimer;
		InputManager.DisableInputForTime(transitionTime + fader.waitTimeOut + fader.transitionTime + 0.5f);

		fader.waitTimeOut = transitionTimer;
		fader.transitionTime = Game.Singleton.fadeOutFromGameOverTime;
		fader.ReverseTransition();
	}

	public void ToggleTransition(bool active)
	{
		NGUITools.SetActive(transitionPanel.gameObject, active);
	}

	public void SetTransitionText(string text)
	{
		transitionLabel.text = text;
	}

	public void OnFadeInEnd()
	{
		LevelStartScreen();
	}

	public void OnFadeOutEnd()
	{
		ToggleTransition(true);
	}
}
