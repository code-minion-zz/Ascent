using UnityEngine;
using System.Collections;

public class ExperienceItemProperty : SecondaryStatItemProperty
{
	protected float value;
	public float ExperienceGainBonus
	{
		get { return value; }
	}

    public override void Initialise() { }
    public override void CheckCondition() { }
    public override void DoAction() { }
}
