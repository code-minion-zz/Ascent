using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {
	
	#region Fields
	
	bool isDoorOpen = false;
	
	#endregion
	
	#region Properties
	
	public bool IsOpen 
	{
		get { return isDoorOpen; }
		set { isDoorOpen = value; }
	}
	
	#endregion
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	#region Collision
	
	void OnCollisionEnter(Collision collision)
	{
		if (collision.transform.name.Contains("Player"))
		{
			if (IsOpen == false)
			{
				transform.RotateAround(transform.localPosition, 
					new Vector3(0.0f, 1.0f, 0.0f), -90.0f);
				Debug.Log("Door opened");
				IsOpen = true;
			}
			else
			{
				transform.RotateAround(transform.localPosition, 
				new Vector3(0.0f, 1.0f, 0.0f), 90.0f);
				Debug.Log("Door closed");
				IsOpen = false;	
			}
		}		
	}
	
	void OnCollisionExit(Collision collisionInfo)
	{
		//IsOpen = true;		
	}	
	
	#endregion
}
