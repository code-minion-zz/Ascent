using UnityEngine;
using System.Collections;

public interface IAscentController 
{
    void EnableInput(AscentInput inputDevice);
    void DisableInput();

    void OnX(ref  InputDevice device);
    void OnY(ref  InputDevice device);
    void OnA(ref  InputDevice device);
    void OnB(ref  InputDevice device);

    void OnX_up(ref  InputDevice device);
    void OnY_up(ref  InputDevice device);
    void OnA_up(ref  InputDevice device);
    void OnB_up(ref  InputDevice device);

    void OnStart(ref  InputDevice device);
    void OnStart_up(ref  InputDevice device);

    void OnBack(ref  InputDevice device);
    void OnBack_up(ref  InputDevice device);

    void OnLTrigger(ref  InputDevice device);
    void OnLBumper(ref  InputDevice device);

    void OnRTrigger(ref  InputDevice device);
    void OnRBumper(ref  InputDevice device);

    void OnDPadLeft(ref  InputDevice device);
    void OnDPadRight(ref  InputDevice device);
    void OnDPadUp(ref  InputDevice device);
    void OnDPadDown(ref  InputDevice device);

    void OnDPadLeft_up(ref  InputDevice device);
    void OnDPadRight_up(ref  InputDevice device);
    void OnDPadUp_up(ref  InputDevice device);
    void OnDPadDown_up(ref  InputDevice device);

    void OnLStickMove(ref  InputDevice device);
    void OnLStick(ref  InputDevice device);
    void OnLStick_up(ref  InputDevice device);

    void OnRStickMove(ref  InputDevice device);
    void OnRStick(ref  InputDevice device);
    void OnRStick_up(ref  InputDevice device);
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

    //public void OnX(ref  InputDevice device)
    //{
    //}

    //public void OnY(ref  InputDevice device)
    //{

    //}

    //public void OnA(ref  InputDevice device)
    //{

    //}

    //public void OnB(ref  InputDevice device)
    //{

    //}

    //public void OnX_up(ref  InputDevice device)
    //{

    //}

    //public void OnY_up(ref  InputDevice device)
    //{

    //}

    //public void OnA_up(ref  InputDevice device)
    //{

    //}

    //public void OnB_up(ref  InputDevice device)
    //{

    //}

    //public void OnStart(ref  InputDevice device)
    //{

    //}

    //public void OnStart_up(ref  InputDevice device)
    //{

    //}

    //public void OnBack(ref  InputDevice device)
    //{

    //}

    //public void OnBack_up(ref  InputDevice device)
    //{

    //}

    //public void OnLTrigger(ref  InputDevice device)
    //{

    //}

    //public void OnLBumper(ref  InputDevice device)
    //{

    //}

    //public void OnRTrigger(ref  InputDevice device)
    //{

    //}

    //public void OnRBumper(ref  InputDevice device)
    //{

    //}

    //public void OnDPadLeft(ref  InputDevice device)
    //{

    //}

    //public void OnDPadRight(ref  InputDevice device)
    //{

    //}

    //public void OnDPadUp(ref  InputDevice device)
    //{

    //}

    //public void OnDPadDown(ref  InputDevice device)
    //{

    //}

    //public void OnDPadLeft_up(ref  InputDevice device)
    //{

    //}

    //public void OnDPadRight_up(ref  InputDevice device)
    //{

    //}

    //public void OnDPadUp_up(ref  InputDevice device)
    //{

    //}

    //public void OnDPadDown_up(ref  InputDevice device)
    //{

    //}

    //public void OnLStickMove(ref  InputDevice device)
    //{
      
    //}

    //public void OnLStick(ref  InputDevice device)
    //{

    //}

    //public void OnLStick_up(ref  InputDevice device)
    //{

    //}

    //public void OnRStickMove(ref  InputDevice device)
    //{

    //}

    //public void OnRStick(ref  InputDevice device)
    //{

    //}

    //public void OnRStick_up(ref  InputDevice device)
    //{

    //}