using UnityEngine;
using System.Collections;

public class EnemyTackle : Action
{
	Color original;
	const float actionTime = 0.5f;
	float timeElapsed = 0.0f;

	Vector3 originalPos;
	Vector3 targetPos;

    public override void Initialise(Character owner)
    {
        base.Initialise(owner);
    }

    public override void StartAbility()
    {
		Debug.Log("tackle");
		original = owner.renderer.material.color;
		owner.renderer.material.color = Color.red;

		timeElapsed = 0.0f;

		originalPos = owner.transform.position;
		targetPos = owner.transform.position + (owner.transform.forward + new Vector3(0.0f, 0.0f, 0.0f)) * 1.1f;

		// Create a collider that will flinch and damage anything I hit
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
		owner.transform.position = originalPos;

		//owner.transform.position = originalPos;
    }

}
