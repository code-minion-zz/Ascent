// Developed by Mana Khamphanpheng 2013

// Dependencies
using UnityEngine;
using System.Collections;

public class WarriorWhirlwind : Action 
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

        // Start the coroutines for handling the animation times.
        //owner.StartCoroutine(UpdateWhirlwindAbility());
        //owner.StartCoroutine(EndWhirlwindAbility());

        
	}

    public override void UpdateAbility()
    {
        base.UpdateAbility();
    }

    //public IEnumerator UpdateWhirlwindAbility()
    //{
    //    // At 1 second into the animation we can do some other things.
    //    yield return new WaitForSeconds(eventAtTime);
    //}

    //public IEnumerator EndWhirlwindAbility()
    //{
    //    // Wait for the end of the animation time.
    //    yield return new WaitForSeconds(animationTime);

    //    owner.StopAbility();
    //}

    public override void EndAbility()
    {
        base.EndAbility();
        owner.Weapon.EnableCollision = false;
        owner.Animator.StopAnimation("Whirlwind");
    }
}
