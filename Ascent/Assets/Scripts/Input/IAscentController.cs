using UnityEngine;
using System.Collections;

public interface IAscentController 
{
    void EnableInput(AscentInput inputDevice);
    void DisableInput();

    void OnX(ref  InControl.InputDevice device);
    void OnY(ref  InControl.InputDevice device);
    void OnA(ref  InControl.InputDevice device);
    void OnB(ref  InControl.InputDevice device);

    void OnX_up(ref  InControl.InputDevice device);
    void OnY_up(ref  InControl.InputDevice device);
    void OnA_up(ref  InControl.InputDevice device);
    void OnB_up(ref  InControl.InputDevice device);

    void OnStart(ref  InControl.InputDevice device);
    void OnStart_up(ref  InControl.InputDevice device);

    void OnBack(ref  InControl.InputDevice device);
    void OnBack_up(ref  InControl.InputDevice device);

    void OnLTrigger(ref  InControl.InputDevice device);
    void OnLBumper(ref  InControl.InputDevice device);

    void OnRTrigger(ref  InControl.InputDevice device);
    void OnRBumper(ref  InControl.InputDevice device);

    void OnDPadLeft(ref  InControl.InputDevice device);
    void OnDPadRight(ref  InControl.InputDevice device);
    void OnDPadUp(ref  InControl.InputDevice device);
    void OnDPadDown(ref  InControl.InputDevice device);

    void OnDPadLeft_up(ref  InControl.InputDevice device);
    void OnDPadRight_up(ref  InControl.InputDevice device);
    void OnDPadUp_up(ref  InControl.InputDevice device);
    void OnDPadDown_up(ref  InControl.InputDevice device);

    void OnLStickMove(ref  InControl.InputDevice device);
    void OnLStick(ref  InControl.InputDevice device);
    void OnLStick_up(ref  InControl.InputDevice device);

    void OnRStickMove(ref  InControl.InputDevice device);
    void OnRStick(ref  InControl.InputDevice device);
    void OnRStick_up(ref  InControl.InputDevice device);
}

// JUST COPY THIS DOWN HERE TO IMPLEMENT THIS INTERFACE

    //public void EnableInput(AscentInput inputDevice)
    //{
    //    // Register everthing here
    //}

    //public void DisableInput(AscentInput inputDevice)
    //{
    //    // Unregister everything here
    //}

    //public void OnX(ref  InControl.InputDevice device)
    //{
    //}

    //public void OnY(ref  InControl.InputDevice device)
    //{

    //}

    //public void OnA(ref  InControl.InputDevice device)
    //{

    //}

    //public void OnB(ref  InControl.InputDevice device)
    //{

    //}

    //public void OnX_up(ref  InControl.InputDevice device)
    //{

    //}

    //public void OnY_up(ref  InControl.InputDevice device)
    //{

    //}

    //public void OnA_up(ref  InControl.InputDevice device)
    //{

    //}

    //public void OnB_up(ref  InControl.InputDevice device)
    //{

    //}

    //public void OnStart(ref  InControl.InputDevice device)
    //{

    //}

    //public void OnStart_up(ref  InControl.InputDevice device)
    //{

    //}

    //public void OnBack(ref  InControl.InputDevice device)
    //{

    //}

    //public void OnBack_up(ref  InControl.InputDevice device)
    //{

    //}

    //public void OnLTrigger(ref  InControl.InputDevice device)
    //{

    //}

    //public void OnLBumper(ref  InControl.InputDevice device)
    //{

    //}

    //public void OnRTrigger(ref  InControl.InputDevice device)
    //{

    //}

    //public void OnRBumper(ref  InControl.InputDevice device)
    //{

    //}

    //public void OnDPadLeft(ref  InControl.InputDevice device)
    //{

    //}

    //public void OnDPadRight(ref  InControl.InputDevice device)
    //{

    //}

    //public void OnDPadUp(ref  InControl.InputDevice device)
    //{

    //}

    //public void OnDPadDown(ref  InControl.InputDevice device)
    //{

    //}

    //public void OnDPadLeft_up(ref  InControl.InputDevice device)
    //{

    //}

    //public void OnDPadRight_up(ref  InControl.InputDevice device)
    //{

    //}

    //public void OnDPadUp_up(ref  InControl.InputDevice device)
    //{

    //}

    //public void OnDPadDown_up(ref  InControl.InputDevice device)
    //{

    //}

    //public void OnLStickMove(ref  InControl.InputDevice device)
    //{
      
    //}

    //public void OnLStick(ref  InControl.InputDevice device)
    //{

    //}

    //public void OnLStick_up(ref  InControl.InputDevice device)
    //{

    //}

    //public void OnRStickMove(ref  InControl.InputDevice device)
    //{

    //}

    //public void OnRStick(ref  InControl.InputDevice device)
    //{

    //}

    //public void OnRStick_up(ref  InControl.InputDevice device)
    //{

    //}