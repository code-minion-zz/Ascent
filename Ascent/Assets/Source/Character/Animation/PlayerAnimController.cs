using UnityEngine;
using System.Collections;
using InControl;

// The player animator controller is in charge of taking player input and mapping this to an action.
// It has a state machine for animation.
// This class should also contain player movement variables.
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Player))]
public class PlayerAnimController : AnimatorController
{
    #region Enums

    public enum EPlayerState // State defines what actions are allowed, and what animations to play
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

    #endregion

    #region Intialization

    public override void Awake()
    {
        base.Awake();

        // Get all the components that we need.
        //col = GetComponent<CapsuleCollider>();	
    }

	public void EnableInput(AscentInput ainput)
	{
		// Register everthing here
        ainput.OnLStickMove += OnDPadUp;
        ainput.OnX += OnX;
        ainput.OnY += OnY;
        ainput.OnA += OnA;
        ainput.OnB += OnB;
	}

	public void DisableInput(AscentInput ainput)
	{
		// Unregister everything here
        ainput.OnLStickMove -= OnDPadUp;
        ainput.OnX -= OnX;
        ainput.OnY -= OnY;
        ainput.OnA -= OnA;
        ainput.OnB -= OnB;
	}


    public void Start()
    {
        if (animator.layerCount == 2)
            animator.SetLayerWeight(1, 1);
    }

    #endregion

    void SmoothLookAt(Vector3 target, float smooth)
    {
        Vector3 dir = target - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * smooth);
    }

    void FixedUpdate()
    {
        // Set our currentState variable to the current state of the Base Layer (0) of animation
        currentBaseState = animator.GetCurrentAnimatorStateInfo(0);

        // Set our combatLayerState variable to the current state of the second Layer (1) of animation
        //if (animator.layerCount == 2)
        //{
        //    combatLayerState = animator.GetCurrentAnimatorStateInfo(1);
        //}


        CollisionStates();
    }

    #region Collision States

    void CollisionStates()
    {
        bool attacking = animator.GetBool("SwingAttack");
        bool jumping = animator.GetBool("Jump");
        bool rolling = animator.GetBool("Roll");

        //// if we are currently in a state called Locomotion (see line 25), then allow Jump input (Space) to set the Jump bool parameter in the Animator to true
        if (currentBaseState.nameHash == movementState ||
            currentBaseState.nameHash == idleState)
        {
            //if (currentBaseState.nameHash == movementState)
            //{
            //    if (inputDevice.Action1.IsPressed)
            //    {
            //        animator.SetBool("Jump", true);
            //    }
            //}

            //if (inputDevice.Action2.IsPressed)
            //{
            //    //animator.SetBool("SwingAttack", true);
            //    Attack(true);
               
            //}

            //if (inputDevice.Action3.IsPressed)
            //{
            //    animator.SetBool("Roll", true);
            //}

        if (!attacking && !jumping && !rolling)
        {
            if (direction.x != 0 || direction.z != 0)
            {
                SmoothLookAt(transform.position + direction, rotationSmooth);
            }
        }
        }
        //// if we are in the jumping state... 
        //else if (currentBaseState.nameHash == jumpState)
        //{
        //    //if (inputDevice.Action2.IsPressed)
        //    //{
        //    //    //animator.SetBool("SwingAttack", true);

        //    //    Attack(true);
        //    //}

        //    //if (inputDevice.Action3.IsPressed)
        //    //{
        //    //    animator.SetBool("Roll", true);
        //    //}

        //    //  ..and not still in transition..
        //    if (!animator.IsInTransition(0))
        //    {
        //        if (useCurves)
        //            // ..set the collider height to a float curve in the clip called ColliderHeight
        //            col.height = animator.GetFloat("ColliderHeight");

        //        // reset the Jump bool so we can jump again, and so that the state does not loop 
        //        animator.SetBool("Jump", false);
        //    }

        //    // Raycast down from the center of the character.. 
        //    Ray ray = new Ray(transform.position + Vector3.up, -Vector3.up);
        //    RaycastHit hitInfo = new RaycastHit();

        //    if (Physics.Raycast(ray, out hitInfo))
        //    {
        //        // ..if distance to the ground is more than 1.75, use Match Target
        //        if (hitInfo.distance > 1.75f)
        //        {

        //            // MatchTarget allows us to take over animation and smoothly transition our character towards a location - the hit point from the ray.
        //            // Here we're telling the Root of the character to only be influenced on the Y axis (MatchTargetWeightMask) and only occur between 0.35 and 0.5
        //            // of the timeline of our animation clip
        //            animator.MatchTarget(hitInfo.point, Quaternion.identity, AvatarTarget.Root, new MatchTargetWeightMask(new Vector3(0, 1, 0), 0), 0.35f, 0.5f);
        //        }
        //    }
        //}

        if (currentBaseState.nameHash == attackState)
        {
            // Transition in process wait to see if it has finished
            if (!animator.IsInTransition(0))
            {
                // Reset so we can attack again.
               //animator.SetBool("SwingAttack", false);
                Attack(false);
            }
        }

        //if (currentBaseState.nameHash == rollState)
        //{
        //    //if (inputDevice.Action2.IsPressed)
        //    //{
        //    //    //animator.SetBool("SwingAttack", true);
        //    //    Attack(true);
        //    //}
        //    //else if (inputDevice.Action1.IsPressed)
        //    //{
        //    //    animator.SetBool("Jump", true);
        //    //}

        //    // Transition in process wait to see if it has finished
        //    if (!animator.IsInTransition(0))
        //    {
        //        // Reset so we can attack again.
        //        animator.SetBool("Roll", false);
        //    }
        //}
    }

    #endregion 

    // Handle everything here...
    void OnDPadUp(ref InControl.InputDevice device)
    {
        float speed = (device.LeftStickX.Value * device.LeftStickX.Value) + (device.LeftStickY.Value * device.LeftStickY.Value);
        speed *= movementSpeed;

        // Direction vector to hold the input key press.
        direction = new Vector3(device.LeftStickX.Value, 0, device.LeftStickY.Value).normalized;

        animator.SetFloat("Speed", speed);
    }

    void OnX(ref  InControl.InputDevice device)
    {
        Attack(true);
    }

    void OnY(ref  InControl.InputDevice device)
    {
        //Jump(true);
    }

    void OnA(ref  InControl.InputDevice device)
    {
        //Roll(true);
    }

    void OnB(ref  InControl.InputDevice device)
    {
        //Roll(true);
    }

    void Attack(bool b)
    {
        //if (currentBaseState.nameHash == movementState ||
        //    currentBaseState.nameHash == idleState)
        {
            animator.SetBool("SwingAttack", b);
            GetComponentInChildren<Weapon>().SetAttackProperties(10, Character.EDamageType.Physical);
            GetComponentInChildren<Weapon>().EnableCollision = b;
        }
    }

    void Roll(bool b)
    {
        animator.SetBool("Roll", b);
    }

    void Jump(bool b)
    {
        if (currentBaseState.nameHash == movementState)
        {
            animator.SetBool("Jump", b);
        }
    }
}
