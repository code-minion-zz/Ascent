using UnityEngine;
using System.Collections;

public class GoldItemProperty : SecondaryStatItemProperty
{
	protected float value;
	public float GoldGainBonus
	{
		get { return value; }
	}


    public override void Initialise(){}
    public override void CheckCondition(){}
    public override void DoAction(){}
}
