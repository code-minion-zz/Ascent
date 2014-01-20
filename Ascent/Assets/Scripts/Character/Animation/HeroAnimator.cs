using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeroAnimator : AnimatorController  
{
    #region Enums

    // TODO: Use these states somewhere.
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

    private Vector3 direction;
    private Vector3 gravityVelocity = Vector3.zero;

    #endregion

    public float MovementSpeed
    {
        get { return movementSpeed; }
        set { movementSpeed = value; }
    }

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
	public override void FixedUpdate () 
    {
        base.FixedUpdate();

        gravityVelocity += Physics.gravity * Time.deltaTime;

        for (int layer = 0; layer < layerCount; ++layer)
        {
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(layer);
            // Check if we are in the movement or idle state.

            // We want the hero to take a hit and stop it
            // when the transition ends.
            if (state.IsName("TakingHit"))
            {
                if (animator.IsInTransition(layer))
                {

                }
                else
                {
                    TakeHit = false;
                }
            }
        }
	}

    #region animations

    //void OnAnimatorMove()
    //{
    //    //Vector3 deltaPos = animator.deltaPosition;
    //    //deltaPos += gravityVelocity * Time.deltaTime;

    //    //if ((controller.Move(deltaPos) & CollisionFlags.Below) != 0)
    //    //{
    //    //    gravityVelocity = Vector3.zero;
    //    //}

    //    //transform.Rotate(animator.deltaRotation.eulerAngles);
    //}

    public void AnimMove(Vector3 direction, float speed)
    {
        //this.direction = direction;
        animator.SetFloat("Speed", speed);
    }

    #endregion
}
