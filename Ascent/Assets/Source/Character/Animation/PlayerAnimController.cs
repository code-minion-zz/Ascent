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
    private AnimatorStateInfo combatLayerState;	    // a reference to the current state of the animator, used for layer 2
    private InputHandler inputHandler;              // 
    private InputDevice inputDevice;                // a reference to the input device of the player

    private CapsuleCollider col;
    //private Player player;

    public bool useCurves;						    // a setting for teaching purposes to show use of curves
    public float movementSpeed = 10.0f;              // Movment speed
    public float rotationSmooth = 10.0f;

    public bool useXboxController = false;

    static int idleState = Animator.StringToHash("Base Layer.Idle");
    static int jumpState = Animator.StringToHash("Base Layer.JumpRunning");				// and are used to check state for various actions to occur
    static int attackState = Animator.StringToHash("Base Layer.SwingRight");
    static int movementState = Animator.StringToHash("Base Layer.Movement");
    static int rollState = Animator.StringToHash("Base Layer.Roll");

    private Vector3 direction;

    #endregion

    #region Properties



    #endregion

    #region Intialization

    public override void Awake()
    {
        base.Awake();
        // Get all the components that we need.
        col = GetComponent<CapsuleCollider>();
        //player = GetComponent<Hero>();
    }

    public override void Start()
    {
        inputHandler = Game.Singleton.InputHandler;

        if (useXboxController)
            inputDevice = inputHandler.GetDevice(1);
        else
            inputDevice = inputHandler.GetDevice(0);

        if (inputDevice == null)
            inputDevice = inputHandler.GetDevice(0);

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
        if (animator.layerCount == 2)
        {
            combatLayerState = animator.GetCurrentAnimatorStateInfo(1);
        }

        float speed = (inputDevice.LeftStickX.Value * inputDevice.LeftStickX.Value) + (inputDevice.LeftStickY.Value * inputDevice.LeftStickY.Value);
        speed *= movementSpeed;

        // Direction vector to hold the input key press.
        direction = new Vector3(inputDevice.LeftStickX.Value, 0, inputDevice.LeftStickY.Value).normalized;

        animator.SetFloat("Speed", speed);

        CollisionStates();
    }

    #region Collision States

    void CollisionStates()
    {
        bool attacking = animator.GetBool("SwingAttack");
        bool jumping = animator.GetBool("Jump");
        bool rolling = animator.GetBool("Roll");

        // if we are currently in a state called Locomotion (see line 25), then allow Jump input (Space) to set the Jump bool parameter in the Animator to true
        if (currentBaseState.nameHash == movementState ||
            currentBaseState.nameHash == idleState)
        {
            if (currentBaseState.nameHash == movementState)
            {
                if (inputDevice.Action1.IsPressed)
                {
                    animator.SetBool("Jump", true);
                }
            }

            if (inputDevice.Action2.IsPressed)
            {
                animator.SetBool("SwingAttack", true);
            }

            if (inputDevice.Action3.IsPressed)
            {
                animator.SetBool("Roll", true);
            }

            if (!attacking && !jumping && !rolling)
            {
                if (direction.x != 0 || direction.z != 0)
                {
                    SmoothLookAt(transform.position + direction, rotationSmooth);
                }
            }
        }
        // if we are in the jumping state... 
        else if (currentBaseState.nameHash == jumpState)
        {
            if (inputDevice.Action2.IsPressed)
            {
                animator.SetBool("SwingAttack", true);
            }

            if (inputDevice.Action3.IsPressed)
            {
                animator.SetBool("Roll", true);
            }

            //  ..and not still in transition..
            if (!animator.IsInTransition(0))
            {
                if (useCurves)
                    // ..set the collider height to a float curve in the clip called ColliderHeight
                    col.height = animator.GetFloat("ColliderHeight");

                // reset the Jump bool so we can jump again, and so that the state does not loop 
                animator.SetBool("Jump", false);
            }

            // Raycast down from the center of the character.. 
            Ray ray = new Ray(transform.position + Vector3.up, -Vector3.up);
            RaycastHit hitInfo = new RaycastHit();

            if (Physics.Raycast(ray, out hitInfo))
            {
                // ..if distance to the ground is more than 1.75, use Match Target
                if (hitInfo.distance > 1.75f)
                {

                    // MatchTarget allows us to take over animation and smoothly transition our character towards a location - the hit point from the ray.
                    // Here we're telling the Root of the character to only be influenced on the Y axis (MatchTargetWeightMask) and only occur between 0.35 and 0.5
                    // of the timeline of our animation clip
                    animator.MatchTarget(hitInfo.point, Quaternion.identity, AvatarTarget.Root, new MatchTargetWeightMask(new Vector3(0, 1, 0), 0), 0.35f, 0.5f);
                }
            }
        }

        if (currentBaseState.nameHash == attackState)
        {
            // Transition in process wait to see if it has finished
            if (!animator.IsInTransition(0))
            {
                // Reset so we can attack again.
                animator.SetBool("SwingAttack", false);
            }
        }

        if (currentBaseState.nameHash == rollState)
        {
            if (inputDevice.Action2.IsPressed)
            {
                animator.SetBool("SwingAttack", true);
            }
            else if (inputDevice.Action1.IsPressed)
            {
                animator.SetBool("Jump", true);
            }

            // Transition in process wait to see if it has finished
            if (!animator.IsInTransition(0))
            {
                // Reset so we can attack again.
                animator.SetBool("Roll", false);
            }
        }
    }

    #endregion 
}
