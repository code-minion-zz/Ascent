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

    #endregion

    #region Fields

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

	protected bool updateHpBar = false;

    #endregion

	public int health = 100;

    #region Initialization

	public override void Initialise()
	{
		hpBar = HudManager.Singleton.AddEnemyLifeBar(transform.localScale);
		hpBar.Init(StatBar.eStat.HP, this);

		PositionHpBar();

		base.Initialise();
	}

    public virtual void OnEnable()
    {
        //containedRoom = Game.Singleton.Tower.CurrentFloor.CurrentRoom;
    }

    #endregion

    #region Update

    // Update is called once per frame
	public override void Update () 
    {
        base.Update();

		if (!IsDead)
		{
			if (hpBar != null)
			{
				if (updateHpBar)
				{
					if (derivedStats.CurrentHealth != derivedStats.MaxHealth)
					{
						if (!hpBar.gameObject.activeInHierarchy)
							NGUITools.SetActive (hpBar.gameObject, true);

						PositionHpBar();
					}
					else
					{
						if (hpBar.gameObject.activeInHierarchy)
							NGUITools.SetActive (hpBar.gameObject, false);
					}
				}
			}
		}

		// TODO: if rat is frozen, tint hp bar blue and apply frozen texture
	}

    #endregion

    #region Operations

    public override void SubUpdate()
    {
        //AI_Agent ai = GetComponentInChildren<AI_Agent>();

        //if (ai != null)
        //{
        //    ai.Process();
        //}

        base.SubUpdate();
    }

	protected virtual void PositionHpBar()
	{
		Vector3 screenPos = Game.Singleton.Tower.CurrentFloor.MainCamera.WorldToViewportPoint(transform.position);
		Vector3 barPos = HudManager.Singleton.hudCamera.ViewportToWorldPoint(screenPos);
		barPos = new Vector3(barPos.x,barPos.y);
		hpBar.transform.position = barPos;
	}

    protected Player GetClosestPlayer()
    {
        //// Find a close player
        //List<Player> players = Game.Singleton.Players;

        //Player closest = null;
        //float distance = Mathf.Infinity;

        //Vector3 position = transform.position;

        //foreach (Player player in players)
        //{
        //    Vector3 diff = player.Transform.position - position;
        //    float curDistance = diff.sqrMagnitude;
        //    if (curDistance < distance)
        //    {
        //        closest = player;
        //        distance = curDistance;
        //    }
        //}

        //if (closest != null)
        //{
        //    if (distance > 75.0f)
        //    {
        //        closest = null;
        //    }
        //}

        //return closest;
        return null;
    }

    protected void MoveTowardPlayer(Player _player)
    {
        //if (_player != null)
        //{
        //    Vector3 direction = Vector3.Normalize((_player.Transform.position - transform.position));
        //    transform.position += direction * Time.deltaTime * 2.5f;
        //}
    }

    //public override void ApplyDamage(int unmitigatedDamage, EDamageType type)
    //{
    //    characterStatistics.CurrentHealth -= unmitigatedDamage;

    //    if (characterStatistics.CurrentHealth <= 0)
    //    {
    //        originalScale = transform.localScale;
    //        waiting = 0.5f;
    //        state = STATE.DEAD;
    //    }
    //    else
    //    {
    //       waiting = 0.5f;
    //       state = STATE.HIT;
    //    }
    //}
	
	void Attack()
	{			
		//if (activeHitBoxes.Count < 1)
		//{
		//    Transform t = (Transform)Instantiate(hitBoxPrefab);
		//    Vector3 boxPos = new Vector3(transform.position.x - 0.05f,rigidbody.centerOfMass.y + 0.1f,transform.position.z + transform.forward.z);
		//    t.GetComponent<HitBox>().Init(HitBox.EBoxAnimation.BA_HIT_THRUST, teamId,10.0f,0.6f);
		//    t.renderer.material.color = Color.blue;
		//    // Setup this hitbox with our collision event code.
		//    t.GetComponent<HitBox>().OnTriggerEnterSteps += OnHitBoxCollideEnter;
		//    t.GetComponent<HitBox>().OnTriggerStaySteps += OnHitBoxCollideStay;
		//    t.GetComponent<HitBox>().OnTriggerExitSteps += OnHitBoxCollideExit;
		//    t.position = boxPos;
		//    t.parent = transform;
		//    activeHitBoxes.Add(t);
		//}
	}
	
	public void KillBox(Transform box)
	{
		//activeHitBoxes.Remove(box);
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

    public override void ApplyDamage(int unmitigatedDamage, Character.EDamageType type)
    {
        // Check to see if the enemy was last damaged by a hero,
        // thus update the floor statistics of the hero. This function may want to pass in
        // the owner that is applying this damage.
        if (lastDamagedBy != null)
        {
            Hero hero = lastDamagedBy as Hero;
            hero.FloorStatistics.TotalDamageDealt += unmitigatedDamage;
        }

        base.ApplyDamage(unmitigatedDamage, type);
    }

	public override void OnDeath ()
	{
		base.OnDeath ();

		HudManager.Singleton.RemoveEnemyLifeBar(hpBar);
	}

    #endregion
}
