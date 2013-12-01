// Desc:	1. Handles input and notifies children of input events.
//			2. Handles state and transitions between panels.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class UIPlayerMenuWindow : MonoBehaviour 
{
	protected Player player;
	public Player Player
	{
		get { return player; }
		set { player = value; }
	}

	protected Dictionary<int, UIPlayerMenuPanel> panels;
	public Dictionary<int, UIPlayerMenuPanel> Panels
	{
		get { return panels; }
		set { panels = value; }
	}

	protected UIPlayerMenuPanel activePanel;

	protected UIPlayerMenuScreen parentScreen;

	public abstract void AddAllMenuPanels();
	public abstract void TransitionToPanel(int panel);
	
	public virtual void CloseWindow()
	{
		gameObject.SetActive(false);
	}

	public virtual void Start()
	{
		panels = new Dictionary<int, UIPlayerMenuPanel>();

		parentScreen = transform.parent.parent.parent.parent.parent.GetComponent<UIPlayerMenuScreen>();

		AddAllMenuPanels();
	}

	public virtual void Initialise(Player player)
	{
		this.player = player;

		activePanel.gameObject.SetActive(true);
	}

	public virtual void Update()
	{
		HandleInputEvents();
	}

	#region Handle and Fire off events
	public delegate void PlayerWindowEventHandler(InputDevice device);

	public event PlayerWindowEventHandler OnMenuUp;
	public event PlayerWindowEventHandler OnMenuDown;
	public event PlayerWindowEventHandler OnMenuLeft;
	public event PlayerWindowEventHandler OnMenuRight;

	public event PlayerWindowEventHandler OnMenuX;
	public event PlayerWindowEventHandler OnMenuY;
	public event PlayerWindowEventHandler OnMenuA;
	public event PlayerWindowEventHandler OnMenuB;
	public event PlayerWindowEventHandler OnMenuStart;
	public event PlayerWindowEventHandler OnMenuBack;

	public event PlayerWindowEventHandler OnMenuLeftTrigger;
	public event PlayerWindowEventHandler OnMenuLeftBumper;
	public event PlayerWindowEventHandler OnMenuRightTrigger;
	public event PlayerWindowEventHandler OnMenuRightBumper;

	protected virtual void HandleInputEvents()
	{
		if (player != null)
		{
			if (InputManager.IsPolling)
			{
				InputDevice device = player.Input;

				// Face
				if (device.A.WasPressed)
				{
					if (OnMenuA != null)
					{
						OnMenuA.Invoke(player.Input);
					}
				}
				if (device.B.WasPressed)
				{
					if (OnMenuB != null)
					{
						OnMenuB.Invoke(player.Input);
					}
				}
				if (device.X.WasPressed)
				{
					if (OnMenuX != null)
					{
						OnMenuX.Invoke(player.Input);
					}
				}
				if (device.Y.WasPressed)
				{
					if (OnMenuY != null)
					{
						OnMenuY.Invoke(player.Input);
					}
				}

				// DPad
				if (device.DPadLeft.WasPressed || device.LeftStickX.WasPressed)
				{
					if (OnMenuLeft != null)
					{
						OnMenuLeft.Invoke(player.Input);
					}
				}
				else if (device.DPadRight.WasPressed || device.LeftStickX.WasPressed)
				{
					if (OnMenuRight != null)
					{
						OnMenuRight.Invoke(player.Input);
					}
				}
				if (device.DPadUp.WasPressed || device.LeftStickY.WasPressed)
				{
					if (OnMenuUp != null)
					{
						OnMenuUp.Invoke(player.Input);
					}
				}
				else if (device.DPadDown.WasPressed || device.LeftStickY.WasPressed)
				{
					if (OnMenuDown != null)
					{
						OnMenuDown.Invoke(player.Input);
					}
				}

				// Start 
				if (device.Start.WasPressed)
				{
					if (OnMenuStart != null)
					{
						OnMenuStart.Invoke(player.Input);
					}
				}

				// Back
				if (device.Back.WasPressed)
				{
					if (OnMenuBack != null)
					{
						OnMenuBack.Invoke(player.Input);
					}
				}

				// Triggers
				if (device.LeftTrigger.WasPressed)
				{
					if (OnMenuLeftTrigger != null)
					{
						OnMenuLeftTrigger.Invoke(player.Input);
					}
				}

				if (device.RightTrigger.WasPressed)
				{
					if (OnMenuRightTrigger != null)
					{
						OnMenuRightTrigger.Invoke(player.Input);
					}
				}

				// Bumpers
				if (device.LeftBumper.WasPressed)
				{
					if (OnMenuLeftBumper != null)
					{
						OnMenuLeftBumper.Invoke(player.Input);
					}
				}

				if (device.RightBumper.WasPressed)
				{
					if (OnMenuRightBumper != null)
					{
						OnMenuRightBumper.Invoke(player.Input);
					}
				}
			}
		}
	}

	#endregion
}
