using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Require these components when using this script
[RequireComponent(typeof(Animator))]
public class CharacterAnimator : MonoBehaviour
{
    protected Animator animator;

	protected bool takeHit = false;
	protected bool dying = false;

	protected bool initialised = false;
    public bool hasAnimations = true;

	//Dictionary<string, bool> states = new Dictionary<string, bool>();

	protected string currentAnim;

#if UNITY_EDITOR
	private List<string> stateNames;
#endif

	/// <summary>
	/// Returns if the animator is taking a hit. Sets the animator to take a hit.
	/// </summary>
    //public bool TakeHit
    //{
    //    get { return takeHit; }
    //    set
    //    {
    //        //takeHit = value;
			
    //        //animator.SetBool("TakeHit", value);

    //        // Comment out this block and uncomment the above to go back to the old method 
    //        //if (value)
    //        //{
    //        //    StartCoroutine("PlayOneShot", "TakeHit");
    //        //}
    //        takeHit = false;
    //    }
    //}

	/// <summary>
	/// Returns if the animator is dying. Sets the animator to the dying state.
	/// </summary>
	public bool Dying
	{
		get { return dying; }
		set
		{
			dying = value;
			animator.SetBool("Dying", value);
		}
	}

    public virtual void Initialise()
    {
		#if UNITY_EDITOR
		InitialiseValidation();
		#endif

		// Gather the components that we require.
		animator = GetComponent<Animator>();

		initialised = true;
    }

	public virtual void Update()
	{

	}

    public virtual void PlayAnimation(string anim)
    {
		currentAnim = anim;
        if (hasAnimations)
        {
            animator.SetBool(anim, true);
        }
    }

	public virtual void PlayAnimation(int animHash)
	{
		animator.SetBool(animHash, true);
	}

    /// <summary>
    /// Plays the desired state immediately, however does not alter any state variables such as trigger bools
    /// </summary>
    /// <param name="nameHash">The name of the state.</param>
    public virtual void PlayImmediately(string nameHash)
    {
		animator.Play(nameHash);
    }

	/// <summary>
	/// Plays the desired state immediately, however does not alter any state variables such as trigger bools
	/// </summary>
	/// <param name="nameHash">The name of the state.</param>
	public virtual void PlayImmediately(int animHash)
	{
		animator.Play(animHash);
	}

	public virtual void StopAnimation(string anim)
	{
        if (hasAnimations)
        {
            animator.SetBool(anim, false);
        }
	}

	public virtual void StopAnimation(int animHash)
	{
		animator.SetBool(animHash, false);
	}

	public virtual void PlayAnimation(string anim, float f)
	{
		animator.SetFloat(anim, f);
	}

	public virtual void PlayAnimation(int animHash, float f)
	{
		animator.SetFloat(animHash, f);
	}

	public IEnumerator PlayOneShot(string anim)
	{
		animator.SetBool(anim, true);

		yield return null;

		animator.SetBool(anim, false);
	}

	public IEnumerator PlayOneShot(int animHash)
	{
		animator.SetBool(animHash, true);

		yield return null;

		animator.SetBool(animHash, false);
	}


#if UNITY_EDITOR
	public void InitialiseValidation()
	{
		stateNames = new List<string>();

		UnityEditorInternal.AnimatorController ac = GetComponent<Animator>().runtimeAnimatorController as UnityEditorInternal.AnimatorController;

		if (ac == null)
		{
			return;
		}
		
		int paramCount = ac.parameterCount;
		for (int i = 0; i < paramCount; ++i)
		{
			stateNames.Add(ac.GetParameter(i).name);
		}

		//// Uncomment this for statenames
		//int layerCount = ac.layerCount;
		//for (int layer = 0; layer < layerCount; layer++)
		//{
		//    UnityEditorInternal.AnimatorControllerLayer animLayer = ac.GetLayer(layer);
		//    UnityEditorInternal.StateMachine sm = animLayer.stateMachine;


		//    string layerName = animLayer.name;
		//    int strLength = layerName.Length;

		//    int stateCount = sm.stateCount;
		//    for (int state = 0; state < stateCount; ++state)
		//    {
		//        string stateName = sm.GetState(state).uniqueName;
		//        stateName = stateName.Substring(strLength + 1, stateName.Length - strLength - 1 );
		//        stateNames.Add(stateName);
		//    }
		//}	
	}

	public bool DoesStateExist(string anim)
	{
		//string str = stateNames.Find(x => x == anim);
		//if (str == null)
		//{
		//    Debug.LogError(string.Format("Animation state does not exist: {0}", anim));
		//    return false;
		//}
		return true;
	}
#endif 
}
