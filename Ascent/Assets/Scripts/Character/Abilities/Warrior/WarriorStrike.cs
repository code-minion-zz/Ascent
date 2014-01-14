using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WarriorStrike : Action
{
	bool performed = false;
	public float radius = 2.25f;
	public float arcAngle = 85.0f;

	private Arc swingArc;

	public override void Initialise(Character owner)
    {
        animationSpeed = 3.0f;
        animationLength = 1.167f / 1.5f;
		coolDownTime = 0.0f;
		cooldownValue = 0.0f;
		currentTime = 0.0f;
		isOnCooldown = false;
		animationTrigger = "SwingAttack";
		specialCost = 0;

		swingArc = new Arc();
		swingArc.radius = radius;
		swingArc.arcAngle = arcAngle;
		swingArc.transform = owner.transform;

		base.Initialise(owner);
    }

	public override void StartAbility()
	{
		currentTime = 0.0f;
		cooldownValue = CooldownTime;
		isOnCooldown = true;
		performed = false;

        animationLength = 1.167f / animationSpeed;
        owner.Animator.Animator.SetFloat("SwordAttackSpeed", animationSpeed);

		swingArc.Process();
        base.StartAbility();
	}

	public override void UpdateAbility()
	{
        base.UpdateAbility();

		swingArc.Process();

		if (!performed)
		{
			if (currentTime >= animationLength * 0.5f)
			{
				List<Character> enemies = new List<Character>();
				if (Game.Singleton.Tower.CurrentFloor.CurrentRoom.CheckCollisionArea(swingArc, Character.EScope.Enemy, ref enemies))
				{
					foreach(Enemy e in enemies)
					{
						e.ApplyDamage(10, Character.EDamageType.Physical);
						e.ApplyKnockback(e.transform.position - owner.transform.position, 1.0f);
					}
				}

				performed = true;
			}
		}
	}

	public override void EndAbility()
	{
        base.EndAbility();
	}

	public override void DebugDraw()
	{		
		swingArc.DebugDraw();
	}
}
