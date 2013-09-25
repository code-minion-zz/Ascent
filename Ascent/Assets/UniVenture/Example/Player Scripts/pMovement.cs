using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]

public class pMovement : MonoBehaviour {

	public float movementSpeed = 4.0f;

	void FixedUpdate ()
	{
		//movement variables (frame-rate independent)
		//since from top-down perspective no movement will occur in y (vertical) direction
		//x movement value
		float keyboardX = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;
		//z movement value
		float keyboardZ = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;
		
		transform.position += new Vector3(keyboardX, 0.0f, keyboardZ);
	}
}