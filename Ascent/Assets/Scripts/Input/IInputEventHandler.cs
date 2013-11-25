using UnityEngine;
using System.Collections;

public interface IInputEventHandler 
{
    void EnableInput(InputDevice inputDevice);
    void DisableInput();

    void OnX(InputDevice device);
    void OnY(InputDevice device);
    void OnA(InputDevice device);
    void OnB(InputDevice device);

    void OnX_up(InputDevice device);
    void OnY_up(InputDevice device);
    void OnA_up(InputDevice device);
    void OnB_up(InputDevice device);

    void OnStart(InputDevice device);
    void OnStart_up(InputDevice device);

    void OnBack(InputDevice device);
    void OnBack_up(InputDevice device);

    void OnLTrigger(InputDevice device);
    void OnLBumper(InputDevice device);

    void OnRTrigger(InputDevice device);
    void OnRBumper(InputDevice device);

    void OnDPadLeft(InputDevice device);
    void OnDPadRight(InputDevice device);
    void OnDPadUp(InputDevice device);
    void OnDPadDown(InputDevice device);

    void OnDPadLeft_up(InputDevice device);
    void OnDPadRight_up(InputDevice device);
    void OnDPadUp_up(InputDevice device);
    void OnDPadDown_up(InputDevice device);

    void OnLStickMove(InputDevice device);
    void OnLStick(InputDevice device);
    void OnLStick_up(InputDevice device);

    void OnRStickMove(InputDevice device);
    void OnRStick(InputDevice device);
    void OnRStick_up(InputDevice device);
}

// JUST COPY THIS DOWN HERE TO IMPLEMENT THIS INTERFACE

    //public void EnableInput(InputDevice inputDevice)
    //{
    //    // Register everthing here
    //}

    //public void DisableInput(InputDevice inputDevice)
    //{
    //    // Unregister everything here
    //}

    //public void OnX(InputDevice device)
    //{
    //}

    //public void OnY(InputDevice device)
    //{

    //}

    //public void OnA(InputDevice device)
    //{

    //}

    //public void OnB(InputDevice device)
    //{

    //}

    //public void OnX_up(InputDevice device)
    //{

    //}

    //public void OnY_up(InputDevice device)
    //{

    //}

    //public void OnA_up(InputDevice device)
    //{

    //}

    //public void OnB_up(InputDevice device)
    //{

    //}

    //public void OnStart(InputDevice device)
    //{

    //}

    //public void OnStart_up(InputDevice device)
    //{

    //}

    //public void OnBack(InputDevice device)
    //{

    //}

    //public void OnBack_up(InputDevice device)
    //{

    //}

    //public void OnLTrigger(InputDevice device)
    //{

    //}

    //public void OnLBumper(InputDevice device)
    //{

    //}

    //public void OnRTrigger(InputDevice device)
    //{

    //}

    //public void OnRBumper(InputDevice device)
    //{

    //}

    //public void OnDPadLeft(InputDevice device)
    //{

    //}

    //public void OnDPadRight(InputDevice device)
    //{

    //}

    //public void OnDPadUp(InputDevice device)
    //{

    //}

    //public void OnDPadDown(InputDevice device)
    //{

    //}

    //public void OnDPadLeft_up(InputDevice device)
    //{

    //}

    //public void OnDPadRight_up(InputDevice device)
    //{

    //}

    //public void OnDPadUp_up(InputDevice device)
    //{

    //}

    //public void OnDPadDown_up(InputDevice device)
    //{

    //}

    //public void OnLStickMove(InputDevice device)
    //{
      
    //}

    //public void OnLStick(InputDevice device)
    //{

    //}

    //public void OnLStick_up(InputDevice device)
    //{

    //}

    //public void OnRStickMove(InputDevice device)
    //{

    //}

    //public void OnRStick(InputDevice device)
    //{

    //}

    //public void OnRStick_up(InputDevice device)
    //{

    //}