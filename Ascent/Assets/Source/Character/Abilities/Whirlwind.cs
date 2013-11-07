// Developed by Mana Khamphanpheng 2013

// Dependencies
using UnityEngine;
using System.Collections;

public class Whirlwind : IAbility 
{
	Character owner;
	private const float animationTime = 2f;
	private const float animationSpeed = 1.1f;
	private float timeElapsed;

	public void Initialise(Character owner)
	{
		this.owner = owner;
	}

	public void StartAbility()
	{
		timeElapsed = 0.0f;
		owner.Animator.PlayAnimation("Whirlwind");
		owner.Weapon.EnableCollision = true;
		owner.Weapon.SetAttackProperties(999,Character.EDamageType.Physical);
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
		owner.Weapon.EnableCollision = false;
		owner.Animator.StopAnimation("Whirlwind");
	}
}
