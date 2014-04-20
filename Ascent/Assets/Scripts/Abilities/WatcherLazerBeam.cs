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

	private WatcherLazer lazer;
	private GameObject lazerCharge; 

	private float rotationAmountPerSecond = 45.0f;
	private float chargeTime = 1.75f;
	private float fireStraightTime = 1.75f;
	private float fireRotatingTime = 8.0f;

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
					owner.transform.Rotate(0.0f, rotationAmountPerSecond * Time.deltaTime, 0.0f);
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
			lazer.gameObject.SetActive(false);
			lazerCharge.gameObject.SetActive(false);
		}

		owner.Animator.PlayAnimation(animationTrigger, false);
		base.EndAbility();
	}

	public void StartCharging()
	{
		if (lazerCharge == null)
		{
			lazerCharge = (GameObject.Instantiate(Resources.Load("Prefabs/Projectiles/WatcherLazerChargeBeam")) as GameObject);
			lazerCharge.transform.position = owner.transform.position + owner.transform.forward;
			lazerCharge.transform.parent = owner.transform;

			lazerCharge.SetActive(true);
		}
		else
		{
			lazerCharge.SetActive(true);
		}
	}

	public void StartFiring()
	{
		if (lazer == null)
		{
			lazer = (GameObject.Instantiate(Resources.Load("Prefabs/Projectiles/WatcherLazerBeam")) as GameObject).GetComponent<WatcherLazer>();
			lazer.Initialise(owner.transform.position + owner.transform.forward * 0.5f, owner);

			lazer.gameObject.SetActive(true);
		}
		else
		{
			lazer.gameObject.SetActive(true);
		}

		if (lazerCharge != null)
		{
			lazerCharge.SetActive(false);
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
