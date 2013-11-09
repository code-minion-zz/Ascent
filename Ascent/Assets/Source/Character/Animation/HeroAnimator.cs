using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CapsuleCollider))]
public class HeroAnimator : AnimatorController  
{
    #region Enums

    public enum EHeroState // State defines what actions are allowed, and what animations to play
    {
        PS_INVALID_STATE = -1,
        PS_STATE_IDLE,
        PS_STATE_MOVE,
        PS_STATE_SPRINT,
        PS_STATE_ATTACK,
        PS_STATE_JUMP,
        PS_STATE_FALLING,
        PS_STATE_CAST,
        PS_STATE_PUSH,
        PS_STATE_PULL,
        PS_STATE_DEATH,
        PS_STATE_FREEZE,
        PS_MAX_STATE
    }

    #endregion

    #region Fields

    public bool useCurves;						    // a setting for teaching purposes to show use of curves
    public float movementSpeed = 10.0f;              // Movment speed
    public float rotationSmooth = 10.0f;


    static int idleState = Animator.StringToHash("Movement." + "Idle");
    static int attackState = Animator.StringToHash("CombatLayer." + "SwingSword");
    static int movementState = Animator.StringToHash("Movement." + "Movement");

    private Vector3 direction;
    private AnimatorStateInfo activeStateInfo;

    public float MovementSpeed
    {
        get { return movementSpeed; }
    }

    #endregion

    public override void Awake()
    {
        base.Awake();
    }

	// Use this for initialization
    public override void Start() 
    {
        base.Start();
	}
	
	// Update is called once per frame
	public override void Update () 
    {
        base.Update();

        bool attacking = animator.GetBool("SwingAttack");
        bool jumping = animator.GetBool("Jump");
        bool rolling = animator.GetBool("Roll");

        for (int layer = 0; layer < layerCount; ++layer)
        {
            if (IsActiveState(layer, movementState) ||
                IsActiveState(layer, idleState))
            {

                // Update the look at when we are moving
                if (direction.x != 0 || direction.z != 0)
                {
                    transform.LookAt(transform.position + direction);
                }
            }
        }
	}

    void SmoothLookAt(Vector3 target, float smooth)
    {
        Vector3 dir = target - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * smooth);
    }

    #region animations

    public void AnimMove(Vector3 direction, float magnitude)
    {
        this.direction = direction;
        animator.SetFloat("Speed", magnitude);
    }

    public virtual void AnimAbility(int abilityID)
    {
        // Must be derived by each class for unique animations
    }

    void AnimRoll(bool b)
    {
        animator.SetBool("Roll", b);
    }

    void AnimJump(bool b)
    {
        if (activeStateInfo.nameHash == movementState)
        {
            animator.SetBool("Jump", b);
        }
    }

    public void AnimFlinch()
    {

    }

    public void AnimDie()
    {

    }

    #endregion
}
