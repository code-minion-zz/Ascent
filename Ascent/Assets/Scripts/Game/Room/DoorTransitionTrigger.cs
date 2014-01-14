using UnityEngine;
using System.Collections;

public class DoorTransitionTrigger : MonoBehaviour 
{
	 
	public void OnTriggerStay(Collider Col)
	{
		Debug.Log("Enter");
	}

	public void OnTriggerExit(Collider Col)
	{
		Debug.Log("Exit");
	}
}
