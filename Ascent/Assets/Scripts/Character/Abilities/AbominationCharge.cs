// Developed by Kit Chan 2013

// Dependencies
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// Charging Action/Skill. 
/// Deals damage and knockback based on distance traveled (in other words, momentum)
/// </summary>
public class AbominationCharge : Action
{
    private float distanceMax = 20.0f;

    private float travelTime;
    private Vector3 startPos;
    private Vector3 targetPos;

    //private CharacterMotor charMotor;

    private Circle circle;

    private AISteeringAgent steering;

    public override void Initialise(Character owner)
    {
        base.Initialise(owner);

        coolDownTime = 1.0f;
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

        RaycastHit hitInfo;
        if (Physics.Raycast(new Ray(startPos, owner.transform.forward), out hitInfo, distanceMax))
        {
            targetPos = hitInfo.point - (owner.transform.forward * 2.0f);

            travelTime = (hitInfo.distance / distanceMax) * animationLength;
        }

        owner.ApplyInvulnerabilityEffect(animationLength);
    }

    public override void UpdateAbility()
    {
        base.UpdateAbility();

        if (currentTime > travelTime)
        {
            currentTime = travelTime;
        }

        Vector3 motion = Vector3.Lerp(startPos, targetPos, currentTime / travelTime);

        owner.transform.position = motion;

        if (currentTime == travelTime)
        {
            List<Character> enemies = new List<Character>();

            if (Game.Singleton.Tower.CurrentFloor.CurrentRoom.CheckCollisionArea(circle, Character.EScope.Hero, ref enemies))
            {
                foreach (Hero e in enemies)
                {
                    int damage = 2;
                    // Apply damage, knockback and stun to the enemy.
                    e.ApplyDamage(damage, Character.EDamageType.Physical, owner);
                    e.ApplyKnockback(e.transform.position - owner.transform.position, 1000000.0f);
                    e.ApplyStunEffect(2.0f);

                    // Create a blood splatter effect on the enemy.
                    Game.Singleton.EffectFactory.CreateBloodSplatter(e.transform.position, e.transform.rotation, e.transform, 3.0f);
                }
            }

            owner.StopAbility();
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