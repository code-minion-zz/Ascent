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
        Vector3 velocity = AIAgent.SteeringAgent.Steer();
        motor.Move(velocity);
    }

   public void InitialiseAI()
   {
       AIAgent.SteeringAgent.RotationSpeed = 15.0f;
	   AIAgent.SteeringAgent.DistanceToKeepFromTarget = 2.5f;
       motor.MaxSpeed = 3.0f;
       motor.MinSpeed = 0.5f;
       motor.Acceleration = 1.0f;

       //AIBehaviour behaviour = null;
       //AITrigger trigger = null;

       //// Defensive behaviour
       //behaviour = AIAgent.MindAgent.AddBehaviour(AIMindAgent.EBehaviour.Defensive);
       //{
       //    // OnAttacked, Triggers if attacked
       //    trigger = behaviour.AddTrigger();
       //    trigger.Priority = AITrigger.EConditionalExit.Stop;
       //    trigger.AddCondition(new AICondition_Attacked(this));
       //    trigger.OnTriggered += OnAttacked;

       //    // OnWanderEnd, Triggers if time exceeds 2s or target reached.
       //    trigger = behaviour.AddTrigger();
       //    trigger.Priority = AITrigger.EConditionalExit.Stop;
       //    trigger.AddCondition(new AICondition_Timer(2.0f));
       //    trigger.AddCondition(new AICondition_ReachedTarget(AIAgent.SteeringAgent), AITrigger.EConditional.Or);
       //    trigger.OnTriggered += OnWanderEnd;

       //    trigger = behaviour.AddTrigger();
       //    trigger.Priority = AITrigger.EConditionalExit.Stop;
       //    trigger.AddCondition(new AICondition_Sensor(transform, AIAgent.MindAgent, new AISensor_Sphere(transform, AISensor.EType.FirstFound, AISensor.EScope.Enemies, 2.5f,  Vector3.zero)));
       //    trigger.OnTriggered += OnAttacked;
       //}

       //// Aggressive
       //behaviour = AIAgent.MindAgent.AddBehaviour(AIMindAgent.EBehaviour.Aggressive);
       //{
       //    // OnAttacked, Triggers if attacked
       //    trigger = behaviour.AddTrigger();
       //    trigger.Priority = AITrigger.EConditionalExit.Stop;
       //    trigger.AddCondition(new AICondition_Attacked(this));
       //    trigger.OnTriggered += OnAttacked;

       //    // OnCanUseTackle, triggers if target in range and action off cooldown
       //    trigger = behaviour.AddTrigger();
       //    trigger.Priority = AITrigger.EConditionalExit.Stop;
       //    trigger.AddCondition(new AICondition_ActionCooldown(loadout.AbilityBinds[tackleAbilityID]));
       //    trigger.AddCondition(new AICondition_Sensor(transform, AIAgent.MindAgent, new AISensor_Arc(transform, AISensor.EType.Target, AISensor.EScope.Enemies, 2.5f, 80.0f, Vector3.zero)));
       //    trigger.OnTriggered += OnCanUseTackle;

       //    trigger = behaviour.AddTrigger();
       //    trigger.Priority = AITrigger.EConditionalExit.Stop;
       //    trigger.AddCondition(new AICondition_ActionEnd(loadout.AbilityBinds[tackleAbilityID]));
       //    trigger.OnTriggered += OnAggressiveEnd;

       //    trigger = behaviour.AddTrigger();
       //    trigger.Priority = AITrigger.EConditionalExit.Stop;
       //    trigger.AddCondition(new AICondition_Timer(7.0f));
       //    trigger.OnTriggered += OnAggressiveEnd;
       //}

       //AIAgent.MindAgent.SetBehaviour(AIMindAgent.EBehaviour.Defensive);
       //AIAgent.SteeringAgent.SetTargetPosition(containedRoom.NavMesh.GetRandomOrthogonalPositionWithinRadius(transform.position, 7.5f));
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
   //        AIAgent.TargetCharacter = lastDamagedBy;
   //    }
   //    else if(AIAgent.SensedCharacters != null && AIAgent.SensedCharacters.Count > 0)
   //    {
   //        AIAgent.TargetCharacter = AIAgent.SensedCharacters[0];
   //    }
   //}

   //public void OnCanUseTackle()
   //{
   //    motor.LookAt(AIAgent.TargetCharacter.transform.position);
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
