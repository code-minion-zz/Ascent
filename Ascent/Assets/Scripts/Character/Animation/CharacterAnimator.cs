using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[RequireComponent(typeof(Animator))]
public class CharacterAnimator : MonoBehaviour
{
    protected Animator animator;

	protected bool takeHit = false;
	protected bool dying = false;

	protected bool initialised = false;
    public bool hasAnimations = true;

	protected string currentAnim;

#if UNITY_EDITOR
	private List<string> stateNames;
#endif

	/// <summary>
	/// Returns if the animator is dying. Sets the animator to the dying state.
	/// </summary>
	public bool Dying
	{
		get { return dying; }
		set
		{
			dying = value;
		}
	}

    public virtual void Initialise()
    {
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
}
