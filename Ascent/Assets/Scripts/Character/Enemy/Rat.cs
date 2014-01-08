// Developed by Mana Khamphanpheng 2013

// Dependencies
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Rat : Enemy 
{
    // TODO: Move this
    private GameObject bloodSplat;

    //public enum ERatState
    //{
    //    Idle,
    //    Idle2,
    //    Wandering,
    //    Seeking,
    //    ActionAttacking,
    //    ActionCharging,
    //    Flinching,
    //    Dying,
    //    Max
    //}

    private float deathSequenceTime = 0.0f;
    private float deathSequenceEnd = 1.0f;
    private Vector3 deathRotation = Vector3.zero;
    private float deathSpeed = 5.0f;

    private List<Character> collidedTargets = new List<Character>();

    //float[] stateTimes = new float[(int)ERatState.Max] { 0.5f,
    //                                                    0.5f,
    //                                                    2.0f,
    //                                                    0.0f,
    //                                                    0.0f,
    //                                                    0.0f,
    //                                                    0.2f,
    //                                                    0.0f };
    //float timeElapsed = 0.0f;
    //ERatState ratState;
    //Transform target;
    //IList<RAIN.Perception.Sensors.RAINSensor> sensors;
    //GameObject aiObject;
    //Transform childTarget;
    //Vector3 targetPos;

   public override void Update()
    {
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
            //else
            {
				
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
        else
        {
            if (stunDuration > 0.0f)
            {
                stunDuration -= Time.deltaTime;

                if (stunDuration < 0.0f)
                {
                    gameObject.renderer.material.color = originalColour;
                }
            }
		}

		OnMove();
    }

   public override void Initialise()
   {
	   deathRotation = new Vector3(0.0f, 0.0f, transform.eulerAngles.z + 90.0f);

	   // TODO: move this.
	   
	   bloodSplat = Resources.Load("Prefabs/Effects/BloodSplat") as GameObject;

	   // Populate with stats
	   baseStatistics = new BaseStats();
	   baseStatistics.Vitality = (int) ( (((float)health * (float)Game.Singleton.NumberOfPlayers) * 0.80f) / 10.0f);
	  
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

	   //originalColour = gameObject.renderer.material.color;

	   base.Initialise();
   }


   // We want to override the on death for this rat as we have some specific behaviour here.
   public override void OnDeath()
   {
	   base.OnDeath();
	   // Play some cool animation
	   // Maybe even play a sound here
	   // Maybe even drop some loot here

	   // Rat is going to destroy itself now
	   //DestroyObject(this.gameObject);
	   //this.gameObject.SetActive(false);
   }

   public void OnCollisionEnter(Collision other)
   {
	   string tag = other.transform.tag;

	   switch (tag)
	   {
		   case "Hero":
			   {
				   Debug.Log("Hero");
				   Character otherCharacter = other.transform.GetComponent<Character>();
				   CollideWithHero(otherCharacter as Hero, other);
			   }
			   break;
	   }
   }

   //public void OnTriggerEnter(Collider other)
   //{
   //    string tag = other.transform.tag;

   //    switch (tag)
   //    {
   //        case "Hero":
   //            {
   //                Debug.Log("Hero");
   //                Character otherCharacter = other.transform.GetComponent<Character>();
   //                CollideWithHero(otherCharacter as Hero, other);
   //            }
   //            break;
   //    }
   //}

   /// <summary>
   /// When the rat collides with a hero
   /// </summary>
   /// <param name="hero"></param>
   private void CollideWithHero(Hero hero, Collision collision)
   {
	   // If there is an object in this list that is the same
	   // as this object it means we have a double collision.
	   //foreach (Object obj in hero.LastObjectsDamagedBy)
	   //{
	   //    if (obj == this)
	   //        return;
	   //}

	   // Find the ability that this rat last performed
	   //Action ability = null;
	   //if (abilities.Count > 0)
	   //{
	   //    ability = abilities[abilities.Count - 1];
	   //}

	   //if (ability != null && ability.GetType() == typeof(EnemyCharge))
	   //{
	   hero.LastObjectsDamagedBy.Add(this);

	   //EnemyCharge charge = ability as EnemyCharge;
	   // Apply damage value to other character
	   //hero.ApplyDamage(charge.damageValue, charge.damageType);
	   hero.ApplyDamage(3, EDamageType.Physical);

	   //ContactPoint contact = collision.contacts[0];
	   Vector3 direction = (collision.transform.position - transform.position).normalized;
	   Quaternion rot = Quaternion.FromToRotation(Vector3.up, direction);


	   hero.ApplyKnockback(direction, 1.0f);


	   // Apply particle blood splatter and make it a parent of the hero so that it will move with the hero.
	   // TODO: make a pool of these emitters and dont instantiate them on the frame.
	   GameObject bloodSplatter = Instantiate(bloodSplat, collision.transform.position, rot) as GameObject;
	   bloodSplatter.transform.parent = hero.transform;

	   // Heroes are going to take a hit and play the animation.
	   hero.Animator.PlayAnimation("TakeHit");
	   //}

	   // Update our list of collided targets
	   // If a weapon has special properties where it may only be able to hit a number of targets, 
	   // we would check to see if the count is too high before adding to the targets list.
	   collidedTargets.Add(hero);

	  // Debug.Log(this.name + " collides with " + hero);
   }

   private void RemoveCollisions()
   {
	   foreach (Character other in collidedTargets)
	   {
		   // Sanity check to make sure that the other character still exists
		   if (other != null)
		   {
			   // We can remove this collision as it is no longer in effect.
			   other.LastObjectsDamagedBy.Remove(this);
		   }
	   }

	   collidedTargets.Clear();
   }
}
