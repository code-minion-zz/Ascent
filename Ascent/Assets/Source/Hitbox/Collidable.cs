using UnityEngine;
using System.Collections.Generic;

public class Collidable : MonoBehaviour {
	#region Fields		

    uint        collisions = 0;
    Character   owner;
    string      ownerTeam;
//    bool        ignoreWall = false;    // ignore walls
//    bool        ignoreGrounded = false;// ignore objects on the ground
//    bool        ignoreFlying = false;  // ignore objects that are too high
//    bool        ignoreFriend = true; // ignore friendly units

    // remember these
    static bool hasInit;
    static int WallLayer;
    static int CharacterLayer;

	#endregion

    #region Events & Delegates

    public delegate void CollisionEventHandler(Character other);
    public  event CollisionEventHandler onCollisionEnterWall;
    public event CollisionEventHandler onCollisionEnterEnemy;
    public event CollisionEventHandler onCollisionEnterFriend;
    //public event CollisionEventHandler onCollisionStayEnemy;
    //public event CollisionEventHandler onCollisionStayFriend;
    //public event CollisionEventHandler onCollisionExit;

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

    void Awake()
    {
		// object starts disabled
		gameObject.SetActive(false);
		
        if (!hasInit)
        {
            hasInit = true;
            WallLayer = LayerMask.NameToLayer("Wall");
            CharacterLayer = LayerMask.NameToLayer("Character");
        }
    }

    void Start() 
	{
		transform.forward = transform.parent.forward;
	}

    public void Init(Character _owner)
    {
        owner = _owner;
        ownerTeam = _owner.tag;
    }

    void SetForwardDirection(Vector3 _forward)
    {
        transform.forward = _forward;
    }
    	
    void OnTriggerEnter(Collider other)
    {
        GameObject go = other.gameObject;
		
        if (go.layer == CharacterLayer)
        {
            if (ownerTeam == go.tag)
            {
				if (onCollisionEnterFriend != null)
                {
					
                	onCollisionEnterFriend(go.GetComponent<Character>());
                	++collisions;
					return;
                }
            }
			else if (onCollisionEnterEnemy != null)					
			{
            // check if we cannot hit this for any reason
			// report enemy collision
            	onCollisionEnterEnemy(go.GetComponent<Character>());
	            ++collisions;
	            return;
			}			
        }
		
        if (go.layer == WallLayer)
        {
            if (onCollisionEnterWall != null)
            {
                onCollisionEnterWall(null);
				
                ++collisions;
                return;
            }
            
        }
    }

    void OnTriggerExit(Collider other)
    {

    }
	
}

