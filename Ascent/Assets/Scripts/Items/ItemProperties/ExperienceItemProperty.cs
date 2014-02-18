using UnityEngine;
using System.Collections;

public class ExperienceItemProperty : ItemProperty
{
	protected StatusEffect.EApplyMethod applyMethod;
	public StatusEffect.EApplyMethod ApplyMethod
	{
		get { return applyMethod; }
		set { applyMethod = value; }
	}

	protected float buffValue;
	public float ExperienceGainBonus
	{
		get { return buffValue; }
		set { buffValue = value; }
	}

    public override void Initialise() { }
    public override void CheckCondition() { }
    public override void DoAction() { }
}
