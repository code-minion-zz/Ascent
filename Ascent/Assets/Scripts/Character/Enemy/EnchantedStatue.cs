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
        // Populate with stats
        baseStatistics = new BaseStats();
        baseStatistics.Vitality = (int)((((float)health * (float)Game.Singleton.NumberOfPlayers) * 0.80f) / 10.0f);

        baseStatistics.CurrencyBounty = 1;
        baseStatistics.ExperienceBounty = 50;
        derivedStats = new DerivedStats(baseStatistics);
        derivedStats.Attack = 5;

        // Add abilities
        Action slam = new EnchantedStatueSlam();
		slam.Initialise(this);
		abilities.Add(slam);
        slamActionID = 0;

		Action stomp = new EnchantedStatueStomp();
        stomp.Initialise(this);
        abilities.Add(stomp);
        stompActionID = 1;

		Action awaken = new EnchantedStatueAwaken();
        awaken.Initialise(this);
        abilities.Add(awaken);
        awakenActionID = 2;

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

        // Passive behaviour
        behaviour = agent.MindAgent.AddBehaviour(AIMindAgent.EBehaviour.Passive);
        {
            OnAttackedTrigger = behaviour.AddTrigger();
            OnAttackedTrigger.Priority = AITrigger.EConditionalExit.Stop;
            OnAttackedTrigger.AddCondition(new AICondition_Sensor(transform, agent.MindAgent, new AISensor_Sphere(transform, AISensor.EType.Closest, AISensor.EScope.Enemies, 4.0f, Vector3.zero)));
            OnAttackedTrigger.OnTriggered += OnAwaken;
        }

        behaviour = agent.MindAgent.AddBehaviour(AIMindAgent.EBehaviour.Aggressive);
        {
            OnAttackedTrigger = behaviour.AddTrigger();
			OnAttackedTrigger.Priority = AITrigger.EConditionalExit.Stop;
			OnAttackedTrigger.AddCondition(new AICondition_ActionCooldown(abilities[stompActionID]));
			OnAttackedTrigger.AddCondition(new AICondition_SurroundedSensor(transform, agent.MindAgent, 1, new AISensor_Sphere(transform, AISensor.EType.Closest, AISensor.EScope.Enemies, 2.5f, Vector3.zero)));
			OnAttackedTrigger.OnTriggered += OnSurrounded;

			OnAttackedTrigger = behaviour.AddTrigger();
            OnAttackedTrigger.AddCondition(new AICondition_ActionCooldown(abilities[slamActionID]));
            OnAttackedTrigger.AddCondition(new AICondition_Sensor(transform, agent.MindAgent, new AISensor_Arc(transform, AISensor.EType.FirstFound, AISensor.EScope.Enemies, 15.0f, 10.0f, Vector3.back * 3.0f)));
            OnAttackedTrigger.OnTriggered += OnTargetInSight;
        }

        agent.MindAgent.SetBehaviour(AIMindAgent.EBehaviour.Passive);
    }

    public void OnAwaken()
    {
        List<Character> characters = agent.SensedCharacters;
        agent.TargetCharacter = characters[0];

        agent.MindAgent.SetBehaviour(AIMindAgent.EBehaviour.Aggressive);

       UseAbility(awakenActionID);
    }

    public void OnSurrounded()
    {
		UseAbility(stompActionID);
    }

    public void OnTargetInSight()
    {
		UseAbility(slamActionID);
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
