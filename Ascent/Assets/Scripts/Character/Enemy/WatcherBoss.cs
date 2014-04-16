// Developed by Mana Khamphanpheng 2013

// Dependencies
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class WatcherBoss : Enemy
{
    private int lazerID;
    private int magicMissilesID;

    public Transform mainEye;
    public Transform[] eyes;

    public override void Initialise()
    {
        base.Initialise();

        // Add abilities
        loadout.SetSize(1);

        Ability ability = new WatcherMagicMissile();
        magicMissilesID = 0;
        loadout.SetAbility(ability, magicMissilesID);

        InitialiseAI();
    }

    public void InitialiseAI()
    {
        AIBehaviour behaviour = null;
        AITrigger trigger = null;

        //// Defensive behaviour
        //behaviour = AIAgent.MindAgent.AddBehaviour(AIMindAgent.EBehaviour.Passive);
        //{
        //    // OnAttacked, Triggers if attacked
        //    trigger = behaviour.AddTrigger();
        //    trigger.Operation = AITrigger.EConditionalExit.Stop;
        //    trigger.AddCondition(new AICondition_Attacked(this));
        //    trigger.OnTriggered += StateTransitionToAggressive;

        //    trigger = behaviour.AddTrigger();
        //    trigger.Operation = AITrigger.EConditionalExit.Stop;
        //    trigger.AddCondition(new AICondition_Sensor(transform, AIAgent.MindAgent, new AISensor_Sphere(transform, AISensor.EType.FirstFound, AISensor.EScope.Enemies, 2.5f, Vector3.zero)));
        //    trigger.OnTriggered += StateTransitionToAggressive;
        //}

        // Aggressive
        behaviour = AIAgent.MindAgent.AddBehaviour(AIMindAgent.EBehaviour.Aggressive);
        {
            // OnCanUseTackle, triggers if target in range and action off cooldown
            trigger = behaviour.AddTrigger();
            trigger.Operation = AITrigger.EConditionalExit.Continue;
            trigger.AddCondition(new AICondition_ActionCooldown(loadout.AbilityBinds[magicMissilesID]));
            //trigger.AddCondition(new AICondition_Sensor(transform, AIAgent.MindAgent, new AISensor_Arc(transform, AISensor.EType.Target, AISensor.EScope.Enemies, 2.5f, 80.0f, Vector3.zero)), AITrigger.EConditional.And);
            trigger.OnTriggered += UseTackle;
        }

        StateTransitionToAggressive();
    }

    public override void StateTransitionToPassive()
    {
        base.StateTransitionToPassive();
    }

    public override void StateTransitionToAggressive()
    {
        base.StateTransitionToAggressive();
    }

    public void UseTackle()
    {
        loadout.UseAbility(magicMissilesID);
    }
}
