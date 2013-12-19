using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwingSword : Action
{
    private int damage = 20;

    public override void Initialise(Character owner)
    {
        base.Initialise(owner);
		
		animationSpeed = 3.0f;
        animationLength = 1.167f / animationSpeed;
        animationTrigger = "SwingAttack";
    }

    public override void StartAbility()
    {
        animationLength = 1.167f / animationSpeed;
        owner.Animator.Animator.SetFloat("SwordAttackSpeed", animationSpeed);

        base.StartAbility();

        owner.DerivedStats.CurrentSpecial += 2;
    }

    public override void UpdateAbility()
    {
        base.UpdateAbility();

        if (currentTime >= (animationLength * 0.5f))
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
