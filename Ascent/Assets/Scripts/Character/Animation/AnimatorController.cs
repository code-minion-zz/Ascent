using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Require these components when using this script
[RequireComponent(typeof(Animator))]
public class AnimatorController : MonoBehaviour
{
    #region Fields

    // Shared between all animator controllers
    protected Animator animator;
    protected int layerCount;
    protected Dictionary<int, AnimatorStateInfo> activeState = new Dictionary<int, AnimatorStateInfo>();

    #endregion

    #region Properties

    public Animator Animator
    {
        get { return animator; }
    }

    #endregion

    #region Initialize

    public virtual void Awake()
    {
        // Gather the components that we require.
        animator = GetComponent<Animator>();

        layerCount = animator.layerCount;
    }

    public virtual void Start()
    {

    }

    #endregion

    //public virtual void Update()
    //{
    //    // Loop through the layers finding the current state info for each layer.
    //    for (int layer = 0; layer < animator.layerCount; ++layer)
    //    {
    //        // Put our active state for the layer into the dictionairy
    //        activeState[layer] = animator.GetCurrentAnimatorStateInfo(layer);
    //    }
    //}

    public virtual void FixedUpdate()
    {
        // Loop through the layers finding the current state info for each layer.
        for (int layer = 0; layer < animator.layerCount; ++layer)
        {
            // Put our active state for the layer into the dictionairy
            activeState[layer] = animator.GetCurrentAnimatorStateInfo(layer);
        }
    }

    // Check if the active state for the given layer is the state
    // that we want to check with the nameHash
    public bool IsActiveState(int layer, int nameHash)
    {
        if (activeState[layer].nameHash == nameHash)
        {
            return true;
        }

        return false;
    }

    public virtual void PlayAnimation(string animation)
    {
        animator.SetBool(animation, true);
    }

    /// <summary>
    /// Plays the desired state immediately, however does not alter any state variables such as trigger bools
    /// </summary>
    /// <param name="nameHash">The name of the state.</param>
    public virtual void PlayImmediately(string nameHash)
    {
        animator.Play(Animator.StringToHash(nameHash));
    }

	public virtual void StopAnimation(string animation)
	{
		animator.SetBool(animation, false);
	}
}
