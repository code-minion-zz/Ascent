using UnityEngine;
using System.Collections;

public abstract class UIPlayerPanel : MonoBehaviour
{
    protected Player player;
    public Player Player
    {
        get { return player; }
        set { player = value; }
    }

    protected InputDevice input;
    public InputDevice Input
    {
        get { return input; }
    }

    public bool reenabled = false;

    protected abstract void SetInitialState();

    protected abstract void OnUp();
    protected abstract void OnDown();
    protected abstract void OnLeft();
    protected abstract void OnRight();

    protected abstract void OnA();
    protected abstract void OnB();
    protected abstract void OnX();
    protected abstract void OnY();

    protected abstract void OnStart();
    protected abstract void OnBack();

    protected abstract void OnLeftTrigger();
    protected abstract void OnLeftBumper();

    protected abstract void OnRightTrigger();
    protected abstract void OnRightBumper();

    protected virtual void OnEnable()
    {
        reenabled = true;
    }

    public virtual void Initialise(Player p)
    {
        player = p;
        input = p.Input;
    }

    public virtual void OnDisable()
    {
        player = null;
        input = null;
    }
	
	protected virtual void Update () 
    {
        if (reenabled)
        {
            reenabled = false;
            SetInitialState();
        }

        if (player != null)
        {
            if (InputManager.IsPolling)
            {
                InputDevice device = input;

                // Face
                if (device.A.WasPressed)
                {
                    OnA();
                }
                if (device.B.WasPressed)
                {
                    OnB();
                }
                if (device.X.WasPressed)
                {
                    OnX();
                }
                if (device.Y.WasPressed)
                {
                    OnY();
                }

                // DPad
                if (device.DPadLeft.WasPressed)
                {
                    OnLeft();
                }
                else if (device.DPadRight.WasPressed)
                {
                    OnRight();
                }
                if (device.DPadUp.WasPressed)
                {
                    OnUp();
                }
                else if (device.DPadDown.WasPressed)
                {
                    OnDown();
                }

                // Start 
                if (device.Start.WasPressed)
                {
                    OnStart();
                }

                // Back
                if (device.Back.WasPressed)
                {
                    OnBack();
                }

                // Triggers
                if (device.LeftTrigger.WasPressed)
                {
                    OnLeftTrigger();
                }

                if (device.RightTrigger.WasPressed)
                {
                    OnRightTrigger();
                }

                // Bumpers
                if (device.LeftBumper.WasPressed)
                {
                    OnLeftBumper();
                }

                if (device.RightBumper.WasPressed)
                {
                    OnRightBumper();
                }
            }
        }
	}    
}
