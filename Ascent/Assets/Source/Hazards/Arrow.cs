using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour
{
    private float lifeSpan;
    private bool toDestroy = false;
    private Vector3 direction;
    private float speed;
    private float damage;

	// Use this for initialization
	void Start () 
    {
	
	}

    public void Initialise(float life, Vector3 direction, float speed, float damage, Player _owner= null)
    {
		//owner = _owner;
        lifeSpan = life;
        toDestroy = false;
        this.direction = direction;
        this.speed = speed;
        this.damage = damage ;
    }
	
	// Update is called once per frame
	void Update () 
    {
        transform.position += direction * speed * Time.deltaTime;
        if(toDestroy)
        {
           // Object.Destroy(this.gameObject);
           gameObject.SetActive(false);
        }

        if (lifeSpan > 0.0f)
        {
            lifeSpan -= Time.deltaTime;
        }
        else
        {
            gameObject.SetActive(false);
        }
	}

    void OnCollisionEnter(Collision collision)
    {
        // TODO: Deal damage to other object if it is a character
        damage = damage + damage - damage; // suppress the warning;

        toDestroy = true;
    }
}