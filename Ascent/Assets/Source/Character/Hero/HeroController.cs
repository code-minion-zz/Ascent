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
	}

    #endregion


    void Update()
    {
        //InControl.InputDevice device = input.Device;

        ////if ((device.LeftStickX.IsNotNull || device.LeftStickY.IsNotNull))
        //{
        //    float speed = (device.LeftStickX.Value * device.LeftStickX.Value) + (device.LeftStickY.Value * device.LeftStickY.Value);
        //    speed *= heroAnimator.MovementSpeed * Time.deltaTime;
        //    speed *= 1000.0f;

        //    // Direction vector to hold the input key press.
        //    Vector3 direction = new Vector3(device.LeftStickX.Value, 0, device.LeftStickY.Value).normalized;

        //    heroAnimator.AnimMove(direction, speed);
        //}

    }


    #region input


    public void EnableInput(AscentInput inputDevice)
    {
        input = inputDevice;
        inputDevice.OnLStickMove += OnLStickMove;
        inputDevice.OnX += OnX;
        inputDevice.OnY += OnY;
        inputDevice.OnA += OnA;
        inputDevice.OnB += OnB;
		inputDevice.OnLeftBumper += OnLBumper;
		inputDevice.OnRightBumper += OnRBumper;
		inputDevice.OnRightTrigger += OnRTrigger;
    }

    public void DisableInput(AscentInput inputDevice)
    {
        inputDevice.OnLStickMove -= OnLStickMove;
        inputDevice.OnX -= OnX;
        inputDevice.OnY -= OnY;
        inputDevice.OnA -= OnA;
        inputDevice.OnB -= OnB;
		inputDevice.OnLeftBumper -= OnLBumper;
		inputDevice.OnRightBumper -= OnRBumper;
		inputDevice.OnRightTrigger -= OnRTrigger;
    }

    public void OnX(ref  InControl.InputDevice device)
    {

    }

    public void OnY(ref  InControl.InputDevice device)
    {
		
    }

    public void OnA(ref  InControl.InputDevice device)
    {
		hero.UseAbility(1); // pass in the ability binded to this key
    }

    public void OnB(ref  InControl.InputDevice device)
    {
		
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
		hero.UseAbility(2); // pass in the ability binded to this key
    }

    public void OnRTrigger(ref  InControl.InputDevice device)
    {
		hero.UseAbility(0); // pass in the ability binded to this key
    }

    public void OnRBumper(ref  InControl.InputDevice device)
    {
		hero.UseAbility(3);
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

        // Tell the hero animator to update the speed and direction.
        heroAnimator.AnimMove(direction, speed);
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
