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
        EnemyStats = EnemyStatLoader.Load(EEnemy.Abomination, this);

        base.Initialise();

        // Add abilities
        loadout.SetSize(3);

        Ability strike = new AbominationStrike();
        strike.Initialise(this);
        strikeActionID = 0;
        loadout.SetAbility(strike, strikeActionID);

        Ability stomp = new AbominationStomp();
        stomp.Initialise(this);
        stompActionID = 1;
        loadout.SetAbility(stomp, stompActionID);


        Ability charge = new AbominationCharge();
        charge.Initialise(this);
        chargeActionID = 2;
        loadout.SetAbility(charge, chargeActionID);


        InitialiseAI();

		vulnerabilities = EStatus.None;
    }

    public void InitialiseAI()
    {
        motor.MaxSpeed = 1.5f;
        AIAgent.Initialise(transform);
        AIAgent.SteeringAgent.RotationSpeed = 2.5f;

        AIBehaviour behaviour = null;

        behaviour = AIAgent.MindAgent.AddBehaviour(AIMindAgent.EBehaviour.Defensive);
        {
            AITrigger trigger = behaviour.AddTrigger();
            trigger.Priority = AITrigger.EConditionalExit.Continue;
            trigger.AddCondition(new AICondition_Timer(1.0f, 0.0f, 0.0f));
            trigger.OnTriggered += OnInitialCharge;

            trigger = behaviour.AddTrigger();
            trigger.Priority = AITrigger.EConditionalExit.Stop;
            trigger.AddCondition(new AICondition_ActionEnd(loadout.AbilityBinds[chargeActionID]));
            trigger.AddCondition(new AICondition_Sensor(transform, AIAgent.MindAgent, new AISensor_Sphere(transform, AISensor.EType.FirstFound, AISensor.EScope.Enemies, 100.0f, Vector3.zero)));
            trigger.OnTriggered += OnInitialChargeEnd;
        }

        behaviour = AIAgent.MindAgent.AddBehaviour(AIMindAgent.EBehaviour.Aggressive);
        {
            ChangeTargetTrigger = behaviour.AddTrigger();
            ChangeTargetTrigger.Priority = AITrigger.EConditionalExit.Stop;
            ChangeTargetTrigger.AddCondition(new AICondition_Timer(6.0f, 0.0f, 5.0f, true));
            ChangeTargetTrigger.AddCondition(new AICondition_ActionEnd(loadout.AbilityBinds[chargeActionID]), AITrigger.EConditional.Or);
            //ChangeTargetTrigger.AddCondition(new AICondition_Sensor(transform, agent.MindAgent, new AISensor_Sphere(transform, AISensor.EType.FirstFound, AISensor.EScope.Enemies, 100.0f, Vector3.zero)));
            ChangeTargetTrigger.AddCondition(new AICondition_Attacked(this));
            ChangeTargetTrigger.OnTriggered += OnCanChangeTarget;

            AITrigger trigger = behaviour.AddTrigger();
            trigger.Priority = AITrigger.EConditionalExit.Stop;
            trigger.AddCondition(new AICondition_ActionCooldown(loadout.AbilityBinds[chargeActionID]));
            trigger.AddCondition(new AICondition_Sensor(transform, AIAgent.MindAgent, new AISensor_Arc(transform, AISensor.EType.FirstFound, AISensor.EScope.Enemies, 15.0f, 25.0f, Vector3.back * 3.0f)));
            trigger.OnTriggered += OnTargetInSight;

            trigger = behaviour.AddTrigger();
            trigger.Priority = AITrigger.EConditionalExit.Stop;
            trigger.AddCondition(new AICondition_ActionCooldown(loadout.AbilityBinds[stompActionID]));
            trigger.AddCondition(new AICondition_SurroundedSensor(transform, AIAgent.MindAgent, 2, new AISensor_Sphere(transform, AISensor.EType.Closest, AISensor.EScope.Enemies, 3.5f, Vector3.zero)));
            trigger.OnTriggered += OnSurrounded;

            trigger = behaviour.AddTrigger();
            trigger.Priority = AITrigger.EConditionalExit.Stop;
            trigger.AddCondition(new AICondition_ActionCooldown(loadout.AbilityBinds[strikeActionID]));
            trigger.AddCondition(new AICondition_Sensor(transform, AIAgent.MindAgent, new AISensor_Arc(transform, AISensor.EType.FirstFound, AISensor.EScope.Enemies, 5.0f, 80.0f, Vector3.back * 1.5f)));
            trigger.OnTriggered += OnCanUseStrike;
        }

        AIAgent.MindAgent.SetBehaviour(AIMindAgent.EBehaviour.Defensive);
    }

    public void OnInitialCharge()
    {
        loadout.UseAbility(chargeActionID);
    }

    public void OnInitialChargeEnd()
    {
        AIAgent.TargetCharacter = AIAgent.SensedCharacters[0];
        AIAgent.MindAgent.SetBehaviour(AIMindAgent.EBehaviour.Aggressive);
    }

    public void OnCanChangeTarget()
    {
        AIAgent.TargetCharacter = lastDamagedBy;
        //agent.TargetCharacter = agent.SensedCharacters[0];
    }

    public void OnSurrounded()
    {
        loadout.UseAbility(stompActionID);
    }

    public void OnTargetInSight()
    {
        loadout.UseAbility(chargeActionID);
    }

    public void OnCanUseStrike()
    {
        loadout.UseAbility(strikeActionID);
    }

    public override void OnDisable()
    {
        AIAgent.MindAgent.ResetBehaviour(AIMindAgent.EBehaviour.Aggressive);
        //AIAgent.MindAgent.ResetBehaviour(AIMindAgent.EBehaviour.Passive);
        //AIAgent.MindAgent.SetBehaviour(AIMindAgent.EBehaviour.Passive);
        AIAgent.SteeringAgent.RemoveTarget();
        motor.StopMotion();
    }
}
