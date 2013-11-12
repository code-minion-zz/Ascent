// Developed by Mana Khamphanpheng 2013

// Dependencies
using UnityEngine;
using System.Collections;

public class Jump : Action 
{
	private const float animationTime = 2.233f;
    //private const float animationTime = 1.367f;
	private const float animationSpeed = 1.5f;
	private float timeElapsed;

	public override void Initialise(Character owner)
	{
        base.Initialise(owner);
	}

    public override void StartAbility()
	{
		timeElapsed = 0.0f;
		owner.Animator.PlayAnimation("Jump");
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
		owner.Animator.StopAnimation("Jump");
	}
}
