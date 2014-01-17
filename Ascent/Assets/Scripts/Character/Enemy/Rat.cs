// Developed by Mana Khamphanpheng 2013

// Dependencies
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Rat : Enemy 
{
    private float deathSequenceTime = 0.0f;
    private float deathSequenceEnd = 1.0f;
    private Vector3 deathRotation = Vector3.zero;
    private float deathSpeed = 5.0f;

    public AIAgent agent;

   public override void Initialise()
	{
		deathRotation = new Vector3(0.0f, 0.0f, transform.eulerAngles.z + 90.0f);

		// Populate with stats
		baseStatistics = new BaseStats();
		baseStatistics.Vitality = (int)((((float)health * (float)Game.Singleton.NumberOfPlayers) * 0.80f) / 10.0f);

		baseStatistics.CurrencyBounty = 1;
		baseStatistics.ExperienceBounty = 50;
		derivedStats = new DerivedStats(baseStatistics);
		derivedStats.Attack = 5;

		// Add abilities
		Action tackle = new EnemyTackle();
		tackle.Initialise(this);
		abilities.Add(tackle);

		// Add abilities
		Action charge = new EnemyCharge();
		charge.Initialise(this);
		abilities.Add(charge);

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
		   // On Attacked trigger
		   // Triggers if attacked
		   trigger = behaviour.AddTrigger();
		   trigger.Priority = AITrigger.EConditionalExit.Stop;
		   trigger.AddCondition(new AICondition_Attacked(this));
		   trigger.OnTriggered += OnAttacked;

		   // On Wander end trigger
		   // Triggers IG time exceeds 2s or target reached.
		   trigger = behaviour.AddTrigger();
		   trigger.Priority = AITrigger.EConditionalExit.Stop;
		   trigger.AddCondition(new AICondition_Timer(2.0f));
		   trigger.AddCondition(new AICondition_ReachedTarget(agent.SteeringAgent), AITrigger.EConditional.Or);
		   trigger.OnTriggered += OnWanderEnd;
	   }

	   // Aggressive
	   behaviour = agent.MindAgent.AddBehaviour(AIMindAgent.EBehaviour.Aggressive);
	   {
		   // On Attacked trigger
		   // Triggers if attacked
		   trigger = behaviour.AddTrigger();
		   trigger.Priority = AITrigger.EConditionalExit.Stop;
		   trigger.AddCondition(new AICondition_Attacked(this));
		   trigger.OnTriggered += OnAttacked;
	   }

	   agent.MindAgent.SetBehaviour(AIMindAgent.EBehaviour.Defensive);
   }

   public override void OnEnable()
   {
       base.OnEnable();
       agent.SteeringAgent.SetTargetPosition(containedRoom.NavMesh.GetRandomOrthogonalPositionWithinRadius(transform.position, 7.5f));
   }

   public override void Update()
   {
        agent.MindAgent.Process();
        agent.SteeringAgent.Process();

        base.Update();

        if (isDead)
        {
            deathSequenceTime += Time.deltaTime;

            // When the rat dies we want to make him kinematic and disabled the collider
            // this is so we can walk over the dead body.
            if (this.transform.rigidbody.isKinematic == false)
            {
                this.transform.rigidbody.isKinematic = true;
                this.transform.collider.enabled = false;
            }

            // Death sequence end
            if (deathSequenceTime >= deathSequenceEnd)
		    {
                // When the death sequence has finished we want to make this object not active
                // This ensures that he will dissapear and not be visible in the game but we can still re-use him later.
                deathSequenceTime = 0.0f;

                this.gameObject.SetActive(false);
			    DestroyObject(this.gameObject);
            }
				
            // During death sequence we can do some thing in here
            // For now we will rotate the rat on the z axis.
            this.transform.eulerAngles = Vector3.Lerp(this.transform.eulerAngles, deathRotation, Time.deltaTime * deathSpeed);

            // If the rotation is done early we can end the sequence.
            if (this.transform.eulerAngles == deathRotation)
            {
                deathSequenceTime = deathSequenceEnd;
            }
        }
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
	   agent.MindAgent.SetBehaviour(AIMindAgent.EBehaviour.Aggressive);
	   agent.SteeringAgent.SetTarget(lastDamagedBy);
   }

   // We want to override the on death for this rat as we have some specific behaviour here.
   public override void OnDeath()
   {
	   base.OnDeath();
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

   /// <summary>
   /// When the rat collides with a hero
   /// </summary>
   /// <param name="hero"></param>
   private void CollideWithHero(Hero hero, Collision collision)
   {
       hero.LastDamagedBy = this;
	   hero.ApplyDamage(3, EDamageType.Physical);

	   //ContactPoint contact = collision.contacts[0];
	   Vector3 direction = (collision.transform.position - transform.position).normalized;
	   Quaternion rot = Quaternion.FromToRotation(Vector3.up, direction);

	   hero.ApplyKnockback(direction, 1.0f);

       Game.Singleton.EffectFactory.CreateBloodSplatter(collision.transform.position, rot, hero.transform, 3.0f);

	   // Heroes are going to take a hit and play the animation.
       // TODO: Make this a chance based scenario. The hero should check also if he can take a hit as well.
	   //hero.Animator.PlayAnimation("TakeHit");
       HeroAnimator heroAnim = hero.Animator as HeroAnimator;
       heroAnim.TakeHit = true;
   }
}
