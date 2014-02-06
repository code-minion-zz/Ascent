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

	private Warrior.ECombatAnimation firstStrikeAnim = Warrior.ECombatAnimation.Strike1;
	private Warrior.ECombatAnimation curStrikeAnim = Warrior.ECombatAnimation.Strike1;
	private Warrior.ECombatAnimation lastStrikeAnim = Warrior.ECombatAnimation.Strike2;

	public override void Initialise(Character owner)
    {
		base.Initialise(owner);

        animationSpeed = 1.5f;
        animationLength = 1.067f;
        cooldownDurationMax = 0.0f;
		animationTrigger = "Strike";
		specialCost = 0;

        // Defines the collision shape and properties of this ability.
		swingArc = new Arc(owner.transform, radius, arcAngle, new Vector3(0.0f, 0.0f, -0.0f));
    }

	public override void StartAbility()
	{
        base.StartAbility();

		performed = false;

		int strikeAnim = (int)curStrikeAnim;
		((HeroAnimator)Owner.Animator).PlayCombatAction(strikeAnim, ((Warrior.ECombatAnimation)strikeAnim).ToString());
    
		++curStrikeAnim;

		if (curStrikeAnim > lastStrikeAnim)
		{
			curStrikeAnim = firstStrikeAnim;
		}

        CanBeInterrupted = false;
	}

	public override void UpdateAbility()
	{
        base.UpdateAbility();

		if (!performed)
		{
			if (timeElapsedSinceStarting >= animationLength * 0.7f)
			{
				List<Character> enemies = new List<Character>();

				if (Game.Singleton.InTower)
				{
					if (Game.Singleton.Tower.CurrentFloor.CurrentRoom.CheckCollisionArea(swingArc, Character.EScope.Enemy, ref enemies))
					{
						foreach (Enemy e in enemies)
						{
							// Apply damage and knockback to the enemey

							e.ApplyDamage(owner.DamageFormulaA(0.0f, 1.0f), Character.EDamageType.Physical, owner);
							e.ApplyKnockback(e.transform.position - owner.transform.position, knockBackValue);

							// Create a blood splatter effect on the enemy.
							Game.Singleton.EffectFactory.CreateBloodSplatter(e.transform.position, e.transform.rotation, e.transform, 2.0f);

                            owner.Stats.CurrentSpecial += 1;
						}
					}
				}

				performed = true;
			}
		}
		else if (timeElapsedSinceStarting >= animationLength * 0.85f)
        {
            CanBeInterrupted = true;
        }
	}

	public override void EndAbility()
	{
		((HeroAnimator)Owner.Animator).CombatAnimationEnd();
        base.EndAbility();
	}

#if UNITY_EDITOR
	public override void DebugDraw()
	{
		swingArc.DebugDraw();
	}
#endif
}
