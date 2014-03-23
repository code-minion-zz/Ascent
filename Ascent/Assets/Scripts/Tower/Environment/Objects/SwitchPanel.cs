using UnityEngine;
using System.Collections;

public class SwitchPanel : MonoBehaviour 
{
	public bool isDown;

	void OnCollisionStay(Collision collision)
	{
		gameObject.renderer.material.color = Color.green;
		isDown = true;
	}

	void OnCollisionExit(Collision collision)
	{
		gameObject.renderer.material.color = Color.red;
		isDown = false;
	}
}
