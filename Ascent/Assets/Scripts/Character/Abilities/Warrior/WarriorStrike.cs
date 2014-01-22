using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WarriorStrike : Action
{
	public float radius = 2.5f;
	public float arcAngle = 85.0f;
    public int damage = 0;
    public float knockBackValue = 0.0f;

    private bool performed = false;
	private Arc swingArc;

	public override void Initialise(Character owner)
    {
        animationSpeed = 2.0f;
        animationLength = 1.167f / animationSpeed;
		coolDownTime = animationLength;
		animationTrigger = "SwingAttack";
		specialCost = 0;

        // Defines the collision shape and properties of this ability.
		swingArc = new Arc(owner.transform, radius, arcAngle, new Vector3(0.0f, 0.0f, -0.10f));

		base.Initialise(owner);
    }

	public override void StartAbility()
	{
        base.StartAbility();

		performed = false;
        animationLength = 1.167f / animationSpeed;
        coolDownTime = animationLength;
        owner.Animator.Animator.SetFloat("SwordAttackSpeed", animationSpeed);

        //if (owner.Weapon != null)
        //{
        //    // Could work out a formula here, maybe the warrior strike takes weapon damage into account.
        //    damage = owner.Weapon.Damage + 10;
        //    knockBackValue = owner.Weapon.KnockBackValue + 1.0f;
        //}
	}

	public override void UpdateAbility()
	{
        base.UpdateAbility();

		if (!performed)
		{
			if (currentTime >= animationLength * 0.7f)
			{
				List<Character> enemies = new List<Character>();

				if (Game.Singleton.Tower.CurrentFloor.CurrentRoom.CheckCollisionArea(swingArc, Character.EScope.Enemy, ref enemies))
				{
					foreach(Enemy e in enemies)
					{
                        // Apply damage and knockback to the enemey.
						e.ApplyDamage(damage, Character.EDamageType.Physical, owner);
                        e.ApplyKnockback(e.transform.position - owner.transform.position, knockBackValue);

                        // Create a blood splatter effect on the enemy.
                        Game.Singleton.EffectFactory.CreateBloodSplatter(e.transform.position, e.transform.rotation, e.transform, 2.0f);

                        // Tell the hud manager to spawn text.
                        HudManager.Singleton.TextDriver.SpawnDamageText(e.gameObject, damage);
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

#if UNITY_EDITOR
	public override void DebugDraw()
	{
		swingArc.DebugDraw();
	}
#endif
}
