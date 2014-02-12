using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Enemy : Character
{
    #region Enums

    public enum STATE
    {
        IDLE,
        SEEK,
        ATTACKING,
        WAIT,
        DEAD,
        HIT,
    }

	public enum EEnemy
	{
		None,
		Rat,
		Abomination
	}

    #endregion

    #region Fields

    protected AIAgent agent;
    public AIAgent AIAgent
    {
        get { return agent; }
        protected set { agent = value; }
    }

	protected EnemyStats enemyStats;
	public EnemyStats EnemyStats
	{
		get { return enemyStats; }
		set 
		{ 
			enemyStats = value;
			stats = value;
		}
	}

    private Player targetPlayer;
    private Vector3 originalScale;

	protected StatBar hpBar;
	public StatBar HPBar
	{
		get { return hpBar; }
		set { hpBar = value; }
	}

    protected Room containedRoom;
    public Room ContainedRoom
    {
        get { return containedRoom; }
		 set { containedRoom = value; }
    }

   protected float deathSequenceTime = 0.0f;
   protected float deathSequenceEnd = 1.0f;
   protected Vector3 deathRotation = Vector3.zero;
   protected float deathSpeed = 5.0f;

	protected bool updateHpBar = false;

    #endregion

	public int health = 100;

    #region Initialization

	public override void Initialise()
	{
		animator = GetComponentInChildren<CharacterAnimator>();
		if (animator == null)
		{
			Debug.LogError("No animator attached to " + name, this);
		}
		animator.Initialise();

		base.Initialise();

        Transform AI = transform.FindChild("AI");
        if(AI == null)
        {
            Debug.LogError("Could not find AI. Attach a new AI GameObject as a child of this Enemy. Attach AIAgent component to AI.", this);
        }
        
        agent = AI.GetComponent<AIAgent>();
        if (agent == null)
        {
            Debug.LogError("Could not find AIAgent component. Attach one to AI.", this);
        }

        deathRotation = new Vector3(0.0f, 0.0f, transform.eulerAngles.z + 90.0f);

        animator.hasAnimations = false;
	}

	public virtual void InitiliseHealthbar()
	{
		hpBar = FloorHUDManager.Singleton.AddEnemyLifeBar(transform.localScale);
		hpBar.Init(StatBar.eStat.HP, this);
        hpBar.gameObject.SetActive(false);

		PositionHpBar();
	}

    public virtual void OnEnable()
    {
        // To be overridden
    }

    public virtual void OnDisable()
    {
        // To be overridden
    }

    #endregion

    #region Update

    // Update is called once per frame
	public override void Update () 
    {
        if (isDead)
        {
            deathSequenceTime += Time.deltaTime;

            // When the rat dies we want to make him kinematic and disabled the collider
            // this is so we can walk over the dead body.
            //if (this.transform.rigidbody.isKinematic == false)
            //{
            //    this.collider.enabled = false;
            //    this.transform.rigidbody.isKinematic = true;
            //    this.transform.collider.enabled = false;
            //}

            // Death sequence end
            if (deathSequenceTime >= deathSequenceEnd)
            {
                // When the death sequence has finished we want to make this object not active
                // This ensures that he will dissapear and not be visible in the game but we can still re-use him later.
                deathSequenceTime = 0.0f;
                this.gameObject.SetActive(false);
            }
            else
            {
                // During death sequence we can do some thing in here
                // For now we will rotate the rat on the z axis.
                this.transform.eulerAngles = Vector3.Lerp(this.transform.eulerAngles, deathRotation, Time.deltaTime * deathSpeed);

                // If the rotation is done early we can end the sequence.
                if (this.transform.eulerAngles == deathRotation)
                {
                    deathSequenceTime = deathSequenceEnd;
                }
            }
        }
        else
        {
            base.Update();

            if (!IsStunned)
            {
                if (activeAbility == null)
                {
                    AIAgent.MindAgent.Process();
                }

                AIAgent.SteeringAgent.Process();
            }

            if (hpBar != null)
            {
                //if (updateHpBar)
                {
                    if (stats.CurrentHealth != stats.MaxHealth)
                    {
                        if (!hpBar.gameObject.activeInHierarchy)
                            NGUITools.SetActive(hpBar.gameObject, true);

                        PositionHpBar();
                    }
                    else
                    {
                        if (hpBar.gameObject.activeInHierarchy)
                            NGUITools.SetActive(hpBar.gameObject, false);
                    }
                }
            }
        }
	}

    #endregion

    #region Operations

	protected virtual void PositionHpBar()
	{
		Vector3 screenPos = Game.Singleton.Tower.CurrentFloor.MainCamera.WorldToViewportPoint(transform.position);
		Vector3 barPos = FloorHUDManager.Singleton.hudCamera.ViewportToWorldPoint(screenPos);
		barPos = new Vector3(barPos.x,barPos.y);
		hpBar.transform.position = barPos;
	}

    #endregion


    #region Collisions on Self

	
	void OnBecameVisible()
	{
		//Debug.Log ("Rat became visible", this);
		if (hpBar != null)
		{
			//hpBar.gameObject.SetActive(true);
			updateHpBar = true;
		}
	}

	void OnBecameInvisible()
	{
		if (hpBar != null)
		{
			updateHpBar = false;
		}
	}

    public override void ApplyDamage(int unmitigatedDamage, Character.EDamageType type, Character owner)
    {
        // Check to see if the enemy was last damaged by a hero,
        // thus update the floor statistics of the hero. This function may want to pass in
        // the owner that is applying this damage.
        if (lastDamagedBy != null)
        {
            // TODO: This might need to move.
            Hero hero = lastDamagedBy as Hero;

            hero.FloorStatistics.TotalDamageDealt += unmitigatedDamage;
        }

        base.ApplyDamage(unmitigatedDamage, type, owner);
    }

	protected override void OnDeath ()
	{
		base.OnDeath ();

        // When the rat dies we want to make him kinematic and disabled the collider
        // this is so we can walk over the dead body.
        if (this.transform.rigidbody.isKinematic == false)
        {
            this.collider.enabled = false;
            this.transform.rigidbody.isKinematic = true;
            this.transform.collider.enabled = false;
        }

		FloorHUDManager.Singleton.RemoveEnemyLifeBar(hpBar);
	}

    #endregion
}
