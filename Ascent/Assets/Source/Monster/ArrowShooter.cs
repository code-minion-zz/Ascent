using UnityEngine;
using System.Collections;

public class ArrowShooter : MonoBehaviour 
{
    public GameObject projectile;
    public Vector3 direction;

	// Use this for initialization
	void Start () 
    {
	}
	
	// Update is called once per frame
	void Update () 
    {
        
        if (Time.frameCount % 35 == 0)
        {
            // Instantiate an arrow
            GameObject arrow = GameObject.Instantiate(projectile, transform.GetChild(1).transform.position +( direction * 1.0f), new Quaternion(0.0f, 0.0f, 0.0f, 1.0f)) as GameObject;

            arrow.transform.parent = transform;

            arrow.rigidbody.AddForce(direction * 50.0f);
        }
	}
}
