// Developed by Mana Khamphanpheng 2013

// Dependencies
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Abomination : Enemy
{
    private AITrigger OnAttackedTrigger;
    private AITrigger OnWanderEndTrigger;
    private AITrigger OnReplicateTrigger;

    private int strikeActionID;
    private int stompActionID;
    private int chargeActionID;

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
        Action strike = new AbominationStrike();
        strike.Initialise(this);
        abilities.Add(strike);
        strikeActionID = 0;

        Action stomp = new AbominationStomp();
        stomp.Initialise(this);
        abilities.Add(stomp);
        stompActionID = 1;

        Action charge = new AbominationCharge();
        charge.Initialise(this);
        abilities.Add(charge);
        chargeActionID = 2;

        originalColour = Color.white;

        base.Initialise();

        InitialiseAI();

        canBeDebuffed = false;
        canBeStunned = false;
        canBeInterrupted = false;
        canBeKnockedBack = false;
    }

    public void InitialiseAI()
    {
        motor.speed = 1.5f;
        agent.Initialise(transform);
        agent.SteeringAgent.RotationSpeed = 1.5f;

        AIBehaviour behaviour = null;

        behaviour = agent.MindAgent.AddBehaviour(AIMindAgent.EBehaviour.Aggressive);
        {
            OnAttackedTrigger = behaviour.AddTrigger();
            OnAttackedTrigger.Priority = AITrigger.EConditionalExit.Stop;
            OnAttackedTrigger.AddCondition(new AICondition_ActionCooldown(abilities[chargeActionID]));
            OnAttackedTrigger.AddCondition(new AICondition_Sensor(transform, agent.MindAgent, new AISensor_Arc(transform, AISensor.EType.FirstFound, AISensor.EScope.Enemies, 15.0f, 25.0f, Vector3.back * 3.0f)));
            OnAttackedTrigger.OnTriggered += OnTargetInSight;

            OnAttackedTrigger = behaviour.AddTrigger();
            OnAttackedTrigger.Priority = AITrigger.EConditionalExit.Stop;
            OnAttackedTrigger.AddCondition(new AICondition_ActionCooldown(abilities[stompActionID]));
            OnAttackedTrigger.AddCondition(new AICondition_SurroundedSensor(transform, agent.MindAgent, 1, new AISensor_Sphere(transform, AISensor.EType.Closest, AISensor.EScope.Enemies, 3.5f, Vector3.zero)));
            OnAttackedTrigger.OnTriggered += OnSurrounded;

            OnAttackedTrigger = behaviour.AddTrigger();
            OnAttackedTrigger.Priority = AITrigger.EConditionalExit.Stop;
            OnAttackedTrigger.AddCondition(new AICondition_ActionCooldown(abilities[strikeActionID]));
            OnAttackedTrigger.AddCondition(new AICondition_Sensor(transform, agent.MindAgent, new AISensor_Arc(transform, AISensor.EType.FirstFound, AISensor.EScope.Enemies, 5.0f, 80.0f, Vector3.back * 1.5f)));
            OnAttackedTrigger.OnTriggered += OnCanUseStrike;
        }

        agent.MindAgent.SetBehaviour(AIMindAgent.EBehaviour.Aggressive);
    }

    public void OnSurrounded()
    {
        UseAbility(stompActionID);
    }

    public void OnTargetInSight()
    {
        UseAbility(chargeActionID);
    }

    public void OnCanUseStrike()
    {
        UseAbility(strikeActionID);
    }

    public override void OnDisable()
    {
        agent.MindAgent.ResetBehaviour(AIMindAgent.EBehaviour.Aggressive);
        agent.MindAgent.ResetBehaviour(AIMindAgent.EBehaviour.Passive);
        agent.MindAgent.SetBehaviour(AIMindAgent.EBehaviour.Passive);
        agent.SteeringAgent.RemoveTarget();
        motor.StopMotion();
    }
}
