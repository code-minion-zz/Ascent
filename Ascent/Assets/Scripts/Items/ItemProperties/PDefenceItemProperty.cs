using UnityEngine;
using System.Collections;

public class PDefenceItemProperty : SecondaryStatItemProperty
{
	public PDefenceItemProperty()
    {
        statType = EStats.PhysicalDefence;
    }

    public override void Initialise() { }
    public override void CheckCondition() { }
    public override void DoAction() { }
}
