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
    protected int layerCount;
    protected Dictionary<int, AnimatorStateInfo> activeState = new Dictionary<int, AnimatorStateInfo>();

    //protected Dictionary<int, List<UnityEditorInternal.State>> layerStates = new Dictionary<int, List<UnityEditorInternal.State>>();
    //protected UnityEditorInternal.AnimatorController unityAnimController;

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

    #region Initialize

    public virtual void Awake()
    {
        // Gather the components that we require.
        animator = GetComponent<Animator>();
        //col = GetComponent<CapsuleCollider>();
        rigidBody = GetComponent<Rigidbody>();

        layerCount = animator.layerCount;
    }

    public virtual void Start()
    {
        //unityAnimController = GetComponent<Animator>().runtimeAnimatorController as UnityEditorInternal.AnimatorController;

        //if (unityAnimController == null)
        //    return;

        //// Get the number of layers on this animator controller.
        //layerCount = unityAnimController.layerCount;

        //// Go through the layers and find the controller for the layer. We then find the state machine from
        //// this layer and populate all the states in our dictionairy.
        //for (int layer = 0; layer < layerCount; layer++)
        //{
        //    // Grab the controller layer and the state machine associated with it.
        //    UnityEditorInternal.AnimatorControllerLayer controllerLayer = unityAnimController.GetLayer(layer);
        //    UnityEditorInternal.StateMachine sm = controllerLayer.stateMachine;

        //    // Grab all the states
        //    List<UnityEditorInternal.State> states = new List<UnityEditorInternal.State>();

        //    // Grab all the states on this layer
        //    for (int i = 0; i < sm.stateCount; ++i)
        //    {
        //        states.Add(sm.GetState(i));
        //    }

        //    layerStates.Add(layer, states);
        //}
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

	public virtual void StopAnimation(string animation)
	{
		animator.SetBool(animation, false);
	}
}
