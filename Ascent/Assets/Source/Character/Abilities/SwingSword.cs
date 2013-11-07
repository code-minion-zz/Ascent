using UnityEngine;
using System.Collections;

public class SwingSword : IAbility
{
    Character owner;
	private const float animationTime = 1.167f;
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
    }

    public void UpdateAbility()
    {
		
		if (timeElapsed < (animationTime / animationSpeed) * 0.50f) // @ 70% time of the animation
		{
			// TODO: Get damage from the owner and ability formula
			owner.Weapon.SetAttackProperties(10, Character.EDamageType.Physical);
			owner.Weapon.EnableCollision = true;
		}

		timeElapsed += Time.deltaTime;

		if (timeElapsed >= animationTime / animationSpeed)
		{
			owner.StopAbility();
		}
    }

    public void EndAbility()
    {
        owner.Weapon.EnableCollision = false;
    }
}
