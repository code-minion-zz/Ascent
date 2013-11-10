// Developed by Mana Khamphanpheng 2013

// Dependencies
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Core;
using RAIN.Action;
using RAIN.Representation;

[RAINAction]
public class UseAbilityAction : RAINAction
{
    Character owner;

	public UseAbilityAction()
	{
		actionName = "UseAbilityAction";
	}

	public override void Start(AI ai)
	{
        if (owner == null)
        {
            owner = ai.Body.GetComponentInChildren<Character>();
        }

        owner.UseAbility("ABC");

        // Changes AI state to acting

        //if (ai.WorkingMemory.ItemExists("acting") == false)
        //{
        //    Debug.LogError("Current AI does not have \"acting\" variable: " + ai);
        //}

        //ai.WorkingMemory.SetItem<bool>("acting", true);

		base.Start(ai);
	}

	public override ActionResult Execute(AI ai)
	{
		return ActionResult.SUCCESS;
	}

	public override void Stop(AI ai)
	{
		base.Stop(ai);
	}
}