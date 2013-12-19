// Developed by Mana Khamphanpheng 2013

// Dependencies
using UnityEngine;
using System.Collections;

public class Roll : Action
{

    public override void Initialise(Character owner)
	{
		animationLength = 1.8f;
		animationSpeed = 1.25f;
        base.Initialise(owner);
	}

    public override void StartAbility()
	{
		currentTime = 0.0f;
		owner.Animator.PlayAnimation("Roll");
	}

    public override void UpdateAbility()
	{
		currentTime += Time.deltaTime;

		if (currentTime >= animationLength / animationSpeed)
		{
			owner.StopAbility();
		}
	}

    public override void EndAbility()
	{
		owner.Animator.StopAnimation("Roll");
	}
}
