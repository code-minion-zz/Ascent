using UnityEngine;
using System.Collections;

public class AttackItemProperty : SecondaryStatItemProperty
{
	public AttackItemProperty()
    {
        statType = EStats.Attack;
    }

    public override void Initialise(){}
    public override void CheckCondition(){}
    public override void DoAction(){}
}
