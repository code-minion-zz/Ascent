using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Require these components when using this script
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class AnimatorController : MonoBehaviour
{
    #region Fields

    // Shared between all animator controllers
    protected Animator animator;
    protected CapsuleCollider col;
    protected Rigidbody rigidBody;

    #endregion

    #region Properties

    public Animator Animator
    {
        get { return animator; }
    }

    public CapsuleCollider Collisder
    {
        get { return col; }
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
        col = GetComponent<CapsuleCollider>();
        rigidBody = GetComponent<Rigidbody>();
    }

    public virtual void Start()
    {

    }

    public virtual void Update()
    {

    }
}
