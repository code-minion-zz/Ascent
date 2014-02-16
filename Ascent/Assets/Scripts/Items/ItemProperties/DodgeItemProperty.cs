using UnityEngine;
using System.Collections;

public class DodgeItemProperty : SecondaryStatItemProperty
{
	public DodgeItemProperty()
    {
        statType = EStats.DodgeChance;
    }

    public override void Initialise() { }
    public override void CheckCondition() { }
    public override void DoAction() { }
}
