// Developed by Kit Chan 2013

// Dependencies
using UnityEngine;
using System.Collections;

public class Charge : Action 
{
	private float timeElapsed = 0.0f;		// how long for
	private float halfWayPoint = 0.2f;
	private float prevAnimatorSpeed = 0f;
	private float actionSpeed = 15f;
	
    private float distanceTraveled;
	private float distanceMax = 12f;
	
	private Animator ownerAnimator;
	
	private bool endCharge = false;
	
    public override void Initialise(Character owner)
    {
        base.Initialise(owner);
		ownerAnimator = owner.Animator.Animator;
		owner.ChargeBall.onCollisionEnterWall += OnHitWall;
		owner.ChargeBall.onCollisionEnterEnemy += OnHitEnemy;
    }
	
    public override void StartAbility()
	{
		Reset ();
		ownerAnimator.SetBool("SwingAttack",true);		// play anim
		ownerAnimator.SetFloat("Speed", 5);
		owner.ChargeBall.gameObject.SetActive(true);	
		prevAnimatorSpeed = ownerAnimator.speed;
	}

    public override void UpdateAbility()
	{
        Vector3 moveVec = owner.transform.forward * Time.deltaTime * actionSpeed;
        owner.transform.position += moveVec;
        distanceTraveled += moveVec.magnitude;
		timeElapsed += Time.deltaTime;
		
		if (timeElapsed > halfWayPoint)
		{
			ownerAnimator.speed = 1f * timeElapsed; // freeze animation when swinging sword down
		}

		if (distanceTraveled >= distanceMax)
		{
			owner.StopAbility();
		}
	}

    public override void EndAbility()
	{	owner.ChargeBall.gameObject.SetActive(false);
		ownerAnimator.speed = prevAnimatorSpeed;
		//ownerAnimator.StopPlayback();
		ownerAnimator.SetBool("SwingAttack",false);
		ownerAnimator.SetFloat("Speed", 0.9f);
	}
	
	private void Reset()
	{	
		timeElapsed = 0.0f;
    	distanceTraveled = 0f;
		endCharge = false;
	}
	
	private void OnHitWall(GameObject go)
	{
		owner.StopAbility();
	}
	
	private void OnHitEnemy(GameObject go)
	{
		owner.StopAbility();
	}
}

/*
// Developed by Kit Chan 2013

// Dependencies
using UnityEngine;
using System.Collections;

public class Charge : IAction 
{
	Character owner;
	private const float animationTime = 2.333f;
	private const float animationSpeed = 2.0f;
	private float timeElapsed;
    //private float distanceMax = 20.0f;
    //private float distanceTraveled;

    public void Initialise(Character owner)
    {
        this.owner = owner;
    }
    
	public void StartAbility()
	{
		timeElapsed = 0.0f;
		owner.Animator.PlayAnimation("SwingAttack");
		
        // get owner's charge box and enable collision
		owner.Weapon.EnableCollision = true;
		owner.Weapon.SetAttackProperties(999,Character.EDamageType.Physical);
	}

	public void UpdateAbility()
	{
//
//        Vector3 moveVec = owner.transform.forward * Time.deltaTime * animationSpeed;
//        distanceTraveled += moveVec.magnitude;
//        Debug.Log(distanceTraveled);
//        owner.transform.position += moveVec;
//
//		if (distanceTraveled >= distanceMax)
//		{
//			owner.StopAbility();
//		}
	}

	public void EndAbility()
	{
		owner.Weapon.EnableCollision = false;
		owner.Animator.StopAnimation("SwingAttack");
	}
}
*/
