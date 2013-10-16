using UnityEngine;
using System.Collections;
using InControl;

// Require these components when using this script
[RequireComponent(typeof (Animator))]
[RequireComponent(typeof (CapsuleCollider))]
[RequireComponent(typeof (Rigidbody))]
public class PlayerAnimController : MonoBehaviour
{
    private Animator anim;							// a reference to the animator on the character
    private AnimatorStateInfo currentBaseState;		// a reference to the current state of the animator, used for base layer
    private AnimatorStateInfo combatLayerState;	    // a reference to the current state of the animator, used for layer 2
    private CapsuleCollider col;					// a reference to the capsule collider of the character

    public float animSpeed = 1.5f;				    // a public setting for overall animator animation speed
    public float lookSmoother = 3f;				    // a smoothing setting for camera motion
    public bool useCurves;						    // a setting for teaching purposes to show use of curves

    public bool useXboxController = false;

    static int idleState = Animator.StringToHash("Base Layer.Idle");
    static int jumpState = Animator.StringToHash("Base Layer.JumpRunning");				// and are used to check state for various actions to occur
    static int attackState = Animator.StringToHash("Base Layer.SwingRight");
    static int movementState = Animator.StringToHash("Base Layer.Movement");
    static int rollState = Animator.StringToHash("Base Layer.Roll");

    private InputHandler inputHandler;
    private InputDevice inputDevice;

    private Vector3 direction;
    private Player player;
 

    void Awake()
    {
        player = GetComponent<Player>();
    }

    void Start()
    {
        // initialising reference variables
        anim = GetComponent<Animator>();
        col = GetComponent<CapsuleCollider>();

        inputHandler = Game.Singleton.InputHandler;

        if (useXboxController)
            inputDevice = inputHandler.GetDevice(1);
        else
            inputDevice = inputHandler.GetDevice(0);

        if (inputDevice == null)
            inputDevice = inputHandler.GetDevice(0);

        if (anim.layerCount == 2)
            anim.SetLayerWeight(1, 1);
    }

    void FixedUpdate()
    {
        // Set our currentState variable to the current state of the Base Layer (0) of animation
        currentBaseState = anim.GetCurrentAnimatorStateInfo(0);

        // Set our combatLayerState variable to the current state of the second Layer (1) of animation
        if (anim.layerCount == 2)
        {
            combatLayerState = anim.GetCurrentAnimatorStateInfo(1);
        }

        // Direction vector to hold the input key press.
        direction = new Vector3(inputDevice.LeftStickX.Value, 0, inputDevice.LeftStickY.Value).normalized;
        
        // Set the speed value based on the pressure push direction on the joystick
        float speed = direction.magnitude;		
        anim.SetFloat("Speed", speed);

        bool attacking = anim.GetBool("SwingAttack");
        bool jumping = anim.GetBool("Jump");
        bool rolling = anim.GetBool("Roll");
        

        // if we are currently in a state called Locomotion (see line 25), then allow Jump input (Space) to set the Jump bool parameter in the Animator to true
        if (currentBaseState.nameHash == movementState || 
            currentBaseState.nameHash == idleState)
        {
            if (currentBaseState.nameHash == movementState)
            {
                if (inputDevice.Action1.IsPressed)
                {
                    anim.SetBool("Jump", true);
                }
            }

            if (inputDevice.Action2.IsPressed)
            {
                anim.SetBool("SwingAttack", true);
            }

            if (inputDevice.Action3.IsPressed)
            {
                anim.SetBool("Roll", true);
            }

            if (!attacking && !jumping && !rolling)
            {
                if (direction.x != 0 || direction.z != 0)
                {
                    transform.LookAt(transform.position + direction);
                }
            }
        }
        // if we are in the jumping state... 
        else if (currentBaseState.nameHash == jumpState)
        {
            if (inputDevice.Action2.IsPressed)
            {
                anim.SetBool("SwingAttack", true);
            }

            if (inputDevice.Action3.IsPressed)
            {
                anim.SetBool("Roll", true);
            }

            //  ..and not still in transition..
            if (!anim.IsInTransition(0))
            {
                if (useCurves)
                    // ..set the collider height to a float curve in the clip called ColliderHeight
                    col.height = anim.GetFloat("ColliderHeight");

                // reset the Jump bool so we can jump again, and so that the state does not loop 
                anim.SetBool("Jump", false);
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
                    anim.MatchTarget(hitInfo.point, Quaternion.identity, AvatarTarget.Root, new MatchTargetWeightMask(new Vector3(0, 1, 0), 0), 0.35f, 0.5f);
                }
            }
        }

        if (currentBaseState.nameHash == attackState)
        {
            // Transition in process wait to see if it has finished
            if (!anim.IsInTransition(0))
            {
                // Reset so we can attack again.
                anim.SetBool("SwingAttack", false);
            }
        }

        if (currentBaseState.nameHash == rollState)
        {
            if (inputDevice.Action2.IsPressed)
            {
                anim.SetBool("SwingAttack", true);
            }
            else if (inputDevice.Action1.IsPressed)
            {
                anim.SetBool("Jump", true);
            }

            // Transition in process wait to see if it has finished
            if (!anim.IsInTransition(0))
            {
                // Reset so we can attack again.
                anim.SetBool("Roll", false);
            }
        }
    }
}
