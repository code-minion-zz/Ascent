// Developed by Mana Khamphanpheng 2013

// Dependencies
using UnityEngine;
using System.Collections;

public class Jump : IAction 
{
	Character owner;
	private const float animationTime = 2.233f;
    //private const float animationTime = 1.367f;
	private const float animationSpeed = 1.5f;
	private float timeElapsed;

	public void Initialise(Character owner)
	{
		this.owner = owner;
	}

	public void StartAbility()
	{
		timeElapsed = 0.0f;
		owner.Animator.PlayAnimation("Jump");
	}

	public void UpdateAbility()
	{
		timeElapsed += Time.deltaTime;

		if (timeElapsed >= animationTime / animationSpeed * 0.6f)
		{
			owner.StopAbility();
		}
	}

	public void EndAbility()
	{
		owner.Animator.StopAnimation("Jump");
	}
}
