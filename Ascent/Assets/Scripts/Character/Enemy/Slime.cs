// Developed by Mana Khamphanpheng 2013

// Dependencies
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Slime : Enemy
{
	private int tackleAbilityID;

	public Vector3 move;

	private AICondition_Timer retargetTimer;
	private AICondition_Timer abilityTimer;

	public override void Initialise()
	{
		base.Initialise();

		// Add abilities
		loadout.SetSize(1);

		Ability tackle = new RatTackle();
		tackleAbilityID = 0;
		loadout.SetAbility(tackle, tackleAbilityID);

		InitialiseAI();
	}

	public void InitialiseAI()
	{
		AIBehaviour behaviour = null;
		AITrigger trigger = null;

		// Defensive behaviour
		behaviour = AIAgent.MindAgent.AddBehaviour(AIMindAgent.EBehaviour.Passive);
		{
			// OnAttacked, Triggers if attacked
			trigger = behaviour.AddTrigger();
			trigger.Operation = AITrigger.EConditionalExit.Stop;
			trigger.AddCondition(new AICondition_Attacked(this));
			trigger.OnTriggered += StateTransitionToAggressive;

			trigger = behaviour.AddTrigger();
			trigger.Operation = AITrigger.EConditionalExit.Stop;
			trigger.AddCondition(new AICondition_Sensor(transform, AIAgent.MindAgent, new AISensor_Sphere(transform, AISensor.EType.FirstFound, AISensor.EScope.Enemies, 5.0f, Vector3.zero)));
			trigger.OnTriggered += StateTransitionToAggressive;
		}

		// Aggressive
		behaviour = AIAgent.MindAgent.AddBehaviour(AIMindAgent.EBehaviour.Aggressive);
		{
			// OnAttacked, Triggers if attacked
			trigger = behaviour.AddTrigger();
			trigger.Operation = AITrigger.EConditionalExit.Stop;
			trigger.AddCondition(new AICondition_Attacked(this));
			trigger.OnTriggered += StateTransitionToAggressive;

			trigger = behaviour.AddTrigger();
			trigger.Operation = AITrigger.EConditionalExit.Stop;
			retargetTimer = new AICondition_Timer(2.0f, 4.0f);
			trigger.AddCondition(retargetTimer);
			trigger.AddCondition(new AICondition_Sensor(transform, AIAgent.MindAgent, new AISensor_Sphere(transform, AISensor.EType.Closest, AISensor.EScope.Enemies, 5.0f, Vector3.zero)));
			trigger.OnTriggered += StateTransitionToAggressive;

			// OnCanUseTackle, triggers if target in range and action off cooldown
			trigger = behaviour.AddTrigger();
			trigger.Operation = AITrigger.EConditionalExit.Continue;
			abilityTimer = new AICondition_Timer(1.0f, 1.5f);
			trigger.AddCondition(abilityTimer);
			trigger.AddCondition(new AICondition_Sensor(transform, AIAgent.MindAgent, new AISensor_Arc(transform, AISensor.EType.Target, AISensor.EScope.Enemies, 2.5f, 80.0f, Vector3.zero)), AITrigger.EConditional.And);
			trigger.OnTriggered += UseTackle;
		}

		StateTransitionToPassive();
	}

	public override void StateTransitionToPassive()
	{
		AIAgent.SteeringAgent.steerTypes = AISteeringAgent.ESteerTypes.Wander | AISteeringAgent.ESteerTypes.ObstacleAvoidance;

		base.StateTransitionToPassive();
	}

	public override void StateTransitionToAggressive()
	{
		AIAgent.SteeringAgent.steerTypes = AISteeringAgent.ESteerTypes.Arrive | AISteeringAgent.ESteerTypes.ObstacleAvoidance;

		if (AIAgent.MindAgent.SensedCharacters != null && AIAgent.MindAgent.SensedCharacters.Count > 0)
		{
			retargetTimer.Reset();
			TargetCharacter = AIAgent.MindAgent.SensedCharacters[0];
		}
		else if (lastDamagedBy != null)
		{
			TargetCharacter = lastDamagedBy;
		}

		base.StateTransitionToAggressive();
	}

	public void UseTackle()
	{
		abilityTimer.Reset();
		loadout.UseAbility(tackleAbilityID);
	}
}
