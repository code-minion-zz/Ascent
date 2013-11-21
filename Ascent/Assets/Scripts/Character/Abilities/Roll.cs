// Developed by Mana Khamphanpheng 2013

// Dependencies
using UnityEngine;
using System.Collections;

public class Roll : Action
{
	private const float animationTime = 1.8f;
	private const float animationSpeed = 1.25f;
	private float timeElapsed;

    public override void Initialise(Character owner)
	{
        base.Initialise(owner);
	}

    public override void StartAbility()
	{
		timeElapsed = 0.0f;
		owner.Animator.PlayAnimation("Roll");
	}

    public override void UpdateAbility()
	{
		timeElapsed += Time.deltaTime;

		if (timeElapsed >= animationTime / animationSpeed)
		{
			owner.StopAbility();
		}
	}

    public override void EndAbility()
	{
		owner.Animator.StopAnimation("Roll");
	}
}
