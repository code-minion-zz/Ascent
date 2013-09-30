using UnityEngine;
using System.Collections;

public class HitBox : MonoBehaviour {
	#region Members
	public enum EBoxAnimation
	{
		BA_INVALID_HIT = -1,
		BA_HIT_THRUST,
		BA_MAX_HIT
	}
		
	EBoxAnimation hitType;
	//Vector3 rotateRate;
	//Vector3 moveRate;
	float projectileSpeed = 10.0f;
	bool retract = false;
	#endregion
	
	#region Properties
	public bool Active
	{
		get
		{
			return enabled;
		}
		set
		{
			enabled = value;
		}
	}
	#endregion
	
	// Use this for initialization
	void Start () {
		Active = true;
		transform.forward = transform.parent.forward;
	}
	
	// Update is called once per frame
	void Update () {
		if (Active)
		{
			if (!retract)
			{
				transform.position = transform.position + (transform.parent.forward * Time.fixedDeltaTime * projectileSpeed);
				Debug.DrawRay(transform.position,transform.parent.forward);
				Debug.DrawRay(transform.parent.position, transform.parent.forward);
				//if (Mathf.Abs(transform.localPosition.z+transform.localPosition.x) > 2)
				if( Vector3.Magnitude(transform.position - transform.parent.position) > 3)
				{
					retract = true;
					Debug.Log("Retract " + retract);
				}
			}
			else 
			{
				
				//transform.Translate(transform.forward * 2 * -Time.deltaTime);
				if (transform.localPosition.z+transform.localPosition.x <= 0)
				{
					Active = false;
					Debug.Log("Active " +Active);
					transform.parent.GetComponent<Player>().KillBox(transform);
					Destroy(this.gameObject);
				}
				transform.position = transform.position + (transform.parent.forward * -Time.fixedDeltaTime * projectileSpeed);
				
			}
		}
	}
	
//	void Shutdown()
//	{
//		DestroyObject(this.gameObject);
//	}
	
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
		switch ((EBoxAnimation)id)
		{
		case EBoxAnimation.HB_HIT_SWORD: // im a sword
			
			break;
		default:
			break;
		}
		
		return false;
	}
	
}

