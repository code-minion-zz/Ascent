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

	private const float showCreditsMinimumTime = 0.5f;
	private float timeElapsed;

	private bool stopInput;
	private bool up;
	private bool down;
	private bool a;
	private bool aRelease;
	private bool b;
	private bool bRelease;
	private bool highlight;
	private GameObject highlightedObject;

	public void Start()
	{
		Time.timeScale = 1f;

		MusicManager.Instance.PlayMusic(MusicManager.MusicSelections.Menu);

		inputCamera.useController = false;
		inputCamera.useTouch = false;
		inputCamera.useKeyboard = false;
		inputCamera.useMouse = false;

		fader.gameObject.SetActive(true);

		fader.onTransitionEnd += OnTransitionEnterEnd;
		fader.onReverseTransitionEnd += OnTransitionExitEnd;

		creditFader.onTransitionEnd += OnEnteredCredits;
		creditFader.onReverseTransitionEnd += OnReturnFromCredits;

		fader.Transition();

		((KeyboardInputDevice)InputManager.KeyBoard).menuMode = true;
	}

    public void Update()
	{
		if (UICamera.selectedObject == null)
			return;

		up = false;
		down = false;
		a = false;
		aRelease = false;
		b = false;
		bRelease = false;

		if (stopInput)
			return;

		var devices = InputManager.Devices;
		foreach (InputDevice d in devices)
		{
			if (!up || !down)
			{
				if (d.LeftStickY.WasPressed)
				{
					if (!up)
					{
						up = d.LeftStickY > 0.05f;

						if (!down)
						{
							down = d.LeftStickY < -0.05f;
						}
					}
				}
				else
				{
					if (!up)
					{
						up = d.DPadUp.WasPressed;

						if (!down)
						{
							down = d.DPadDown.WasPressed;
						}
					}
				}
			}

			if (!a)
				a = d.A.WasPressed || d.Start.WasPressed;

			if (!aRelease)
				aRelease = d.A.WasReleased || d.Start.WasReleased;

			if (!b)
				b = d.B.WasPressed;

			if (!bRelease)
				bRelease = d.B.WasReleased;
		}

		if (showingCredits)
		{
			timeElapsed += Time.deltaTime;

			if (timeElapsed < showCreditsMinimumTime)
				return;

			if (aRelease || bRelease)
			{
				stopInput = true;
				showingCredits = false;
				creditFader.ReverseTransition();
				credits.gameObject.SetActive(false);
			}

			return;
		}

		if (UICamera.selectedObject != null && !UICamera.selectedObject.GetComponent<UIButton>().isEnabled)
		{
			UICamera.selectedObject = Buttons[0].gameObject;
			Deselect();
		}

        int playerCount = 0;
        for (int i = 0; i < InputManager.Devices.Count; ++i)
        {
            Buttons[i].enabled = true;
            ++playerCount;
        }

        if (playerCount > 4)
        {
            playerCount = 4;
        }
		else if (playerCount == 0)
		{
			playerCount = 1;
		}

		if (playerCount == 4)
		{
			Buttons[0].GetComponent<UIButtonKeys>().selectOnUp = Buttons[5].GetComponent<UIButtonKeys>();
			Buttons[0].GetComponent<UIButtonKeys>().selectOnDown = Buttons[1].GetComponent<UIButtonKeys>();

			Buttons[1].GetComponent<UIButtonKeys>().selectOnUp = Buttons[0].GetComponent<UIButtonKeys>();
			Buttons[1].GetComponent<UIButtonKeys>().selectOnDown = Buttons[2].GetComponent<UIButtonKeys>();

			Buttons[2].GetComponent<UIButtonKeys>().selectOnUp = Buttons[1].GetComponent<UIButtonKeys>();
			Buttons[2].GetComponent<UIButtonKeys>().selectOnDown = Buttons[3].GetComponent<UIButtonKeys>();

			Buttons[3].GetComponent<UIButtonKeys>().selectOnUp = Buttons[2].GetComponent<UIButtonKeys>();
			Buttons[3].GetComponent<UIButtonKeys>().selectOnDown = Buttons[4].GetComponent<UIButtonKeys>();

			Buttons[4].GetComponent<UIButtonKeys>().selectOnUp = Buttons[3].GetComponent<UIButtonKeys>();
			Buttons[4].GetComponent<UIButtonKeys>().selectOnDown = Buttons[5].GetComponent<UIButtonKeys>();

			Buttons[5].GetComponent<UIButtonKeys>().selectOnUp = Buttons[4].GetComponent<UIButtonKeys>();
			Buttons[5].GetComponent<UIButtonKeys>().selectOnDown = Buttons[0].GetComponent<UIButtonKeys>();

			if (!Buttons[0].isEnabled)
				Buttons[0].isEnabled = true;

			if (!Buttons[1].isEnabled)
				Buttons[1].isEnabled = true;

			if (!Buttons[2].isEnabled)
				Buttons[2].isEnabled = true;

			if (!Buttons[3].isEnabled)
				Buttons[3].isEnabled = true;
		}
		else if (playerCount == 3)
		{
			Buttons[0].GetComponent<UIButtonKeys>().selectOnUp = Buttons[5].GetComponent<UIButtonKeys>();
			Buttons[0].GetComponent<UIButtonKeys>().selectOnDown = Buttons[1].GetComponent<UIButtonKeys>();

			Buttons[1].GetComponent<UIButtonKeys>().selectOnUp = Buttons[0].GetComponent<UIButtonKeys>();
			Buttons[1].GetComponent<UIButtonKeys>().selectOnDown = Buttons[2].GetComponent<UIButtonKeys>();

			Buttons[2].GetComponent<UIButtonKeys>().selectOnUp = Buttons[1].GetComponent<UIButtonKeys>();
			Buttons[2].GetComponent<UIButtonKeys>().selectOnDown = Buttons[4].GetComponent<UIButtonKeys>();

			Buttons[4].GetComponent<UIButtonKeys>().selectOnUp = Buttons[2].GetComponent<UIButtonKeys>();
			Buttons[4].GetComponent<UIButtonKeys>().selectOnDown = Buttons[5].GetComponent<UIButtonKeys>();

			Buttons[5].GetComponent<UIButtonKeys>().selectOnUp = Buttons[4].GetComponent<UIButtonKeys>();
			Buttons[5].GetComponent<UIButtonKeys>().selectOnDown = Buttons[0].GetComponent<UIButtonKeys>();

			if (!Buttons[0].isEnabled)
				Buttons[0].isEnabled = true;

			if (!Buttons[1].isEnabled)
				Buttons[1].isEnabled = true;

			if (!Buttons[2].isEnabled)
				Buttons[2].isEnabled = true;

			Buttons[3].isEnabled = false;
		}
		else if (playerCount == 2)
		{
			Buttons[0].GetComponent<UIButtonKeys>().selectOnUp = Buttons[5].GetComponent<UIButtonKeys>();
			Buttons[0].GetComponent<UIButtonKeys>().selectOnDown = Buttons[1].GetComponent<UIButtonKeys>();

			Buttons[1].GetComponent<UIButtonKeys>().selectOnUp = Buttons[0].GetComponent<UIButtonKeys>();
			Buttons[1].GetComponent<UIButtonKeys>().selectOnDown = Buttons[4].GetComponent<UIButtonKeys>();

			Buttons[4].GetComponent<UIButtonKeys>().selectOnUp = Buttons[1].GetComponent<UIButtonKeys>();
			Buttons[4].GetComponent<UIButtonKeys>().selectOnDown = Buttons[5].GetComponent<UIButtonKeys>();

			Buttons[5].GetComponent<UIButtonKeys>().selectOnUp = Buttons[4].GetComponent<UIButtonKeys>();
			Buttons[5].GetComponent<UIButtonKeys>().selectOnDown = Buttons[0].GetComponent<UIButtonKeys>();

			if (!Buttons[0].isEnabled)
				Buttons[0].isEnabled = true;

			if (!Buttons[1].isEnabled)
				Buttons[1].isEnabled = true;

			Buttons[2].isEnabled = false;
			Buttons[3].isEnabled = false;
		}
		else if (playerCount <= 1)
		{
			Buttons[0].GetComponent<UIButtonKeys>().selectOnUp = Buttons[5].GetComponent<UIButtonKeys>();
			Buttons[0].GetComponent<UIButtonKeys>().selectOnDown = Buttons[4].GetComponent<UIButtonKeys>();

			Buttons[4].GetComponent<UIButtonKeys>().selectOnUp = Buttons[0].GetComponent<UIButtonKeys>();
			Buttons[4].GetComponent<UIButtonKeys>().selectOnDown = Buttons[5].GetComponent<UIButtonKeys>();

			Buttons[5].GetComponent<UIButtonKeys>().selectOnUp = Buttons[4].GetComponent<UIButtonKeys>();
			Buttons[5].GetComponent<UIButtonKeys>().selectOnDown = Buttons[0].GetComponent<UIButtonKeys>();

			if(!Buttons[0].isEnabled)
				Buttons[0].isEnabled = true;

			Buttons[1].isEnabled = false;
			Buttons[2].isEnabled = false;
			Buttons[3].isEnabled = false;

		}

		if (up)
		{
			UICamera.selectedObject.GetComponent<UIButtonKeys>().OnKey(KeyCode.UpArrow);

			if (highlight)
				Deselect();
		}
		else if (down)
		{
			UICamera.selectedObject.GetComponent<UIButtonKeys>().OnKey(KeyCode.DownArrow);

			if(highlight)
				Deselect();
		}
		if(a)
		{
			UICamera.selectedObject.GetComponent<UIButton>().OnPress(true);
			highlightedObject = UICamera.selectedObject;
			highlight = true;
		}
		if (aRelease)
		{
			if (highlightedObject == UICamera.selectedObject && highlightedObject.GetComponent<UIButton>().isEnabled)
			{
				EventDelegate.Execute(UICamera.selectedObject.GetComponent<UIButton>().onClick);
				stopInput = true;
				return;
			}
			else
			{
				Deselect();
			}
		}
		if(b)
		{
			//UICamera.selectedObject.GetComponent<UIButtonKeys>().OnKey(KeyCode.UpArrow);
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

	private void Deselect()
	{
		highlightedObject = null;
		highlight = false;
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

			((KeyboardInputDevice)InputManager.KeyBoard).menuMode = false;
			
			StopMusic();

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

			((KeyboardInputDevice)InputManager.KeyBoard).menuMode = false;
			
			StopMusic();

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

			((KeyboardInputDevice)InputManager.KeyBoard).menuMode = false;
			
			StopMusic();

			fader.ReverseTransition();
        }
    }

	public void OnPlayerFourPressed()
	{
		if (InputManager.Devices.Count >= 4)
		{
			Game.Singleton.Tower.numberOfPlayers = 4;
			Game.Singleton.Tower.currentFloorNumber = 1;
			Game.Singleton.Tower.keys = 0;
			Game.Singleton.Tower.lives = 1;
			modeToLoad = Game.EGameState.TowerPlayer4;

			((KeyboardInputDevice)InputManager.KeyBoard).menuMode = false;

			StopMusic();

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
		stopInput = false;
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
		stopInput = false;
	}

	void StopMusic()
	{		
		MusicManager.Instance.SlowStop();
	}

	public void OnTransitionEnterEnd()
	{
		//inputCamera.useController = true;
		//inputCamera.useKeyboard = true;
	}

	public void OnTransitionExitEnd()
	{
		Game.Singleton.LoadLevel(modeToLoad);
	}

	void OnEnable()
	{		
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
