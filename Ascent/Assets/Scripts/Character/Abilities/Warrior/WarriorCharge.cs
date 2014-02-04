// Developed by Kit Chan 2013

// Dependencies
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// Charging Action/Skill. 
/// Deals damage and knockback based on distance traveled (in other words, momentum)
/// </summary>
public class WarriorCharge : Action 
{	
	private float distanceMax = 7.5f;
	
	private CharacterAnimator ownerAnimator;
    //private HeroAnimatorController heroController;
	
    private float travelTime;
	private float originalAnimationTime;

    private Vector3 startPos;
    private Vector3 targetPos;

   // private CharacterMotor charMotor;

    private Circle circle;
	private Arc arc;
	
    public override void Initialise(Character owner)
    {
        base.Initialise(owner);
		ownerAnimator = owner.Animator;
        //heroController = owner.Animator as HeroAnimatorController;

        coolDownTime = 5.0f;
        animationTrigger = "Charge";
        specialCost = 5;

		animationLength = 0.35f;
		originalAnimationTime = animationLength;

        travelTime = animationLength;

        //charMotor = owner.GetComponentInChildren<CharacterMotor>();

        circle = new Circle(owner.transform, 1.0f, new Vector3(0.0f, 0.0f, 0.0f));
		arc = new Arc(owner.transform, 10.0f, 25.0f, Vector3.back * 1.5f);


    }
	
    public override void StartAbility()
	{
        base.StartAbility();

		((HeroAnimator)Owner.Animator).PlayCombatAction((int)Warrior.ECombatAnimations.Charge);

        startPos = owner.transform.position;
		Vector3 rayStart = startPos;
		rayStart.y = 1.0f;

		Character closestCharacter = null;
		List<Character> enemies = new List<Character>();
		if (Game.Singleton.InTower)
		{
			if (Game.Singleton.Tower.CurrentFloor.CurrentRoom.CheckCollisionArea(arc, Character.EScope.Enemy, ref enemies))
			{
				float closestDistance = 1000000.0f;

				foreach (Character e in enemies)
				{
					float distance = (owner.transform.position - e.transform.position).sqrMagnitude;

					if (distance < closestDistance)
					{
						closestDistance = distance;
						closestCharacter = e;
					}
				}
			}
		}

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
			RaycastHit hitInfo;
			if (Physics.Raycast(new Ray(rayStart, owner.transform.forward), out hitInfo, distanceMax))
			{
				targetPos = hitInfo.point - (owner.transform.forward);

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

        owner.ApplyInvulnerabilityEffect(animationLength);
	}

    public override void UpdateAbility()
	{
        base.UpdateAbility();


        Vector3 motion = Vector3.Lerp(startPos, targetPos, currentTime / travelTime);
        owner.transform.position = motion;

        if (currentTime == animationLength)
        {
           List<Character> enemies = new List<Character>();

		   if (Game.Singleton.InTower)
		   {
			   if (Game.Singleton.Tower.CurrentFloor.CurrentRoom.CheckCollisionArea(circle, Character.EScope.Enemy, ref enemies))
			   {
				   foreach (Enemy e in enemies)
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
		   }

           owner.StopAbility();
        }
	}

    public override void EndAbility()
	{
        base.EndAbility();
	}

#if UNITY_EDITOR
    public override void DebugDraw()
    {
        circle.DebugDraw();
		if (currentTime < travelTime * 0.25f)
		{
			arc.DebugDraw();
		}

        Debug.DrawLine(startPos, targetPos, Color.red);
        Debug.DrawLine(startPos, owner.transform.position, Color.green);
    }
#endif
}