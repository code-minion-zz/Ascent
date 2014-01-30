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
    public UIPlayerMenuPanel ActivePanel
    {
        get { return activePanel; }
        protected set { activePanel = value; }
    }

	protected UIPlayerMenuScreen parentScreen;
    public UIPlayerMenuScreen ParentScreen
    {
        get { return parentScreen; }
        protected set { parentScreen = value; }
    }

    protected bool ready;
    public bool Ready
    {
        get { return ready; }
        protected set { ready = value; }
    }

	public abstract void AddAllMenuPanels();
	public abstract void TransitionToPanel(int panel);

	public virtual void OnEnable()
	{
		if(activePanel != null)
		{
			activePanel.gameObject.SetActive(true);
		}
	}
	
	public virtual void CloseWindow()
	{
		gameObject.SetActive(false);
	}

    public virtual void ReadyWindow(bool ready)
    {
        this.ready = ready;
    }

    public virtual void Initialise()
    {
        panels = new Dictionary<int, UIPlayerMenuPanel>();

        parentScreen = transform.parent.parent.GetComponent<UIPlayerMenuScreen>();

        AddAllMenuPanels();

    }

	public virtual void SetPlayer(Player player)
	{
		this.player = player;

		//activePanel.gameObject.SetActive(true);
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

                if (device == null)
                {
                    return;
                }

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
                if (device.LeftStickX.WasPressed)
                {
                    if (device.LeftStickX.Value > 0.1f)
                    {
                        if (OnMenuRight != null)
                        {
                            OnMenuRight.Invoke(player.Input);
                        }
                    }
                    else if (device.LeftStickX.Value < -0.1f)
                    {
                        if (OnMenuLeft != null)
                        {
                            OnMenuLeft.Invoke(player.Input);
                        }
                    }
                }
                else
                {
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
                }


                if (device.LeftStickY.WasPressed)
                {
                    if (device.LeftStickY.Value > 0.1f)
                    {
                        if (OnMenuUp != null)
                        {
                            OnMenuUp.Invoke(player.Input);
                        }
                    }
                    else if (device.LeftStickY.Value < -0.1f)
                    {
                        if (OnMenuDown != null)
                        {
                            OnMenuDown.Invoke(player.Input);
                        }
                    }
                }
                else
                {
                    if (device.DPadUp.WasPressed)
                    {
                        if (OnMenuUp != null)
                        {
                            OnMenuUp.Invoke(player.Input);
                        }
                    }
                    else if (device.DPadDown.WasPressed)
                    {
                        if (OnMenuDown != null)
                        {
                            OnMenuDown.Invoke(player.Input);
                        }
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
