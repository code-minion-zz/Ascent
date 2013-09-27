using UnityEngine;
using System.Collections;

public class HitBox : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	void OnCollisionEnter(Collision collision)
	{
		foreach (ContactPoint contact in collision.contacts)
		{
			if (contact.otherCollider.transform.parent.name.Contains("Monster"))
			{
				isHit = true;	
				contact.otherCollider.transform.parent.GetComponent<Monster>().TakeDamage(9);
				Vector3 Force = contact.normal*1000;
				contact.otherCollider.transform.parent.rigidbody.AddForce(Force);
				Debug.Log("hit " + -Force);
			}
		}
	}
	
	void OnCollisionExit(Collision collisionInfo)
	{
		isHit = false;		
	}
	
	bool isHit
	{
		get{return isHit;}
		set{;}
	}
	bool enabled = false;
}

