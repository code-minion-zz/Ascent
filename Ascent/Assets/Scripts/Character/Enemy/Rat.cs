// Developed by Mana Khamphanpheng 2013

// Dependencies
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Rat : Enemy 
{
	private int tackleAbilityID;

    public Vector3 move;

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

    public override void Update()
    {
        if(!isDead)
        {
			Vector3 velocity = Vector3.zero;

			if (targetCharacter != null)
			{
				velocity = AIAgent.SteeringAgent.Steer(velocity);
			}
			else if(targetPosition != Vector3.zero)
			{
				velocity = AIAgent.SteeringAgent.Steer(targetPosition);
			}

            motor.Move(velocity);

            AIAgent.MindAgent.Process();
        }

        base.Update();
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

		   // OnWanderEnd, Triggers if time exceeds 2s or target reached.
		   trigger = behaviour.AddTrigger();
		   trigger.Operation = AITrigger.EConditionalExit.Stop;
		   trigger.AddCondition(new AICondition_Timer(2.0f));
		   trigger.AddCondition(new AICondition_ReachedTarget(AIAgent.SteeringAgent), AITrigger.EConditional.Or);
		   trigger.OnTriggered += ChooseNewWanderTarget;

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
		   trigger.Operation = AITrigger.EConditionalExit.Stop;
		   trigger.AddCondition(new AICondition_Attacked(this));
		   trigger.OnTriggered += StateTransitionToAggressive;

		   // OnCanUseTackle, triggers if target in range and action off cooldown
		   trigger = behaviour.AddTrigger();
		   trigger.Operation = AITrigger.EConditionalExit.Stop;
		   trigger.AddCondition(new AICondition_ActionCooldown(loadout.AbilityBinds[tackleAbilityID]));
		   trigger.AddCondition(new AICondition_Sensor(transform, AIAgent.MindAgent, new AISensor_Arc(transform, AISensor.EType.Target, AISensor.EScope.Enemies, 2.5f, 80.0f, Vector3.zero)));
		   trigger.OnTriggered += UseTackle;
	   }

	   StateTransitionToPassive();
   }

   public override void StateTransitionToPassive()
   {
	   AIAgent.SteeringAgent.steerTypes = AISteeringAgent.ESteerTypes.Wander | AISteeringAgent.ESteerTypes.ObstacleAvoidance;
	   ChooseNewWanderTarget();

	   base.StateTransitionToPassive();
   }

   public override void StateTransitionToAggressive()
   {
	   AIAgent.SteeringAgent.steerTypes = AISteeringAgent.ESteerTypes.Arrive | AISteeringAgent.ESteerTypes.ObstacleAvoidance;

	   base.StateTransitionToAggressive();
   }

   public void UseTackle()
   {
	   loadout.UseAbility(tackleAbilityID);
   }

   //public void OnWanderEnd()
   //{
   //    // Choose a new target location
   //    AIAgent.SteeringAgent.SetTargetPosition(containedRoom.NavMesh.GetRandomOrthogonalPositionWithinRadius(transform.position, 7.5f));

   //    // Reset behaviour
   //    AIAgent.MindAgent.ResetBehaviour(AIMindAgent.EBehaviour.Defensive);
	   
   //}

   //public void OnAggressiveEnd()
   //{
   //    motor.MaxSpeed = 3.0f;
   //    motor.MinSpeed = 0.5f;
   //    motor.Acceleration = 1.0f;

   //    AIAgent.SteeringAgent.RemoveTarget();
   //    motor.StopMotion();

   //    AIAgent.MindAgent.SetBehaviour(AIMindAgent.EBehaviour.Defensive);
   //    AIAgent.MindAgent.ResetBehaviour(AIMindAgent.EBehaviour.Defensive);
   //    OnWanderEnd();
   //}

   //public void OnAttacked()
   //{
   //    AIAgent.MindAgent.SetBehaviour(AIMindAgent.EBehaviour.Aggressive);
   //    AIAgent.MindAgent.ResetBehaviour(AIMindAgent.EBehaviour.Aggressive);

   //    if (lastDamagedBy != null)
   //    {
   //        AIAgent.MindAgent.TargetCharacter = lastDamagedBy;
   //    }
   //    else if(AIAgent.MindAgent.SensedCharacters != null && AIAgent.MindAgent.SensedCharacters.Count > 0)
   //    {
   //        AIAgent.MindAgent.TargetCharacter = AIAgent.MindAgent.SensedCharacters[0];
   //    }
   //}

   //public void OnCanUseTackle()
   //{
   //    motor.LookAt(AIAgent.MindAgent.TargetCharacter.transform.position);
   //    AIAgent.SteeringAgent.RemoveTarget();
   //    motor.StopMotion();
   //    loadout.UseAbility(tackleAbilityID);
   //}

   //public override void OnDisable()
   //{
   //    motor.StopMotion();
   //    AIAgent.MindAgent.ResetBehaviour(AIMindAgent.EBehaviour.Aggressive);
   //    AIAgent.MindAgent.SetBehaviour(AIMindAgent.EBehaviour.Defensive);
   //    AIAgent.SteeringAgent.RemoveTarget();
   //    OnWanderEnd();
   //}
}
