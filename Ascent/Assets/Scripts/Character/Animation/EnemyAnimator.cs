using UnityEngine;
using System.Collections;

public class EnemyAnimator : CharacterAnimator 
{

    public override void PlayAnimation(int animationID)
    {
        animator.SetBool("NewAnimation", true);
        animator.SetInteger("Animation", animationID);
    }

    public override void Update()
    {
        if (animator.IsInTransition(0))
        {
            animator.SetBool("NewAnimation", false);
        }

        DebugKeys();
    }


    private KeyCode[] keycodes = new KeyCode[] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, 
                                                 KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6,
                                                 KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9};
    public void DebugKeys()
    {
        for (int i = 0; i < keycodes.Length; ++i )
        {
            if (Input.GetKeyUp(keycodes[i]))
            {
                PlayAnimation(i);
                break;
            }
        }
    }
}

