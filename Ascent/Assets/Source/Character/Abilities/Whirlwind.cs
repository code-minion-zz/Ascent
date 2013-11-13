// Developed by Mana Khamphanpheng 2013

// Dependencies
using UnityEngine;
using System.Collections;

public class Whirlwind : Action 
{
	//Character owner;
	private const float animationTime = 2.333f;
	private const float animationSpeed = 2.0f;
	private float timeElapsed;

	public override void Initialise(Character owner)
	{
		base.Initialise(owner);
	}

	public override void StartAbility()
	{
		timeElapsed = 0.0f;
		owner.Animator.PlayAnimation("Whirlwind");
		owner.Weapon.EnableCollision = true;
		owner.Weapon.SetAttackProperties(999,Character.EDamageType.Physical);
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
		owner.Weapon.EnableCollision = false;
		//owner.Animator.StopAnimation("Whirlwind");
	}
}
