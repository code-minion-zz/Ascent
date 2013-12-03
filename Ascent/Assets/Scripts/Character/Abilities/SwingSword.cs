using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwingSword : Action
{
	private const float animationSpeed = 3.0f;
    private int damage = 10;

    public override void Initialise(Character owner)
    {
        base.Initialise(owner);
        animationLength = 1.167f / animationSpeed;
        animationTrigger = "SwingAttack";
    }

    public override void StartAbility()
    {
        animationLength = 1.167f / animationSpeed;
        owner.Animator.Animator.SetFloat("SwordAttackSpeed", animationSpeed);

        base.StartAbility();
    }

    public override void UpdateAbility()
    {
        if (currentTime <= (animationLength * 0.9f))
        {
            owner.Weapon.SetAttackProperties(damage, Character.EDamageType.Physical);
            owner.Weapon.EnableCollision = true;
        }
    }

    public override void EndAbility()
    {
        base.EndAbility();
        owner.Weapon.EnableCollision = false;
    }
}
