using UnityEngine;
using System.Collections;

public class EnterRoomTrigger : EnvironmentTrigger 
{
	bool initialised = false;

	void OnEnable()
	{
		initialised = true;
	}

	protected override bool  HasTriggerBeenMet()
	{
		return true;
	}
}
