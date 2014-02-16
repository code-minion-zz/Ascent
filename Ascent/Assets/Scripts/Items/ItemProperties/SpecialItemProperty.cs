using UnityEngine;
using System.Collections;

public class SpecialItemProperty : SecondaryStatItemProperty
{
	public SpecialItemProperty()
    {
        statType = EStats.SpecialPerStrike;
    }

    public override void Initialise() { }
    public override void CheckCondition() { }
    public override void DoAction() { }
}
