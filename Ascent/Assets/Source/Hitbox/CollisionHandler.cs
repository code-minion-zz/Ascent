using UnityEngine;
using System.Collections.Generic;

public class CollisionHandler : MonoBehaviour {
	#region Fields		

    uint collisions = 0;

    Character owner;

	#endregion
	
	#region Properties
	public uint CollisionCount
	{
		get { return collisions; }
	}

    public bool HasCollided
    {
        get { return (collisions > 0); }
    }
	#endregion

    #region Events & Delegates
    //public Eve
    #endregion

    void Start () 
	{
		transform.forward = transform.parent.forward;
	}
	
	void OnDisable()
	{
	}
	
	/// <summary>
	/// Informs the hitbox of it's purpose in life
	/// </summary>
	/// <param name='animType'>
	/// Animation type, as defined by 
	/// </param>
	public void Init()
	{
	}
	
	// Update is called once per frame
//    void Update () {
//        elapsedLifeTime += Time.fixedDeltaTime;
//        if (Active)
//        {
//            switch (hitType)
//            {
//            case EBoxAnimation.BA_HIT_THRUST:
//                if (!retract)
//                {
//                    transform.position = transform.position + (transform.parent.forward * Time.fixedDeltaTime * projectileSpeed);
//                    Debug.DrawRay(transform.position,transform.parent.forward);
//                    Debug.DrawRay(transform.parent.position, transform.parent.forward);
//                    if ( elapsedLifeTime > totalLifeTime/2 ) 
//                    {
//                        retract = true;
//                    }
//                }
//                else 
//                {					
//                    if (transform.localPosition.z+transform.localPosition.x <= 0)
//                    {
//                        Active = false;
//                        //Debug.Log("Active " +Active);
//                        //DestroySelf();
//                    }
//                    transform.position = transform.position - (transform.parent.forward * Time.fixedDeltaTime * projectileSpeed);
					
//                }
//                break;
//            default:
//                Debug.Log("this should never happen");
//                break;
//            }
//        }
//    }
	
//    void DestroySelf()
//    {
//        //if (transform.parent.name.Contains("Monster"))
//        //{
//        //    transform.parent.GetComponent<Monster>().KillBox(transform);
//        //}
//        //else if (transform.parent.name.Contains("Player"))
//        //{
//        //    transform.parent.GetComponent<Player>().KillBox(transform);			
//        //}		
		
//        Destroy(this.gameObject);			
//    }
	
////	void Shutdown()
////	{
////		DestroyObject(this.gameObject);
////	}
	
////	void OnCollisionEnter(Collision collision)
////	{
////		foreach (ContactPoint contact in collision.contacts)
////		{
////			if (contact.otherCollider.transform.parent.name.Contains("Monster"))
////			{
////				isHit = true;	
////				contact.otherCollider.transform.parent.GetComponent<Monster>().TakeDamage(9);
               
////				Vector3 Force = contact.normal*1000;
////				contact.otherCollider.transform.parent.rigidbody.AddForce(Force);
////				Debug.Log("hit " + -Force);
////			}
////		}
////	}
	
////	void OnCollisionExit(Collision collisionInfo)
////	{
////		isHit = false;		
////	}

//    void OnTriggerEnter(Collider other)
//    {
//        if (OnTriggerEnterSteps != null)
//        {
//            OnTriggerEnterSteps(other);
//        }
//    }

//    void OnTriggerExit(Collider other)
//    {
//        if (OnTriggerExitSteps != null)
//        {
//            OnTriggerExitSteps(other);
//        }
//        // Called when the collision ends and the two objects do not overlap

//    }

//    void OnTriggerStay(Collider other)
//    {
//        if (OnTriggerStaySteps != null)
//        {
//            OnTriggerStaySteps(other);
//        }
//    }
	
//    public bool Fire(int id)
//    {
//        switch ((EBoxAnimation)id)
//        {
//        case EBoxAnimation.BA_HIT_THRUST: // im a sword
			
//            break;
//        default:
//            break;
//        }
		
//        return false;
//    }
	
}

