using UnityEngine;
using System.Collections;

public class BlessingOfSpirit : Blessing
{
	public override void ApplyStatusEffect(Character caster, Character target)
	{
		((Hero)target).Lives += 1;

		base.ApplyStatusEffect(caster, target);
	}
}
