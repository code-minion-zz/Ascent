using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]
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

    // Movement speed this can be ultered to change the movement speed of the hero. Changing this will make the hero 
    // walk or run depending on the value. Lower value is closer to walk.
    public float movementSpeed = 10.0f;

    // The following are movement layer states
    // We have a few movement states for the hero which are idle, jumping and running.
    //static int idleState = Animator.StringToHash("Movement" + "Idle");
    //static int jumpState = Animator.StringToHash("Movement" + "JumpRunning");
    //static int movementState = Animator.StringToHash("Movement" + "Movement");

    // Combat layer states
    // Taking a hit, swinging the sword and general combat mode.
    //static int combatState = Animator.StringToHash("CombatLayer." + "CombatMode");
    //static int attackState = Animator.StringToHash("Movement." + "SwingSword");
    //static int takingHit = Animator.StringToHash("Movement." + "TakingHit");
    //static int whirlWindAttack = Animator.StringToHash("Movement." + "WhirlWind");

    private Vector3 direction;
    private CharacterController controller;
    private bool useCurves = true;
    private Vector3 gravityVelocity = Vector3.zero;

    #endregion

    #region Properties

    public float MovementSpeed
    {
        get { return movementSpeed; }
        set { movementSpeed = value; }
    }

    public CharacterController Controller
    {
        get { return controller; }
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

        // Select the collider component that we will use.
        controller = GetComponent<CharacterController>();
	}

	void OnAnimationEvent()
	{

	}

    public void AnimationBegin()
    {
        Debug.Log("War Stomp Begin");
    }

	// Events called by the animation
    public void EnableWeaponCollider()
	{

        //Debug.Log("Enable Collider ");
        
	}

	public void DisableWeaponCollider()
	{
		//Debug.Log ("Disable collider");
	}
	
	// Update is called once per frame
	public override void Update () 
    {
        base.Update();

        gravityVelocity += Physics.gravity * Time.deltaTime;

        bool takeHit = animator.GetBool("TakeHit");
        bool whirlWind = animator.GetBool("Whirlwind");
        bool dying = animator.GetBool("Dying");

        for (int layer = 0; layer < layerCount; ++layer)
        {
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(layer);
            // Check if we are in the movement or idle state.
            if (state.IsName("Movement") ||
                state.IsName("Idle"))
            {
                // We want to only change the direction of the player when we can.
                if (!takeHit && !whirlWind && !dying)
                {
                    transform.LookAt(transform.position + direction);
                }
            }

            if (state.IsName("WhirlWind"))
            {
                if (!animator.IsInTransition(layer))
                {
                    // While in transition
                    // We don't want to take hit.
                    StopAnimation("TakeHit");
                }
                else
                {
                    StopAnimation("Whirlwind");
                }
            }
		

            // If the active state is the attack state
            if (state.IsName("SwingSword"))
            {
                if (!animator.IsInTransition(layer))
                {
                    // Here we can get the normalized time. big BOI
                    //Debug.Log("Swing attack Interval: " + animator.GetCurrentAnimatorStateInfo(layer).normalizedTime % 1.0f);
                }
                else
                {
                    //StopAnimation("SwingAttack");
                }
            }

            // We want the hero to take a hit and stop it
            // when the transition ends.
            if (state.IsName("TakingHit"))
            {
                if (!animator.IsInTransition(layer))
                {

                }
                else
                {
                    StopAnimation("TakeHit");
                }
            }

            // If the hero is jumping
            if (state.IsName("JumpRunning"))
            {
                //  ..and not still in transition..
                if (!animator.IsInTransition(layer))
                {
                    if (useCurves)
                    {
                        // ..set the collider height to a float curve in the clip called ColliderHeight
                        controller.height = animator.GetFloat("ColliderHeight");
                    }

                    // reset the Jump bool so we can jump again, and so that the state does not loop 
                    //animator.SetBool("Jump", false);
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
        }
	}

    #region animations

    void OnAnimatorMove()
    {
        Vector3 deltaPos = animator.deltaPosition;
        deltaPos += gravityVelocity * Time.deltaTime;

        if ((controller.Move(deltaPos) & CollisionFlags.Below) != 0)
        {
            gravityVelocity = Vector3.zero;
        }

        transform.Rotate(animator.deltaRotation.eulerAngles);
    }

    public void AnimMove(Vector3 direction, float speed)
    {
        this.direction = direction;
        animator.SetFloat("Speed", speed);
    }

    #endregion
}
