using UnityEngine;
using System.Collections;

public class SkullTrigger : EnvironmentTrigger
{
	public GroundSkull skull;

	protected override bool HasTriggerBeenMet()
	{
		repeatable = true;

		return (skull.BothEyesOn);
	}
}
