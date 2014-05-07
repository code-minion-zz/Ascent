using UnityEngine;
using System.Collections;

public class PauseScreen : MonoBehaviour 
{
	public UICamera inputCamera;

	public SceneFadeInFadeOut fader;

	public GameObject main;
	public GameObject restartConfirm;
	public GameObject titleConfirm;

	public UIButton[] Buttons;
	public UISprite[] ButtonMarkers;

	public UIButton[] confirmRestartButtons;
	public UISprite[] confirmRestartButtonMarkers;

	public UIButton[] confirmTitleButtons;
	public UISprite[] confirmTitleButtonMarkers;

	private GameObject lastSelected;

	public float transitionOutTime = 0.85f;

	private bool horizontalControl;
	private float deselectTimer;

	private bool done;

	void OnEnable()
	{
		inputCamera.useController = true;
		inputCamera.useTouch = false;
		inputCamera.useKeyboard = true;
		inputCamera.useMouse = false;

		for (int i = 0; i < Buttons.Length; ++i)
		{
			ButtonMarkers[i].enabled = false;
		}
	}

	void OnDisable()
	{
		inputCamera.useController = false;
		inputCamera.useTouch = false;
		inputCamera.useKeyboard = false;
		inputCamera.useMouse = false;

		UICamera.selectedObject = null;
	}

	public void Update()
	{
		if (done)
			return;

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
			deselectTimer += RealTime.deltaTime;
			if (deselectTimer >= 0.05f)
			{
				if (Input.GetKey(KeyCode.UpArrow))
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
					UICamera.selectedObject = Buttons[2].gameObject;
					deselectTimer = 0.0f;
				}

			}
		}
		else
		{
			deselectTimer = 0.0f;
		}

		for (int i = 0; i < Buttons.Length; ++i)
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

	public void OnResume()
	{
		FloorHUDManager.Singleton.ShowPauseScreen(false);
	}

	public void OnRestartSelect()
	{
		lastSelected = UICamera.selectedObject;
		main.gameObject.SetActive(false);
		restartConfirm.gameObject.SetActive(true);
	}

	public void OnRestartCancel()
	{
		UICamera.selectedObject = lastSelected;
		main.gameObject.SetActive(true);
		restartConfirm.gameObject.SetActive(false);
	}

	public void OnRestartConfirm()
	{
		fader.gameObject.SetActive(true);
		fader.transitionTime = transitionOutTime;
		fader.onReverseTransitionEnd += OnRestartFadeOutEnd;
		fader.ReverseTransitionNow();

		inputCamera.useController = false;
		inputCamera.useTouch = false;
		inputCamera.useKeyboard = false;
		inputCamera.useMouse = false;

		done = true;
	}

	public void OnRestartFadeOutEnd()
	{
		Game.Singleton.Tower.LoadFloor();
	}
	
	public void OnTitleSelect()
	{
		lastSelected = UICamera.selectedObject;
		main.gameObject.SetActive(false);
		titleConfirm.gameObject.SetActive(true);
	}

	public void OnTitleCancel()
	{
		UICamera.selectedObject = lastSelected;
		main.gameObject.SetActive(true);
		titleConfirm.gameObject.SetActive(false);
	}

	public void OnTitleConfirm()
	{
		fader.gameObject.SetActive(true);
		fader.transitionTime = transitionOutTime;
		fader.ReverseTransitionNow();
		fader.onReverseTransitionEnd += OnTitleFadeOutEnd;

		inputCamera.useController = false;
		inputCamera.useTouch = false;
		inputCamera.useKeyboard = false;
		inputCamera.useMouse = false;

		done = true;
	}

	public void OnTitleFadeOutEnd()
	{
		Destroy(Game.Singleton.Tower.CurrentFloor);
		Game.Singleton.LoadLevel(Game.EGameState.MainMenu);
	}
}
