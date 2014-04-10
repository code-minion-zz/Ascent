// Developed by Mana Khamphanpheng 2013

// Dependencies
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Slime : Enemy
{
    //private AITrigger OnAttackedTrigger;
    private AITrigger OnWanderEndTrigger;
    //private AITrigger OnReplicateTrigger;

    private int replicateActionID;

    public override void Initialise()
    {
        EnemyStats = EnemyStatLoader.Load(EEnemy.Rat, this);

		base.Initialise();

        // Add abilities
        loadout.SetSize(1);

        Ability replicate = new SlimeReplicate();
        replicate.Initialise(this);
        replicateActionID = 0;
        loadout.SetAbility(replicate, replicateActionID);
       
        InitialiseAI();
    }

    public void InitialiseAI()
    {

        AIAgent.SteeringAgent.RotationSpeed = 50.0f;
        motor.MaxSpeed = 2.0f;
        motor.MinSpeed = 0.3f;
        motor.Acceleration = 0.3f;

        AIBehaviour behaviour = null;

        // Defensive behaviour
        behaviour = AIAgent.MindAgent.AddBehaviour(AIMindAgent.EBehaviour.Defensive);
        {
			//// OnAttacked, Triggers if attacked
			//OnAttackedTrigger = behaviour.AddTrigger();
			//OnAttackedTrigger.Priority = AITrigger.EConditionalExit.Stop;
			//OnAttackedTrigger.AddCondition(new AICondition_Attacked(this));
			//OnAttackedTrigger.AddCondition(new AICondition_ActionCooldown(loadout.AbilityBinds[replicateActionID]), AITrigger.EConditional.And);
			//OnAttackedTrigger.OnTriggered += OnAttacked;

			//OnReplicateTrigger = behaviour.AddTrigger();
			//OnReplicateTrigger.AddCondition(new AICondition_Timer(5.5f, 2.0f, 10.0f));
			//OnReplicateTrigger.OnTriggered += OnAttacked;

            // OnWanderEnd, Triggers if time exceeds 2s or target reached.
            OnWanderEndTrigger = behaviour.AddTrigger();
            OnWanderEndTrigger.Priority = AITrigger.EConditionalExit.Stop;
            OnWanderEndTrigger.AddCondition(new AICondition_Timer(1.5f, 2.0f, 4.0f));
            OnWanderEndTrigger.AddCondition(new AICondition_ReachedTarget(AIAgent.SteeringAgent), AITrigger.EConditional.Or);
            OnWanderEndTrigger.OnTriggered += OnWanderEnd;

        }

        AIAgent.MindAgent.SetBehaviour(AIMindAgent.EBehaviour.Defensive);
        AIAgent.SteeringAgent.SetTargetPosition(containedRoom.NavMesh.GetRandomOrthogonalPositionWithinRadius(transform.position, 7.5f));
    }

    public void OnWanderEnd()
    {
        // Choose a new target location
        AIAgent.SteeringAgent.SetTargetPosition(containedRoom.NavMesh.GetRandomOrthogonalPositionWithinRadius(transform.position, 7.5f));

        // Reset behaviour
        OnWanderEndTrigger.Reset();

    }

    public void OnAttacked()
    {
        loadout.UseAbility(replicateActionID);
        //OnAttackedTrigger.Reset();
        //OnReplicateTrigger.Reset();
    }

    public void OnCollisionEnter(Collision other)
    {
        string tag = other.transform.tag;

        switch (tag)
        {
            case "Hero":
                {
                    Character otherCharacter = other.transform.GetComponent<Character>();

                    if (otherCharacter != null)
                    {
                        if (!IsInState(EStatus.Stun) && !otherCharacter.IsInState(EStatus.Invulnerability))
                        {
                            CollideWithHero(otherCharacter as Hero, other);
                        }
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

        Game.Singleton.EffectFactory.CreateBloodSplatter(collision.transform.position, rot, hero.transform);

		CombatEvaluator combatEvaluator = new CombatEvaluator(this, hero);
        combatEvaluator.Add(new PhysicalDamageProperty(1.0f, 1.0f));
		combatEvaluator.Add(new KnockbackCombatProperty(direction, 10.0f));
		combatEvaluator.Apply();
    }
}
