// Developed by Kit Chan 2013

// Dependencies
using UnityEngine;
using System.Collections;

public class Charge : IAbility 
{
	Character owner;
	private const float animationTime = 2.333f;
	private const float animationSpeed = 2.0f;
	private float timeElapsed;

	public void Initialise(Character owner)
	{
		this.owner = owner;
	}

	public void StartAbility()
	{
		timeElapsed = 0.0f;
		owner.Animator.PlayAnimation("SwingAttack");
		owner.Weapon.EnableCollision = true;
		owner.Weapon.SetAttackProperties(999,Character.EDamageType.Physical);
	}

	public void UpdateAbility()
	{
		timeElapsed += Time.deltaTime;

        owner.transform.position += owner.transform.forward * Time.fixedDeltaTime * animationSpeed;

		if (timeElapsed >= animationTime / animationSpeed * 0.7f)
		{
			owner.StopAbility();
		}
	}

	public void EndAbility()
	{
		owner.Weapon.EnableCollision = false;
		owner.Animator.StopAnimation("SwingAttack");
	}
}
