using UnityEngine;
using System.Collections;
using InControl;

public class Player 
{
    public int health = 100;

	#region Fields
	
	// Private member variables.
	private Transform transform;
	// Player identifier
	private int playerId = 0;
	// Movement speed variables
	public float movementSpeed = 5.0f;		
	// Handling input for this player.
	private InputHandler inputHandler;
	
	// Set Object transform.
	[HideInInspector]
	public Transform ObjectTransform
	{
		get { return transform; }
		set { transform = value; }
	}
	
	// Set the position of the players transform.
	[HideInInspector]
	public Vector3 Position
	{
		get { return transform.position; }
		set { transform.position = value; }
	}
	
	#endregion
	
	// Constructor
	public Player(int playerId)
	{
		this.playerId = playerId;
	}

	// Use this for initialization
	public void Start () 
	{
		// Get a reference to our unity GameObject so we can ulter the materials
		GameObject obj = transform.gameObject;
		// Get the input handler component for this transform.
		inputHandler = GameObject.Find("Game").GetComponent<InputHandler>();
		
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
		InputDevice inputDevice = inputHandler.GetDevice(playerId);
		
		if (inputDevice != null)
		{
			// Update the transform by the movement
			if (inputDevice.Action1.IsPressed)
			{
				Debug.Log("Action One: " + playerId);
			}
			
			float x = inputDevice.LeftStickX.Value * Time.deltaTime * movementSpeed;
			float z = inputDevice.LeftStickY.Value * Time.deltaTime * movementSpeed;
			transform.Translate(x, 0, z);
		}
		else
		{
			// Error no device
			Debug.Log("No Device for this player");
		}
	}

    public void TakeDamage(int _damage)
    {
        health -= _damage;

        if (health <= 0)
        {
            transform.gameObject.renderer.material.color = Color.black;
        }
    }
}
