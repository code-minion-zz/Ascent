using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeroAnimator : CharacterAnimator  
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
	
	// Update is called once per frame
	public override void Update() 
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        // Check if we are in the movement or idle state.

        if (state.IsName("WhirlWind"))
        {
            if (!animator.IsInTransition(0))
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

        // We want the hero to take a hit and stop it
        // when the transition ends.
        if (state.IsName("TakingHit"))
        {
            if (animator.IsInTransition(0))
            {

            }
            else
            {
                TakeHit = false;
                //StopAnimation("TakeHit");
            }
        }

#if UNITY_EDITOR
        DebugKeys();
#endif
	}

#if UNITY_EDITOR
    void DebugKeys()
    {
        //if(Input.GetKeyUp(KeyCode.Alpha1))
        //{
        //    animator.Play("");
        //}
        //else if (Input.GetKeyUp(KeyCode.Alpha2))
        //{
        //}
        //else if (Input.GetKeyUp(KeyCode.Alpha3))
        //{
        //}
        //else if (Input.GetKeyUp(KeyCode.Alpha4))
        //{
        //}
        //else if (Input.GetKeyUp(KeyCode.Alpha5))
        //{
        //}
        //else if (Input.GetKeyUp(KeyCode.Alpha6))
        //{
        //}
    }
#endif

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
