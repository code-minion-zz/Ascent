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
        Action replicate = new SlimeReplicate();
        replicate.Initialise(this);
        abilities.Add(replicate);
        slamActionID = 0;

        Action stomp = new SlimeReplicate();
        stomp.Initialise(this);
        abilities.Add(stomp);
        stompActionID = 1;

        Action awaken = new SlimeReplicate();
        awaken.Initialise(this);
        abilities.Add(awaken);
        awakenActionID = 2;

        originalColour = Color.white;

        base.Initialise();

        InitialiseAI();
    }

    public void InitialiseAI()
    {
        motor.speed = 1.5f;
        agent.Initialise(transform);

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

            OnAttackedTrigger.AddCondition(new AICondition_ActionCooldown(abilities[slamActionID]));
            OnAttackedTrigger.AddCondition(new AICondition_Sensor(transform, agent.MindAgent, new AISensor_Arc(transform, AISensor.EType.Target, AISensor.EScope.Enemies, 20.0f, 10.0f, Vector3.back * 3.0f)));
            OnAttackedTrigger.OnTriggered += OnTargetInSight;
        }

        agent.MindAgent.SetBehaviour(AIMindAgent.EBehaviour.Passive);
    }

    public void OnAwaken()
    {
        List<Character> characters = agent.SensedCharacters;
        agent.TargetCharacter = characters[0];

        agent.MindAgent.SetBehaviour(AIMindAgent.EBehaviour.Aggressive);

       // UseAbility(awakenActionID);
    }

    public void OnSurrounded()
    {

    }

    public void OnTargetInSight()
    {

    }

    public override void OnEnable()
    {
        //agent.MindAgent.ResetBehaviour(AIMindAgent.EBehaviour.Aggressive);
        //agent.MindAgent.SetBehaviour(AIMindAgent.EBehaviour.Passive);
    }
}
