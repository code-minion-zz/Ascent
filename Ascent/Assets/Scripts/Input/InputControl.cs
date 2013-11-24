﻿using System;
using UnityEngine;

public class InputControl
{
	public static readonly InputControl Null = new InputControl(InputDevice.InputControlType.Null);

	InputControlState thisState;
	InputControlState lastState;

	public Enum type;

	public InputControl(Enum target)
	{
		type = target;
	}


	public void UpdateWithState(bool state)
	{
		if (IsNull)
		{
			throw new InvalidOperationException("A null control cannot be updated.");
		}

		lastState = thisState;

		if (thisState != state)
		{
			thisState.Set(state);
		}
	}


	public void UpdateWithValue(float value)
	{
		if (IsNull)
		{
			throw new InvalidOperationException("A null control cannot be updated.");
		}

		lastState = thisState;

		if (thisState != value)
		{
			thisState.Set(value);
		}
	}


	public bool State
	{
		get { return thisState.State; }
	}


	public bool LastState
	{
		get { return lastState.State; }
	}


	public float Value
	{
		get { return thisState.Value; }
	}


	public float LastValue
	{
		get { return lastState.Value; }
	}


	public bool HasChanged
	{
		get { return thisState != lastState; }
	}


	public bool IsPressed
	{
		get { return thisState.State; }
	}


	public bool WasPressed
	{
		get { return thisState && !lastState; }
	}


	public bool WasReleased
	{
		get { return !thisState && lastState; }
	}


	public bool IsNull
	{
		get { return this == Null; }
	}


	public bool IsNotNull
	{
		get { return this != Null; }
	}


	public override string ToString()
	{
		return string.Format("[InputControl: Handle={0}, Value={1}]", type.ToString(), Value);
	}


	public static implicit operator bool(InputControl control)
	{
		return control.State;
	}


	public static implicit operator float(InputControl control)
	{
		return control.Value;
	}
}