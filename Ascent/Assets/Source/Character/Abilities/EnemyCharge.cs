using UnityEngine;
using System.Collections;

public class EnemyCharge : Action
{
    // TODO: Move this somewhere else
    public int damageValue = 10;
    public Character.EDamageType damageType = Character.EDamageType.Physical;


	Color original;
	const float actionTime = 0.75f;
	float timeElapsed = 0.0f;

	Vector3 originalPos;
	Vector3 targetPos;

	Transform target;

	public override void Initialise(Character owner)
	{
		base.Initialise(owner);
	}

	public override void StartAbility()
	{
		original = owner.renderer.material.color;
		owner.renderer.material.color = Color.magenta;
		timeElapsed = 0.0f;

		originalPos = owner.transform.position;

		if (target == null)
		{
			targetPos = owner.transform.position + (owner.transform.forward + new Vector3(0.0f, 0.0f, 0.0f)) * 10.0f;
			owner.transform.LookAt(targetPos, Vector3.up);

		}
		else
		{
			
			targetPos = owner.transform.position + Vector3.Normalize(target.position - owner.transform.position) * 10.0f;
			owner.transform.LookAt( target.position , Vector3.up);
		}
		//owner.rigidbody.AddForce(owner.transform.forward * 800.0f, ForceMode.Acceleration);

		// Create a collider that will stun and damage anything I hit
	}

	public override void UpdateAbility()
	{
		timeElapsed += Time.deltaTime;

		Mathf.Clamp(timeElapsed, 0.0f, actionTime);

		owner.transform.position = Vector3.Lerp(originalPos, targetPos, timeElapsed);

		if (timeElapsed > actionTime)
		{
			owner.StopAbility();
		}
	}

	public override void EndAbility()
	{
		owner.renderer.material.color = original;
		target = null;
		//owner.transform.position = originalPos;
	}

	public void SetTarget(Transform target)
	{
		this.target = target;
	}
}
