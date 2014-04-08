using UnityEngine;
using System.Collections;

public class EnemyMotor : CharacterMotor 
{
	protected override void ProcessMovement()
	{
		GetComponent<CharacterAnimator>().PlayAnimation("Movement", currentSpeed/MaxSpeed);
		base.ProcessMovement();
	}
}
