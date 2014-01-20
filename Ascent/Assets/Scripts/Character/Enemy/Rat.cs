// Developed by Mana Khamphanpheng 2013

// Dependencies
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Rat : Enemy 
{
   public override void Initialise()
	{
		// Populate with stats
		baseStatistics = new BaseStats();
		baseStatistics.Vitality = (int)((((float)health * (float)Game.Singleton.NumberOfPlayers) * 0.80f) / 10.0f);

		baseStatistics.CurrencyBounty = 1;
		baseStatistics.ExperienceBounty = 50;
		derivedStats = new DerivedStats(baseStatistics);
		derivedStats.Attack = 5;

		// Add abilities
		Action tackle = new EnemyTackle();
		tackle.Initialise(this);
		abilities.Add(tackle);

		originalColour = Color.white;

		base.Initialise();

		InitialiseAI();
	}

   public void InitialiseAI()
   {
	   agent.Initialise(transform);

	   AIBehaviour behaviour = null;
	   AITrigger trigger = null;

	   // Defensive behaviour
	   behaviour = agent.MindAgent.AddBehaviour(AIMindAgent.EBehaviour.Defensive);
	   {
		   // OnAttacked, Triggers if attacked
		   trigger = behaviour.AddTrigger();
		   trigger.Priority = AITrigger.EConditionalExit.Stop;
		   trigger.AddCondition(new AICondition_Attacked(this));
		   trigger.OnTriggered += OnAttacked;

		   // OnWanderEnd, Triggers if time exceeds 2s or target reached.
		   trigger = behaviour.AddTrigger();
		   trigger.Priority = AITrigger.EConditionalExit.Stop;
		   trigger.AddCondition(new AICondition_Timer(2.0f));
		   trigger.AddCondition(new AICondition_ReachedTarget(agent.SteeringAgent), AITrigger.EConditional.Or);
		   trigger.OnTriggered += OnWanderEnd;
	   }

	   // Aggressive
	   behaviour = agent.MindAgent.AddBehaviour(AIMindAgent.EBehaviour.Aggressive);
	   {
		   // OnAttacked, Triggers if attacked
		   trigger = behaviour.AddTrigger();
		   trigger.Priority = AITrigger.EConditionalExit.Stop;
		   trigger.AddCondition(new AICondition_Attacked(this));
		   trigger.OnTriggered += OnAttacked;

		   // OnCanUseTackle, triggers if target in range and action off cooldown
		   trigger = behaviour.AddTrigger();
		   trigger.Priority = AITrigger.EConditionalExit.Stop;
		   trigger.AddCondition(new AICondition_ActionCooldown(abilities[0]));
		   trigger.AddCondition(new AICondition_Sensor(transform, agent.MindAgent, new AISensor_Arc(transform, AISensor.EType.Target, AISensor.EScope.Enemies, 2.5f, 80.0f, Vector3.zero)));
		   trigger.OnTriggered += OnCanUseTackle;

		   trigger = behaviour.AddTrigger();
		   trigger.Priority = AITrigger.EConditionalExit.Stop;
		   trigger.AddCondition(new AICondition_Timer(2.0f));
		   trigger.OnTriggered += OnAggressiveEnd;
	   }

	   agent.MindAgent.SetBehaviour(AIMindAgent.EBehaviour.Defensive);
	   agent.SteeringAgent.SetTargetPosition(containedRoom.NavMesh.GetRandomOrthogonalPositionWithinRadius(transform.position, 7.5f));
   }

   public override void Update()
   {
        base.Update();

		//transform.forward = new Vector3(Game.Singleton.Players[0].Input.LeftStickX, 0.0f, Game.Singleton.Players[0].Input.LeftStickY);
    }

   public void OnWanderEnd()
   {
	   // Choose a new target location
	   agent.SteeringAgent.SetTargetPosition(containedRoom.NavMesh.GetRandomOrthogonalPositionWithinRadius(transform.position, 7.5f));

	   // Reset behaviour
	   agent.MindAgent.ResetBehaviour(AIMindAgent.EBehaviour.Defensive);
	   
   }

   public void OnAggressiveEnd()
   {
	   agent.SteeringAgent.RemoveTarget();
	   motor.StopMotion();

	   agent.MindAgent.SetBehaviour(AIMindAgent.EBehaviour.Defensive);
	   agent.MindAgent.ResetBehaviour(AIMindAgent.EBehaviour.Defensive);
	   OnWanderEnd();

	   motor.speed = 3.0f;
   }

   public void OnAttacked()
   {
	   agent.MindAgent.SetBehaviour(AIMindAgent.EBehaviour.Aggressive);
	   agent.MindAgent.ResetBehaviour(AIMindAgent.EBehaviour.Aggressive);
	   agent.TargetCharacter = lastDamagedBy;
	   motor.speed = 5.0f;
   }

	

   public void OnCanUseTackle()
   {
	   
	   //agent.MindAgent.SetBehaviour(AIMindAgent.EBehaviour.Aggressive);
	   //agent.TargetCharacter = lastDamagedBy;
	   UseAbility(0);
	   
	   //agent.enabled = false;
   }
}
