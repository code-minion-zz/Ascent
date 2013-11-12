using UnityEngine;
using System.Collections;
using InControl;

[RequireComponent(typeof(CapsuleCollider))]
public class HeroController : MonoBehaviour, IAscentController
{
    HeroAnimator heroAnimator;
    //Weapon heroWeapon;
    Hero hero;
    AscentInput input;
	bool movingLastFrame = false;

    public AscentInput Input
    {
        get { return input; }
    }

    #region Intialization

	public void Initialise(Hero hero)
	{
		this.hero = hero;
		heroAnimator = hero.Animator as HeroAnimator;
		//heroWeapon = hero.Weapon;
	}

    #endregion


    #region input


    public void EnableInput(AscentInput inputDevice)
    {
        input = inputDevice;
        // Register everthing here
		inputDevice.OnLStickMove += OnLStickMove;
		inputDevice.OnX += OnX;
		inputDevice.OnY += OnY;
		inputDevice.OnA += OnA;
		inputDevice.OnB += OnB;
    }

    public void DisableInput(AscentInput inputDevice)
    {
        // Unregister everything here
		inputDevice.OnLStickMove -= OnLStickMove;
		inputDevice.OnX -= OnX;
		inputDevice.OnY -= OnY;
		inputDevice.OnA -= OnA;
		inputDevice.OnB -= OnB;
    }

    public void OnX(ref  InControl.InputDevice device)
    {
        hero.UseAbility(0); // pass in the ability binded to this key
    }

    public void OnY(ref  InControl.InputDevice device)
    {
		hero.UseAbility(3);
    }

    public void OnA(ref  InControl.InputDevice device)
    {
		hero.UseAbility(1); // pass in the ability binded to this key
    }

    public void OnB(ref  InControl.InputDevice device)
    {
		hero.UseAbility(2); // pass in the ability binded to this key
    }

    public void OnX_up(ref  InControl.InputDevice device)
    {

    }

    public void OnY_up(ref  InControl.InputDevice device)
    {

    }

    public void OnA_up(ref  InControl.InputDevice device)
    {

    }

    public void OnB_up(ref  InControl.InputDevice device)
    {

    }

    public void OnStart(ref  InControl.InputDevice device)
    {

    }

    public void OnStart_up(ref  InControl.InputDevice device)
    {

    }

    public void OnBack(ref  InControl.InputDevice device)
    {

    }

    public void OnBack_up(ref  InControl.InputDevice device)
    {

    }

    public void OnLTrigger(ref  InControl.InputDevice device)
    {

    }

    public void OnLBumper(ref  InControl.InputDevice device)
    {

    }

    public void OnRTrigger(ref  InControl.InputDevice device)
    {

    }

    public void OnRBumper(ref  InControl.InputDevice device)
    {

    }

    public void OnDPadLeft(ref  InControl.InputDevice device)
    {

    }

    public void OnDPadRight(ref  InControl.InputDevice device)
    {

    }

    public void OnDPadUp(ref  InControl.InputDevice device)
    {

    }

    public void OnDPadDown(ref  InControl.InputDevice device)
    {

    }

    public void OnDPadLeft_up(ref  InControl.InputDevice device)
    {

    }

    public void OnDPadRight_up(ref  InControl.InputDevice device)
    {

    }

    public void OnDPadUp_up(ref  InControl.InputDevice device)
    {

    }

    public void OnDPadDown_up(ref  InControl.InputDevice device)
    {

    }

    public void OnLStickMove(ref  InControl.InputDevice device)
    {
        float speed = (device.LeftStickX.Value * device.LeftStickX.Value) + (device.LeftStickY.Value * device.LeftStickY.Value);
        speed *= heroAnimator.MovementSpeed * Time.deltaTime;
        speed *= 1000.0f;

        // Direction vector to hold the input key press.
        Vector3 direction = new Vector3(device.LeftStickX.Value, 0, device.LeftStickY.Value).normalized;

        heroAnimator.AnimMove(direction, speed);

        //hero.Move(direction, speed);
        // Move(direction, speed);
    }

    public void OnLStick(ref  InControl.InputDevice device)
    {

    }

    public void OnLStick_up(ref  InControl.InputDevice device)
    {

    }

    public void OnRStickMove(ref  InControl.InputDevice device)
    {

    }

    public void OnRStick(ref  InControl.InputDevice device)
    {

    }

    public void OnRStick_up(ref  InControl.InputDevice device)
    {

    }


    #endregion
}
