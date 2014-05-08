// Developed by Kit Chan 2013

// Dependencies
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// Charging Action/Skill. 
/// Deals damage and knockback based on distance traveled (in other words, momentum)
/// </summary>
public class WarriorCharge : Ability 
{	
	private float distanceMax = 12.5f;
	
    private float travelTime;
	private float originalAnimationTime;

    private Vector3 startPos;
    private Vector3 targetPos;

   // private CharacterMotor charMotor;
	private int checkAtFrame = 3;
	private int frameCount = 0;

    private Circle circle;

#pragma warning disable 0414
	private Arc arc;

	List<Character> enemies;
	int enemiesFoundLastCount = 0;
	
    public override void Initialise(Character owner)
    {
        base.Initialise(owner);

        cooldownFullDuration = 2.0f;
        animationTrigger = "Charge";
        specialCost = 3;

		animationLength = 0.35f;
		originalAnimationTime = animationLength;

        travelTime = animationLength;

        //charMotor = owner.GetComponentInChildren<CharacterMotor>();

        circle = new Circle(owner.transform, 1.5f, new Vector3(0.0f, 0.0f, 0.0f));
		arc = new Arc(owner.transform, 5.0f, 7.5f, Vector3.zero);

        isInstantCast = false;
    }
	
    public override void StartAbility()
	{
        base.StartAbility();

		SoundManager.PlaySound(AudioClipType.heavyhit, owner.transform.position, 1f);

        ((HeroAnimator)Owner.Animator).PlayCombatAction((int)Warrior.ECombatAnimation.Charge, Warrior.ECombatAnimation.Charge.ToString());

        startPos = owner.transform.position;
		Vector3 rayStart = startPos;
		rayStart.y = 1.0f;

		// Find the closest character
		Character closestCharacter = null;

		// Charge to the closest character
		if (closestCharacter != null)
		{
			RaycastHit hitInfo;
			if (Physics.Raycast(new Ray(rayStart, closestCharacter.transform.position - rayStart), out hitInfo, distanceMax))
			{
				targetPos = hitInfo.point - (owner.transform.forward);

				travelTime = (hitInfo.distance / distanceMax) * originalAnimationTime;
				animationLength = travelTime;
			}
		}
		else
		{
			int layerMask = ((1 << (int)Layer.Environment)) | ((1 << (int)Layer.Block));
			RaycastHit hitInfo;

			if (Physics.SphereCast(new Ray(rayStart, owner.transform.forward), 0.1f, out hitInfo, distanceMax, layerMask))
			{
				targetPos = rayStart + (owner.transform.forward * hitInfo.distance);

				travelTime = (hitInfo.distance / distanceMax) * originalAnimationTime;
				animationLength = travelTime;
			}
			else
			{
				targetPos = startPos + owner.transform.forward * (distanceMax);

				travelTime = originalAnimationTime;
				animationLength = travelTime;
			}
		}

		targetPos.y = owner.transform.position.y;

		frameCount = checkAtFrame;

        //owner.ApplyStatusEffect(new InvulnerabilityBuff(owner, owner, animationLength));

		enemies = new List<Character>();
		enemiesFoundLastCount = 0;
	}

    public override void StartCast()
    {
        ((HeroAnimator)Owner.Animator).PlayCombatAction((int)Warrior.ECombatAnimation.ChargeCrouch, Warrior.ECombatAnimation.Charge.ToString());
    }

    public override void UpdateAbility()
	{
        base.UpdateAbility();

        Vector3 motion = Vector3.Lerp(startPos, targetPos, timeElapsedSinceStarting / travelTime);
        owner.transform.position = motion;

		if (frameCount >= checkAtFrame)
		{
			DoDamageAlongPath();
			frameCount = 0;
		}
        else if (timeElapsedSinceStarting == animationLength)
        {
			DoDamageAtEndOfPath();
			owner.Loadout.StopAbility();
        }

		++frameCount;
	}

	private bool DoDamageAlongPath()
	{
		return DoDamageCheck();
	}

	private void DoDamageAtEndOfPath()
	{
		DoDamageCheck();
	}

	private bool DoDamageCheck()
	{
		bool collisionsFound = false;

		if (Game.Singleton.InTower)
		{
			Room curRoom = Game.Singleton.Tower.CurrentFloor.CurrentRoom;
			if (curRoom.CheckCollisionArea(circle, Character.EScope.Enemy, ref enemies))
			{
				if (enemiesFoundLastCount != enemies.Count)
				{
					for (int i = enemiesFoundLastCount; i < enemies.Count; ++i)
					{
						// Apply damage, knockback and stun to the enemy.
						CombatEvaluator combatEvaluator = new CombatEvaluator(owner, enemies[i]);
						combatEvaluator.Add(new PhysicalDamageProperty(1.0f, 1.5f));
						combatEvaluator.Add(new StatusEffectCombatProperty(new StunnedDebuff(owner, enemies[i], 1.5f)));
						combatEvaluator.Apply();

						// Create a blood splatter effect on the enemy.
                        EffectFactory.Singleton.CreateBloodSplatter(enemies[i].transform.position, enemies[i].transform.rotation);
					}

					enemiesFoundLastCount = enemies.Count;
				}

				collisionsFound = true;

				curRoom.ProcessCollisionBreakables(circle);
			}
		}

		return collisionsFound;
	}

    public override void EndAbility()
	{
        ((HeroAnimator)Owner.Animator).CombatAnimationEnd();
        base.EndAbility();
	}

#if UNITY_EDITOR
    public override void DebugDraw()
    {
        circle.DebugDraw();
		if (timeElapsedSinceStarting < travelTime * 0.25f)
		{
			arc.DebugDraw();
		}

        Debug.DrawLine(startPos, targetPos, Color.red);
        Debug.DrawLine(startPos, owner.transform.position, Color.green);
    }
#endif
}