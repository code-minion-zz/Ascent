// Developed by Mana Khamphanpheng 2013

// Dependencies
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Abomination : Enemy
{
    private AITrigger ChangeTargetTrigger;

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
        motor.MovementSpeed = 1.5f;
        agent.Initialise(transform);
        agent.SteeringAgent.RotationSpeed = 1.5f;

        AIBehaviour behaviour = null;

        behaviour = agent.MindAgent.AddBehaviour(AIMindAgent.EBehaviour.Defensive);
        {
            AITrigger trigger = behaviour.AddTrigger();
            trigger.Priority = AITrigger.EConditionalExit.Stop;
            trigger.AddCondition(new AICondition_Timer(1.0f, 0.0f, 0.0f));
            trigger.OnTriggered += OnInitialCharge;

            trigger = behaviour.AddTrigger();
            trigger.Priority = AITrigger.EConditionalExit.Stop;
            trigger.AddCondition(new AICondition_ActionEnd(abilities[chargeActionID]));
            trigger.AddCondition(new AICondition_Sensor(transform, agent.MindAgent, new AISensor_Sphere(transform, AISensor.EType.FirstFound, AISensor.EScope.Enemies, 50.0f, Vector3.zero)));
            trigger.OnTriggered += OnInitialChargeEnd;
        }

        behaviour = agent.MindAgent.AddBehaviour(AIMindAgent.EBehaviour.Aggressive);
        {
            ChangeTargetTrigger = behaviour.AddTrigger();
            ChangeTargetTrigger.Priority = AITrigger.EConditionalExit.Stop;
            ChangeTargetTrigger.AddCondition(new AICondition_Timer(6.0f, 0.0f, 5.0f, true));
            ChangeTargetTrigger.AddCondition(new AICondition_Attacked(this));
            ChangeTargetTrigger.OnTriggered += OnCanChangeTarget;

            AITrigger trigger = behaviour.AddTrigger();
            trigger.Priority = AITrigger.EConditionalExit.Stop;
            trigger.AddCondition(new AICondition_ActionCooldown(abilities[chargeActionID]));
            trigger.AddCondition(new AICondition_Sensor(transform, agent.MindAgent, new AISensor_Arc(transform, AISensor.EType.FirstFound, AISensor.EScope.Enemies, 15.0f, 25.0f, Vector3.back * 3.0f)));
            trigger.OnTriggered += OnTargetInSight;

            trigger = behaviour.AddTrigger();
            trigger.Priority = AITrigger.EConditionalExit.Stop;
            trigger.AddCondition(new AICondition_ActionCooldown(abilities[stompActionID]));
            trigger.AddCondition(new AICondition_SurroundedSensor(transform, agent.MindAgent, 1, new AISensor_Sphere(transform, AISensor.EType.Closest, AISensor.EScope.Enemies, 3.5f, Vector3.zero)));
            trigger.OnTriggered += OnSurrounded;

            trigger = behaviour.AddTrigger();
            trigger.Priority = AITrigger.EConditionalExit.Stop;
            trigger.AddCondition(new AICondition_ActionCooldown(abilities[strikeActionID]));
            trigger.AddCondition(new AICondition_Sensor(transform, agent.MindAgent, new AISensor_Arc(transform, AISensor.EType.FirstFound, AISensor.EScope.Enemies, 5.0f, 80.0f, Vector3.back * 1.5f)));
            trigger.OnTriggered += OnCanUseStrike;
        }

        agent.MindAgent.SetBehaviour(AIMindAgent.EBehaviour.Defensive);
    }

    public void OnInitialCharge()
    {
        UseAbility(chargeActionID);
    }

    public void OnInitialChargeEnd()
    {
        agent.TargetCharacter = agent.SensedCharacters[0];
        agent.MindAgent.SetBehaviour(AIMindAgent.EBehaviour.Aggressive);
    }

    public void OnCanChangeTarget()
    {
        agent.TargetCharacter = lastDamagedBy;
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
