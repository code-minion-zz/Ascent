using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RatTackle : Ability
{
	private Circle damageArea;
	private float prevSpeed;
    private float prevAccel;
	private bool executedDamage;

    public override void Initialise(Character owner)
    {
        base.Initialise(owner);

		animationLength = 0.375f;
		animationSpeed = 1.0f;
		animationTrigger = "Strike";
		cooldownFullDuration = 1.5f;
		specialCost = 0;

		damageArea = new Circle(owner.transform, 0.5f, new Vector3(0.0f, 0.0f, 1.0f));
    }

    public override void StartAbility()
    {
		base.StartAbility();

		executedDamage = false;

		owner.Motor.IsHaltingMovementToPerformAction = true;

		owner.Animator.PlayAnimation(animationTrigger, true);
	}

    public override void UpdateAbility()
    {
		base.UpdateAbility();

		if (timeElapsedSinceStarting >= animationLength * 0.45f && !executedDamage)
		{
			List<Character> characters = new List<Character>();

			if (Game.Singleton.Tower.CurrentFloor.CurrentRoom.CheckCollisionArea(damageArea, Character.EScope.Hero, ref characters))
			{
				foreach (Character c in characters)
				{
					// Apply damage and knockback to the enemey.
					CombatEvaluator combatEvaluator = new CombatEvaluator(owner, c);
					combatEvaluator.Add(new PhysicalDamageProperty(owner.Stats.Attack, 1.0f));
					combatEvaluator.Add(new KnockbackCombatProperty(c.transform.position - owner.transform.position, 1.0f));
					combatEvaluator.Apply();

					// Create a blood splatter effect on the enemy.
                    EffectFactory.Singleton.CreateBloodSplatter(c.transform.position, c.transform.rotation);
				}

				executedDamage = true;
			}
		}
    }

    public override void EndAbility()
    {
		owner.Motor.IsHaltingMovementToPerformAction = false;
		owner.Animator.PlayAnimation(animationTrigger, false);
        base.EndAbility();
    }

#if UNITY_EDITOR
	public override void DebugDraw()
	{
		damageArea.DebugDraw();
	}
#endif

}
