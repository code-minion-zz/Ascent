using UnityEngine;
using System.Collections;

public class SwitchTrigger : EnvironmentTrigger
{
	public SwitchPanel[] switches;

	protected override bool HasTriggerBeenMet()
	{
		repeatable = true;

		int total = switches.Length;
		int accum = 0;

		for (int i = 0; i < total; ++i)
		{
			if (switches[i].IsDown)
			{
				accum++;
			}
		}
		return (accum == total);
	}
}
