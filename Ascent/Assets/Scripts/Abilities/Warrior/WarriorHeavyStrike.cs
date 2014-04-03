using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WarriorHeavyStrike : Ability 
{
    bool performed = false;
    public float radius = 4.0f;
    public float arcAngle = 200.0f;

	private MeleeWeaponTrail trail;

	private float previousTrailLength;

    private Arc swingArc;

    public override void Initialise(Character owner)
    {
        animationSpeed = 1.15f;
        animationLength = 1.233f;
        cooldownFullDuration = 1.0f;
        timeElapsedSinceStarting = 0.0f;
        animationTrigger = "HeavyStrike";
        specialCost = 3;

		swingArc = new Arc(owner.transform, radius, arcAngle, Vector3.zero);

		trail = ((HeroAnimator)owner.Animator).weaponTrail;
		//previousTrailLength = trail._tip.transform.position.x;

        base.Initialise(owner);
    }

    public override void StartAbility()
    {
        timeElapsedSinceStarting = 0.0f;
        performed = false;

        animationLength = 1.167f / animationSpeed;
		//owner.Animator.PlayAnimation(animationTrigger);
        ((HeroAnimator)Owner.Animator).PlayCombatAction((int)Warrior.ECombatAnimation.HeavyStrike, Warrior.ECombatAnimation.HeavyStrike.ToString());

		//Vector3 pos = trail._tip.transform.position;
		////pos.x = trailLength;
		//._tip.transform.position = pos;
		//trail.ExtendLength(2.0f);
		trail.tipToUse = 1;
		 

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
					Room curRoom = Game.Singleton.Tower.CurrentFloor.CurrentRoom;
					if (curRoom.CheckCollisionArea(swingArc, Character.EScope.Enemy, ref enemies))
					{
						foreach (Enemy e in enemies)
						{
							CombatEvaluator combatEvaluator = new CombatEvaluator(owner, e);
							combatEvaluator.Add(new PhysicalDamageProperty(1.0f, 1.5f));
							combatEvaluator.Add(new KnockbackCombatProperty(e.transform.position - owner.transform.position, 10000000.0f));
							combatEvaluator.Apply();

							// Create a blood splatter effect on the enemy.
							Game.Singleton.EffectFactory.CreateBloodSplatter(e.transform.position, e.transform.rotation, e.transform);
						}
					}

					curRoom.ProcessCollisionBreakables(swingArc);
				}

                performed = true;
            }
        }
    }

    public override void EndAbility()
    {
		//Vector3 pos = trail._tip.transform.position;
		//pos.x = previousTrailLength;
		//trail._tip.transform.position = pos;
		trail.tipToUse = 0;
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
