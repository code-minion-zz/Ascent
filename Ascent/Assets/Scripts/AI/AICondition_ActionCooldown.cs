using UnityEngine;
using System.Collections;

public class AICondition_ActionCooldown : AICondition
{
	Action action;

	public AICondition_ActionCooldown(Action action)
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
