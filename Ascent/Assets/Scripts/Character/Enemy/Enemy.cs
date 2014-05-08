using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Enemy : Character
{
    #region Enums

	public enum EEnemy
	{
		None,
		Rat,
		Abomination
	}

    #endregion

    #region Fields

	public AIAgent agent;
    public AIAgent AIAgent
    {
        get { return agent; }
        protected set { agent = value; }
    }

	private Vector3 targetPosition;
	public Vector3 TargetPosition
	{
		get { return targetPosition; }
		set { targetPosition = value; }
	}

	private Character targetCharacter;
	public Character TargetCharacter
	{
		get { return targetCharacter; }
		set 
		{ 
			targetCharacter = value;
			AIAgent.MindAgent.TargetCharacter = value;
		}
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

	public float enragePercentage = 0.0f;
	protected bool enraged;

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
   protected float deathSequenceEnd = 2.5f;
   private bool deathAnimComplete = false;
   protected float deathSinkTime = 0.0f;
   protected float deathSinkEnd = 3.0f;
   protected Vector3 deathPosition;
   protected Vector3 deathRotation = Vector3.zero;
   protected float deathSpeed = 5.0f;

	protected bool updateHpBar = false;

    #endregion

    #region Initialization

	public override void Initialise()
	{
        EnemyStats = EnemyStatLoader.Load(EEnemy.Rat, this);

		if (Game.Singleton.NumberOfPlayers == 2 )
		{
			health = health * Mathf.RoundToInt((float)Game.Singleton.NumberOfPlayers * 1.1f);
		}
        else if (Game.Singleton.NumberOfPlayers == 3)
        {
            health = health * Mathf.RoundToInt((float)Game.Singleton.NumberOfPlayers * 1.2f);
        }
		else if (Game.Singleton.NumberOfPlayers == 4)
		{
			health = health * Mathf.RoundToInt((float)Game.Singleton.NumberOfPlayers * 1.3f);
		}

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
        if (isDead)
        {
			if (!deathAnimComplete)
			{
				deathSequenceTime = 0.0f;
			}
			else
			{
				animator.animator.SetLayerWeight(0, 0.0f);
				animator.animator.SetLayerWeight(1, 1.0f);
				deathSequenceTime = deathSequenceEnd;
				animator.PlayAnimation("DeathIdle", true);
				animator.animator.enabled = false;
			}
        }
    }

    public virtual void OnDisable()
    {
		if (isDead)
		{
			if (!deathAnimComplete)
			{
				deathSequenceTime = 0.0f;
			}
			else
			{
				animator.animator.SetLayerWeight(0, 0.0f);
				animator.animator.SetLayerWeight(1, 1.0f);
				deathSequenceTime = deathSequenceEnd;
				animator.PlayAnimation("DeathIdle", true);
				animator.animator.enabled = false;
			}
		}
    }

    #endregion

    #region Update

    // Update is called once per frame
	public override void Update() 
    {
        if (isDead)
        {
			if (deathAnimComplete)
				return;

			ResetColor();
            if (deathSequenceTime != deathSequenceEnd)
            {
                deathSequenceTime += Time.deltaTime;
                if (deathSequenceTime >= deathSequenceEnd)
                {
                    deathPosition = transform.position;
                    animator.animator.SetLayerWeight(0, 0.0f);
                    animator.animator.SetLayerWeight(1, 1.0f);
                    deathSequenceTime = deathSequenceEnd;
                    animator.PlayAnimation("DeathIdle", true);
                    animator.animator.enabled = false;
					deathAnimComplete = true;
                    return;
                }
            }
            animator.PlayAnimation("Death", true);
        }
        else
        {
            base.Update();

			if (!enraged && enragePercentage != 0.0f && (float)EnemyStats.CurrentHealth / (float)EnemyStats.MaxHealth <= enragePercentage)
			{
				Enrage();
				enraged = true;
			}

			if(IsInState(EStatus.Stun))
			{
				animator.PlayAnimation("Stunned", true);
			}
            else if (CanMove && CanAct)
            {
				animator.PlayAnimation("Stunned", false);

				Vector3 velocity = Vector3.zero;

				if (TargetCharacter != null)
				{
					velocity = AIAgent.SteeringAgent.Steer(TargetCharacter.gameObject);
				}
				else
				{
					velocity = AIAgent.SteeringAgent.Steer(TargetPosition);
				}

				motor.Move(velocity);

				if (loadout.ActiveAbility == null)
				{
					AIAgent.MindAgent.Process();
				}
            }

            if (hpBar != null)
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

    #endregion

	protected virtual void Enrage()
	{
		// to be overidden
	}

	protected virtual void PositionHpBar()
	{
		Vector3 screenPos = Game.Singleton.Tower.CurrentFloor.MainCamera.WorldToViewportPoint(transform.position);
		screenPos.y += 0.075f;
		screenPos.x -= 0.030f;
		Vector3 barPos = FloorHUDManager.Singleton.hudCamera.ViewportToWorldPoint(screenPos);
		barPos = new Vector3(barPos.x,barPos.y);
		hpBar.transform.position = barPos;
	}
	
	void OnBecameVisible()
	{
		if (hpBar != null)
		{
			updateHpBar = true;
		}
	}

	void OnBecameInvisible()
	{
		if (hpBar != null)
		{
			updateHpBar = false;
		}

        if (isDead)
        {
            deathSequenceTime = deathSequenceEnd;
            animator.PlayAnimation("DeathIdle", true);
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

            Transform colliders = transform.FindChild("Colliders");
            if (colliders != null)
            {
                colliders.gameObject.SetActive(false);
            }
        }

		FloorHUDManager.Singleton.RemoveEnemyLifeBar(hpBar);
	}

	public virtual void StateTransitionToPassive()
	{
		AIAgent.MindAgent.SetBehaviour(AIMindAgent.EBehaviour.Passive);
	}

	public virtual void StateTransitionToDefensive()
	{
		AIAgent.MindAgent.SetBehaviour(AIMindAgent.EBehaviour.Defensive);
	}

	public virtual void StateTransitionToAggressive()
	{
		AIAgent.MindAgent.SetBehaviour(AIMindAgent.EBehaviour.Aggressive);
	}

	public virtual void StateTransitionToEvasive()
	{
		AIAgent.MindAgent.SetBehaviour(AIMindAgent.EBehaviour.Evasive);
	}

	public virtual void ChooseNewWanderTarget()
	{
		targetPosition = (containedRoom.NavMesh.GetRandomOrthogonalPositionWithinRadius(transform.position, 7.5f));
	}
}
