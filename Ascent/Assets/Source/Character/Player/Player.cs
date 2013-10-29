using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

public class Player : Character
{
	#region Enums
	public enum EPlayerState // State defines what actions are allowed, and what animations to play
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

    private Color playerColor = Color.white;
    private Material playerMat = null;
    public bool jumping = false;
    public bool attacking = false;
    public Vector3 playerDirection = Vector3.zero;
	
	public int teamId = 1;
	
	// Player identifier
	private int playerId = 0;
	// Movement speed variables
	public float movementSpeed = 5.0f;
    public float jumpSpeed = 25.0f;
	
	// Hitbox Prefab
	public Transform hitBoxPrefab; // hitboxes represent projectiles
	List<Transform> activeHitBoxes; // active projectiles
	
	public EPlayerState playerState;
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

    // This function is always called immediately when Instantiated and is called before the Start() function
    public override void Awake()
    {
        base.Awake();
        Destroy(animatorController);
        animatorController = gameObject.AddComponent<PlayerAnimController>();
    }

	// Use this for initialization
	public override void Start () 
	{
        switch (playerId)
        {
            case 0:
                playerColor = Color.red;
                break;

            case 1:
                playerColor = Color.green;
                break;

            case 2:
                playerColor = Color.blue;
                break;

            default:
                playerColor = Color.white;
                break;
        }

        // Be careful not sure which material it will get first.
        GameObject obj = transform.gameObject;
        playerMat = obj.GetComponentInChildren<Renderer>().material;

        if (playerMat != null)
            //playerMat.color = playerColor;

		activeHitBoxes = new List<Transform>();
	}
	#endregion
	
	#region Update

	// Update is called once per frame
	public override void Update() 
	{

	}

	#endregion
	
	public void Skill(int skillId)
	{
        switch (skillId)
        {
            case 0: // jump
            {
                if (!jumping)
                {
                    return;
                }
            }
            break;
	        case 1: // attack normal
		    {
				if (activeHitBoxes.Count < 1)
				{
                    attacking = true;
                    // Create a hitbox
                    Transform t = (Transform)Instantiate(hitBoxPrefab);
                    t.renderer.material.color = playerColor;
                    // Initialize the hitbox
                    Vector3 boxPos = new Vector3(Position.x - 0.05f, rigidbody.centerOfMass.y + 0.1f, Position.z + transform.forward.z);
                    t.GetComponent<HitBox>().Init(HitBox.EBoxAnimation.BA_HIT_THRUST, teamId, 10.0f, 0.6f);
                    t.position = boxPos;
                    // Make the parent this player
                    t.parent = transform;
                    // Setup this hitbox with our collision event code.
                    t.GetComponent<HitBox>().OnTriggerEnterSteps += OnHitBoxCollideEnter;
                    t.GetComponent<HitBox>().OnTriggerStaySteps += OnHitBoxCollideStay;
                    t.GetComponent<HitBox>().OnTriggerExitSteps += OnHitBoxCollideExit;
                    activeHitBoxes.Add(t);
				}
		    }
		    break;
        }
	}
	
	public void KillBox(Transform box)
	{
        attacking = false;
		activeHitBoxes.Remove(box);
	}

    void OnHitBoxCollideEnter(Collider other)
    {
        // When players hit box collides with object.
        if (other.transform.tag == "Monster")
        {
            Monster monster = other.transform.GetComponent<Monster>();

            if (monster != null)
            {
                // Make the monster take damage.
                monster.TakeDamage(25);
            }
        }
    }

    void OnHitBoxCollideStay(Collider other)
    {

    }

    void OnHitBoxCollideExit(Collider other)
    {

    }

    void OnCollisionEnter(Collision collision)
    {

    }
}
