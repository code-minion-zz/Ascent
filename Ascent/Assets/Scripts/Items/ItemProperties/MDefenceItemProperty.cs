using UnityEngine;
using System.Collections;

public class MDefenceItemProperty : SecondaryStatItemProperty
{
	public MDefenceItemProperty()
    {
        statType = EStats.MagicalDefence;
    }

    public override void Initialise(){}
    public override void CheckCondition(){}
    public override void DoAction(){}
}