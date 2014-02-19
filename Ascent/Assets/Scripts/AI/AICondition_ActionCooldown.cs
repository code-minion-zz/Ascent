using UnityEngine;
using System.Collections;

public class AICondition_ActionCooldown : AICondition
{
	Ability action;

	public AICondition_ActionCooldown(Ability action)
	{
		this.action = action;
	}

	public override bool HasBeenMet()
	{
		return !action.IsOnCooldown;
	}

	public override string ToString()
	{
		return "ActionCD(" + action.AnimationTrigger + "): " + action.RemainingCooldown;
	}
}
