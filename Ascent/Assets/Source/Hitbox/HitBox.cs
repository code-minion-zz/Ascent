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
		if (collision.transform.name.Contains("Player"))
		{
			isHit = true;	
			Debug.Log("hit");
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
}
