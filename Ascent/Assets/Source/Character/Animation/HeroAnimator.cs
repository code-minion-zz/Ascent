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

    private AnimatorStateInfo currentBaseState;		// a reference to the current state of the animator, used for base layer
    private AnimatorStateInfo combatLayerState;	    // a reference to the current state of the animator, used for layer 2

    //private CapsuleCollider col;

    public bool useCurves;						    // a setting for teaching purposes to show use of curves
    public float movementSpeed = 10.0f;              // Movment speed
    public float rotationSmooth = 10.0f;

    static int idleState = Animator.StringToHash("Movement." + "Idle");
    static int attackState = Animator.StringToHash("CombatLayer." + "SwingRight");
    static int movementState = Animator.StringToHash("Movement." + "Movement");

    private Vector3 direction;

    private List<AnimationClip> currentAnimationClips = new List<AnimationClip>();

    public float MovementSpeed
    {
        get { return movementSpeed; }
    }

    public List<AnimationClip> CurrentAnimationClips
    {
        get { return currentAnimationClips; }
    }

    #endregion

    public override void Awake()
    {
        base.Awake();

        // Get all the components that we need.
        //col = GetComponent<CapsuleCollider>();
    }

	// Use this for initialization
    public void Start() 
    {

	}

    AnimationInfo[] GetCurrentAnimationInfo(int layer)
    {
        return animator.GetCurrentAnimationClipState(layer);
    }
	
	// Update is called once per frame
	public void Update () 
    {
        bool attacking = animator.GetBool("SwingAttack");
        bool jumping = animator.GetBool("Jump");
        bool rolling = animator.GetBool("Roll");

        // Set our currentState variable to the current state of the Base Layer (0) of animation
        currentBaseState = animator.GetCurrentAnimatorStateInfo(0);

        // Set our combatLayerState variable to the current state of the second Layer (1) of animation
        if (animator.layerCount == 2)
        {
            combatLayerState = animator.GetCurrentAnimatorStateInfo(1);
        }

        if (currentBaseState.nameHash == movementState ||
            currentBaseState.nameHash == idleState)
        {
            if (!attacking && !jumping && !rolling)
            {
                if (direction.x != 0 || direction.z != 0)
                {
                    transform.LookAt(transform.position + direction);
                    //SmoothLookAt(transform.position + direction, rotationSmooth);
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
        if (currentBaseState.nameHash == movementState)
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
