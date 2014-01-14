// Developed by Mana Khamphanpheng 2013

// Dependencies
using UnityEngine;
using System.Collections;

public class Whirlwind : Action 
{
	//Character owner;
	//private const float animationTime = 2.333f;
    //private const float eventAtTime = 1.0f;
	//private const float animationSpeed = 2.0f;

	public override void Initialise(Character owner)
	{
		base.Initialise(owner);

        animationLength = 2.333f;
        animationSpeed = 2.0f;
        coolDownTime = 5.0f;
        animationTrigger = "Whirlwind";
        specialCost = 5;
	}

	public override void StartAbility()
	{
        base.StartAbility();

		//owner.Weapon.EnableCollision = true;
		//owner.Weapon.SetAttackProperties(20, Character.EDamageType.Physical);
	}

    public override void UpdateAbility()
    {
        base.UpdateAbility();
    }

    public override void EndAbility()
    {
        base.EndAbility();
        owner.Animator.StopAnimation("Whirlwind");
    }
}
