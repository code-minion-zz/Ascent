// Developed by Mana Khamphanpheng 2013

// Dependencies
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Core;
using RAIN.Action;

[RAINAction]
public class RayDetectAction : RAINAction
{
	public RayDetectAction()
	{
		actionName = "RayDetectAction";
	}

	public override void Start(AI ai)
	{

		base.Start(ai);
	}

	public override ActionResult Execute(AI ai)
	{
        ai.Motor.MoveTo(Vector3.zero);
		return ActionResult.RUNNING;
	}

	public override void Stop(AI ai)
	{
		base.Stop(ai);
	}
}