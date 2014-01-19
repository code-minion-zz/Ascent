// Developed by Mana Khamphanpheng 2013

// Dependencies
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Slime : Enemy
{
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

        originalColour = Color.white;

        base.Initialise();

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
            trigger.AddCondition(new AICondition_ActionCooldown(abilities[0]));
            trigger.OnTriggered += OnAttacked;

            // OnWanderEnd, Triggers if time exceeds 2s or target reached.
            trigger = behaviour.AddTrigger();
            trigger.Priority = AITrigger.EConditionalExit.Stop;
            trigger.AddCondition(new AICondition_Timer(3.5f));
            trigger.AddCondition(new AICondition_ReachedTarget(agent.SteeringAgent), AITrigger.EConditional.Or);
            trigger.OnTriggered += OnWanderEnd;
        }

        agent.MindAgent.SetBehaviour(AIMindAgent.EBehaviour.Defensive);
        agent.SteeringAgent.SetTargetPosition(containedRoom.NavMesh.GetRandomOrthogonalPositionWithinRadius(transform.position, 7.5f));
    }

    public void OnWanderEnd()
    {
        // Choose a new target location
        agent.SteeringAgent.SetTargetPosition(containedRoom.NavMesh.GetRandomOrthogonalPositionWithinRadius(transform.position, 7.5f));

        // Reset behaviour
        agent.MindAgent.ResetBehaviour(AIMindAgent.EBehaviour.Defensive);

    }

    public void OnAttacked()
    {
        UseAbility(0);
    }

    public void OnCanUseTackle()
    {
       
    }

    public void OnCollisionEnter(Collision other)
    {
        string tag = other.transform.tag;

        switch (tag)
        {
            case "Hero":
                {
                    Character otherCharacter = other.transform.GetComponent<Character>();

                    if (!IsStunned && !otherCharacter.IsInvulnerable)
                    {
                        CollideWithHero(otherCharacter as Hero, other);
                    }
                }
                break;
        }
    }

    private void CollideWithHero(Hero hero, Collision collision)
    {
        hero.LastDamagedBy = this;

        Vector3 direction = (collision.transform.position - transform.position).normalized;
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, direction);

        Game.Singleton.EffectFactory.CreateBloodSplatter(collision.transform.position, rot, hero.transform, 3.0f);

        hero.ApplyDamage(1, EDamageType.Physical, this);
        hero.ApplyKnockback(direction, 10.0f);
    }
}
