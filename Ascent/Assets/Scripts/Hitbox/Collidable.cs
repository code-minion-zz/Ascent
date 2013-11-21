using UnityEngine;
using System.Collections.Generic;

public class Collidable : MonoBehaviour 
{
#pragma warning disable 0414

	#region Fields		

    uint        collisions = 0;
    //Character   owner;
    string      ownerTeam;

    // set once to remember what values these have
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

	/// <summary>
	/// Gets how many collisions this object has had since creation.
	/// Useful if you want to stop an attack after a certain number of collisions,
	/// or use the number of collisions for other calculations
	/// </summary>
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
       // owner = _owner;
        ownerTeam = _owner.tag;
    }

    void SetForwardDirection(Vector3 _forward)
    {
        transform.forward = _forward;
    }
    	
	/// <summary>
	/// Handles the trigger enter event. The IF statements filter the collision to figure out what it is.
	/// Once we know what it is, fire the corresponding event if it has subscribers.
	/// </summary>
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

