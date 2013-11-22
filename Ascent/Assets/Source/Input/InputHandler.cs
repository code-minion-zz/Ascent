using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using InControl;

/// <summary>
/// Input handler class. Should be used as a singleton.
/// </summary>
public class InputHandler : MonoBehaviour 
{	
	#region Fields

	// Map to hold the player id and device assigned.
    InputDevice keyBoard;
	List<InputDevice> gamePads = new List<InputDevice>();
	
	#endregion

    public int NumberOfDevices
    {
        get { return gamePads.Count; }
    }

    void Awake()
    {
        // Setup the device manager and the events
        InputManager.Setup();
        InputManager.OnDeviceAttached += inputDevice => Debug.Log("Attached: " + inputDevice.Name + " " + inputDevice.Meta);
        InputManager.OnDeviceDetached += inputDevice => Debug.Log("Detached: " + inputDevice.Name + " " + inputDevice.Meta);
        //InputManager.OnActiveDeviceChanged += inputDevice => Debug.Log("Active device changed to: " + inputDevice.Name);

        EnumerateInputDevices();
        //TestInputMappings();
    }

    void EnumerateInputDevices()
    {
        // Get the available devices.
        int count = InputManager.Devices.Count;
        Debug.Log("Total Devices: " + count);

        // Grab the keyboard 
        keyBoard = InputManager.Devices[0];
        Debug.Log(0 + " " + keyBoard.Name + " " + keyBoard.Meta);

        if (keyBoard == null)
        {
            Debug.LogError("No keyboard Attached");
        }

        // Grab all other devices
        for (int i = 1; i < count; ++i)
        {
            // Add the device to list of gamepads

            InputDevice device = InputManager.Devices[i];

            gamePads.Add(device);

            Debug.Log(i + " " + device.Name + " " + device.Meta);
        }
    }

    public InputDevice GetKeyboard()
    {
        return (keyBoard);
    }

    public InputDevice GetFirstGamepad()
    {
        return (gamePads[0]);
    }
	
	public InputDevice GetDevice(int playerId)
	{
        return (gamePads[playerId]);
	}
	
	void FixedUpdate()
	{
//#if UNITY_EDITOR
        InputManager.Update();
//#endif
	}
	
	// Update is called once per frame
	void Update () 
	{
//#if !UNITY_EDITOR
//        InputManager.Update();
//#endif
        // For each input device binded by the players - send an event
	}

    #region Tests
    void TestInputMappings()
	{
		var complete = InputControlMapping.Range.Complete;
		var positive = InputControlMapping.Range.Positive;
		var negative = InputControlMapping.Range.Negative;
		var noInvert = false;
		var doInvert = true;

		TestInputMapping( complete, complete, noInvert, -1.0f, 0.0f, 1.0f );
		TestInputMapping( complete, negative, noInvert, -1.0f, -0.5f, 0.0f );
		TestInputMapping( complete, positive, noInvert, 0.0f, 0.5f, 1.0f );

		TestInputMapping( negative, complete, noInvert, -1.0f, 1.0f, 0.0f );
		TestInputMapping( negative, negative, noInvert, -1.0f, 0.0f, 0.0f );
		TestInputMapping( negative, positive, noInvert, 0.0f, 1.0f, 0.0f );

		TestInputMapping( positive, complete, noInvert, 0.0f, -1.0f, 1.0f );
		TestInputMapping( positive, negative, noInvert, 0.0f, -1.0f, 0.0f );
		TestInputMapping( positive, positive, noInvert, 0.0f, 0.0f, 1.0f );

		TestInputMapping( complete, complete, doInvert, 1.0f, 0.0f, -1.0f );
		TestInputMapping( complete, negative, doInvert, 1.0f, 0.5f, 0.0f );
		TestInputMapping( complete, positive, doInvert, 0.0f, -0.5f, -1.0f );

		TestInputMapping( negative, complete, doInvert, 1.0f, -1.0f, 0.0f );
		TestInputMapping( negative, negative, doInvert, 1.0f, 0.0f, 0.0f );
		TestInputMapping( negative, positive, doInvert, 0.0f, -1.0f, 0.0f );

		TestInputMapping( positive, complete, doInvert, 0.0f, 1.0f, -1.0f );
		TestInputMapping( positive, negative, doInvert, 0.0f, 1.0f, 0.0f );
		TestInputMapping( positive, positive, doInvert, 0.0f, 0.0f, -1.0f );
	}	
	
	void TestInputMapping( InputControlMapping.Range sourceRange, InputControlMapping.Range targetRange, bool invert, float expectA, float expectB, float expectC )
	{
		var mapping = new InputControlMapping() {
			SourceRange = sourceRange,
			TargetRange = targetRange,
			Invert      = invert
		};

		float value;

		string sr = "Complete";
		if (sourceRange == InputControlMapping.Range.Negative)
			sr = "Negative";
		else
		if (sourceRange == InputControlMapping.Range.Positive)
			sr = "Positive";

		string tr = "Complete";
		if (targetRange == InputControlMapping.Range.Negative)
			tr = "Negative";
		else
		if (targetRange == InputControlMapping.Range.Positive)
			tr = "Positive";

		value = mapping.MapValue( -1.0f );
		if (Mathf.Abs( value - expectA ) > Single.Epsilon)
		{
			Debug.LogError( "Got unexpected value A " + value + " instead of " + expectA + " (SR = " + sr + ", TR = " + tr + ")" );
		}

		value = mapping.MapValue( 0.0f );
		if (Mathf.Abs( value - expectB ) > Single.Epsilon)
		{
			Debug.LogError( "Got unexpected value B " + value + " instead of " + expectB + " (SR = " + sr + ", TR = " + tr + ")" );
		}

		value = mapping.MapValue( 1.0f );
		if (Mathf.Abs( value - expectC ) > Single.Epsilon)
		{
			Debug.LogError( "Got unexpected value C " + value + " instead of " + expectC + " (SR = " + sr + ", TR = " + tr + ")" );
		}
	}	
	#endregion
}
