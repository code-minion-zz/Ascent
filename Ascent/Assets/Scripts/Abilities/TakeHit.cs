using UnityEngine;
using System.Collections;

public class TakeHit : Ability 
{
	public float durationBeforeCanBeInterupted;

    public override void Initialise(Character owner)
    {
        base.Initialise(owner);
    }

    public override void StartAbility()
    {
        base.StartAbility();
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
