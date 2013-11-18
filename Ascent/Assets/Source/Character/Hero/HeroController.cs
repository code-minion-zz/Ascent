using UnityEngine;
using System.Collections;
using InControl;

[RequireComponent(typeof(CapsuleCollider))]
public class HeroController : MonoBehaviour
{
    private HeroAnimator heroAnimator;
    private Hero hero;
    private InControl.InputDevice device;

    private bool isEnabled;

    public InControl.InputDevice InputDevice
    {
        get { return device; }
    }

    #region Intialization

	public void Initialise(Hero hero, InControl.InputDevice device)
	{
        this.device = device;
		this.hero = hero;

		heroAnimator = hero.Animator as HeroAnimator;

        isEnabled = true;
	}

    #endregion


    void Update()
    {

        if (device == null)
        {
            Debug.Log("asd");
            // lost connection to device
            isEnabled = false;
        }

        if (isEnabled)
        {
            // Left Stick
            if ((device.LeftStickX.IsNotNull || device.LeftStickY.IsNotNull))
            {
                float speed = (device.LeftStickX.Value * device.LeftStickX.Value) + (device.LeftStickY.Value * device.LeftStickY.Value);
                speed *= heroAnimator.MovementSpeed * Time.deltaTime;
                speed *= 1000.0f;

                // Direction vector to hold the input key press.
                Vector3 direction = new Vector3(device.LeftStickX.Value, 0, device.LeftStickY.Value).normalized;

                heroAnimator.AnimMove(direction, speed);
            }

            if (device.LeftStickButton.WasPressed) // L Stick down
            {
                // Not used yet
            }

            // Right Stick
            if ((device.RightStickX.IsNotNull || device.RightStickY.IsNotNull))
            {
                // Not used yet
            }

            if (device.RightStickButton.WasPressed) // R Stick down
            {
                // Not used yet
            }


            // Face buttons
            if (device.Action1.WasPressed) // A
            {
                hero.UseAbility(1);
            }

            if (device.Action2.WasReleased) // B
            {
                // not used yet
            }

            if (device.Action3.WasPressed) // X
            {
                hero.UseAbility(0);
            }

            if (device.Action4.WasPressed) // Y
            {
                // not used yet
            }

            if (device.Buttons[6].WasPressed) // Back
            {
                // not used yet
            }

            if (device.Buttons[7].WasPressed) // Start
            {
                // not used yet
            }


            // Triggers
            if (device.LeftTrigger.WasPressed) // L trigger
            {
                // not used yet
            }

            if (device.RightTrigger.WasPressed) // R trigger
            {
                // not used yet
            }

            // Bumpers
            if (device.LeftBumper.WasPressed) // L bump
            {
                hero.UseAbility(2);
            }

            if (device.RightBumper.WasPressed) // R bump
            {
                hero.UseAbility(3);
            }
        }

    }

    public void EnableInput(bool state)
    {
        isEnabled = state;
    }
}
