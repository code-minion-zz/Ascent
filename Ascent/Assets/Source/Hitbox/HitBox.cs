using UnityEngine;
using System.Collections;

public class HitBox : MonoBehaviour {
	#region Enums
	enum EHitType
	{
		HB_INVALID_HIT = -1,
		HB_HIT_SWORD,
		HB_MAX_HIT
	}
	#endregion
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}
	
//	void OnCollisionEnter(Collision collision)
//	{
//		foreach (ContactPoint contact in collision.contacts)
//		{
//			if (contact.otherCollider.transform.parent.name.Contains("Monster"))
//			{
//				isHit = true;	
//				contact.otherCollider.transform.parent.GetComponent<Monster>().TakeDamage(9);
//				Vector3 Force = contact.normal*1000;
//				contact.otherCollider.transform.parent.rigidbody.AddForce(Force);
//				Debug.Log("hit " + -Force);
//			}
//		}
//	}
	
//	void OnCollisionExit(Collision collisionInfo)
//	{
//		isHit = false;		
//	}
	
	public bool Fire(int id)
	{
        switch ((EHitType)id)
		{
		case EHitType.HB_HIT_SWORD: // im a sword
			
			break;
		}
		
		return false;
	}
	
	public bool isHit
	{
		get{return isHit;}
		set{;}
	}
	public bool enabled = false;
}

