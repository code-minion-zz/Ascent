using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Require these components when using this script
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class AnimatorController : MonoBehaviour
{
    #region Fields

    // Shared between all animator controllers
    protected Animator animator;
    protected Rigidbody rigidBody;

    #endregion

    #region Properties

    public Animator Animator
    {
        get { return animator; }
    }

    public Rigidbody RigidBody
    {
        get { return rigidBody; }
    }

    #endregion

    public virtual void Awake()
    {
        // Gather the components that we require.
        animator = GetComponent<Animator>();
        //col = GetComponent<CapsuleCollider>();
        rigidBody = GetComponent<Rigidbody>();

        Debug.Log( Animator);
    }

    public virtual void PlayAnimation(string animation)
    {
        animator.SetBool(animation, true);
    }

	public virtual void StopAnimation(string animation)
	{
		animator.SetBool(animation, false);
	}
}
