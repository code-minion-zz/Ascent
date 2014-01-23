using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(CharacterAnimator))]
[RequireComponent(typeof(CharacterMotor))]
[RequireComponent(typeof(Shadow))]
public abstract class BaseCharacter : MonoBehaviour
{
	protected CharacterMotor motor;
	public CharacterMotor Motor
	{
		get { return motor; }
		protected set { motor = value; }
	}

	protected CharacterAnimator animator;
	public CharacterAnimator Animator
	{
		get { return animator; }
		protected set { animator = value; }
	}

	protected Color originalColour = Color.white;
	public Color OriginalColor
	{
		get { return originalColour; }
		set { originalColour = value; }
	}

	public virtual void Initialise()
	{
		Shadow shadow = GetComponentInChildren<Shadow>();
		if (shadow == null)
		{
			Debug.LogError("No Shadow attached to " + name, this);
		}
		shadow.Initialise();

		motor = GetComponentInChildren<CharacterMotor>();
		if (motor == null)
		{
			Debug.LogError("No motor attached to " + name, this);
		}
		motor.Initialise();

		animator = GetComponentInChildren<CharacterAnimator>();
		if (animator == null)
		{
			Debug.LogError("No animator attached to " + name, this);
		}
		animator.Initialise();

		SetColor(OriginalColor);
	}

	public virtual void SetColor(Color color)
	{
		Renderer[] renderers = GetComponentsInChildren<Renderer>();
		foreach (Renderer render in renderers)
		{
			render.material.color = color;
		}
	}

	public virtual void ResetColor()
	{
		Renderer[] renderers = GetComponentsInChildren<Renderer>();
		foreach (Renderer render in renderers)
		{
			render.material.color = originalColour;
		}
	}

	public virtual void Update()
	{
		// Override
	}
}
