using UnityEngine;
using System.Collections;

public class KillEverythingTrigger : EnvironmentTrigger
{
	private Enemy[] enemies;

	public void OnEnable()
	{
		// Up 2 levels (Room->Triggers->This) back to Room. 
		enemies = transform.parent.parent.gameObject.GetComponentsInChildren<Enemy>();
	}

	protected override bool HasTriggerBeenMet()
	{
		if (enemies == null)
		{
			return true;
		}
		if (enemies.Length == 0)
		{
			return true;
		}

		int total = enemies.Length;
		int accum = 0;

		for (int i = 0; i < total; ++i)
		{
			if (enemies[i].IsDead)
			{
				accum++;
			}
		}

		return accum == total;	
	}
}
