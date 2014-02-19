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
    private float distanceMax = 50.0f;

    private float travelTime;
    private Vector3 startPos;
    private Vector3 targetPos;

    //private CharacterMotor charMotor;

    private Circle circle;

    private AISteeringAgent steering;

    private bool hitSomething;
    private bool hitWall;

    public override void Initialise(Character owner)
    {
        base.Initialise(owner);

        cooldownFullDuration = 5.0f;
        animationTrigger = "Charge";
        specialCost = 0;

        animationLength = 1.5f;
        travelTime = animationLength;

        //charMotor = owner.GetComponentInChildren<CharacterMotor>();

        circle = new Circle(owner.transform, 2.0f, new Vector3(0.0f, 0.0f, 0.0f));
        
        Enemy enemy = owner as Enemy;
        steering = enemy.AIAgent.SteeringAgent;
    }

    public override void StartAbility()
    {
        base.StartAbility();

        steering.CanRotate = false;

        startPos = owner.transform.position;

        hitSomething = false;
        hitWall = false;

        RaycastHit hitInfo;
        if (Physics.Raycast(new Ray(startPos, owner.transform.forward), out hitInfo, distanceMax))
        {
            targetPos = hitInfo.point - (owner.transform.forward * 2.0f);

            travelTime = (hitInfo.distance / distanceMax) * animationLength;

            hitSomething = true;
            hitWall = true;
        }

		targetPos.y = startPos.y;

		owner.ApplyStatusEffect(new InvulnerabilityBuff(owner, owner, animationLength));
    }

    public override void UpdateAbility()
    {
        base.UpdateAbility();

        if (timeElapsedSinceStarting > travelTime)
        {
            timeElapsedSinceStarting = travelTime;
        }

        Vector3 motion = Vector3.Lerp(startPos, targetPos, timeElapsedSinceStarting / travelTime);

        owner.transform.position = motion;

        if (timeElapsedSinceStarting == travelTime)
        {
            List<Character> enemies = new List<Character>();

            if (Game.Singleton.Tower.CurrentFloor.CurrentRoom.CheckCollisionArea(circle, Character.EScope.Hero, ref enemies))
            {
                foreach (Hero e in enemies)
                {
                    // Apply damage, knockback and stun to the enemy.
                    e.ApplyDamage(owner.DamageFormulaA(5, 1.0f), false, Character.EDamageType.Physical, owner);
                    e.ApplyKnockback(e.transform.position - owner.transform.position, 1000000.0f);
					e.ApplyStatusEffect(new StunnedDebuff(owner, e, 1.5f));

                    // Create a blood splatter effect on the enemy.
                    Game.Singleton.EffectFactory.CreateBloodSplatter(e.transform.position, e.transform.rotation, e.transform, 3.0f);
                }

                hitSomething = true;
                hitWall = false;
            }

            if (hitSomething)
            {
                Game.Singleton.Tower.CurrentFloor.FloorCamera.ShakeCamera(0.05f, 0.02f);
            }
            if (hitWall)
            {
				owner.ApplyStatusEffect(new StunnedDebuff(owner, owner, 2.5f));
            }

            owner.Loadout.StopAbility();
        }
    }

    public override void EndAbility()
    {
        steering.CanRotate = true;

        base.EndAbility();
    }

#if UNITY_EDITOR
    public override void DebugDraw()
    {
        circle.DebugDraw();

        Debug.DrawLine(startPos, targetPos, Color.red);
        Debug.DrawLine(startPos, owner.transform.position, Color.green);
    }
#endif
}