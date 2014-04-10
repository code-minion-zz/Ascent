// Developed by Mana Khamphanpheng 2013

// Dependencies
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Imp : Enemy
{
    private int chargeActionID;

    public override void Initialise()
    {
        EnemyStats = EnemyStatLoader.Load(EEnemy.Rat, this);

		base.Initialise();

        // Add abilities
        loadout.SetSize(1);

        Ability charge = new ImpStrike();
        charge.Initialise(this);
        chargeActionID = 0;
        loadout.SetAbility(charge, chargeActionID);

        InitialiseAI();
    }

    public void InitialiseAI()
    {
		AIAgent.SteeringAgent.RotationSpeed = 15.0f;
		AIAgent.SteeringAgent.DistanceToKeepFromTarget = 1.5f;
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
            trigger.AddCondition(new AICondition_Timer(4.0f));
            trigger.AddCondition(new AICondition_ReachedTarget(AIAgent.SteeringAgent), AITrigger.EConditional.Or);
            trigger.OnTriggered += OnWanderEnd;

            trigger = behaviour.AddTrigger();
            trigger.Priority = AITrigger.EConditionalExit.Stop;
            trigger.AddCondition(new AICondition_ActionCooldown(loadout.AbilityBinds[chargeActionID]));
            trigger.AddCondition(new AICondition_Sensor(transform, AIAgent.MindAgent, new AISensor_Arc(transform, AISensor.EType.FirstFound, AISensor.EScope.Enemies, 2.5f, 80.0f, Vector3.zero)));
            trigger.OnTriggered += OnCanUseCharge;
        }

        // Aggressive
        behaviour = AIAgent.MindAgent.AddBehaviour(AIMindAgent.EBehaviour.Aggressive);
        {
            // OnAttacked, Triggers if attacked
            trigger = behaviour.AddTrigger();
            trigger.Priority = AITrigger.EConditionalExit.Stop;
            trigger.AddCondition(new AICondition_Attacked(this));
            trigger.OnTriggered += OnAttacked;

            trigger = behaviour.AddTrigger();
            trigger.Priority = AITrigger.EConditionalExit.Stop;
            trigger.AddCondition(new AICondition_ActionCooldown(loadout.AbilityBinds[chargeActionID]));
            trigger.AddCondition(new AICondition_Sensor(transform, AIAgent.MindAgent, new AISensor_Arc(transform, AISensor.EType.FirstFound, AISensor.EScope.Enemies, 2.5f, 80.0f, Vector3.zero)));
            trigger.OnTriggered += OnCanUseCharge;

			trigger = behaviour.AddTrigger();
            trigger.Priority = AITrigger.EConditionalExit.Stop;
            trigger.AddCondition(new AICondition_HP(enemyStats, AICondition.EType.Percentage, AICondition.ESign.EqualOrLess, 0.15f));
            trigger.OnTriggered += OnLowHP;
        }

        behaviour = AIAgent.MindAgent.AddBehaviour(AIMindAgent.EBehaviour.Evasive);
        {
            trigger = behaviour.AddTrigger();
            trigger.Priority = AITrigger.EConditionalExit.Stop;
            trigger.AddCondition(new AICondition_Attacked(this));
        }

        AIAgent.MindAgent.SetBehaviour(AIMindAgent.EBehaviour.Defensive);
        AIAgent.SteeringAgent.SetTargetPosition(containedRoom.NavMesh.GetRandomPositionWithinRadius(transform.position, 7.5f));
        //AIAgent.SteeringAgent.RotationSpeed = 5.0f;
       // AIAgent.SteeringAgent.CloseEnoughRange = .5f;
        //motor.MaxSpeed = 2.0f;
    }

    public void OnWanderEnd()
    {
        // Choose a new target location
        AIAgent.SteeringAgent.SetTargetPosition(containedRoom.NavMesh.GetRandomPositionWithinRadius(transform.position, 7.5f));

        // Reset behaviour
        AIAgent.MindAgent.ResetBehaviour(AIMindAgent.EBehaviour.Defensive);

    }

    public void OnAttacked()
    {
        AIAgent.TargetCharacter = lastDamagedBy;
		motor.LookAt(lastDamagedBy.transform.position);
    }

    public void OnCanUseCharge()
    {
        loadout.UseAbility(chargeActionID);
        AIAgent.TargetCharacter = AIAgent.SensedCharacters[0];

        AIAgent.MindAgent.SetBehaviour(AIMindAgent.EBehaviour.Aggressive);
    }

    public void OnLowHP()
    {
        AIAgent.MindAgent.SetBehaviour(AIMindAgent.EBehaviour.Evasive);
        AIAgent.SteeringAgent.IsRunningAway = true;
		AIAgent.SteeringAgent.RotationSpeed = 30.0f;
		Motor.MaxSpeed = 7.5f;
		Motor.Acceleration = 7.5f;
    }

    public override void OnDisable()
    {
        motor.StopMotion();
        AIAgent.MindAgent.ResetBehaviour(AIMindAgent.EBehaviour.Aggressive);
        AIAgent.MindAgent.SetBehaviour(AIMindAgent.EBehaviour.Defensive);
        AIAgent.SteeringAgent.IsRunningAway = false;
        AIAgent.SteeringAgent.RemoveTarget();
        OnWanderEnd();
    }
}
