// Developed by Mana Khamphanpheng 2013

// Dependencies
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Archer : Enemy
{
    private int shootArrowID;

    public override void Initialise()
    {
        EnemyStats = EnemyStatLoader.Load(EEnemy.Rat, this);

        base.Initialise();

        // Add abilities
        loadout.SetSize(1);

        Ability fireArrow = new ArcherShootArrow();
        shootArrowID = 0;
        loadout.SetAbility(fireArrow, shootArrowID);

        InitialiseAI();
    }

    public void InitialiseAI()
    {
        AIAgent.Initialise(transform);

        AIAgent.SteeringAgent.RotationSpeed = 15.0f;
        AIAgent.SteeringAgent.DistanceToKeepFromTarget = 1.5f;
        motor.MaxSpeed = 0.0f;
        motor.MinSpeed = 0.0f;
        motor.Acceleration = 1.0f;

        AIBehaviour behaviour = null;
        AITrigger trigger = null;

        // Defensive behaviour
        behaviour = AIAgent.MindAgent.AddBehaviour(AIMindAgent.EBehaviour.Defensive);
        {
            trigger = behaviour.AddTrigger();
            trigger.Priority = AITrigger.EConditionalExit.Continue;
            trigger.AddCondition(new AICondition_Sensor(transform, AIAgent.MindAgent, new AISensor_Sphere(transform, AISensor.EType.Closest, AISensor.EScope.Enemies, 10.0f, Vector3.zero)), AITrigger.EConditional.And);
            trigger.OnTriggered += OnTargetInRange;

            trigger = behaviour.AddTrigger();
            trigger.Priority = AITrigger.EConditionalExit.Stop;
            trigger.AddCondition(new AICondition_ActionCooldown(loadout.AbilityBinds[shootArrowID]));
            trigger.OnTriggered += OnShootEnd;

        }

        AIAgent.MindAgent.SetBehaviour(AIMindAgent.EBehaviour.Defensive);
    }

    public void OnTargetInRange()
    {
        AIAgent.TargetCharacter = AIAgent.SensedCharacters[0];

    }

    public void OnShootEnd()
    {
        if (AIAgent.TargetCharacter != null)
        {
            Motor.LookAt(AIAgent.TargetCharacter.transform.position);
        }

        loadout.UseAbility(shootArrowID);
        //AIAgent.TargetCharacter = null;
    }
}
