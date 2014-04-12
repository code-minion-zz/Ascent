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

    public int health;
    public int attack;

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

    public EnemyAnimator EnemyAnimator
    {
        get { return Animator.GetComponent<EnemyAnimator>(); }
    }

   protected float deathSequenceTime = 0.0f;
   protected float deathSequenceEnd = 2.0f;
   protected float deathSinkTime = 0.0f;
   protected float deathSinkEnd = 1.0f;
   protected Vector3 deathPosition;
   protected Vector3 deathRotation = Vector3.zero;
   protected float deathSpeed = 5.0f;

	protected bool updateHpBar = false;

    #endregion

    #region Initialization

	public override void Initialise()
	{
        EnemyStats = EnemyStatLoader.Load(EEnemy.Rat, this);
        EnemyStats.SecondaryStats.health = health;
        EnemyStats.SecondaryStats.attack = attack;
        EnemyStats.Reset();

		animator = GetComponentInChildren<EnemyAnimator>();
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

        loadout = new EnemyAbilityLoadout();
        loadout.Initialise(this);

        deathRotation = new Vector3(0.0f, 0.0f, transform.eulerAngles.z + 90.0f);
	}

	public virtual void InitiliseHealthbar()
	{
        if (FloorHUDManager.Singleton != null)
        {
            hpBar = FloorHUDManager.Singleton.AddEnemyLifeBar(transform.localScale);

            if (hpBar != null)
            {
                hpBar.Init(StatBar.eStat.HP, this);
                hpBar.gameObject.SetActive(false);
            }
        }
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
	public override void Update() 
    {
        if (isDead)
        {
			ResetColor();
			animator.PlayAnimation("Death", true);
			if (deathSequenceTime != deathSequenceEnd)
			{
				deathSequenceTime += Time.deltaTime;
				if (deathSequenceTime > deathSequenceEnd)
				{
					deathSequenceTime = deathSequenceEnd;
					deathPosition = transform.position;
				}


				shadow.FadeOut(deathSequenceTime / deathSequenceEnd);
			}
			else if (deathSequenceTime == deathSequenceEnd)
			{
				deathSinkTime += Time.deltaTime;

				Vector3 targetPos = deathPosition;
				targetPos.y -= 2.0f;
				transform.position = Vector3.Lerp(deathPosition, targetPos, deathSinkTime);

				if (deathSinkTime > deathSinkEnd)
				{
					deathSinkTime = deathSinkEnd;
					this.gameObject.SetActive(false);
				}
			}
            Debug.Log(transform.forward);
        }
        else
        {
            base.Update();

            if (CanMove && CanAct)
            {
                //if (!loadout.IsAbilityActive)
                //{
                //    AIAgent.MindAgent.Process();
                //}

                //AIAgent.SteeringAgent.Process();
            }

            if (hpBar != null)
            {
                //if (updateHpBar)
                {
                    if (stats.CurrentHealth != stats.MaxHealth)
                    {
                        if (!hpBar.gameObject.activeInHierarchy)
                        {
                            NGUITools.SetActive(hpBar.gameObject, true);
                        }

                        PositionHpBar();
                    }
                    else
                    {
                        if (hpBar.gameObject.activeInHierarchy)
                        {
                            NGUITools.SetActive(hpBar.gameObject, false);
                        }

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
		screenPos.y += 0.075f;
		screenPos.x -= 0.025f;
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

    public override void ApplyCombatEffects(DamageResult result)
    {
        // Check to see if the enemy was last damaged by a hero,
        // thus update the floor statistics of the hero. This function may want to pass in
        // the owner that is applying this damage.
        if (lastDamagedBy != null)
        {
            // TODO: This might need to move.
            if (lastDamagedBy is Hero)
            {
                Hero hero = lastDamagedBy as Hero;

			    hero.FloorStatistics.TotalDamageDealt += result.finalDamage;
            }
        }

		base.ApplyCombatEffects(result);
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
