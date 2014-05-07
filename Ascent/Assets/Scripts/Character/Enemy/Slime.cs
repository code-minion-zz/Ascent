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

	public override void Initialise()
	{
		base.Initialise();

		// Add abilities
		loadout.SetSize(1);

		Ability tackle = new SlimeStrike();
		tackleAbilityID = 0;
		loadout.SetAbility(tackle, tackleAbilityID);

		InitialiseAI();

		deathSequenceEnd = 3.0f;

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
			trigger.AddCondition(new AICondition_Sensor(transform, AIAgent.MindAgent, new AISensor_Sphere(transform, AISensor.EType.FirstFound, AISensor.EScope.Enemies, 2.5f, Vector3.zero)));
			trigger.OnTriggered += StateTransitionToAggressive;
		}

		// Aggressive
		behaviour = AIAgent.MindAgent.AddBehaviour(AIMindAgent.EBehaviour.Aggressive);
		{
			// OnAttacked, Triggers if attacked
			trigger = behaviour.AddTrigger();
			trigger.Operation = AITrigger.EConditionalExit.Continue;
			trigger.AddCondition(new AICondition_Attacked(this));
			trigger.OnTriggered += StateTransitionToAggressive;

			// OnCanUseTackle, triggers if target in range and action off cooldown
			trigger = behaviour.AddTrigger();
			trigger.Operation = AITrigger.EConditionalExit.Continue;
			trigger.AddCondition(new AICondition_ActionCooldown(loadout.AbilityBinds[tackleAbilityID]));
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

		if (lastDamagedBy != null)
		{
			TargetCharacter = lastDamagedBy;
		}
		else if (AIAgent.MindAgent.SensedCharacters != null && AIAgent.MindAgent.SensedCharacters.Count > 0)
		{
			TargetCharacter = AIAgent.MindAgent.SensedCharacters[0];
		}

		base.StateTransitionToAggressive();
	}

	public void UseTackle()
	{
		loadout.UseAbility(tackleAbilityID);
	}
}
