using UnityEngine;
using System.Collections;


[RequireComponent(typeof(CharacterController))]
public class HeroController : MonoBehaviour, IInputEventHandler
{
    HeroAnimator heroAnimator;
    //Weapon heroWeapon;
    Hero hero;
    InputDevice input;

    public InputDevice Input
    {
        get { return input; }
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

			if (device.LeftStickButton.WasPressed)
			{
				//hero.UseAbility(0); // pass in the ability binded to this key
			}

			// R Stick

			if ((device.RightStickX.IsNotNull || device.RightStickY.IsNotNull))
			{
			}

			if (device.RightStickButton.WasPressed)
			{
				//hero.UseAbility(0); // pass in the ability binded to this key
			}

			// Face
			if (device.Action1.WasPressed)
			{
				//hero.UseAbility(0); // pass in the ability binded to this key
			}
			if (device.Action2.WasPressed)
			{
				//hero.UseAbility(0); // pass in the ability binded to this key
			}
            //if (device.Action3.WasPressed)
            //{
            //    hero.UseAbility(0); // pass in the ability binded to this key
            //}
            //if (device.Action4.WasPressed)
            //{
            //    hero.UseAbility(0); // pass in the ability binded to this key
            //}
            if (device.X.WasReleased)
            {
                hero.UseAbility(0);
            }

			// DPad
			if (device.DPadLeft.WasPressed)
			{
				//hero.UseAbility(0); // pass in the ability binded to this key
			}
			else if (device.DPadRight.WasPressed)
			{
				//hero.UseAbility(0); // pass in the ability binded to this key
			}
			if (device.DPadUp.WasPressed)
			{
				//hero.UseAbility(0); // pass in the ability binded to this key
			}
			else if (device.DPadDown.WasPressed)
			{
				//hero.UseAbility(0); // pass in the ability binded to this key
			}

			// Start 
			if (device.Start.WasPressed)
			{
				//hero.UseAbility(0); // pass in the ability binded to this key
			}

			// Back
			if (device.Back.WasPressed)
			{
				//hero.UseAbility(0); // pass in the ability binded to this key
			}

			// Triggers
            if (device.LeftTrigger.WasPressed && device.Y.WasReleased)
			{
				//hero.UseAbility(0); // pass in the ability binded to this key
			}

			if (device.RightTrigger.WasPressed)
			{
				//hero.UseAbility(0); // pass in the ability binded to this key
			}

			// Bumpers
            if (device.LeftBumper.IsPressed && device.Y.WasPressed)
			{
				hero.UseAbility(4); // pass in the ability binded to this key
			}

			if (device.RightBumper.WasPressed)
			{
				//hero.UseAbility(0); // pass in the ability binded to this key
			}
		}
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
