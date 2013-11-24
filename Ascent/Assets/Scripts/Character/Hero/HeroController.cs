using UnityEngine;
using System.Collections;


[RequireComponent(typeof(CharacterController))]
public class HeroController : MonoBehaviour, IAscentController
{
    HeroAnimator heroAnimator;
    //Weapon heroWeapon;
    Hero hero;
    AscentInput input;

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
        InputDevice device = input.Device;

        // L Stick
        //if ((device.LeftStickX.IsNotNull || device.LeftStickY.IsNotNull))
        {
            float speed = (device.LeftStickX.Value * device.LeftStickX.Value) + (device.LeftStickY.Value * device.LeftStickY.Value);
            speed *= heroAnimator.MovementSpeed * Time.deltaTime;
            speed *= 1000.0f;

            // Direction vector to hold the input key press.
            Vector3 direction = new Vector3(device.LeftStickX.Value, 0, device.LeftStickY.Value).normalized;

            heroAnimator.AnimMove(direction, speed);
        }

        // R Stick

        // Face
        //if (device.Action1.WasPressed)
        //{
        //    hero.UseAbility(1); // pass in the ability binded to this key
        //}

        // Start 
        
        // Back
        
        // Triggers

        // Bumpers


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

    public void DisableInput()
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

    public void OnX(ref  InputDevice device)
    {
        hero.UseAbility(0); // pass in the ability binded to this key
    }

    public void OnY(ref  InputDevice device)
    {
		
    }

    public void OnA(ref  InputDevice device)
    {
		hero.UseAbility(1); // pass in the ability binded to this key
    }

    public void OnB(ref  InputDevice device)
    {
		
    }

    public void OnX_up(ref  InputDevice device)
    {

    }

    public void OnY_up(ref  InputDevice device)
    {

    }

    public void OnA_up(ref  InputDevice device)
    {

    }

    public void OnB_up(ref  InputDevice device)
    {

    }

    public void OnStart(ref  InputDevice device)
    {

    }

    public void OnStart_up(ref  InputDevice device)
    {

    }

    public void OnBack(ref  InputDevice device)
    {

    }

    public void OnBack_up(ref  InputDevice device)
    {

    }

    public void OnLTrigger(ref  InputDevice device)
    {

    }

    public void OnLBumper(ref  InputDevice device)
    {
		hero.UseAbility(2); // pass in the ability binded to this key
    }

    public void OnRTrigger(ref  InputDevice device)
    {
		
    }

    public void OnRBumper(ref  InputDevice device)
    {
		hero.UseAbility(3);
    }

    public void OnDPadLeft(ref  InputDevice device)
    {

    }

    public void OnDPadRight(ref  InputDevice device)
    {

    }

    public void OnDPadUp(ref  InputDevice device)
    {

    }

    public void OnDPadDown(ref  InputDevice device)
    {

    }

    public void OnDPadLeft_up(ref  InputDevice device)
    {

    }

    public void OnDPadRight_up(ref  InputDevice device)
    {

    }

    public void OnDPadUp_up(ref  InputDevice device)
    {

    }

    public void OnDPadDown_up(ref  InputDevice device)
    {

    }

    public void OnLStickMove(ref  InputDevice device)
    {
        float speed = (device.LeftStickX.Value * device.LeftStickX.Value) + (device.LeftStickY.Value * device.LeftStickY.Value);
        speed *= heroAnimator.MovementSpeed * Time.deltaTime;
        speed *= 1000.0f;

        // Direction vector to hold the input key press.
        Vector3 direction = new Vector3(device.LeftStickX.Value, 0, device.LeftStickY.Value).normalized;

        // Tell the hero animator to update the speed and direction.
        heroAnimator.AnimMove(direction, speed);
    }

    public void OnLStick(ref  InputDevice device)
    {

    }

    public void OnLStick_up(ref  InputDevice device)
    {

    }

    public void OnRStickMove(ref  InputDevice device)
    {

    }

    public void OnRStick(ref  InputDevice device)
    {

    }

    public void OnRStick_up(ref  InputDevice device)
    {

    }


    #endregion
}
