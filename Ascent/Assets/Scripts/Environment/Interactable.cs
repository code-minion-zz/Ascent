using UnityEngine;
using System.Collections;

public class Interactable : MonoBehaviour 
{
	protected TriggerRegion triggerRegion;
	public TriggerRegion TriggerRegion
	{
		get { return triggerRegion; }
	}

	private Renderer[] renderers;
	public Renderer[] Renderers
	{
		get
		{
			return renderers;
		}
	}

	public virtual void Start()
	{
		triggerRegion = GetComponent<TriggerRegion>();

		renderers = GetComponentsInChildren<Renderer>();
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
				mat.SetFloat("_Outline", 0.003f);
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
}
