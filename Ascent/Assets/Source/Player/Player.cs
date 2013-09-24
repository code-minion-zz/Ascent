using UnityEngine;
using System.Collections;

public class Player
{
	#region Fields
	
	public Vector3 position = new Vector3();
	public GameObject gameObject;
	//private Transform transform;
	public int playerId = 0;
	public float movementSpeed = 5.0f;
	
	
	#endregion
	
	public Player(int playerId)
	{
		this.playerId = playerId;
	}

	// Use this for initialization
	public void Start () 
	{
		//GameObject obj = transform.gameObject;
		
		switch (playerId)
		{
		case 0:
			gameObject.renderer.material.color = Color.red;
			break;
			
		case 1:
			gameObject.renderer.material.color = Color.green;
			break;
			
		case 2:
			gameObject.renderer.material.color = Color.blue;
			break;
			
		default:
			gameObject.renderer.material.color = Color.white;
			break;
		}
	}
	
	// Update is called once per frame
	public void Update () 
	{
		// Update the transform by the movement
		float x = Input.GetAxis("Horizontal") * Time.deltaTime * movementSpeed;
		float z = Input.GetAxis("Vertical") * Time.deltaTime * movementSpeed;
		//transform.Translate(x, 0, z);
		Vector3 prevPos = gameObject.transform.position;
		Vector3 newPos = new Vector3(x + prevPos.x, 0, z + prevPos.z);
		gameObject.transform.position = newPos;
		
		// Update internal position
		//position = transform.position;
	}
	
	public void Draw() {
		
	}
	
	public Vector3 GetPos()
	{
		return gameObject.transform.position;
	}
}
