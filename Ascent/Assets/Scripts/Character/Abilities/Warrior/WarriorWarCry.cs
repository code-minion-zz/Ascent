using UnityEngine;
using System.Collections;

public class WarriorWarCry : Action 
{
    public override void Initialise(Character owner)
	{
		base.Initialise(owner);

        animationLength = 1.333f;
        animationSpeed = 1.5f;
        animationTrigger = "WarCry";
        cooldownDurationMax = 10.0f;
        specialCost = 10;

		Validate();
	}

	public override void StartAbility()
	{
        base.StartAbility();

        PDefenceBuff buff = new PDefenceBuff();
        buff.ApplyBuff(owner, owner, 15.0f);

        ((HeroAnimator)Owner.Animator).PlayCombatAction((int)Warrior.ECombatAnimation.Warcry, Warrior.ECombatAnimation.Warcry.ToString());
	}

    public override void UpdateAbility()
    {
        base.UpdateAbility();
    }

    public override void EndAbility()
    {
        ((HeroAnimator)Owner.Animator).CombatAnimationEnd();
        base.EndAbility();
    }
}
