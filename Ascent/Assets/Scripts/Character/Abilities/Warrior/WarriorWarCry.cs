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
        cooldownFullDuration = 10.0f;
        specialCost = 10;

		Validate();
	}

	public override void StartAbility()
	{
        base.StartAbility();

        //PDefenceBuff buff = new PDefenceBuff();
		//buff.ApplyStatusEffect(owner, owner, StatusEffect.EApplyMethod.Percentange, 0.5f, 15.0f);

		//SpeedBuff buff2 = new SpeedBuff();
		//buff2.ApplyStatusEffect(owner, owner, StatusEffect.EApplyMethod.Fixed, 5.0f, 15.0f);

		//owner.ApplyStatusEffect(new SleepingDebuff(owner, owner, 5.0f));

		//SpecialBuff buff = new SpecialBuff();
		//buff.ApplyStatusEffect(owner, owner, SecondaryStatBuff.EBuffType.Fixed, 5.0f, 15.0f);

        //HealthRegenBuff buff = new HealthRegenBuff();
        //buff.ApplyStatusEffect(owner, owner, StatusEffect.EBuffType.Fixed, 5, 10, 10.0f);

		//AccuracyBuff buff = new AccuracyBuff();
		//buff.ApplyStatusEffect(owner, owner, SecondaryStatModifierEffect.EApplyMethod.Percentange, 0.5f, 15.0f);

        //owner.ApplyStatusEffect(new StunnedDebuff(owner, owner, 2.0f));

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
