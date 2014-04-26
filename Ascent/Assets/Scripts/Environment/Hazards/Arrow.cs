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
		Character hitCharacter = null;

        if (collision.transform.gameObject.layer == (int)Layer.Hero ||
			collision.transform.gameObject.layer == (int)Layer.Monster)
        {
			hitCharacter = collision.transform.GetComponent<Character>();

			CombatEvaluator combatEvaluator = new CombatEvaluator(null, hitCharacter);

			if (!hitYet)
			{
				combatEvaluator.Add(new TrapDamageProperty(damage, 1.0f));
				hitYet = true;
			}

			combatEvaluator.Add(new KnockbackCombatProperty(-collision.contacts[0].normal, 1000000.0f));
			combatEvaluator.Apply();
            EffectFactory.Singleton.CreateBloodSplatter(collision.transform.position, collision.transform.rotation);

			toDestroy = true;
        }
        else if (collision.transform.gameObject != owner && collision.transform.parent != owner)
		{
			SoundManager.PlaySound(AudioClipType.pop,transform.position,.1f);
            toDestroy = true;
        }
    }
}