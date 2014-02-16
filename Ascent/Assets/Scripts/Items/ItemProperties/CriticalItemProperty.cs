using UnityEngine;
using System.Collections;

public class CriticalItemProperty : SecondaryStatItemProperty
{
	public CriticalItemProperty()
    {
        statType = EStats.CriticalHitChance;
    }

    public override void Initialise() { }
    public override void CheckCondition() { }
    public override void DoAction() { }
}
