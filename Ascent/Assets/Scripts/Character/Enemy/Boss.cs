// Developed by Mana Khamphanpheng 2013

// Dependencies
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using RAIN.Core;
using RAIN.Action;
using RAIN.Representation;

public class Boss : Enemy
{
    // TODO: Move this
    private GameObject bloodSplat;

    public enum ERatState
    {
        Idle,
        Idle2,
        Wandering,
        Seeking,
        ActionAttacking,
        ActionCharging,
        Flinching,
        Dying,
        Max
    }

    private float deathSequenceTime = 0.0f;
    private float deathSequenceEnd = 1.0f;
    private Vector3 deathRotation = Vector3.zero;
    private float deathSpeed = 5.0f;
    private List<Character> collidedTargets = new List<Character>();

    float[] stateTimes = new float[(int)ERatState.Max] { 0.5f,
														0.5f,
														2.0f,
														0.0f,
														0.0f,
														0.0f,
														0.2f,
														0.0f };
    float timeElapsed = 0.0f;
    ERatState ratState;
    Transform target;
    IList<RAIN.Perception.Sensors.RAINSensor> sensors;
    GameObject aiObject;
    Transform childTarget;
    Vector3 targetPos;


    public override void Update()
    {
        base.Update();

        RemoveCollisions();

        if (isDead)
        {
            deathSequenceTime += Time.deltaTime;

            // When the rat dies we want to make him kinematic and disabled the collider
            // this is so we can walk over the dead body.
            if (this.transform.rigidbody.isKinematic == false)
            {
                this.transform.rigidbody.isKinematic = true;

                foreach (Collider col in this.transform.GetComponentsInChildren<Collider>())
                {
                    col.enabled = false;
                }
            }

            // Death sequence end
            if (deathSequenceTime >= deathSequenceEnd)
            {
                // When the death sequence has finished we want to make this object not active
                // This ensures that he will dissapear and not be visible in the game but we can still re-use him later.
                deathSequenceTime = 0.0f;
                this.gameObject.SetActive(false);
            }
            else
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
            else
            {
                 timeElapsed += Time.deltaTime;

                if (aiObject.activeSelf == true)
                {
                    UpdateSmart();
                }
                else
                {
                    UpdateStandard();
                }
            }
        }
    }

    public override void Initialise()
    {

		deathRotation = new Vector3(0.0f, 0.0f, transform.eulerAngles.z + 90.0f);

		// TODO: move this.
		bloodSplat = Resources.Load("BloodSplat/BloodSplat") as GameObject;

        // Grab the AI Rig from Rain AI
        if (ai == null)
        {

            ai = gameObject.GetComponentInChildren<RAIN.Core.AIRig>();
            //ai = gameObject.AddComponent<RAIN.Core.AIRig>();

            if (ai == null)
            {
                Debug.LogError("No AIRig attached to this: " + this.name);
            }

            ai.AI.WorkingMemory.SetItem<Vector3>("targetPos", Vector3.zero);

            Transform go = transform.FindChild("AI");
            aiObject = go.gameObject;
            //aiObject.SetActive(false);
        }

        // ai.enabled = false;

        // Populate with stats
        baseStatistics = new BaseStats();
		baseStatistics.Vitality = 25;
		derivedStats = new DerivedStats(baseStatistics);

        // Add abilities
        Action tackle = new EnemyTackle();
        tackle.Initialise(this);
        abilities.Add(tackle);

        // Add abilities
        Action charge = new BossCharge();
        charge.Initialise(this);
        abilities.Add(charge);

        // Grab sensors
        sensors = ai.AI.Senses.Sensors;

        //foreach(RAIN.Perception.Sensors.RAINSensor sensor in sensors)
        //{
        //    Debug.Log(sensor.SensorName);
        //}



        StartState(ERatState.Wandering);

        timeElapsed = Random.Range(0.0f, stateTimes[(int)ERatState.Wandering]);

        originalColour = gameObject.renderer.material.color;

		base.Initialise();
    }


    public void UpdateStandard()
    {
        switch (ratState)
        {
            case ERatState.Idle:
            case ERatState.Idle2:
                {
                    //Debug.Log("IDLE");
                    if (timeElapsed > stateTimes[(int)ERatState.Idle])
                    {
                        StartState(ERatState.Wandering);
                    }
                }
                break;
            case ERatState.Wandering:
                {
                    // Detect the player
                    rigidbody.AddForce(transform.forward * 100.0f);
                }
                break;
            case ERatState.Seeking:
                {
                    // move to the player

                    // Check if the player is within range to attack

                    // Check if the player is within range to Charge
                }
                break;
        }
    }

