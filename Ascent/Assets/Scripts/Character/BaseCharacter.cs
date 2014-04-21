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

	protected Transform model;
	public Transform Model
	{
		get { return model; }
		protected set { model = value; }
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

	protected Shadow shadow;

	public virtual void Initialise()
	{
		renderers = GetComponentsInChildren<Renderer>();

		motor = GetComponentInChildren<CharacterMotor>();
		if (motor == null)
		{
			Debug.LogError("No motor attached to " + name, this);
		}
		motor.Initialise();

		model = transform.FindChild("Model");
		if (motor == null)
		{
			Debug.LogError("No model attached to " + name, this);
		}

		SetColor(OriginalColor);
	}

	public virtual void SetColor(Color color)
	{
		//if (renderers != null)
		//{
		//    foreach (Renderer render in renderers)
		//    {
		//        render.material.color = color;
		//    }
		//}
	}

	public virtual void ResetColor()
	{
		//if (renderers != null)
		//{
		//    foreach (Renderer render in renderers)
		//    {
		//        render.material.color = originalColour;
		//    }
		//}
	}

	public void EnableHighlight(Color color)
	{
		Renderer[] renderers = Renderers;
		foreach (Renderer render in renderers)
		{
			foreach (Material mat in render.materials)
			{
				mat.shader = Shader.Find("Outlined/Diffuse");
				mat.SetColor("_OutlineColor", color);
				//mat.SetColor("_Color", color);
			}
		}
	}

	public void StopHighlight()
	{
		Renderer[] renderers = Renderers;
		foreach (Renderer render in renderers)
		{
			foreach (Material mat in render.materials)
			{
				mat.shader = Shader.Find("Diffuse");
			}
		}
	}

	public virtual void Update()
	{
		// Override
	}
}
