using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WarriorStrike : BaseHeroAbility
{
	public float radius = 2.5f;
	public float arcAngle = 85.0f;
    public int damage = 0;
    public float knockBackValue = 0.0f;

    private bool performed = false;
	private Arc swingArc;

	private Warrior.ECombatAnimation firstStrikeAnim = Warrior.ECombatAnimation.Strike1;
	private Warrior.ECombatAnimation curStrikeAnim = Warrior.ECombatAnimation.Strike1;
	private Warrior.ECombatAnimation lastStrikeAnim = Warrior.ECombatAnimation.Strike3;

	public override void Initialise(Character owner)
    {
		base.Initialise(owner);

        animationSpeed = 1.5f;
        animationLength = 0.633f;
        cooldownFullDuration = 0.0f;
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

		owner.Motor.Move((((Hero)owner).HeroController.MoveDirection.normalized) * 0.45f);

        SoundManager.PlaySound(AudioClipType.swordSlash, owner.transform.position, 0.3f);

	}

	public override void UpdateAbility()
	{
        base.UpdateAbility();

		if (!performed)
		{
			if (timeElapsedSinceStarting >= animationLength * 0.65f)
			{
				GameObject strike = GameObject.Instantiate(Resources.Load("Prefabs/Effects/Strike/BaseStrikeEffect")) as GameObject;
				strike.transform.position = owner.transform.position + (owner.transform.forward *0.75f)  + new Vector3(0.0f, 0.75f, 0.0f);
				strike.transform.rotation = owner.transform.rotation;

				List<Character> enemies = new List<Character>();

				if (Game.Singleton.InTower)
				{
					Room curRoom = Game.Singleton.Tower.CurrentFloor.CurrentRoom;
					if (curRoom.CheckCollisionArea(swingArc, Character.EScope.Enemy, ref enemies))
					{
						foreach (Enemy e in enemies)
						{
							// Apply damage and knockback to the enemey
							CombatEvaluator combatEvaluator = new CombatEvaluator(owner, e);
							combatEvaluator.Add(new PhysicalDamageProperty(1.0f, 1.0f));
							combatEvaluator.Add(new KnockbackCombatProperty(e.transform.position - owner.transform.position, knockBackValue));
							combatEvaluator.Apply();

                            Vector3 hitPos = e.collider.ClosestPointOnBounds(swingArc.Position);

                            EffectFactory.Singleton.CreateRandHitEffect(hitPos, e.transform.rotation);
							// Create a blood splatter effect on the enemy.
                            EffectFactory.Singleton.CreateBloodSplatter(e.transform.position, e.transform.rotation);

                            owner.Stats.CurrentSpecial += (int)((Hero)owner).HeroStats.SpecialPerStrike;
						}
					}
					
					curRoom.ProcessCollisionBreakables(swingArc);
				}

				performed = true;
			}
		}
		else if (timeElapsedSinceStarting >= animationLength * 0.65f)
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
