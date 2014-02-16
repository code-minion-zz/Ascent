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
        base.Initialise();

		EnemyStats = EnemyStatLoader.Load(EEnemy.Rat, this);

		// Add abilities
		Action tackle = new RatTackle();
		tackle.Initialise(this);
		abilities.Add(tackle);

		InitialiseAI();
	}

   public void InitialiseAI()
   {
	   AIAgent.Initialise(transform);

       AIAgent.SteeringAgent.RotationSpeed = 15.0f;
	   AIAgent.SteeringAgent.DistanceToKeepFromTarget = 1.25f;
       motor.MaxSpeed = 3.0f;
       motor.MinSpeed = 0.5f;
       motor.Acceleration = 1.0f;

	   AIBehaviour behaviour = null;
	   AITrigger trigger = null;

	   // Defensive behaviour
	   behaviour = AIAgent.MindAgent.AddBehaviour(AIMindAgent.EBehaviour.Defensive);
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
		   trigger.AddCondition(new AICondition_ReachedTarget(AIAgent.SteeringAgent), AITrigger.EConditional.Or);
		   trigger.OnTriggered += OnWanderEnd;
	   }

       // Aggressive
       behaviour = AIAgent.MindAgent.AddBehaviour(AIMindAgent.EBehaviour.Aggressive);
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
           trigger.AddCondition(new AICondition_Sensor(transform, AIAgent.MindAgent, new AISensor_Arc(transform, AISensor.EType.Target, AISensor.EScope.Enemies, 2.5f, 80.0f, Vector3.zero)));
           trigger.OnTriggered += OnCanUseTackle;

           trigger = behaviour.AddTrigger();
           trigger.Priority = AITrigger.EConditionalExit.Stop;
           trigger.AddCondition(new AICondition_Timer(7.0f));
           trigger.OnTriggered += OnAggressiveEnd;
       }

	   AIAgent.MindAgent.SetBehaviour(AIMindAgent.EBehaviour.Defensive);
	   AIAgent.SteeringAgent.SetTargetPosition(containedRoom.NavMesh.GetRandomOrthogonalPositionWithinRadius(transform.position, 7.5f));
   }

   public void OnWanderEnd()
   {
	   // Choose a new target location
	   AIAgent.SteeringAgent.SetTargetPosition(containedRoom.NavMesh.GetRandomOrthogonalPositionWithinRadius(transform.position, 7.5f));

	   // Reset behaviour
	   AIAgent.MindAgent.ResetBehaviour(AIMindAgent.EBehaviour.Defensive);
	   
   }

   public void OnAggressiveEnd()
   {
	   AIAgent.SteeringAgent.RemoveTarget();
	   motor.StopMotion();

	   AIAgent.MindAgent.SetBehaviour(AIMindAgent.EBehaviour.Defensive);
	   AIAgent.MindAgent.ResetBehaviour(AIMindAgent.EBehaviour.Defensive);
	   OnWanderEnd();

	   motor.MaxSpeed = 3.0f;
   }

   public void OnAttacked()
   {
	   AIAgent.MindAgent.SetBehaviour(AIMindAgent.EBehaviour.Aggressive);
	   AIAgent.MindAgent.ResetBehaviour(AIMindAgent.EBehaviour.Aggressive);
	   AIAgent.TargetCharacter = lastDamagedBy;
	   motor.MaxSpeed = 5.0f;
   }

   public void OnCanUseTackle()
   {
	   UseAbility(0);
   }

   public override void OnDisable()
   {
       motor.StopMotion();
       AIAgent.MindAgent.ResetBehaviour(AIMindAgent.EBehaviour.Aggressive);
       AIAgent.MindAgent.SetBehaviour(AIMindAgent.EBehaviour.Defensive);
       AIAgent.SteeringAgent.RemoveTarget();
       OnWanderEnd();
       motor.MaxSpeed = 3.0f;
   }
}
