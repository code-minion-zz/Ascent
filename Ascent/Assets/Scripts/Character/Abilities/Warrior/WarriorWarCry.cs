using UnityEngine;
using System.Collections;

public class WarriorWarCry : Action 
{
    public override void Initialise(Character owner)
	{
		base.Initialise(owner);

        animationLength = 1.167f;
        animationSpeed = 2.0f;
        animationTrigger = "WarCry";
        coolDownTime = 5.0f;
        specialCost = 5;

		Validate();
	}

	public override void StartAbility()
	{
        base.StartAbility();

        PDefenceBuff buff = new PDefenceBuff();
        buff.ApplyBuff(owner, owner, 15.0f);
	}

    public override void UpdateAbility()
    {
        base.UpdateAbility();
    }

    public override void EndAbility()
    {
        base.EndAbility();
    }
}
