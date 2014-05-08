using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuButtonFunctions : MonoBehaviour 
{
    public UIButton[] Buttons;
	public UISprite[] ButtonMarkers;

	public SceneFadeInFadeOut fader;
	public SceneFadeInFadeOut creditFader;

	public UIWidget credits;

	public UICamera inputCamera;

	private float deselectTimer;

	private bool showingCredits;

	private Game.EGameState modeToLoad;

	private const float showCreditsMinimumTime = 1.25f;
	private float timeElapsed;

	public void Start()
	{
		inputCamera.useController = false;
		inputCamera.useTouch = false;
		inputCamera.useKeyboard = false;
		inputCamera.useMouse = false;

		fader.gameObject.SetActive(true);

		fader.onTransitionEnd += OnTransitionEnterEnd;
		fader.onReverseTransitionEnd += OnTransitionExitEnd;

		creditFader.onTransitionEnd += OnEnteredCredits;
		creditFader.onReverseTransitionEnd += OnReturnFromCredits;
		
		MusicManager musicMan = GameObject.Find("MusicManager").GetComponent<MusicManager>();
		musicMan.PlayMusic(MusicManager.MusicSelections.Menu);

		fader.Transition();
	}

    public void Update()
	{
		if (showingCredits)
		{
			timeElapsed += Time.deltaTime;

			if (timeElapsed < showCreditsMinimumTime)
				return;

			if (Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.Backspace))
			{
				showingCredits = false;
				creditFader.ReverseTransition();
				credits.gameObject.SetActive(false);
			}
			if (Input.GetButtonUp("P1 B") || Input.GetButtonUp("P1 A"))
			{
				showingCredits = false;
				creditFader.ReverseTransition();
				credits.gameObject.SetActive(false);
			}
			if (Input.GetMouseButtonUp(0))
			{
				showingCredits = false;
				creditFader.ReverseTransition();
				credits.gameObject.SetActive(false);
			}

			return;
		}

        int playerCount = 0;
        for (int i = 0; i < InputManager.Devices.Count; ++i)
        {
            if (i > 2)
            {
                break;
            }

            Buttons[i].enabled = true;
            ++playerCount;
        }

		if (playerCount == 3)
		{
			Buttons[0].GetComponent<UIButtonKeys>().selectOnUp = Buttons[4].GetComponent<UIButtonKeys>();
			Buttons[0].GetComponent<UIButtonKeys>().selectOnDown = Buttons[1].GetComponent<UIButtonKeys>();

			Buttons[1].GetComponent<UIButtonKeys>().selectOnUp = Buttons[0].GetComponent<UIButtonKeys>();
			Buttons[1].GetComponent<UIButtonKeys>().selectOnDown = Buttons[2].GetComponent<UIButtonKeys>();

			Buttons[2].GetComponent<UIButtonKeys>().selectOnUp = Buttons[1].GetComponent<UIButtonKeys>();
			Buttons[2].GetComponent<UIButtonKeys>().selectOnDown = Buttons[3].GetComponent<UIButtonKeys>();

			Buttons[3].GetComponent<UIButtonKeys>().selectOnUp = Buttons[2].GetComponent<UIButtonKeys>();
			Buttons[3].GetComponent<UIButtonKeys>().selectOnDown = Buttons[4].GetComponent<UIButtonKeys>();

			Buttons[4].GetComponent<UIButtonKeys>().selectOnUp = Buttons[3].GetComponent<UIButtonKeys>();
			Buttons[4].GetComponent<UIButtonKeys>().selectOnDown = Buttons[0].GetComponent<UIButtonKeys>();
		}
		else if (playerCount == 2)
		{
			Buttons[0].GetComponent<UIButtonKeys>().selectOnUp = Buttons[4].GetComponent<UIButtonKeys>();
			Buttons[0].GetComponent<UIButtonKeys>().selectOnDown = Buttons[1].GetComponent<UIButtonKeys>();

			Buttons[1].GetComponent<UIButtonKeys>().selectOnUp = Buttons[0].GetComponent<UIButtonKeys>();
			Buttons[1].GetComponent<UIButtonKeys>().selectOnDown = Buttons[3].GetComponent<UIButtonKeys>();

			Buttons[3].GetComponent<UIButtonKeys>().selectOnUp = Buttons[1].GetComponent<UIButtonKeys>();
			Buttons[3].GetComponent<UIButtonKeys>().selectOnDown = Buttons[4].GetComponent<UIButtonKeys>();

			Buttons[4].GetComponent<UIButtonKeys>().selectOnUp = Buttons[3].GetComponent<UIButtonKeys>();
			Buttons[4].GetComponent<UIButtonKeys>().selectOnDown = Buttons[0].GetComponent<UIButtonKeys>();

			Buttons[2].isEnabled = false;
		}
		else if (playerCount <= 1)
		{
			Buttons[0].GetComponent<UIButtonKeys>().selectOnUp = Buttons[4].GetComponent<UIButtonKeys>();
			Buttons[0].GetComponent<UIButtonKeys>().selectOnDown = Buttons[3].GetComponent<UIButtonKeys>();

			Buttons[3].GetComponent<UIButtonKeys>().selectOnUp = Buttons[0].GetComponent<UIButtonKeys>();
			Buttons[3].GetComponent<UIButtonKeys>().selectOnDown = Buttons[4].GetComponent<UIButtonKeys>();

			Buttons[4].GetComponent<UIButtonKeys>().selectOnUp = Buttons[3].GetComponent<UIButtonKeys>();
			Buttons[4].GetComponent<UIButtonKeys>().selectOnDown = Buttons[0].GetComponent<UIButtonKeys>();

			Buttons[1].isEnabled = false;
			Buttons[2].isEnabled = false;
		}


		if ((UICamera.selectedObject != null && UICamera.hoveredObject != null) && 
			(UICamera.selectedObject != UICamera.hoveredObject))
		{
			UICamera.selectedObject = UICamera.hoveredObject;
		}
		else if ((UICamera.selectedObject == null && UICamera.hoveredObject != null))
		{
			UICamera.selectedObject = UICamera.hoveredObject;
		}
		else if ((UICamera.selectedObject == null && UICamera.hoveredObject == null))
		{
			// There is a small time frame between deselecting and selecting something new
			// if this timer isn't used, it always thinks that nothing is selected.
			deselectTimer += Time.deltaTime;
			if (deselectTimer >= 0.05f)
			{
				if(Input.GetKey(KeyCode.UpArrow))
				{
					UICamera.selectedObject = Buttons[0].gameObject; 
					deselectTimer = 0.0f;
				}
				else if (Input.GetKey(KeyCode.DownArrow))
				{
					UICamera.selectedObject = Buttons[0].gameObject; 
					deselectTimer = 0.0f;
				}
				else if (Input.GetAxis("Vertical") > 0.0f)
				{
					UICamera.selectedObject = Buttons[1].gameObject; 
					deselectTimer = 0.0f;
				}
				else if (Input.GetAxis("Vertical") < 0.0f)
				{
					UICamera.selectedObject = Buttons[4].gameObject; 
					deselectTimer = 0.0f;
				}
				
			}
		}
		else
		{
			deselectTimer = 0.0f;
		}

		for(int i = 0; i < Buttons.Length; ++i)
		{
			if (UICamera.selectedObject == Buttons[i].gameObject)
			{
				ButtonMarkers[i].enabled = true;
			}
			else
			{
				ButtonMarkers[i].enabled = false;
			}
		}
    }

    public void OnPlayerOnePressed()
    {
        if (InputManager.Devices.Count >= 1)
        {
            Game.Singleton.Tower.numberOfPlayers = 1;
            Game.Singleton.Tower.currentFloorNumber = 1;
            Game.Singleton.Tower.keys = 0;
            Game.Singleton.Tower.lives = 1;
			modeToLoad = Game.EGameState.TowerPlayer1;

			inputCamera.useController = false;
			inputCamera.useTouch = false;
			inputCamera.useKeyboard = false;
			inputCamera.useMouse = false;

			fader.ReverseTransition();
        }
    }

    public void OnPlayerTwoPressed()
    {
        if (InputManager.Devices.Count >= 2)
        {
            Game.Singleton.Tower.numberOfPlayers = 2;
            Game.Singleton.Tower.currentFloorNumber = 1;
            Game.Singleton.Tower.keys = 0;
            Game.Singleton.Tower.lives = 1;
			modeToLoad = Game.EGameState.TowerPlayer2;

			inputCamera.useController = false;
			inputCamera.useTouch = false;
			inputCamera.useKeyboard = false;
			inputCamera.useMouse = false;

			fader.ReverseTransition();
        }
    }

    public void OnPlayerThreePressed()
    {
        if (InputManager.Devices.Count >= 3)
        {
            Game.Singleton.Tower.numberOfPlayers = 3;
            Game.Singleton.Tower.currentFloorNumber = 1;
            Game.Singleton.Tower.keys = 0;
            Game.Singleton.Tower.lives = 1;
			modeToLoad = Game.EGameState.TowerPlayer3;

			inputCamera.useController = false;
			inputCamera.useTouch = false;
			inputCamera.useKeyboard = false;
			inputCamera.useMouse = false;

			fader.ReverseTransition();
        }
    }

	public void OnCreditsPressed()
	{
		for (int i = 0; i < Buttons.Length; ++i)
		{
			Buttons[i].gameObject.SetActive(false);
			ButtonMarkers[i].gameObject.SetActive(false);
		}

		creditFader.gameObject.SetActive(true);
		creditFader.Transition();

		showingCredits = true;

		timeElapsed = 0.0f;
	}

	public void OnEnteredCredits()
	{
		credits.gameObject.SetActive(true);
	}

	public void OnReturnFromCredits()
	{
		for (int i = 0; i < Buttons.Length; ++i)
		{
			Buttons[i].gameObject.SetActive(true);
			ButtonMarkers[i].gameObject.SetActive(true);
			ButtonMarkers[i].enabled = false;
		}
		UICamera.selectedObject = Buttons[0].gameObject;
		ButtonMarkers[0].gameObject.SetActive(true);
		ButtonMarkers[0].enabled = false;
	}

	public void OnTransitionEnterEnd()
	{
		inputCamera.useController = true;
		inputCamera.useTouch = true;
		inputCamera.useKeyboard = true;
		inputCamera.useMouse = true;
	}

	public void OnTransitionExitEnd()
	{
		Game.Singleton.LoadLevel(modeToLoad);
	}

    public void Exit()
    {
#if UNITY_EDITOR
		Debug.Log("Application.Quit();");
#else
		Application.Quit();
#endif
	}
}
