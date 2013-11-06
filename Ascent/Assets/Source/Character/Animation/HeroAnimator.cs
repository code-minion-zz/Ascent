using UnityEngine;
using System.Collections;

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
    //private AnimatorStateInfo combatLayerState;	    // a reference to the current state of the animator, used for layer 2

    //private CapsuleCollider col;

    public bool useCurves;						    // a setting for teaching purposes to show use of curves
    public float movementSpeed = 10.0f;              // Movment speed
    public float rotationSmooth = 10.0f;

    static int idleState = Animator.StringToHash("Base Layer.Idle");
    //static int jumpState = Animator.StringToHash("Base Layer.JumpRunning");				// and are used to check state for various actions to occur
    static int attackState = Animator.StringToHash("Base Layer.SwingRight");
    static int movementState = Animator.StringToHash("Base Layer.Movement");
    //static int rollState = Animator.StringToHash("Base Layer.Roll");

    private Vector3 direction;

    public float MovementSpeed
    {
        get { return movementSpeed; }
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
        if (animator.layerCount == 2)
        {
            animator.SetLayerWeight(1, 1);
        }
	}
	
	// Update is called once per frame
	public void Update () 
    {
        // Set our currentState variable to the current state of the Base Layer (0) of animation
        currentBaseState = animator.GetCurrentAnimatorStateInfo(0);

        // Set our combatLayerState variable to the current state of the second Layer (1) of animation
        //if (animator.layerCount == 2)
        //{
        //    combatLayerState = animator.GetCurrentAnimatorStateInfo(1);
        //}

        bool attacking = animator.GetBool("SwingAttack");
        bool jumping = animator.GetBool("Jump");
        bool rolling = animator.GetBool("Roll");

        //// if we are currently in a state called Locomotion (see line 25), then allow Jump input (Space) to set the Jump bool parameter in the Animator to true
        if (currentBaseState.nameHash == movementState ||
            currentBaseState.nameHash == idleState)
        {
            if (!attacking && !jumping && !rolling)
            {
                if (direction.x != 0 || direction.z != 0)
                {
                    SmoothLookAt(transform.position + direction, rotationSmooth);
                }
            }
        }

        if (currentBaseState.nameHash == attackState)
        {
            // Transition in process wait to see if it has finished
            if (!animator.IsInTransition(0))
            {
                // Reset so we can attack again.
                //AnimAttack(false);
                animator.SetBool("SwingAttack", false);
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

    //public void AnimAttack(bool b)
    //{
    //    //if (currentBaseState.nameHash == movementState ||
    //    //    currentBaseState.nameHash == idleState)
    //    {
    //        animator.SetBool("SwingAttack", b);

    //    }
    //}

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
