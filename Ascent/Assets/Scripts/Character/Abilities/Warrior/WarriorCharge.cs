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
    private float distanceTraveled;
	private float distanceMax = 7.5f;
	
	private Animator ownerAnimator;
    private HeroAnimator heroController;
	
    private float travelTime;
    private Vector3 startPos;
    private Vector3 targetPos;

    private CharacterMotor charMotor;

    private Circle circle;
	
    public override void Initialise(Character owner)
    {
        base.Initialise(owner);
		ownerAnimator = owner.Animator.Animator;
        heroController = owner.Animator as HeroAnimator;

		owner.ChargeBall.onCollisionEnterWall += OnHitWall;
		owner.ChargeBall.onCollisionEnterEnemy += OnHitEnemy;

        coolDownTime = 5.0f;
        animationTrigger = "SwingAttack";
        specialCost = 5;

        animationLength = 0.35f;
        travelTime = animationLength;

        charMotor = owner.GetComponentInChildren<CharacterMotor>();

        circle = new Circle(owner.transform, 2.0f, new Vector3(0.0f, 0.0f, 0.0f));
    }
	
    public override void StartAbility()
	{
        base.StartAbility();
		Reset ();

        startPos = owner.transform.position;

        RaycastHit hitInfo;
        if (Physics.Raycast(new Ray(startPos, owner.transform.forward), out hitInfo, distanceMax))
        {
            targetPos = hitInfo.point - (owner.transform.forward );

            travelTime = (hitInfo.distance / distanceMax) * animationLength;
        }
        else
        {
            targetPos = startPos + owner.transform.forward * (distanceMax);
            travelTime = animationLength;
        }

        owner.ApplyInvulnerabilityEffect(animationLength);
        //charMotor.canMove = false;
	}

    public override void UpdateAbility()
	{
        base.UpdateAbility();

        
        //motor.SpecialMove((targetPos - owner.transform.position) * 2.0f);

        if (currentTime > travelTime)
        {
            currentTime = travelTime;
        }

        Vector3 motion = Vector3.Lerp(startPos, targetPos, currentTime / travelTime);

        owner.transform.position = motion;

        if (currentTime == travelTime)
        {
           List<Character> enemies = new List<Character>();

           if (Game.Singleton.Tower.CurrentFloor.CurrentRoom.CheckCollisionArea(circle, Character.EScope.Enemy, ref enemies))
           {
               foreach (Enemy e in enemies)
               {
                   e.LastDamagedBy = owner;
                   e.ApplyDamage(2, Character.EDamageType.Physical, owner);
                   e.ApplyKnockback(e.transform.position - owner.transform.position, 1000000.0f);
                   e.ApplyStunEffect(2.0f);

                   // Create a blood splatter effect on the enemy.
                   Game.Singleton.EffectFactory.CreateBloodSplatter(e.transform.position, e.transform.rotation, e.transform, 3.0f);
               }
           }

           owner.StopAbility();
        }
        //motor.SpecialMove(motion);
       


       // Vector3 moveVec = owner.transform.forward * Time.deltaTime * actionSpeed;
		//timeElapsed += Time.deltaTime;
		
        //if (endCharge)
        //{			
        //    owner.transform.position += moveVec * Time.deltaTime;
        //    if (timeElapsed > 1-timeElapsed)
        //    {
        //        owner.StopAbility();
        //        //Debug.Log ("Ended Charge");
        //    }
        //}
        //else
        //{			
        //    //owner.transform.position += moveVec;

        //    //heroController.Controller.Move(moveVec);

        //    distanceTraveled += moveVec.magnitude;
			
        //    if (timeElapsed > halfWayPoint)
        //    {
        //        ownerAnimator.speed = 0.1f * timeElapsed; // freeze animation when swinging sword down
        //    }
	
        //    if (distanceTraveled >= distanceMax)
        //    {
        //        EndCharge();
        //    }
        //}
	}

    public override void EndAbility()
	{	
        owner.ChargeBall.gameObject.SetActive(false);
		ownerAnimator.SetBool("SwingAttack",false);

        //charMotor.canMove = true;
	}
	
	private void Reset()
	{	
		//timeElapsed = 0.0f;
    	distanceTraveled = 0f;
	}
	
	/// <summary>
	/// Handles event where the Charge collider touches a wall or heavy object.
	/// Ends the Charge.
	/// </summary>
	/// <param name='other'>
	/// Other.
	/// </param>
	private void OnHitWall(Character other)
	{
		EndCharge();
	}
	
	/// <summary>
	/// Handles the Event where the Charge collider touches an enemy.
	/// Applies damage and knockback equal to distance traveled.
	/// </summary>
	private void OnHitEnemy(Character other)
	{
		EndCharge();
        other.LastDamagedBy = owner;
		other.ApplyDamage((int)(10 * distanceTraveled),Character.EDamageType.Physical, owner);
		other.ApplyKnockback(Vector3.Normalize(other.transform.position-owner.ChargeBall.transform.position),5f + distanceTraveled * 15f);
	}
	
	/// <summary>
	/// Disables collider, starts playing the swing-down animation that occurs at the end of Charge
	/// </summary>
	private void EndCharge()
	{
		ownerAnimator.speed = 0.8f;
		owner.ChargeBall.gameObject.SetActive(false);
	}

#if UNITY_EDITOR
    public override void DebugDraw()
    {
        circle.DebugDraw();
    }
#endif
}