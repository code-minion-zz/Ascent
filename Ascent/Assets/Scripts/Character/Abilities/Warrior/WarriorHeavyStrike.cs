using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WarriorHeavyStrike : Action 
{
    bool performed = false;
    public float radius = 4.0f;
    public float arcAngle = 200.0f;

    private Arc swingArc;

    public override void Initialise(Character owner)
    {
        animationSpeed = 1.5f;
        animationLength = 1.233f;
        cooldownDurationMax = 5.0f;
        timeElapsedSinceStarting = 0.0f;
        animationTrigger = "HeavyStrike";
        specialCost = 0;

		swingArc = new Arc(owner.transform, radius, arcAngle, Vector3.zero);


        base.Initialise(owner);
    }

    public override void StartAbility()
    {
        timeElapsedSinceStarting = 0.0f;
        performed = false;

        animationLength = 1.167f / animationSpeed;
		//owner.Animator.PlayAnimation(animationTrigger);
        ((HeroAnimator)Owner.Animator).PlayCombatAction((int)Warrior.ECombatAnimations.HeavyStrike, Warrior.ECombatAnimations.HeavyStrike.ToString());

        base.StartAbility();
    }

    public override void UpdateAbility()
    {
        base.UpdateAbility();

        if (!performed)
        {
            if (timeElapsedSinceStarting >= animationLength * 0.5f)
            {
                List<Character> enemies = new List<Character>();

				if (Game.Singleton.InTower)
				{
					if (Game.Singleton.Tower.CurrentFloor.CurrentRoom.CheckCollisionArea(swingArc, Character.EScope.Enemy, ref enemies))
					{
						foreach (Enemy e in enemies)
						{
							e.ApplyDamage(25, Character.EDamageType.Physical, owner);
							e.ApplyKnockback(e.transform.position - owner.transform.position, 100000000000.0f);

							// Create a blood splatter effect on the enemy.
							Game.Singleton.EffectFactory.CreateBloodSplatter(e.transform.position, e.transform.rotation, e.transform, 2.0f);

							// Tell the hud manager to spawn text.
							HudManager.Singleton.TextDriver.SpawnDamageText(e.gameObject, 25);
						}
					}
				}

                performed = true;
            }
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
