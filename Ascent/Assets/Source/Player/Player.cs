using UnityEngine;
using System.Collections;

public class Player 
{
	#region Fields
	
	public Vector3 position;
	private Transform transform;
	private int playerId = 0;
	public float movementSpeed = 5.0f;
	
	// Set Object transform
	public Transform ObjectTransform
	{
		get { return transform; }
		set { transform = value; }
	}
	
	#endregion
	
	public Player(int playerId)
	{
		this.playerId = playerId;
	}

	// Use this for initialization
	public void Start () 
	{
		GameObject obj = transform.gameObject;
		
		switch (playerId)
		{
		case 0:
			obj.renderer.material.color = Color.red;
			break;
			
		case 1:
			obj.renderer.material.color = Color.green;
			break;
			
		case 2:
			obj.renderer.material.color = Color.blue;
			break;
			
		default:
			obj.renderer.material.color = Color.white;
			break;
		}
	}
	
	// Update is called once per frame
	public void Update () 
	{
		// Update the transform by the movement
		float x = Input.GetAxis("Horizontal") * Time.deltaTime * movementSpeed;
		float z = Input.GetAxis("Vertical") * Time.deltaTime * movementSpeed;
		transform.Translate(x, 0, z);
		
		// Update internal position
		position = transform.position;
	}
	
	public void Draw() {
		
	}
}
