using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {
	
	#region Fields
	public float doorOpenAngle = 90.0f;
	public float smoothing = 2.0f;
	
	bool isDoorOpen = false;
	bool isLocked = false;
	
	private Vector3 defaultRot;
	private Vector3 openRot;
	
	#endregion
	
	#region Properties
	
	public bool IsOpen 
	{
		get { return isDoorOpen; }
		set { isDoorOpen = value; }
	}
	
	public bool IsLocked
	{
		get { return isLocked; }
		set { isLocked = value; }
	}
	
	#endregion
	
	// Use this for initialization
	void Start () 
	{
		defaultRot = transform.eulerAngles;
		openRot = new Vector3(defaultRot.x, defaultRot.y + doorOpenAngle, defaultRot.z);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (IsOpen)
		{
			// Open the door
			transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, openRot,
				Time.deltaTime * smoothing);
		}
		else
		{
			transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, defaultRot, 
				Time.deltaTime * smoothing);			
		}
	}
	
	#region Collision
	
	void OnCollisionEnter(Collision collision)
	{
		if (collision.transform.name.Contains("Player"))
		{
			if (IsOpen == false)
			{
				/*transform.RotateAround(transform.localPosition, 
					new Vector3(0.0f, 1.0f, 0.0f), -90.0f);*/
				Debug.Log("Door opened");
				IsOpen = true;
			}
			else
			{
				/*transform.RotateAround(transform.localPosition, 
				new Vector3(0.0f, 1.0f, 0.0f), 90.0f);*/
				Debug.Log("Door closed");
				IsOpen = false;	
			}
			
			//IsOpen = false;
		}		
	}
	
	void OnCollisionExit(Collision collisionInfo)
	{
		//IsOpen = true;		
	}	
	
	#endregion
}
