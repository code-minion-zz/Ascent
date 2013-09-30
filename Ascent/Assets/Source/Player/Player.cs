using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

public class Player : MonoBehaviour
{
	#region Enums
	enum EPlayerState // State defines what actions are allowed, and what animations to play
	{
		PS_INVALID_STATE = -1,
		PS_STATE_IDLE,
		PS_STATE_MOVE,
		PS_STATE_SPRINT,
		PS_STATE_ATTACK,
		PS_STATE_JUMP,
		PS_STATE_FALLING,
		PS_STATE_CAST,
		PS_STATE_PUSH,
		PS_STATE_PULL,
		PS_STATE_DEATH,
		PS_STATE_FREEZE,
		PS_MAX_STATE		
	}
	#endregion

	#region Fields
	
    private bool jumping = false;
    public int health = 100;
	
	// Player identifier
	private int playerId = 0;
	// Movement speed variables
	public float movementSpeed = 5.0f;		
	// Handling input for this player.
	private InputHandler inputHandler;
	
	// Hitbox Prefab
	public Transform hitBoxPrefab;
	
	private Vector3 forward = new Vector3(0.0f, 0.0f, 0.0f);
		
	List<Transform> hitBoxes; // active melee attacks
	
	EPlayerState playerState; // 
	
	#endregion
	
	#region Properties

	// Set the position of the players transform.
	public Vector3 Position
	{
		get { return transform.position; }
		set { transform.position = value; }
	}
	
	// Gets the game object of this transform
	public GameObject GameObject
	{
		get { return transform.gameObject; }
	}
	
	// Gets the transform for this player.
	public Transform Transform
	{
		get { return transform; }
	}
	
	// The player Id of this player.
	public int PlayerID
	{
		get { return playerId; }
		set { playerId = value; }
	}
	
	#endregion	
	
	#region Initialization

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
		hitBoxes = new List<Transform>();
		//Transform hitBox = transform.GetChild(0);
		//hitBox.renderer.enabled = false;
		//hitBox.GetComponent<HitBox>().enabled = false;
	}
	#endregion
	
	#region Update
	// Update is called once per frame
	public void Update () 
	{
		InputDevice inputDevice = inputHandler.GetDevice(playerId);
		
		if (inputDevice == null)
		{
			Debug.Log("Player " + playerId + "'s inputDevice does not exist");
			return;
		}
			// Update the transform by the movement
			if (inputDevice.Action1.IsPressed)
			{
			//	Debug.Log("Action One: " + playerId);
				Skill (0);
			}			
			// Update the transform by the movement
			if (inputDevice.Action2.IsPressed)
			{
				Debug.Log("Action Two: " + playerId);
				Skill (1);
			}
			
			float x = inputDevice.LeftStickX.Value * Time.deltaTime * movementSpeed;
			float z = inputDevice.LeftStickY.Value * Time.deltaTime * movementSpeed;
			
			Vector3 direction = Vector3.Normalize(new Vector3(x, 0.0f, z));

            if (x != 0.0f || z != 0.0f)
            {

                if (transform.rigidbody.velocity.magnitude < 6.0f)
                {
                    transform.LookAt(Position + (direction * 100.0f));
                    transform.rigidbody.AddForce(transform.forward *2.0f, ForceMode.Impulse);
                }
                //transform.position += (transform.forward * movementSpeed * Time.deltaTime);
                

                Debug.DrawRay(Position, transform.forward, Color.red);
            }

            if (jumping)
            {
                Physics.Raycast(new Ray(transform.position, -transform.up), 5.0f);
                Debug.DrawRay(transform.position, -transform.up, Color.red);
            }
//			if (hitBoxes.Count > 0)
//			{
//				List<int> toRemove = new List<int>();
//				for (int i = hitBoxes.Count; i < hitBoxes.Count; --i)
//				{
//					if(!hitBoxes[i].GetComponent<HitBox>().Active)
//					{
//						toRemove.Add(i);
//					}
//				}				
//				foreach ( int i in toRemove )
//				{
//					DestroyObject(hitBoxes[i].gameObject);
//					hitBoxes.RemoveAt(i);
//				}
				
//		}
//		else
//		{
//			// Error no device
//			//Debug.Log("No Device for this player");
//		}
	}
	#endregion
	
	public void Skill(int skillId)
	{
		switch (skillId)
		{
        case 0: // jump
        {
            if(!jumping)
            {
                gameObject.rigidbody.AddForce(Vector3.up * 10.0f, ForceMode.Impulse);
                jumping = true;

                return;
            }
        }
        break;
        case 1: // attack normal
	    {
			if (hitBoxes.Count < 1)
			{
				Transform t = (Transform)Instantiate(hitBoxPrefab);
				Vector3 boxPos = new Vector3(Position.x - 0.05f,rigidbody.centerOfMass.y + 0.1f,Position.z + transform.forward.z);
				//t.position = Position + transform.forward;
				t.position = boxPos;
				t.parent = transform;
				hitBoxes.Add(t);
				Debug.Log ("Removing hitbox from list");
			}
	        //transform.GetComponentInChildren<HitBox>().Fire();
	        //transform.GetChild(0).renderer.enabled = true;
	        //transform.GetChild(0).position = transform.position + (transform.forward * 2.0f);
			//transform.GetChild(0).parent = transform.parent;
	    }
	    break;
		}

	}
	
	public void KillBox(Transform box)
	{
		hitBoxes.Remove(box);
	}


    public void TakeDamage(int _damage)
    {
        health -= _damage;

        if (health <= 0)
        {
            transform.gameObject.renderer.material.color = Color.black;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.parent != null)
        {
            if (collision.transform.parent.name == "GridHelper")
            {
                jumping = false;
            }
        }
    }
}
