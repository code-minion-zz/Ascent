using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Arrow : MonoBehaviour
{
    private float lifeSpan;
    private bool toDestroy = false;
    private Vector3 direction;
    private float speed;
    private float damage;
    private GameObject owner;

    private List<Hero> heroesHit;
    bool hitYet;

    public void Initialise(float life, GameObject owner, Vector3 direction, float speed, int damage)
    {
		//owner = _owner;
        lifeSpan = life;
        toDestroy = false;
        this.direction = direction;
        this.speed = speed;
        this.damage = damage;
        this.owner = owner;

        heroesHit = new List<Hero>();
        hitYet = false;
    }
	
	// Update is called once per frame
	void Update () 
    {
        //transform.position += direction * speed * Time.deltaTime;
        if(toDestroy)
        {
			transform.position = Vector3.zero;
			transform.rotation = Quaternion.identity;
           	// Object.Destroy(this.gameObject);
           	gameObject.SetActive(false);
        }

        if (lifeSpan > 0.0f)
        {
			transform.position += direction * speed * Time.deltaTime;
            lifeSpan -= Time.deltaTime;
        }
        else
        {
			lifeSpan = 0.0f;
            gameObject.SetActive(false);
        }
	}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Hero")
        {
            CollideWithHero(collision.transform.GetComponent<Character>() as Hero, collision);
        }
        //else if (collision.transform.tag == "Monster")
        //{
        //}
        // The arrows should collide with everything except for its owner.
        else if (collision.transform.gameObject != owner && collision.transform.parent != owner)
        {
            toDestroy = true;
        }
    }


	/// <summary>
	/// When the arrow collides with a hero.
	/// </summary>
	/// <param name="hero">Hero.</param>
	/// <param name="collision">Collision.</param>
	private void CollideWithHero(Hero hero, Collision collision)
	{
        
		// Apply damage to the hero
		// Apply damage and knockback to the enemey.
		CombatEvaluator combatEvaluator = new CombatEvaluator(null, hero);

         if(!hitYet)
         {
            combatEvaluator.Add(new TrapDamageProperty(2.0f, 1.0f));
            hitYet = true;
         }

		combatEvaluator.Add(new KnockbackCombatProperty(-collision.contacts[0].normal, 1000000.0f));
		combatEvaluator.Apply();
        Game.Singleton.EffectFactory.CreateBloodSplatter(collision.transform.position, collision.transform.rotation, hero.transform, 3.0f);

        toDestroy = true;
	}
}