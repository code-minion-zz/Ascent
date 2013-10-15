using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

public class Player : MonoBehaviour
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

    private PlayerAnimator animator;
    private CharacterStatistics characterStatistics;
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

    public CharacterStatistics CharacterStats
    {
        get { return characterStatistics; }
    }

    public PlayerAnimator Animator
    {
        get { return animator; }
    }
	
	#endregion	
	
	#region Initialization

    // This function is always called immediately when Instantiated and is called before the Start() function
    void Awake()
    {

    }

	// Use this for initialization
	public void Start () 
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
            playerMat.color = playerColor;

		activeHitBoxes = new List<Transform>();
		//Transform hitBox = transform.GetChild(0);
		//hitBox.renderer.enabled = false;
		//hitBox.GetComponent<HitBox>().enabled = false;

        // Attach character stats component
        characterStatistics = gameObject.AddComponent<CharacterStatistics>();
        characterStatistics.Init();
        characterStatistics.Health.Set(100.0f, 100.0f);

        animator = gameObject.GetComponent<PlayerAnimator>();
	}
	#endregion
	
	#region Update
	// Update is called once per frame
	public void Update () 
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
                    //gameObject.rigidbody.AddForce(Vector3.up * 10.0f, ForceMode.Impulse);
                    //jumping = true;
                    animator.PlayAnimation(PlayerAnimator.EAnimState.Jump);
                    //Debug.Log("jump");

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

    public void Move(Vector3 _direction)
    {
        if (_direction.x != 0.0f || _direction.z != 0.0f)
        {
            Vector3 newDir = new Vector3(_direction.x, 0.0f, _direction.z) * 100.0f;
            transform.LookAt(transform.position + newDir);
            animator.PlayAnimation(PlayerAnimator.EAnimState.Run);
        }

        //if (_direction.x != 0.0f || _direction.z != 0.0f)
        //{
        //    if (transform.rigidbody.velocity.magnitude < 6.0f)
        //    {
        //        transform.LookAt(transform.position + (_direction * 100.0f));
        //        transform.rigidbody.AddForce(transform.forward * 2.0f, ForceMode.Impulse);
        //    }
        //    //transform.position += (transform.forward * movementSpeed * Time.deltaTime);

        //    Debug.DrawRay(transform.position, transform.forward, Color.red);

        //    animator.PlayAnimation(PlayerAnimator.EAnimState.Run);
        //}
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

    public void TakeDamage(int _damage)
    {
        HealthStat health =  CharacterStats.GetComponent<HealthStat>();

        health -= _damage;
        if (health <= 0)
        {
            // On Death settings
            if (playerMat != null)
                playerMat.color = Color.black
                    ;
            //transform.gameObject.renderer.material.color = Color.black;
			playerState = EPlayerState.PS_STATE_DEATH;
        }
    }

    void OnCollisionEnter(Collision collision)
    {

    }
}
