using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour 
{
    public float lifeSpan;
    private bool toDestroy = false;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(toDestroy)
        {
            Object.Destroy(this.gameObject);
        }

        if (lifeSpan > 0.0f)
        {
            lifeSpan -= Time.deltaTime;
        }
        else
        {
            Object.Destroy(this.gameObject);
        }
	}

    void OnCollisionEnter(Collision collision)
    {
        toDestroy = true;
    }
}
