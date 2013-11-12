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

    public float movementSpeed = 10.0f;              // Movment speed
    public float rotationSmooth = 10.0f;


    static int idleState = Animator.StringToHash("Movement." + "Idle");
    static int jumpState = Animator.StringToHash("Movement." + "JumpRunning");
    static int attackState = Animator.StringToHash("CombatLayer." + "SwingSword");
    static int movementState = Animator.StringToHash("Movement." + "Movement");

    private Vector3 direction;
    private AnimatorStateInfo activeStateInfo;
    private CapsuleCollider col;
    private bool useCurves = true;						     // a setting for teaching purposes to show use of curves

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

        col = GetComponent<CapsuleCollider>();
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

            // If the hero is jumping
            if (IsActiveState(layer, jumpState))
            {
                //  ..and not still in transition..
                if (!animator.IsInTransition(layer))
                {
                    if (useCurves)
                    {
                        // ..set the collider height to a float curve in the clip called ColliderHeight
                        col.height = animator.GetFloat("ColliderHeight");
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
