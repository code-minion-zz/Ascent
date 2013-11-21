// Developed by Kit Chan 2013

// Dependencies
using UnityEngine;
using System.Collections;

/// <summary>
/// Charging Action/Skill. 
/// Deals damage and knockback based on distance traveled (in other words, momentum)
/// </summary>
public class Charge : Action 
{
	private float timeElapsed = 0.0f;		// how long for
	private float halfWayPoint = 0.2f;
	//private float endChargeTime = 0.2f;
	private float prevAnimatorSpeed = 0f;
	private float actionSpeed = 15f;
	
    private float distanceTraveled;
	private float distanceMax = 10f;
	
	private Animator ownerAnimator;
    private HeroAnimator heroController;
	
	private bool endCharge = false;
	
    public override void Initialise(Character owner)
    {
        base.Initialise(owner);
		ownerAnimator = owner.Animator.Animator;
        heroController = owner.Animator as HeroAnimator;

		owner.ChargeBall.onCollisionEnterWall += OnHitWall;
		owner.ChargeBall.onCollisionEnterEnemy += OnHitEnemy;
    }
	
    public override void StartAbility()
	{
		Reset ();
		ownerAnimator.SetBool("SwingAttack",true);		// play anim
		owner.ChargeBall.gameObject.SetActive(true);	
		prevAnimatorSpeed = ownerAnimator.speed;
	}

    public override void UpdateAbility()
	{
        Vector3 moveVec = owner.transform.forward * Time.deltaTime * actionSpeed;
		timeElapsed += Time.deltaTime;
		
		if (endCharge)
		{			
			owner.transform.position += moveVec * Time.deltaTime;
			if (timeElapsed > 1-timeElapsed)
			{
				owner.StopAbility();
				//Debug.Log ("Ended Charge");
			}
		}
		else
		{			
	        //owner.transform.position += moveVec;

            heroController.Controller.Move(moveVec);

	        distanceTraveled += moveVec.magnitude;
			
			if (timeElapsed > halfWayPoint)
			{
				ownerAnimator.speed = 0.1f * timeElapsed; // freeze animation when swinging sword down
			}
	
			if (distanceTraveled >= distanceMax)
			{
				EndCharge();
			}
		}
	}

    public override void EndAbility()
	{	
        owner.ChargeBall.gameObject.SetActive(false);
		ownerAnimator.speed = prevAnimatorSpeed;
		ownerAnimator.SetBool("SwingAttack",false);
	}
	
	private void Reset()
	{	
		timeElapsed = 0.0f;
    	distanceTraveled = 0f;
		endCharge = false;
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
		other.ApplyDamage((int)(10 * distanceTraveled),Character.EDamageType.Physical);
		other.ApplyKnockback(Vector3.Normalize(other.transform.position-owner.ChargeBall.transform.position),5f + distanceTraveled * 15f);
	}
	
	/// <summary>
	/// Disables collider, starts playing the swing-down animation that occurs at the end of Charge
	/// </summary>
	private void EndCharge()
	{
		endCharge = true;
		timeElapsed = 0f;
		ownerAnimator.speed = 0.8f;
		owner.ChargeBall.gameObject.SetActive(false);
	}
}