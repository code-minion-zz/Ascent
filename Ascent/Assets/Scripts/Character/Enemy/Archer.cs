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
        AIBehaviour behaviour = null;
        AITrigger trigger = null;

        // Defensive behaviour
        behaviour = AIAgent.MindAgent.AddBehaviour(AIMindAgent.EBehaviour.Defensive);
        {
			trigger = behaviour.AddTrigger("Select closest target in range.");
			trigger.Operation = AITrigger.EConditionalExit.Continue;
			trigger.AddCondition(new AICondition_Sensor(transform, AIAgent.MindAgent, new AISensor_Sphere(transform, AISensor.EType.Closest, AISensor.EScope.Enemies, 10.0f, Vector3.zero)), AITrigger.EConditional.And);
			trigger.OnTriggered += TargetInRange;

			trigger = behaviour.AddTrigger("Fire at target.");
			trigger.Operation = AITrigger.EConditionalExit.Stop;
			trigger.AddCondition(new AICondition_Timer(0.5f, 1.5f));
			trigger.OnTriggered += OnShootEnd;
        }

        AIAgent.MindAgent.SetBehaviour(AIMindAgent.EBehaviour.Defensive);

		AIAgent.SteeringAgent.steerTypes = AISteeringAgent.ESteerTypes.FleeInRange | AISteeringAgent.ESteerTypes.ObstacleAvoidance;
    }

	public override void Update()
	{
		base.Update();

		if (TargetCharacter != null && !isDead)
		{
			float speed = 1.5f;
			Vector3 targetDir = TargetCharacter.transform.position - transform.position;
			float step = speed * Time.deltaTime;
			Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
			transform.rotation = Quaternion.LookRotation(newDir);
		}
	}

    public void TargetInRange()
    {
        //AIAgent.MindAgent.TargetCharacter = AIAgent.MindAgent.SensedCharacters[0];

		TargetCharacter = AIAgent.MindAgent.SensedCharacters[0];
    }

    public void OnShootEnd()
    {
        if (AIAgent.MindAgent.TargetCharacter != null)
        {
            Motor.LookAt(AIAgent.MindAgent.TargetCharacter.transform.position);
        }

        loadout.UseAbility(shootArrowID);

		AIAgent.MindAgent.ResetBehaviour(AIMindAgent.EBehaviour.Defensive);
    }
}
