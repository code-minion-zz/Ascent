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

	private bool up;
	private bool down;
	private bool left;
	private bool right;
	private bool a;
	private bool aRelease;
	private bool b;
	private bool bRelease;
	private bool start;
	private bool highlight;
	private GameObject highlightedObject;

	private bool restartFloorConfirm;
	private bool returnMainConfirm;

	void OnEnable()
	{
		inputCamera.useController = false;
		inputCamera.useTouch = false;
		inputCamera.useKeyboard = false;
		inputCamera.useMouse = false;

		for (int i = 0; i < Buttons.Length; ++i)
		{
			ButtonMarkers[i].enabled = false;
		}

		((KeyboardInputDevice)InputManager.KeyBoard).menuMode = true;
	}

	void OnDisable()
	{
		inputCamera.useController = false;
		inputCamera.useTouch = false;
		inputCamera.useKeyboard = false;
		inputCamera.useMouse = false;

		UICamera.selectedObject = null;

		((KeyboardInputDevice)InputManager.KeyBoard).menuMode = false;
	}

	public void Update()
	{
		if (done)
			return;

		up = false;
		down = false;
		left = false;
		right = false;
		a = false;
		aRelease = false;
		b = false;
		bRelease = false;
		start = false;

		var devices = InputManager.Devices;
		foreach (InputDevice d in devices)
		{
			if (!d.IsConnected)
			{
				Debug.Log(d.IsConnected);
				continue;
			}

			if (!d.InUse)
			{
				Debug.Log(d.Name);
				continue;
			}

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

			if (!left || !right)
			{
				if (d.LeftStickX.WasPressed)
				{
					if (!right)
					{
						right = d.LeftStickX > 0.05f;

						if (!left)
						{
							left = d.LeftStickX < -0.05f;
						}
					}
				}
				else
				{
					if (!right)
					{
						right = d.DPadRight.WasPressed;

						if (!left)
						{
							left = d.DPadLeft.WasPressed;
						}
					}
				}
			}

			if (!a)
				a = d.A.WasPressed;

			if (!aRelease)
				aRelease = d.A.WasReleased;

			if (!b)
				b = d.B.WasPressed;

			if (!bRelease)
				bRelease = d.B.WasReleased;

			if (!start)
				start = d.Start.WasReleased;
		}

		if (UICamera.selectedObject != null && !UICamera.selectedObject.GetComponent<UIButton>().isEnabled)
		{
			UICamera.selectedObject = Buttons[0].gameObject;
			Deselect();
		}

		if (up)
		{
			UIButtonKeys current = UICamera.selectedObject.GetComponent<UIButtonKeys>();

			if (current.selectOnUp != null)
			{
				current.OnKey(KeyCode.UpArrow);

				if (highlight)
					Deselect();
			}
		}
		else if (down)
		{
			UIButtonKeys current = UICamera.selectedObject.GetComponent<UIButtonKeys>();

			if (current.selectOnDown != null)
			{
				current.OnKey(KeyCode.DownArrow);

				if (highlight)
					Deselect();
			}
		}
		else if(right)
		{
			UIButtonKeys current = UICamera.selectedObject.GetComponent<UIButtonKeys>();

			if (current.selectOnRight != null)
			{
				current.OnKey(KeyCode.RightArrow);

				if (highlight)
					Deselect();
			}
		}
		else if(left)
		{
			UIButtonKeys current = UICamera.selectedObject.GetComponent<UIButtonKeys>();

			if (current.selectOnLeft != null)
			{
				current.OnKey(KeyCode.LeftArrow);

				if (highlight)
					Deselect();
			}
		}

		if (start)
		{
			OnResume();
			return;
		}

		if (a)
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
				//stopInput = true;
				return;
			}
			else
			{
				Deselect();
			}
		}
		if (b)
		{
			if (restartFloorConfirm)
			{
				OnRestartCancel();
			}
			else if (returnMainConfirm)
			{
				OnTitleCancel();
			}
			else
			{
				UICamera.selectedObject = Buttons[0].gameObject;
			}
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

	private void Deselect()
	{
		highlightedObject = null;
		highlight = false;
	}

	public void OnResume()
	{
		OnRestartCancel();
		OnTitleCancel();
		UICamera.selectedObject = Buttons[0].gameObject;
		FloorHUDManager.Singleton.ShowPauseScreen(false);
	}

	public void OnRestartSelect()
	{
		lastSelected = UICamera.selectedObject;
		main.gameObject.SetActive(false);
		restartConfirm.gameObject.SetActive(true);
		restartFloorConfirm = true;
	}

	public void OnRestartCancel()
	{
		UICamera.selectedObject = lastSelected;
		main.gameObject.SetActive(true);
		restartConfirm.gameObject.SetActive(false);
		restartFloorConfirm = false;
		UICamera.selectedObject = Buttons[1].gameObject;
	}

	public void OnRestartConfirm()
	{
		fader.gameObject.SetActive(true);
		fader.transitionTime = transitionOutTime;
		fader.onReverseTransitionEnd += OnRestartFadeOutEnd;
		fader.ReverseTransitionNow();
		MusicManager.Instance.SlowStop();

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

		returnMainConfirm = true;
	}

	public void OnTitleCancel()
	{
		UICamera.selectedObject = lastSelected;
		main.gameObject.SetActive(true);
		titleConfirm.gameObject.SetActive(false);

		returnMainConfirm = false;
		UICamera.selectedObject = Buttons[2].gameObject;
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

		MusicManager.Instance.SlowStop();

		done = true;
	}

	public void OnTitleFadeOutEnd()
	{
		Destroy(Game.Singleton.Tower.CurrentFloor);
		Game.Singleton.LoadLevel(Game.EGameState.MainMenu);
	}
}
