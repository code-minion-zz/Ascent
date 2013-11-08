// Developed by Mana Khamphanpheng 2013

// Dependencies
using UnityEngine;
using System.Collections;

public class Roll : IAction
{
	Character owner;
	private const float animationTime = 1.8f;
	private const float animationSpeed = 1.25f;
	private float timeElapsed;

	public void Initialise(Character owner)
	{
		this.owner = owner;
	}

	public void StartAbility()
	{
		timeElapsed = 0.0f;
		owner.Animator.PlayAnimation("Roll");
	}

	public void UpdateAbility()
	{
		timeElapsed += Time.deltaTime;

		if (timeElapsed >= animationTime / animationSpeed)
		{
			owner.StopAbility();
		}
	}

	public void EndAbility()
	{
		owner.Animator.StopAnimation("Roll");
	}
}
