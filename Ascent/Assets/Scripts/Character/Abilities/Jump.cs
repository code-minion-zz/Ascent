// Developed by Mana Khamphanpheng 2013

// Dependencies
using UnityEngine;
using System.Collections;

public class Jump : Action 
{
	public override void Initialise(Character owner)
	{
		animationLength = 2.233f;
		animationSpeed = 1.5f;

        base.Initialise(owner);
	}

    public override void StartAbility()
	{
		currentTime = 0.0f;
		owner.Animator.PlayAnimation("Jump");
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
		owner.Animator.StopAnimation("Jump");
	}
}
