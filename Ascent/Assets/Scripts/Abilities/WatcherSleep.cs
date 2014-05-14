using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WatcherSleep : Ability
{
	private string wakeAnimationTrigger = "Wake";
	private float wakeAnimationTime = 0.967f;
	private float sleepTime = 2.5f;

	private enum EState
	{
		Sleep,
		Wake
	}

	public override void Initialise(Character owner)
	{
		base.Initialise(owner);

		animationLength = sleepTime + wakeAnimationTime;
		animationSpeed = 1.0f;
		animationTrigger = "Sleep";
		cooldownFullDuration = 0.0f;
		specialCost = 0;
	}

	public override void StartAbility()
	{
		base.StartAbility();

		owner.Animator.PlayAnimation(animationTrigger, true);
	}

	public override void UpdateAbility()
	{
		if (timeElapsedSinceStarting > sleepTime)
		{
			owner.Animator.PlayAnimation(animationTrigger, false);
			owner.Animator.PlayAnimation(wakeAnimationTrigger, true);
		}

		base.UpdateAbility();
	}

	public override void EndAbility()
	{
		owner.Animator.PlayAnimation(wakeAnimationTrigger, false);
		base.EndAbility();
	}
}
