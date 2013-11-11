// Developed by Mana Khamphanpheng 2013

// Dependencies
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using RAIN.Core;
using RAIN.Action;
using RAIN.Representation;

public class Rat : Enemy 
{
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

	Vector3 targetPos;

	Transform target;

	IList<RAIN.Perception.Sensors.RAINSensor> sensors;

	public override void Start()
	{
		Initialise();
	}

	public override void Initialise()
	{
		// Grab the AI Rig from Rain AI
		if (ai == null)
		{
			ai = gameObject.GetComponentInChildren<RAIN.Core.AIRig>();

			if (ai == null)
			{
				Debug.LogError("No AIRig attached to this: " + this.name);
			}
		}

		


		// Populate with stats
        characterStatistics = new CharacterStatistics();
        characterStatistics.MaxHealth = 100;
        characterStatistics.CurrentHealth = 100;

        // Add abilities
        Action tackle = new EnemyTackle();
		tackle.Initialise(this);
		abilities.Add(tackle);

        // Add abilities
        Action charge = new EnemyCharge();
        charge.Initialise(this);
        abilities.Add(charge);

		// Grab sensors
		sensors = ai.AI.Senses.Sensors;

        //foreach(RAIN.Perception.Sensors.RAINSensor sensor in sensors)
        //{
        //    Debug.Log(sensor.SensorName);
        //}
		
		

		StartState(ERatState.Idle);
	}

	public override void Update()
	{
		timeElapsed += Time.deltaTime;

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
					//Debug.Log("WANDER");
					if (timeElapsed > stateTimes[(int)ERatState.Wandering])
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
					else
					{
						Wander();
					}
				}
				break;
			case ERatState.Seeking:
				{
					if (timeElapsed > stateTimes[(int)ERatState.Seeking])
					{
						//Debug.Log("SEEK");
						ai.AI.Motor.MoveTo(target.position);

						// Detect attack range now
						sensors[1].MatchAspectName("heroVisual");
						IList<RAIN.Entities.Aspects.RAINAspect> matches = sensors[1].Matches;
						if (matches.Count > 0)
						{
							// Can attack this target
							StartState(ERatState.ActionAttacking);
							return;
						}

						// Detect heroes in charge range now
						sensors[2].MatchAspectName("heroVisual");
						matches = sensors[2].Matches;
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

					if(activeAbility == null)
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
        DestroyObject(this.gameObject);
    }

	protected void StartState(ERatState ratState)
	{
		timeElapsed = 0.0f;

		switch (ratState)
	    {
			case ERatState.Idle:
				{
					ai.AI.WorkingMemory.SetItem<bool>("moving", false);
					//Animator.PlayAnimation("Idle");
				}
				break;
			case ERatState.Idle2:
				{
					ai.AI.WorkingMemory.SetItem<bool>("moving", false);
					//Animator.PlayAnimation("Idle");
				}
				break;
			case ERatState.Wandering:
				{
					ai.AI.WorkingMemory.SetItem<bool>("moving", true);
					//Animator.PlayAnimation("Walk");
				}
				break;
			case ERatState.Seeking:
				{
					ai.AI.WorkingMemory.SetItem<bool>("moving", true);
					//Animator.PlayAnimation("Run");
				}
				break;
			case ERatState.ActionAttacking:
				{
					ai.AI.WorkingMemory.SetItem<bool>("moving", false);
					UseAbility("EnemyTackle");
				}
				break;
			case ERatState.ActionCharging:
				{
					ai.AI.WorkingMemory.SetItem<bool>("moving", false);

					EnemyCharge charge = GetAbility("EnemyCharge") as EnemyCharge;

					charge.SetTarget(target);

					UseAbility("EnemyCharge");
				}
				break;
			case ERatState.Flinching:
				{
					ai.AI.WorkingMemory.SetItem<bool>("moving", false);
					//Animator.PlayAnimation("Flinch");
				}
				break;
			case ERatState.Dying:
				{
					ai.AI.WorkingMemory.SetItem<bool>("moving", false);
					//Animator.PlayAnimation("Die");
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

		targetPos = transform.position + directionVector;
	}
}
