// Developed by Kit Chan 2013

// Dependencies
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// Charging Action/Skill. 
/// Deals damage and knockback based on distance traveled (in other words, momentum)
/// </summary>
public class AbominationCharge : Ability
{
    private float distanceMax = 20.0f;

    private float travelTime;
    private float originalAnimationTime;

    private Vector3 startPos;
    private Vector3 targetPos;

    // private CharacterMotor charMotor;
    private int checkAtFrame = 3;
    private int frameCount = 0;

    private Circle circle;
    private Arc arc;

	private bool started = false;

    List<Character> enemies;
    int enemiesFoundLastCount = 0;

    public override void Initialise(Character owner)
    {
        base.Initialise(owner);

        cooldownFullDuration = 2.0f;
        animationTrigger = "Charge";

		animationSpeed = 1.0f;
        animationLength = 1.0f;
        originalAnimationTime = animationLength;

        travelTime = animationLength;

        circle = new Circle(owner.transform, 1.5f, new Vector3(0.0f, 0.0f, 0.0f));
        arc = new Arc(owner.transform, 5.0f, 7.5f, Vector3.zero);

		canBeInterrupted = false;
		isInstantCast = true;
    }

    public override void StartAbility()
    {
		started = false;
		owner.SetColor(Color.red);
		animationLength = originalAnimationTime;

        base.StartAbility();
    }

    public override void UpdateAbility()
    {
        base.UpdateAbility();

		if (timeElapsedSinceStarting > 0.5f && !started)
		{

			// Find target point
			owner.Motor.IsHaltingRotationToPerformAction = true;

			startPos = owner.transform.position;
			Vector3 rayStart = startPos + (owner.transform.forward * 0.5f);
			rayStart.y = 1.5f;

			Vector3 rayStart2 = rayStart + (owner.transform.right * 0.25f);
			Vector3 rayStart3 = rayStart - (owner.transform.right * 0.25f);

			int layerMask = ((1 << (int)Layer.Environment));
			RaycastHit hitInfo;

			bool rayHit = false;

			// Find closest Ray
			float closestTravelTime = 100000000.0f;
			Vector3 closestTarget = Vector3.zero;

			if (Physics.SphereCast(new Ray(rayStart, owner.transform.forward), 1.0f, out hitInfo, distanceMax, layerMask))
			{
				targetPos = rayStart + ((owner.transform.forward * hitInfo.distance) - (owner.transform.forward));

				travelTime = (hitInfo.distance / distanceMax) * originalAnimationTime;
				animationLength = travelTime;

				if (travelTime < closestTravelTime)
				{
					closestTravelTime = travelTime;
					closestTarget = targetPos;
				}

				rayHit = true;
			}

			if (Physics.SphereCast(new Ray(rayStart2, owner.transform.forward), 0.5f, out hitInfo, distanceMax, layerMask))
			{
				targetPos = rayStart2 + ((owner.transform.forward * hitInfo.distance) - (owner.transform.forward));

				travelTime = (hitInfo.distance / distanceMax) * originalAnimationTime;
				animationLength = travelTime;

				if (travelTime < closestTravelTime)
				{
					travelTime = closestTravelTime;
					closestTarget = targetPos;
				}

				rayHit = true;
			}

			if (Physics.SphereCast(new Ray(rayStart3, owner.transform.forward), 0.5f, out hitInfo, distanceMax, layerMask))
			{
				targetPos = rayStart3 + ((owner.transform.forward * hitInfo.distance) - (owner.transform.forward));

				travelTime = (hitInfo.distance / distanceMax) * originalAnimationTime;
				animationLength = travelTime;

				if (travelTime < closestTravelTime)
				{
					travelTime = closestTravelTime;
					closestTarget = targetPos;
				}

				rayHit = true;
			}

			if (rayHit)
			{
				targetPos = closestTarget;
				travelTime = closestTravelTime;
				animationLength = travelTime;
			}
			else
			{
				targetPos = startPos + owner.transform.forward * (distanceMax);

				travelTime = originalAnimationTime;
				animationLength = travelTime;
			}

			targetPos.y = owner.transform.position.y;

			frameCount = checkAtFrame;

			enemies = new List<Character>();
			enemiesFoundLastCount = 0;

			owner.ResetColor();
			started = true;
			timeElapsedSinceStarting = 0.0f;

		}
		else if (started) 
		{
			// Move to target point
			Vector3 motion = Vector3.Lerp(startPos, targetPos, timeElapsedSinceStarting / travelTime);
			owner.transform.position = motion;

			if (frameCount >= checkAtFrame)
			{
				DoDamageAlongPath();
				frameCount = 0;
			}

			if(timeElapsedSinceStarting == animationLength)
			{
				DoDamageAtEndOfPath();

				owner.Motor.IsHaltingRotationToPerformAction = false;

				//owner.Loadout.StopAbility();
			}
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

        // Apply damage, knockback and stun to the enemy.
        CombatEvaluator combatEvaluator = new CombatEvaluator(owner, owner);
        combatEvaluator.Add(new StatusEffectCombatProperty(new StunnedDebuff(owner, owner, 2.5f)));
        combatEvaluator.Apply();

		Game.Singleton.Tower.CurrentFloor.FloorCamera.ShakeCamera(1.0f, 1.0f);
    }

    private bool DoDamageCheck()
    {
        bool collisionsFound = false;

        if (Game.Singleton.InTower)
        {
            Room curRoom = Game.Singleton.Tower.CurrentFloor.CurrentRoom;
            if (curRoom.CheckCollisionArea(circle, Character.EScope.Hero, ref enemies))
            {
                if (enemiesFoundLastCount != enemies.Count)
                {
                    for (int i = enemiesFoundLastCount; i < enemies.Count; ++i)
                    {
                        // Apply damage, knockback and stun to the enemy.
                        CombatEvaluator combatEvaluator = new CombatEvaluator(owner, enemies[i]);
                        combatEvaluator.Add(new PhysicalDamageProperty(0.0f, 1.5f));
                        combatEvaluator.Add(new StatusEffectCombatProperty(new StunnedDebuff(owner, enemies[i], 1.5f)));
                        combatEvaluator.Apply();

                        // Create a blood splatter effect on the enemy.
                        Game.Singleton.EffectFactory.CreateBloodSplatter(enemies[i].transform.position, enemies[i].transform.rotation, enemies[i].transform, 3.0f);
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
		//owner.ResetColor();
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