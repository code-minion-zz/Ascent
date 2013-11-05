using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Hero : Character 
{
    private List<Transform> activeHitBoxes = new List<Transform>();
    private PlayerAnimController animController;

	protected HeroAbilities abilities;
	protected HeroEquipment equipment;

    // Initially populated through unity.
    // We may want to encapsulate this later.
    protected Transform weaponSlot;	// Player weapon slot
    protected Weapon equipedWeapon;
    protected GameObject weaponPrefab;

    private Color playerColor = Color.white;
    public int teamId = 1;

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

    #region Initialization

	public abstract void Initialise(HeroSave saveData);

    // This function is always called immediately when Instantiated and is called before the Start() function
    public override void Awake()
    {
        base.Awake();
    }

	// Use this for initialization
    public override void  Start() 
	{
	    base.Start();
	}

    public void SetColor(Color color)
    {
        playerColor = color;
    }

    

    #endregion


    public void Skill(int skillId)
    {
        switch (skillId)
        {
            case 1: // attack normal
                {
                    if (activeHitBoxes.Count < 1)
                    {
                        //// Create a hitbox
                        //Transform t = (Transform)Instantiate(hitBoxPrefab);
                        //t.renderer.material.color = playerColor;
                        //// Initialize the hitbox
                        //Vector3 boxPos = new Vector3(Position.x - 0.05f, rigidbody.centerOfMass.y + 0.1f, Position.z + transform.forward.z);
                        //t.GetComponent<HitBox>().Init(HitBox.EBoxAnimation.BA_HIT_THRUST, teamId, 10.0f, 0.6f);
                        //t.position = boxPos;
                        //// Make the parent this player
                        //t.parent = transform;
                        //// Setup this hitbox with our collision event code.
                        //t.GetComponent<HitBox>().OnTriggerEnterSteps += OnHitBoxCollideEnter;
                        //t.GetComponent<HitBox>().OnTriggerStaySteps += OnHitBoxCollideStay;
                        //t.GetComponent<HitBox>().OnTriggerExitSteps += OnHitBoxCollideExit;
                        //activeHitBoxes.Add(t);
                    }
                }
                break;
        }
    }

    public void KillBox(Transform box)
    {
        activeHitBoxes.Remove(box);
    }

    void OnHitBoxCollideEnter(Collider other)
    {
        // When players hit box collides with object.
        if (other.transform.tag == "Monster")
        {
			Enemy monster = other.transform.GetComponent<Enemy>();

            if (monster != null)
            {
                // Make the monster take damage.
                monster.ApplyDamage(25, EDamageType.Physical);
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
