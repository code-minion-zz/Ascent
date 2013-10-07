using UnityEngine;
using System.Collections;

public class HitBox : MonoBehaviour {
	#region Members
	public enum EBoxAnimation
	{
		BA_INVALID_HIT = -1,
		BA_HIT_THRUST,			// moves forward at a constant rate, does not rotate
		BA_MAX_HIT
	}
		
	// behaviour member variables
	EBoxAnimation hitType;		
	//float rotateRate		= 0.0f;
	float projectileSpeed 	= 10.0f;
	float totalLifeTime		= 0.5f;
	
	// 
	float elapsedLifeTime	= 0.0f;	
	bool retract = false;
	
	public int teamId = 0;
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
	
	/// <summary>
	/// Informs the hitbox of it's purpose in life
	/// </summary>
	/// <param name='animType'>
	/// Animation type, as defined by 
	/// </param>
	public void Init(EBoxAnimation animType, int team, float speed = 10.0f, float lifeTime = 0.5f, float rotation = 0.0f)
	{
		teamId = team;
		hitType = animType;
		projectileSpeed = speed;
		totalLifeTime = lifeTime;
		//rotateRate = rotation;
	}
	
	// Update is called once per frame
	void Update () {
		elapsedLifeTime += Time.fixedDeltaTime;
		if (Active)
		{
			switch (hitType)
			{
			case EBoxAnimation.BA_HIT_THRUST:
				if (!retract)
				{
					transform.position = transform.position + (transform.parent.forward * Time.fixedDeltaTime * projectileSpeed);
					Debug.DrawRay(transform.position,transform.parent.forward);
					Debug.DrawRay(transform.parent.position, transform.parent.forward);
					if ( elapsedLifeTime > totalLifeTime/2 ) 
					{
						retract = true;
						Debug.Log("Retract " + retract);
					}
				}
				else 
				{					
					if (transform.localPosition.z+transform.localPosition.x <= 0)
					{
						Active = false;
						//Debug.Log("Active " +Active);
						DestroySelf();
					}
					transform.position = transform.position - (transform.parent.forward * Time.fixedDeltaTime * projectileSpeed);
					
				}
				break;
			default:
				Debug.Log("this should never happen");
				break;
			}
		}
	}
	
	void DestroySelf()
	{
		if (transform.parent.name.Contains("Monster"))
		{
			transform.parent.GetComponent<Monster>().KillBox(transform);
		}
		else if (transform.parent.name.Contains("Player"))
		{
			transform.parent.GetComponent<Player>().KillBox(transform);			
		}		
		
		Destroy(this.gameObject);			
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
		case EBoxAnimation.BA_HIT_THRUST: // im a sword
			
			break;
		default:
			break;
		}
		
		return false;
	}
	
}

