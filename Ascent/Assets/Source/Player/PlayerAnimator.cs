using UnityEngine;
using System.Collections;

public struct AscentAnimation
{
    public string name;
    public bool loop;
    public bool interrupt;
}

public class PlayerAnimator : MonoBehaviour
{
    public enum EAnimState
    {
        Invalid = -1,
        Idle,   // CombatModeA
        Walk,   // Walk
        Run,    // Run
        Jump,   // JumpRunning
        Strike, // SwingRight
        Hit,    // TakingHit
        Die,    // Dying
        GetUp,  // GetUp
        Max,
    }

    Animation anim;
    EAnimState currentAnim = EAnimState.Idle;
    EAnimState queuedAnim = EAnimState.Invalid;
    AscentAnimation[] animations = new AscentAnimation[(int)EAnimState.Max];

	// Use this for initialization
	void Start () 
    {
        anim = GetComponentInChildren<Animation>();

        animations[0].name = "CombatModeA";
        animations[0].loop = true;
        animations[0].interrupt = true;

        animations[1].name = "Walk";
        animations[1].loop = false;
        animations[1].interrupt = true;
        
        animations[2].name = "Run";
        animations[2].loop = false;
        animations[2].interrupt = true;

        animations[3].name = "JumpRunning";
        animations[3].loop = false;
        animations[3].interrupt = false;

        animations[4].name = "SwingRight";
        animations[4].loop = false;
        animations[4].interrupt = false;

        animations[5].name = "TakingHit";
        animations[5].loop = false;
        animations[5].interrupt = true;

        animations[6].name = "Dying";
        animations[6].loop = false;
        animations[6].interrupt = true;

        animations[7].name = "GetUp";
        animations[7].loop = false;
        animations[7].interrupt = true;
	}
	
	// Update is called once per frame
	void Update () 
    {
        TestAnimations();

        // Check for new animation
        if (queuedAnim != EAnimState.Invalid)
        {
            // Check if current anim can be interupted before replacing it
            if (animations[(int)currentAnim].interrupt)
            {
                currentAnim = queuedAnim;
                queuedAnim = EAnimState.Invalid;
                anim.Play(animations[(int)currentAnim].name);
            }
            else if (!anim.isPlaying)
            {
                currentAnim = queuedAnim;
                queuedAnim = EAnimState.Invalid;
                anim.Play(animations[(int)currentAnim].name);
            }

            return;
        }

        // Check to see if this animation needs to be looped
        if (animations[(int)currentAnim].loop)
        {
            if (!anim.isPlaying)
            {
                Debug.Log(animations[(int)currentAnim].name);
                anim.Play(animations[(int)currentAnim].name);
            }
        }

        // Check if animation has ended and needs to fade back into idle.
        else
        {
            if (!anim.isPlaying)
            {
                anim.Play(animations[(int)0].name);
            }
        }
	}

    public void PlayAnimation(EAnimState _animation)
    {
        queuedAnim = _animation;
    }

    void TestAnimations()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            queuedAnim = (EAnimState)0;
            Debug.Log(currentAnim.ToString());
        }
        else if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            queuedAnim = (EAnimState)1;
            Debug.Log(currentAnim.ToString());
        }
        else if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            queuedAnim = (EAnimState)2;
            Debug.Log(currentAnim.ToString());
        }
        else if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            queuedAnim = (EAnimState)3;
            Debug.Log(currentAnim.ToString());
        }
        else if (Input.GetKeyUp(KeyCode.Alpha5))
        {
            queuedAnim = (EAnimState)4;
            Debug.Log(currentAnim.ToString());
        }
        else if (Input.GetKeyUp(KeyCode.Alpha6))
        {
            queuedAnim = (EAnimState)5;
            Debug.Log(currentAnim.ToString());
        }
        else if (Input.GetKeyUp(KeyCode.Alpha7))
        {
            queuedAnim = (EAnimState)6;
            Debug.Log(currentAnim.ToString());
        }
        else if (Input.GetKeyUp(KeyCode.Alpha8))
        {
            queuedAnim = (EAnimState)7;
            Debug.Log(currentAnim.ToString());
        }
    }
}
