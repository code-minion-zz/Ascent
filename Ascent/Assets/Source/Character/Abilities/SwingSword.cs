using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwingSword : IAction
{
    private Character owner;
	private const float animationTime = 1.167f;
	private const float animationSpeed = 3.0f;
    private int damage = 10;
	private float timeElapsed;
    private string animationClip = "SwingAttack";

    public void Initialise(Character owner)
    {
        this.owner = owner;
    }

    public void StartAbility()
    {
		timeElapsed = 0.0f;
        owner.Animator.PlayAnimation(animationClip);
        owner.Animator.Animator.SetFloat("SwordAttackSpeed", animationSpeed);
    }

    public void UpdateAbility()
    {
        if (timeElapsed < (animationTime / animationSpeed) * 0.50f) // @ 70% time of the animation
        {
            // TODO: Get damage from the owner and ability formula
            owner.Weapon.SetAttackProperties(damage, Character.EDamageType.Physical);
            owner.Weapon.EnableCollision = true;

        }

        timeElapsed += Time.deltaTime;

        if (timeElapsed >= (animationTime / animationSpeed))
        {
            owner.StopAbility();
        }
    }

    public void EndAbility()
    {
        owner.Weapon.EnableCollision = false;
        owner.Animator.StopAnimation(animationClip);
    }
}
