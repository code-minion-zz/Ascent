// Developed by Mana Khamphanpheng 2013

// Dependencies
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class EnchantedStatue : Enemy
{
    private AITrigger OnAttackedTrigger;
    private AITrigger OnWanderEndTrigger;
    private AITrigger OnReplicateTrigger;

    private int slamActionID;
    private int stompActionID;
    private int awakenActionID;

    public override void Initialise()
    {
        EnemyStats = EnemyStatLoader.Load(EEnemy.Rat, this);

		base.Initialise();

        // Add abilities
        loadout.SetSize(3);

        Ability slam = new EnchantedStatueSlam();
		slam.Initialise(this);
        slamActionID = 0;
        loadout.SetAbility(slam, slamActionID);

		Ability stomp = new EnchantedStatueStomp();
        stomp.Initialise(this);
        stompActionID = 1;
        loadout.SetAbility(stomp, stompActionID);

		Ability awaken = new EnchantedStatueAwaken();
        awaken.Initialise(this);
        awakenActionID = 2;
        loadout.SetAbility(awaken, awakenActionID);

        InitialiseAI();

		vulnerabilities = EStatus.None;
    }

    public void InitialiseAI()
    {
        motor.MaxSpeed = 1.5f;
        AIAgent.Initialise(transform);
		AIAgent.SteeringAgent.RotationSpeed = 1.5f;
		AIAgent.SteeringAgent.DistanceToKeepFromTarget = 2.5f;

        AIBehaviour behaviour = null;

        // Passive behaviour
        behaviour = AIAgent.MindAgent.AddBehaviour(AIMindAgent.EBehaviour.Passive);
        {
            OnAttackedTrigger = behaviour.AddTrigger();
            OnAttackedTrigger.Priority = AITrigger.EConditionalExit.Stop;
            OnAttackedTrigger.AddCondition(new AICondition_Sensor(transform, AIAgent.MindAgent, new AISensor_Sphere(transform, AISensor.EType.Closest, AISensor.EScope.Enemies, 4.0f, Vector3.zero)));
            OnAttackedTrigger.OnTriggered += OnAwaken;
        }

        behaviour = AIAgent.MindAgent.AddBehaviour(AIMindAgent.EBehaviour.Aggressive);
        {
            OnAttackedTrigger = behaviour.AddTrigger();
			OnAttackedTrigger.Priority = AITrigger.EConditionalExit.Stop;
			OnAttackedTrigger.AddCondition(new AICondition_ActionCooldown(loadout.AbilityBinds[stompActionID]));
			OnAttackedTrigger.AddCondition(new AICondition_SurroundedSensor(transform, AIAgent.MindAgent, 2, new AISensor_Sphere(transform, AISensor.EType.Closest, AISensor.EScope.Enemies, 2.5f, Vector3.zero)));
			OnAttackedTrigger.OnTriggered += OnSurrounded;

			OnAttackedTrigger = behaviour.AddTrigger();
            OnAttackedTrigger.AddCondition(new AICondition_ActionCooldown(loadout.AbilityBinds[slamActionID]));
            //OnAttackedTrigger.AddCondition(new AICondition_Sensor(transform, AIAgent.MindAgent, new AISensor_Arc(transform, AISensor.EType.FirstFound, AISensor.EScope.Enemies, 15.0f, 10.0f, Vector3.back * 3.0f)));
			OnAttackedTrigger.AddCondition(new AICondition_Sensor(transform, AIAgent.MindAgent, new AISensor_Arc(transform, AISensor.EType.FirstFound, AISensor.EScope.Enemies, 5.0f, 25.0f, Vector3.back * 1.5f)));
            OnAttackedTrigger.OnTriggered += OnTargetInSight;
        }

        AIAgent.MindAgent.SetBehaviour(AIMindAgent.EBehaviour.Passive);
    }

    public void OnAwaken()
    {
        List<Character> characters = AIAgent.SensedCharacters;
        AIAgent.TargetCharacter = characters[0];

        AIAgent.MindAgent.SetBehaviour(AIMindAgent.EBehaviour.Aggressive);

        loadout.UseAbility(awakenActionID);
    }

    public void OnSurrounded()
    {
        loadout.UseAbility(stompActionID);
    }

    public void OnTargetInSight()
    {
        loadout.UseAbility(slamActionID);
    }

    public override void OnDisable()
    {
        AIAgent.MindAgent.ResetBehaviour(AIMindAgent.EBehaviour.Aggressive);
        AIAgent.MindAgent.ResetBehaviour(AIMindAgent.EBehaviour.Passive);
        AIAgent.MindAgent.SetBehaviour(AIMindAgent.EBehaviour.Passive);
        AIAgent.SteeringAgent.RemoveTarget();
        motor.StopMotion();
    }
}
