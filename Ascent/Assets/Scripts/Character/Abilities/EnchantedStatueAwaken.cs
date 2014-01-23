using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnchantedStatueAwaken : Action
{
	private Circle damageArea;
	private bool executedDamage;

	public override void Initialise(Character owner)
	{
		base.Initialise(owner);

		animationLength = 1.0f;
		animationSpeed = 1.0f;
		animationTrigger = "Awaken";
		coolDownTime = 2.0f;
		specialCost = 0;

		damageArea = new Circle(owner.transform, 3.5f, new Vector3(0.0f, 0.0f, 0.0f));
	}

	public override void StartAbility()
	{
		base.StartAbility();

		owner.Motor.StopMotion();
		owner.Motor.EnableMovementForce(false);
		owner.SetColor(Color.red);

		executedDamage = false;
	}

	public override void UpdateAbility()
	{
		base.UpdateAbility();

		if (currentTime >= animationLength * 1.0f)
		{
			owner.Motor.EnableMovementForce(true);
			owner.ResetColor();
		}
		else if (currentTime >= animationLength * 0.8f)
		{
			owner.Motor.StopMotion();
			owner.Motor.EnableMovementForce(false);
		}
		else if (currentTime >= animationLength * 0.40f && !executedDamage)
		{
			List<Character> characters = new List<Character>();

			if (Game.Singleton.Tower.CurrentFloor.CurrentRoom.CheckCollisionArea(damageArea, Character.EScope.Hero, ref characters))
			{
				foreach (Character c in characters)
				{
					// Apply damage and knockback to the enemey.
					c.ApplyDamage(1, Character.EDamageType.Physical, owner);
					c.ApplyKnockback(c.transform.position - owner.transform.position, 1.0f);

					// Create a blood splatter effect on the enemy.
					Game.Singleton.EffectFactory.CreateBloodSplatter(c.transform.position, c.transform.rotation, c.transform, 2.0f);

					// Tell the hud manager to spawn text.
					HudManager.Singleton.TextDriver.SpawnDamageText(c.gameObject, 5);
				}

				executedDamage = true;
			}
		}
	}

	public override void EndAbility()
	{
		base.EndAbility();
		owner.Motor.EnableMovementForce(true);
		owner.ResetColor();
	}

#if UNITY_EDITOR
	public override void DebugDraw()
	{
		damageArea.DebugDraw();
	}
#endif

}
