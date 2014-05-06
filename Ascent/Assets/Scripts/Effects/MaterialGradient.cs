using UnityEngine;
using System.Collections;

public class MaterialGradient : MonoBehaviour 
{
	public Gradient gradient;
	public float timeMultiplier = 1.0f;

	private Color curColor;
	private float time;

	void Start ()
	{
		renderer.material.SetColor ("_TintColor", new Color(0, 0, 0, 0));
	}

	void Update () 
	{
		time +=Time.deltaTime*timeMultiplier;
		curColor = gradient.Evaluate(time);

		renderer.material.SetColor ("_TintColor", curColor);
	}
}
