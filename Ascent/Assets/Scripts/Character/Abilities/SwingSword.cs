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
        owner.Animator.Animator.SetFloat("SwordAttackSpeed", animationSpeed);
    }

    public override void StartAbility()
    {
        base.StartAbility();
    }

    public override void UpdateAbility()
    {
        if (currentTime <= (animationLength * 0.5f))
        {
            owner.Weapon.SetAttackProperties(damage, Character.EDamageType.Physical);
            owner.Weapon.EnableCollision = true;
        }
        else if (currentTime >= (animationLength))
        {
            owner.StopAbility();
        }
    }

    public override void EndAbility()
    {
        base.EndAbility();
        owner.Weapon.EnableCollision = false;
    }
}
