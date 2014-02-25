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

	private Renderer[] renderers;
	public Renderer[] Renderers
	{
		get
		{
			return renderers;
		}
	}

	public virtual void Initialise()
	{
		renderers = GetComponentsInChildren<Renderer>();
		
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

		SetColor(OriginalColor);
	}

	public virtual void SetColor(Color color)
	{
		if (renderers != null)
		{
			foreach (Renderer render in renderers)
			{
				render.material.color = color;
			}
		}
	}

	public virtual void ResetColor()
	{
		if (renderers != null)
		{
			foreach (Renderer render in renderers)
			{
				render.material.color = originalColour;
			}
		}
	}

	public virtual void Update()
	{
		// Override
	}
}
