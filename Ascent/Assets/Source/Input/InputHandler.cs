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
	
	enum PLAYER_ID
	{
		PLAYER_ONE,
		PLAYER_TWO,
		PLAYER_THREE
	};


	// Map to hold the player id and device assigned.
	Dictionary<int, InputDevice> playerDevices = new Dictionary<int, InputDevice>();
	
	#endregion

    void Awake()
    {
        // Setup the device manager and the events
        InputManager.Setup();
        InputManager.OnDeviceAttached += OnAttached;
        //InputManager.OnDeviceAttached += inputDevice => Debug.Log("Attached: " + inputDevice.Name);
        InputManager.OnDeviceDetached += inputDevice => Debug.Log("Detached: " + inputDevice.Name);
        InputManager.OnActiveDeviceChanged += inputDevice => Debug.Log("Active device changed to: " + inputDevice.Name);


        SetupPlayerDevices();
        TestInputMappings();
    }

    void OnAttached(InputDevice device)
    {
        Debug.Log("Attached: " + device.Name);
    }
	
	// Use this for initialization
	void Start () 
	{

	}
	
	public InputDevice GetNextAvailable()
	{
		foreach (InputDevice device in InputManager.Devices)
		{
			if (!device.InUse)
			{
				device.InUse = true;
				return (device);
			}
		}
		
		return (null);
	}
	
	public InputDevice GetDevice(int playerId)
	{
		foreach(KeyValuePair<int, InputDevice> kvp in playerDevices)
		{
			if (playerId == kvp.Key)
				return (kvp.Value);
		}
		
		// No Device assigned for this player.
		// We need to create one by getting the next available.
		InputDevice dev = GetNextAvailable();
		if (dev != null)
		{
			playerDevices.Add (playerId, dev);
			return (dev);
		}
		
		// No more available devices.
		return (null);
	}
	
	void SetupPlayerDevices()
	{
		// Get the available devices.
		int count = InputManager.Devices.Count;
		Debug.Log ("Total Devices: " + count);
		
		int i = 0;
		foreach (InputDevice device in InputManager.Devices)
		{
			// Add the device to the dict of player devices
			device.InUse = true;
			playerDevices.Add(i, device);
			Debug.Log (i + " " + device.Name);
			i++;			
		}
	}
	
	void FixedUpdate()
	{
		InputManager.Update();
	}
	
	// Update is called once per frame
	void Update () 
	{
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
