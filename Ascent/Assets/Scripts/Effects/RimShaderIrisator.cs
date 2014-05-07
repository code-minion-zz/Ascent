using UnityEngine;
using System.Collections;

public class RimShaderIrisator : MonoBehaviour 
{
	public float topStr=2.0f;
	public float botStr = 1.0f;
	public float minSpeed = 1.0f;
	public float maxSpeed = 1.0f;
	private float speed=0.0f;
	private float timeGoes;
	private bool  timeGoesUp=true;

	void  RandomizeSpeed ()
	{
		speed = Random.Range(minSpeed, maxSpeed);
	}

	void  Start ()
	{
		timeGoes = botStr;
		speed = Random.Range(minSpeed, maxSpeed);
	}

	void  Update ()
	{
		if (timeGoes > topStr)
		{
			timeGoesUp = false;
			RandomizeSpeed();
		}

		if (timeGoes<botStr)
		{
			timeGoesUp = true;
			RandomizeSpeed();
		}

		if (timeGoesUp)
		{
			timeGoes += Time.deltaTime * speed;
		}

		if (!timeGoesUp)
		{
			timeGoes -= Time.deltaTime * speed;
		}

		float currStr = timeGoes;

		renderer.material.SetFloat( "_AllPower", currStr );
	}
}
