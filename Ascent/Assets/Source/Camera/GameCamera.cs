using UnityEngine;
using System.Collections;

/// <summary>
/// The camera that views the game and moves to follow the players.
/// </summary> 
public class GameCamera : MonoBehaviour
{
	#region Properties
	// Plug the unity camera into here
	public Transform cameraTransform;
	
	// Movement speed of the character
	public float movementSpeed = 5.0f;
	#endregion
	
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Update the transform by the movement
		float x = Input.GetAxis("Horizontal") * Time.deltaTime * movementSpeed;
		float z = Input.GetAxis("Vertical") * Time.deltaTime * movementSpeed;
		transform.Translate(x, 0, z);
	}
}
