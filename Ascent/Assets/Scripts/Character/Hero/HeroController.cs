using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[RequireComponent(typeof(CharacterController))]
public class HeroController : MonoBehaviour, IInputEventHandler
{
    private HeroAnimator heroAnimator;
    private Hero hero;
    private InputDevice input;
    private bool actionBindingsEnabled = false;

    public InputDevice Input
    {
        get { return input; }
    }

    /// <summary>
    /// Enables or disables the action binding, what this means is that
    /// we can repurpose buttons for example the Y button for opening chests.
    /// </summary>
    public bool EnableActionBinding
    {
        get { return actionBindingsEnabled; }
        set { actionBindingsEnabled = value; }
    }

    #region Intialization

	public void Initialise(Hero hero)
	{
		this.hero = hero;
		heroAnimator = hero.Animator as HeroAnimator;
	}

    #endregion


    void Update()
    {
		if (InputManager.IsPolling)
		{
			InputDevice device = input;

			// L Stick
			if ((device.LeftStickX.IsNotNull || device.LeftStickY.IsNotNull))
			{
				float speed = (device.LeftStickX.Value * device.LeftStickX.Value) + (device.LeftStickY.Value * device.LeftStickY.Value);
				speed *= heroAnimator.MovementSpeed * Time.deltaTime;
				speed *= 1000.0f;

				// Direction vector to hold the input key press.
				Vector3 direction = new Vector3(device.LeftStickX.Value, 0, device.LeftStickY.Value).normalized;

				heroAnimator.AnimMove(direction, speed);
			}


            if (device.X.WasPressed)
            {
                hero.UseAbility(0);
            }

            if(device.Back.WasPressed)
            {
                hero.RefreshEverything();
            }

            if (device.LeftBumper.WasPressed)
            {
                hero.UseAbility(1);
            }
            else if (device.LeftTrigger.WasPressed)
            {
                hero.UseAbility(4);
            }
            else if(device.RightBumper.WasPressed)
            {
                hero.UseAbility(2); // pass in the ability binded to this key
                
            }
            else if (device.RightTrigger.WasPressed)
            {
                hero.UseAbility(3);
            }

			// We can bind something to this key.
			if (device.Y.WasPressed)
			{
				ProcessInteractions();
			}


			//if (!actionBindingsEnabled)
			//{
			//    // We can bind something to this key.
			//    if (device.Y.WasPressed)
			//    {
                    
			//    }
			//}
		}
    }

	public void ProcessInteractions()
	{
		// NOTE: Only one of these may occur each time the button is pressed

		Room curRoom = Game.Singleton.Tower.CurrentFloor.CurrentRoom;
		Vector3 position = transform.position;

		// Is there a chest to open?
		List<TreasureChest> chests = curRoom.Chests;
		if (chests != null)
		{
			// Are we in range of it?
			foreach (TreasureChest c in chests)
			{
				if(c.TriggerRegion.IsInside(position))
				{
					// Can it be opened?
					if (c.IsClosed)
					{
						c.OpenChest(); // I open the chest. No one else can.
						
						return; // An interaction has occured. Exit function now.
					}
				}
			}
		}

		// Is there an item?
		List<LootDrop> loot = curRoom.LootDrops;
		if (loot != null)
		{
			// Find the closest item
			LootDrop closestDrop = null;
			float closestDistance = 10000.0f;
			foreach (LootDrop l in loot)
			{
				if (!l.CanBePickedUp)
				{
					continue;
				}

				float distance = (position - l.transform.position).sqrMagnitude;

				if (distance < closestDistance)
				{
					closestDistance = distance;
					closestDrop = l;
				}
			}

			if (closestDrop != null)
			{
				// Am I within range of the item?
				if (closestDrop.TriggerRegion.IsInside(position))
				{
					closestDrop.PickUp(hero.HeroInventory); // I pick it up. No one else can!

					return; // An interaction has occured. Exit function now.
				}
			}
		}


		// Is there a door?
		// Find closest door
		// Has it already been opened?

		// Is there a block?
		// Find the closest block
		// Is it already being interacted with?
		// Has someone else started to interact with it?
	}


    #region input


    public void EnableInput(InputDevice inputDevice)
    {
		input = inputDevice;

		if (!InputManager.IsPolling)
		{
			inputDevice.OnLStickMove += OnLStickMove;
			inputDevice.OnX += OnX;
			inputDevice.OnY += OnY;
			inputDevice.OnA += OnA;
			inputDevice.OnB += OnB;
			inputDevice.OnLeftBumper += OnLBumper;
			inputDevice.OnRightBumper += OnRBumper;
			inputDevice.OnRightTrigger += OnRTrigger;
		}
    }

    public void DisableInput()
    {
		if (!InputManager.IsPolling)
		{
			input.OnLStickMove -= OnLStickMove;
			input.OnX -= OnX;
			input.OnY -= OnY;
			input.OnA -= OnA;
			input.OnB -= OnB;
			input.OnLeftBumper -= OnLBumper;
			input.OnRightBumper -= OnRBumper;
			input.OnRightTrigger -= OnRTrigger;
		}

		input = null;
    }

    public void OnX(InputDevice device)
    {
        hero.UseAbility(0); // pass in the ability binded to this key
    }

    public void OnY(InputDevice device)
    {
		
    }

    public void OnA(InputDevice device)
    {
		hero.UseAbility(1); // pass in the ability binded to this key
    }

    public void OnB(InputDevice device)
    {
		
    }

    public void OnX_up(InputDevice device)
    {

    }

    public void OnY_up(InputDevice device)
    {

    }

    public void OnA_up(InputDevice device)
    {

    }

    public void OnB_up(InputDevice device)
    {

    }

    public void OnStart(InputDevice device)
    {

    }

    public void OnStart_up(InputDevice device)
    {

    }

    public void OnBack(InputDevice device)
    {

    }

    public void OnBack_up(InputDevice device)
    {

    }

    public void OnLTrigger(InputDevice device)
    {

    }

    public void OnLBumper(InputDevice device)
    {
		hero.UseAbility(2); // pass in the ability binded to this key
    }

    public void OnRTrigger(InputDevice device)
    {
		
    }

    public void OnRBumper(InputDevice device)
    {
		hero.UseAbility(3);
    }

    public void OnDPadLeft(InputDevice device)
    {

    }

    public void OnDPadRight(InputDevice device)
    {

    }

    public void OnDPadUp(InputDevice device)
    {

    }

    public void OnDPadDown(InputDevice device)
    {

    }

    public void OnDPadLeft_up(InputDevice device)
    {

    }

    public void OnDPadRight_up(InputDevice device)
    {

    }

    public void OnDPadUp_up(InputDevice device)
    {

    }

    public void OnDPadDown_up(InputDevice device)
    {

    }

    public void OnLStickMove(InputDevice device)
    {
        float speed = (device.LeftStickX.Value * device.LeftStickX.Value) + (device.LeftStickY.Value * device.LeftStickY.Value);
        speed *= heroAnimator.MovementSpeed * Time.deltaTime;
        speed *= 1000.0f;

        // Direction vector to hold the input key press.
        Vector3 direction = new Vector3(device.LeftStickX.Value, 0, device.LeftStickY.Value).normalized;

        // Tell the hero animator to update the speed and direction.
        heroAnimator.AnimMove(direction, speed);
    }

    public void OnLStick(InputDevice device)
    {

    }

    public void OnLStick_up(InputDevice device)
    {

    }

    public void OnRStickMove(InputDevice device)
    {

    }

    public void OnRStick(InputDevice device)
    {

    }

    public void OnRStick_up(InputDevice device)
    {

    }


    #endregion
}
