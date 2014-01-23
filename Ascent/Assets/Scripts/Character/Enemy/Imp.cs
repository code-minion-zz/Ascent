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
		base.Initialise();

        // Populate with stats
        baseStatistics = new BaseStats();
        baseStatistics.Vitality = (int)((((float)health * (float)Game.Singleton.NumberOfPlayers) * 0.80f) / 10.0f);

        baseStatistics.CurrencyBounty = 1;
        baseStatistics.ExperienceBounty = 50;
        derivedStats = new DerivedStats(baseStatistics);
        derivedStats.Attack = 5;

        // Add abilities
        Action charge = new ImpStrike();
        charge.Initialise(this);
        abilities.Add(charge);
        chargeActionID = 0;

        originalColour = Color.white;

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
            trigger.AddCondition(new AICondition_Timer(4.0f));
            trigger.AddCondition(new AICondition_ReachedTarget(agent.SteeringAgent), AITrigger.EConditional.Or);
            trigger.OnTriggered += OnWanderEnd;

            trigger = behaviour.AddTrigger();
            trigger.Priority = AITrigger.EConditionalExit.Stop;
            trigger.AddCondition(new AICondition_ActionCooldown(abilities[chargeActionID]));
            trigger.AddCondition(new AICondition_Sensor(transform, agent.MindAgent, new AISensor_Arc(transform, AISensor.EType.FirstFound, AISensor.EScope.Enemies, 2.5f, 80.0f, Vector3.zero)));
            trigger.OnTriggered += OnCanUseCharge;
        }

        // Aggressive
        behaviour = agent.MindAgent.AddBehaviour(AIMindAgent.EBehaviour.Aggressive);
        {
            // OnAttacked, Triggers if attacked
            trigger = behaviour.AddTrigger();
            trigger.Priority = AITrigger.EConditionalExit.Stop;
            trigger.AddCondition(new AICondition_Attacked(this));
            trigger.OnTriggered += OnAttacked;

            trigger = behaviour.AddTrigger();
            trigger.Priority = AITrigger.EConditionalExit.Stop;
            trigger.AddCondition(new AICondition_ActionCooldown(abilities[chargeActionID]));
            trigger.AddCondition(new AICondition_Sensor(transform, agent.MindAgent, new AISensor_Arc(transform, AISensor.EType.FirstFound, AISensor.EScope.Enemies, 2.5f, 80.0f, Vector3.zero)));
            trigger.OnTriggered += OnCanUseCharge;

			trigger = behaviour.AddTrigger();
            trigger.Priority = AITrigger.EConditionalExit.Stop;
            trigger.AddCondition(new AICondition_HP(DerivedStats, AICondition.EType.Percentage, AICondition.ESign.EqualOrLess, 0.25f));
            trigger.OnTriggered += OnLowHP;
        }

        behaviour = agent.MindAgent.AddBehaviour(AIMindAgent.EBehaviour.Evasive);
        {
            trigger = behaviour.AddTrigger();
            trigger.Priority = AITrigger.EConditionalExit.Stop;
            trigger.AddCondition(new AICondition_Attacked(this));
        }

        agent.MindAgent.SetBehaviour(AIMindAgent.EBehaviour.Defensive);
        agent.SteeringAgent.SetTargetPosition(containedRoom.NavMesh.GetRandomPositionWithinRadius(transform.position, 7.5f));
        agent.SteeringAgent.RotationSpeed = 5.0f;
        agent.SteeringAgent.CloseEnoughRange = .5f;
        motor.MovementSpeed = 2.0f;
    }

    public void OnWanderEnd()
    {
        // Choose a new target location
        agent.SteeringAgent.SetTargetPosition(containedRoom.NavMesh.GetRandomPositionWithinRadius(transform.position, 7.5f));

        // Reset behaviour
        agent.MindAgent.ResetBehaviour(AIMindAgent.EBehaviour.Defensive);

    }

    public void OnAttacked()
    {
        agent.TargetCharacter = lastDamagedBy;
    }

    public void OnCanUseCharge()
    {
        UseAbility(chargeActionID);
        agent.TargetCharacter = agent.SensedCharacters[0];

        agent.MindAgent.SetBehaviour(AIMindAgent.EBehaviour.Aggressive);
    }

    public void OnLowHP()
    {
        agent.MindAgent.SetBehaviour(AIMindAgent.EBehaviour.Evasive);
        agent.SteeringAgent.IsRunningAway = true;
    }

    public override void OnDisable()
    {
        motor.StopMotion();
        agent.MindAgent.ResetBehaviour(AIMindAgent.EBehaviour.Aggressive);
        agent.MindAgent.SetBehaviour(AIMindAgent.EBehaviour.Defensive);
        agent.SteeringAgent.IsRunningAway = false;
        agent.SteeringAgent.RemoveTarget();
        OnWanderEnd();
    }
}