    public void UpdateSmart()
    {
        switch (ratState)
        {
            case ERatState.Idle:
            case ERatState.Idle2:
                {
                    //Debug.Log("IDLE");
                    if (timeElapsed > stateTimes[(int)ERatState.Idle])
                    {
                        StartState(ERatState.Wandering);
                    }
                }
                break;
            case ERatState.Wandering:
                {

                    ////Debug.Log("WANDER");
                    if (timeElapsed > stateTimes[(int)ERatState.Wandering])
                    {
                        StartState(ERatState.Idle);
                    }
                    else
                    {
                        // Detect heroes eye range now
                        sensors[0].MatchAspectName("heroVisual");

                        IList<RAIN.Entities.Aspects.RAINAspect> matches = sensors[0].Matches;

                        //foreach(RAIN.Entities.Aspects.RAINAspect match in matches)
                        //{
                        //    Debug.Log( match.AspectName);
                        //}

                        if (matches.Count > 0)
                        {
                            target = matches[0].Entity.Form.transform;
                            StartState(ERatState.Seeking);
                        }
                    }
                }
                break;
            case ERatState.Seeking:
                {
                    if (timeElapsed > stateTimes[(int)ERatState.Seeking])
                    {
                        //Debug.Log("SEEK");
                        ai.AI.Motor.MoveTo(target.position);

                        //// Detect attack range now
                        //sensors[1].MatchAspectName("heroVisual");
                        IList<RAIN.Entities.Aspects.RAINAspect> matches = sensors[1].Matches;
                        //if (matches.Count > 0)
                        //{
                        //    // Can attack this target
                        //    StartState(ERatState.ActionAttacking);
                        //    return;
                        //}

                        // Detect heroes in charge range now
                        sensors[1].MatchAspectName("heroVisual");
                        matches = sensors[1].Matches;
                        if (matches.Count > 0)
                        {
                            // Can attack this target
                            StartState(ERatState.ActionCharging);
                            return;
                        }
                    }
                    else
                    {
                        StartState(ERatState.Idle);
                    }
                }
                break;
            case ERatState.ActionAttacking:
                {
                    //Debug.Log("Attacking");

                    if (activeAbility == null)
                    {
                        StartState(ERatState.Idle);
                    }
                    base.Update();
                }
                break;
            case ERatState.ActionCharging:
                {
                    //Debug.Log("Charging");

                    if (activeAbility == null)
                    {
                        StartState(ERatState.Idle);
                    }
                    base.Update();
                }
                break;
            case ERatState.Flinching:
                {
                    if (timeElapsed > stateTimes[(int)ERatState.Flinching])
                    {
                        StartState(ERatState.Wandering);
                    }
                }
                break;
            case ERatState.Dying:
                {
                    // Do nothing
                }
                break;
            default:
                {
                    Debug.LogError("Invalid rat state entered.");
                }
                break;

        }
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

    protected void StartState(ERatState ratState)
    {
        timeElapsed = 0.0f;

        switch (ratState)
        {
            case ERatState.Idle:
                {
                    ai.AI.WorkingMemory.SetItem<bool>("moving", false);
                    ai.AI.WorkingMemory.SetItem<Vector3>("targetPos", Vector3.zero);
                    //Animator.PlayAnimation("Idle");
                    target = null;
                }
                break;
            case ERatState.Idle2:
                {
                    ai.AI.WorkingMemory.SetItem<bool>("moving", false);
                    ai.AI.WorkingMemory.SetItem<Vector3>("targetPos", Vector3.zero);
                    //Animator.PlayAnimation("Idle");
                    target = null;
                }
                break;
            case ERatState.Wandering:
                {
                    //ai.AI.WorkingMemory.SetItem<bool>("movingToVector", true);
                    //Animator.PlayAnimation("Walk");
                    Wander();
                    target = null;
                }
                break;
            case ERatState.Seeking:
                {
                    ai.AI.WorkingMemory.SetItem<bool>("moving", true);
                    ai.AI.WorkingMemory.SetItem<Vector3>("targetPos", Vector3.zero);
                    //Animator.PlayAnimation("Run");
                }
                break;
            case ERatState.ActionAttacking:
                {
                    ai.AI.WorkingMemory.SetItem<bool>("moving", false);
                    ai.AI.WorkingMemory.SetItem<Vector3>("targetPos", Vector3.zero);
                    UseAbility("EnemyTackle");
                }
                break;
            case ERatState.ActionCharging:
                {
                    ai.AI.WorkingMemory.SetItem<Vector3>("targetPos", Vector3.zero);
                    ai.AI.WorkingMemory.SetItem<bool>("moving", false);

                    BossCharge charge = GetAbility("BossCharge") as BossCharge;

                    charge.SetTarget(target);

                    UseAbility("BossCharge");
                }
                break;
            case ERatState.Flinching:
                {
                    ai.AI.WorkingMemory.SetItem<Vector3>("targetPos", Vector3.zero);
                    ai.AI.WorkingMemory.SetItem<bool>("moving", false);
                    //Animator.PlayAnimation("Flinch");
                    target = null;
                }
                break;
            case ERatState.Dying:
                {
                    ai.AI.WorkingMemory.SetItem<Vector3>("targetPos", Vector3.zero);
                    ai.AI.WorkingMemory.SetItem<bool>("moving", false);
                    //Animator.PlayAnimation("Die");
                    target = null;
                }
                break;
            default:
                {
                    Debug.LogError("Invalid rat state entered.");
                }
                break;
        }

        this.ratState = ratState;
    }

    protected void Wander()
    {
        float distanceToWander = Random.Range(2.0f, 5.0f);

        //Set Random Direction
        Vector2 newVector = Random.insideUnitCircle;
        Vector3 directionVector = new Vector3();

        directionVector.x = newVector.x * distanceToWander;
        directionVector.z = newVector.y * distanceToWander;
        directionVector.y = 0;

        //float angle = Vector3.Angle(ai.AI.Body.transform.forward, directionVector);

        //Debug.Log(targetPos);
        //childTarget.transform.localPosition = directionVector;

        //childTarget.transform.position = Vector3.zero;

        //target = childTarget;

        targetPos = transform.position + directionVector;

        ai.AI.WorkingMemory.SetItem<Vector3>("targetPos", targetPos);
    }

    public void OnCollisionEnter(Collision other)
    {
        string tag = other.transform.tag;

        switch (tag)
        {
            case "Hero":
                {
                    Character otherCharacter = other.transform.GetComponent<Character>();

                    CollidWithHero(otherCharacter as Hero, other);
                }
                break;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Wall") && ratState == ERatState.ActionCharging)
        {
            ApplyStunEffect(2.0f);
            gameObject.renderer.material.color = Color.yellow;
        }
    }

    /// <summary>
    /// When the rat collides with a hero
    /// </summary>
    /// <param name="hero"></param>
    private void CollidWithHero(Hero hero, Collision collision)
    {
        // If there is an object in this list that is the same
        // as this object it means we have a double collision.
        foreach (Object obj in hero.LastObjectsDamagedBy)
        {
            if (obj == this)
                return;
        }

        // Find the ability that this rat last performed
        Action ability = null;
        if (abilities.Count > 0)
        {
            ability = abilities[abilities.Count - 1];
        }

        if (ability != null && ability.GetType() == typeof(BossCharge))
        {
            BossCharge charge = ability as BossCharge;
            // Apply damage value to other character
            hero.ApplyDamage(charge.damageValue, charge.damageType);

            ContactPoint contact = collision.contacts[0];
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);

            // Apply particle blood splatter and make it a parent of the hero so that it will move with the hero.
            // TODO: make a pool of these emitters and dont instantiate them on the frame.
            GameObject bloodSplatter = Instantiate(bloodSplat, contact.point, rot) as GameObject;
            bloodSplatter.transform.parent = hero.transform;

            // Heroes are going to take a hit and play the animation.
            hero.Animator.PlayAnimation("TakeHit");
        }

        hero.LastObjectsDamagedBy.Add(this);

        // Update our list of collided targets
        // If a weapon has special properties where it may only be able to hit a number of targets, 
        // we would check to see if the count is too high before adding to the targets list.
        collidedTargets.Add(hero);

        Debug.Log(this.name + " collides with " + hero);
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
