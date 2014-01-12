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

    public virtual void Update()
    {
        // Loop through the layers finding the current state info for each layer.
        for (int layer = 0; layer < animator.layerCount; ++layer)
        {
            // Put our active state for the layer into the dictionairy
            activeState[layer] = animator.GetCurrentAnimatorStateInfo(layer);
        }

        //// Get a reference to the Animator Controller:
        //UnityEditorInternal.AnimatorController ac = GetComponent<Animator>().runtimeAnimatorController as UnityEditorInternal.AnimatorController;
 
        //// Number of layers:
        //int layerCount = ac.layerCount;
        //Debug.Log(string.Format("Layer Count: {0}", layerCount));
 
        //// Names of each layer:
        //for (int layer = 0; layer < layerCount; layer++) {
        //    Debug.Log(string.Format("Layer {0}: {1}", layer, ac.GetLayer(layer).name));
        //}
 
        //// States on layer 0:
        //UnityEditorInternal.StateMachine sm = ac.GetLayer(0).stateMachine;

        //for (int i = 0; i < sm.stateCount; ++i)
        //{
        //    if (sm.GetState(i).uniqueName == "Movement.WarStomp")
        //    {
        //        Debug.Log(string.Format("State: {0}", sm.GetState(i).uniqueName));
        //    }
        //}
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
       // animator.SetBool(animation, true);
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
