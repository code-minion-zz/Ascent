using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WatcherLazerBeam : Ability
{
	private enum EWatcherLazerState
	{
		Charging,
		Firing,
		FiringRotating,
	}

	private EWatcherLazerState state = EWatcherLazerState.Charging; 

	private int numberOfLazers = 4;

	private WatcherLazer[] lazer;
	private GameObject[] lazerCharge; 

	private float rotationAmountPerSecond = 45.0f;
	private float chargeTime = 1.75f;
	private float fireStraightTime = 1.75f;
	private float fireRotatingTime = 8.0f;

	private bool clockwise;

	private float timeElapsed;

	public override void Initialise(Character owner)
	{
		base.Initialise(owner);

		animationLength = chargeTime + fireRotatingTime + fireStraightTime;
		animationSpeed = 1.0f;
		animationTrigger = "Beam";
		cooldownFullDuration = 0.0f;
		specialCost = 0;
	}

	public void Enrage()
	{
		rotationAmountPerSecond = 75.0f;
	}

	public override void StartAbility()
	{
		base.StartAbility();

		owner.Animator.PlayAnimation(animationTrigger, true);

		state = EWatcherLazerState.Charging;
		timeElapsed = 0.0f;
		StartCharging();
	}

	public override void UpdateAbility()
	{
		base.UpdateAbility();

		timeElapsed += Time.deltaTime;

		float stateDuration = CurrentStateDuration();
		if (timeElapsed >= stateDuration)
		{
			timeElapsed = stateDuration;
		}

		switch (state)
		{
			case EWatcherLazerState.Charging:
				{
					if (timeElapsed >= stateDuration)
					{
						StartFiring();
						timeElapsed = 0.0f;
						state = EWatcherLazerState.Firing;

						clockwise = Random.Range(0, 2) == 0 ? false : true;
					}
				}
				break;
			case EWatcherLazerState.Firing:
				{
					if (timeElapsed >= stateDuration)
					{
						state = EWatcherLazerState.FiringRotating;
						timeElapsed = 0.0f;
					}
				}
				break;
			case EWatcherLazerState.FiringRotating:
				{
					owner.transform.Rotate(0.0f, clockwise ? rotationAmountPerSecond * Time.deltaTime : -rotationAmountPerSecond * Time.deltaTime, 0.0f);
				}
				break;
			default: 
				break;
		}


	}

	public override void EndAbility()
	{
		if (lazer != null)
		{
			for (int i = 0; i < numberOfLazers; ++i)
			{
				lazer[i].gameObject.SetActive(false);
				lazerCharge[i].gameObject.SetActive(false);
			}
		}

		owner.Animator.PlayAnimation(animationTrigger, false);
		base.EndAbility();
	}

	public void StartCharging()
	{
		if (lazerCharge == null)
		{
			lazerCharge = new GameObject[numberOfLazers];

			for (int i = 0; i < numberOfLazers; ++i)
			{
				lazerCharge[i] = (GameObject.Instantiate(Resources.Load("Prefabs/Projectiles/WatcherLazerChargeBeam")) as GameObject);

				Vector3 direction = MathUtility.ConvertHeadingToVector((((360.0f / (float)numberOfLazers) * (float)i) - 90.0f) * Mathf.Deg2Rad);
				direction.z = direction.y;
				direction.y = lazerCharge[i].transform.position.y;

				lazerCharge[i].transform.position = owner.transform.position + direction * 0.75f;
				lazerCharge[i].transform.LookAt((owner.transform.position + direction * 0.75f) - direction);

				lazerCharge[i].transform.parent = owner.transform;

				lazerCharge[i].SetActive(true);
			}
		}
		else
		{
			for (int i = 0; i < numberOfLazers; ++i)
			{
				lazerCharge[i].SetActive(true);
			}
		}
	}

	public void StartFiring()
	{
		if (lazer == null)
		{
			lazer = new WatcherLazer[numberOfLazers];

			for (int i = 0; i < numberOfLazers; ++i)
			{
				lazer[i] = (GameObject.Instantiate(Resources.Load("Prefabs/Projectiles/WatcherLazerBeam")) as GameObject).GetComponent<WatcherLazer>();

				Vector3 direction = MathUtility.ConvertHeadingToVector((((360.0f / (float)numberOfLazers) * (float)i) - 90.0f) * Mathf.Deg2Rad);

				direction.z = direction.y;
				direction.y = lazer[i].transform.position.y;

				lazer[i].Initialise(owner.transform.position + direction * 0.75f, owner);
				lazer[i].transform.LookAt((owner.transform.position + direction * 0.75f) - direction);

				lazer[i].gameObject.SetActive(true);
			}
		}
		else
		{
			for (int i = 0; i < numberOfLazers; ++i)
			{
				lazer[i].gameObject.SetActive(true);
			}
		}

		if (lazerCharge != null)
		{
			for (int i = 0; i < numberOfLazers; ++i)
			{
				lazerCharge[i].gameObject.SetActive(false);
			}
		}
	}

	public float CurrentStateDuration()
	{
		// Note: This should probably be put in an array.

		switch (state)
		{
			case EWatcherLazerState.Charging: return chargeTime;
			case EWatcherLazerState.Firing: return fireStraightTime;
			case EWatcherLazerState.FiringRotating: return fireRotatingTime;
			default:
				break;
		}

		return 0.0f;
	}
}
