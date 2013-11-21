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
		//if (owner == null)
		//{
		//    owner = ai.Body.GetComponentInChildren<Character>();
		//}

		//// Changes AI state to acting
		//if (ai.WorkingMemory.ItemExists("acting") == false)
		//{
		//    Debug.LogError("Current AI does not have \"acting\" variable: " + ai);
		//}

		//ai.WorkingMemory.SetItem<bool>("acting", true);

		//IList<RAIN.Perception.Sensors.RAINSensor> sensors = ai.Senses.Sensors;

		//foreach(RAIN.Perception.Sensors.RAINSensor sensor in sensors)
		//{
		//    Debug.Log(sensor.GetType());
		//    Debug.Log( sensor.SensorName);
		//    Debug.Log(sensor.Matches);
		//    //sensor.MatchAspect(aspect);
		//    //sensor.MatchAspectName("heroVisual");
           
		//}

		//// Turn on this ability.
		//owner.UseAbility("EnemyCharge");  
     
		////ai.Motor.MoveTo

		ai.Motor.MoveTo(Vector3.zero);

		base.Start(ai);
	}

	public override ActionResult Execute(AI ai)
	{
		ai.Motor.Move();

        return ActionResult.SUCCESS;
	}

	public override void Stop(AI ai)
	{
		base.Stop(ai);
	}
}