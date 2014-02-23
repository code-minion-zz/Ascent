using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour
{
    private float lifeSpan;
    private bool toDestroy = false;
    private Vector3 direction;
    private float speed;
    private int damage;

	// Use this for initialization
	void Start () 
    {
	}

    public void Initialise(float life, Vector3 direction, float speed, int damage, Player _owner= null)
    {
		//owner = _owner;
        lifeSpan = life;
        toDestroy = false;
        this.direction = direction;
        this.speed = speed;
        this.damage = damage;
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
		toDestroy = true;

		switch (collision.transform.tag)
		{
			case "Hero":
				{
					CollideWithHero(collision.transform.GetComponent<Character>() as Hero, collision);
				}
				break;
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
		combatEvaluator.Add(new TrapDamageProperty(1.0f, 1.0f));
		combatEvaluator.Add(new KnockbackCombatProperty(-collision.contacts[0].normal, 1.0f));
		combatEvaluator.Apply();
	}
}