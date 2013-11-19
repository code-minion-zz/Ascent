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

    public int teamId = 2;

    private Player targetPlayer;
    //private float waiting = 0.0f;
    private Vector3 originalScale;

    protected RAIN.Core.AIRig ai;

	protected StatBar hpBar;
	public StatBar HPBar
	{
		get { return hpBar; }
		set { hpBar = value; }
	}

    #endregion

    #region Initialization

	public virtual void Initialise()
	{
		hpBar = HudManager.AddEnemyLifeBar();
		hpBar.Init(StatBar.eStat.HP, characterStatistics);
	}

    public override void Awake()
    {
        // To be derived
    }

	public override void Start () 
    {
        // To be derived
	}

    #endregion

    #region Update

    // Update is called once per frame
	public override void Update () 
    {
        base.Update();

		if (hpBar != null)
		{
			if (characterStatistics.CurrentHealth != characterStatistics.MaxHealth)
			{
				hpBar.enabled = true;
			}
			else
			{
				hpBar.enabled = false;
			}
			Vector3 screenPos = Game.Singleton.camera.WorldToViewportPoint(transform.position);

			// TODO : Check if Rat is within main camera Frustum. If so, set the HP Bar's position
			// in the HUD camera so that it overlays the rat.
			//HudManager.Singleton.
		}

		// if rat is frozen, tint hp bar blue and apply frozen texture

        //switch(state)
        //{
        //    case STATE.IDLE:
        //        {
        //            if (Time.frameCount % 10 == 0)
        //            {
        //                targetPlayer = GetClosestPlayer();

        //                if (targetPlayer != null)
        //                {
        //                    state = STATE.SEEK;
        //                    waiting = 3.5f;
        //                }
        //            }
        //        }
        //        break;
        //    case STATE.SEEK:
        //        {
        //            Vector3 direction = targetPlayer.Transform.position - transform.position;
        //            transform.rotation = Quaternion.LookRotation(direction, new Vector3(0.0f, 1.0f, 0.0f));

        //            Debug.DrawLine(transform.position, targetPlayer.Transform.position);

        //            float distance = direction.sqrMagnitude;
        //            if (distance > -5.0f && distance < 2.0f)
        //            {
        //                state = STATE.ATTACKING;
        //                break;
        //            }
        //            else if (waiting > 0.0f)
        //            {
        //                waiting -= Time.deltaTime;
        //            }
        //            else
        //            {
        //                waiting = 0.35f;
        //                state = STATE.WAIT;
        //                break;
        //            }

        //            MoveTowardPlayer(targetPlayer);
        //        }
        //        break;
        //    case STATE.ATTACKING:
        //        {
        //            //AttackTarget(targetPlayer);
        //            Attack ();

        //            targetPlayer = null;
        //            state = STATE.WAIT;
        //            waiting = 0.35f;                    
        //        }
        //        break;
        //    case STATE.WAIT:
        //        {
        //            if (waiting > 0.0f)
        //            {
        //                waiting -= Time.deltaTime;
        //            }
        //            else
        //            {
        //                state = STATE.IDLE;
        //            }
        //        }
        //        break;
        //    case STATE.HIT:
        //        {
        //            if (waiting > 0.0f)
        //            {
        //                waiting -= Time.deltaTime;
        //            }
        //            else
        //            {
        //                state = STATE.IDLE;
        //            }
        //        }
        //        break;
        //    case STATE.DEAD:
        //        {
        //            if (waiting > 0.0f)
        //            {
        //                waiting -= Time.deltaTime;

        //                transform.localScale = Vector3.Lerp(originalScale, new Vector3(0.0f, 0.0f, 0.0f), 0.1f * Time.deltaTime);
        //            }
        //            else
        //            {
        //                Object.Destroy(this.gameObject);
        //            }
        //        }
        //        break;
        //}
	}

    #endregion

    #region Operations

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

    #region HitBox Collisions

    void OnHitBoxCollideEnter(Collider other)
    {
        //// When monster hit box collides with player.
        //if (other.transform.tag == "Player")
        //{
        //    Player player = other.transform.GetComponent<Player>();

        //    if (player != null)
        //    {
        //        // Make the monster take damage.
        //        player.TakeDamage(25);
        //    }
        //}
    }

    void OnHitBoxCollideStay(Collider other)
    {

    }

    void OnHitBoxCollideExit(Collider other)
    {

    }

    #endregion

    #region Collisions on Self

    void OnCollisionEnter(Collision collision)
	{

    }

    #endregion
}
