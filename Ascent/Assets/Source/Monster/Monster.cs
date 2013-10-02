using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Monster : MonoBehaviour 
{
    public enum STATE
    {
        IDLE,
        SEEK,
        ATTACKING,
        WAIT,
        DEAD,
        HIT,
    }

    public int health = 100;
    private Player targetPlayer;
    public STATE state = STATE.IDLE;
    private float waiting = 0.0f;
    private Vector3 originalScale;
    private int attack = 10;
    private Color originalColor;	
	
	List<Transform> hitBoxes; // active melee attacks

    //List<MonsterAIState> listAIStates = new List<MonsterAIState>();

    /// <summary>
    /// Enum of monster AI movement states, multiple can be active at once
    /// </summary>	
    
    enum EMonsterAiFlag
    {
        MA_INVALID_STATE = 0x00,
        MA_STATE_WANDER = 0x01,	// Face and move random direction
        MA_STATE_SEEK = 0x02,	// Run towards
        MA_STATE_ESCAPE = 0x04,	// Run away
        MA_STATE_NOROTATE = 0x08,	// Halt rotation
        MA_STATE_NOMOVE = 0x16, // Halt movement
        MA_MAX_STATE = 0x80
    };



    protected char monsterState
    {
        get;
        set;
    }	

	// Use this for initialization
	void Start () 
    {

	}
	
	// Update is called once per frame
	void Update () 
    {
        switch(state)
        {
            case STATE.IDLE:
                {
                    if (Time.frameCount % 10 == 0)
                    {
                        targetPlayer = GetClosestPlayer();

                        if (targetPlayer != null)
                        {
                            state = STATE.SEEK;
                            waiting = 3.5f;
                        }
                    }
                }
                break;
            case STATE.SEEK:
                {
                    Vector3 direction = targetPlayer.Transform.position - transform.position;
                    transform.rotation = Quaternion.LookRotation(direction, new Vector3(0.0f, 1.0f, 0.0f));

                    Debug.DrawLine(transform.position, targetPlayer.Transform.position);

                    float distance = direction.sqrMagnitude;
                    if (distance > -5.0f && distance < 2.0f)
                    {
                        state = STATE.ATTACKING;
                        break;
                    }
                    else if (waiting > 0.0f)
                    {
                        waiting -= Time.deltaTime;
                    }
                    else
                    {
                        waiting = 0.35f;
                        state = STATE.WAIT;
                        break;
                    }

                    MoveTowardPlayer(targetPlayer);
                }
                break;
            case STATE.ATTACKING:
                {
                    AttackTarget(targetPlayer);

                    targetPlayer = null;
                    state = STATE.WAIT;
                    waiting = 0.35f;                    
                }
                break;
            case STATE.WAIT:
                {
                    if (waiting > 0.0f)
                    {
                        waiting -= Time.deltaTime;
                    }
                    else
                    {
                        state = STATE.IDLE;
                    }
                }
                break;
            case STATE.HIT:
                {
                    if (waiting > 0.0f)
                    {
                        waiting -= Time.deltaTime;
                    }
                    else
                    {
                        state = STATE.IDLE;
                    }
                }
                break;
            case STATE.DEAD:
                {
                    if (waiting > 0.0f)
                    {
                        waiting -= Time.deltaTime;

                        transform.localScale = Vector3.Lerp(originalScale, new Vector3(0.0f, 0.0f, 0.0f), 0.1f * Time.deltaTime);
                    }
                    else
                    {
                        Object.Destroy(this.gameObject);
                    }
                }
                break;
        }

       
	}

    protected Player GetClosestPlayer()
    {
        // Find a close player
        Game game = GameObject.Find("Game").GetComponent<Game>();
        List<Player> players = game.Players;

        Player closest = null;
        float distance = Mathf.Infinity;

        Vector3 position = transform.position;

        foreach (Player player in players)
        {
            Vector3 diff = player.Transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = player;
                distance = curDistance;
            }
        }

        if (closest != null)
        {
            if (distance > 75.0f)
            {
                closest = null;
            }
        }

        return closest;
    }

    protected void MoveTowardPlayer(Player _player)
    {
        if (_player != null)
        {
            Vector3 direction = Vector3.Normalize((_player.Transform.position - transform.position));
            transform.position += direction * Time.deltaTime * 2.5f;
        }
    }

    public void TakeDamage(int _damage)
    {
        health -= _damage;

        if (health <= 0)
        {
            originalScale = transform.localScale;
            waiting = 0.5f;
            state = STATE.DEAD;
        }
        else
        {
           waiting = 0.5f;
           state = STATE.HIT;
           originalColor = transform.renderer.material.color;
        }
    }

    void AttackTarget(Player _player)
    {
        _player.TakeDamage(10);
    }	
	
	void OnCollisionEnter(Collision collision)
	{
		foreach (ContactPoint contact in collision.contacts)
		{
			Collider hitBoxCollider = contact.otherCollider;
			if (hitBoxCollider.name.Contains("HitBox"))
			{
				if (hitBoxCollider.enabled)
				{
					TakeDamage(25);
					Vector3 Force = contact.normal * 200.0f;
					transform.rigidbody.AddForce(Force);
				}
			}
		}
	}
}
