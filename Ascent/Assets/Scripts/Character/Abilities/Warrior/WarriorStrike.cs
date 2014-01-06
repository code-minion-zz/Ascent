using UnityEngine;
using System.Collections;

public class WarriorStrike : Action
{
	public override void Initialise(Character owner)
    {
		animationLength = 0.0f;
		animationSpeed = 1.0f;
		coolDownTime = 0.0f;
		cooldownValue = 0.0f;
		currentTime = 0.0f;
		isOnCooldown = false;
		animationTrigger = Name;
		specialCost = 0;

		base.Initialise(owner);
    }

	public override void StartAbility()
	{
		currentTime = 0.0f;
		cooldownValue = CooldownTime;
		isOnCooldown = true;
	}

	public override void UpdateAbility()
	{
		float timeVal = Time.deltaTime * animationSpeed;
		currentTime += timeVal;

		if (currentTime >= Length)
		{
			owner.StopAbility();
		}
	}

	public override void EndAbility()
	{
		currentTime = 0.0f;
	}
}
