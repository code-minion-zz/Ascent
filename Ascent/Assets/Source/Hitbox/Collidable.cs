using UnityEngine;
using System.Collections.Generic;

public class Collidable : MonoBehaviour {
	#region Fields		

    uint        collisions = 0;
    Character   owner;
    string         ownerTeam;
    bool        ignoreWall = false;    // ignore walls
    bool        ignoreGrounded = false;// ignore objects on the ground
    bool        ignoreFlying = false;  // ignore objects that are too high
    bool        ignoreFriend = true; // ignore friendly units

    // remember these
    static bool hasInit;
    static int WallLayer;
    static int CharacterLayer;

	#endregion

    #region Events & Delegates

    public delegate void CollisionEventHandler(GameObject go);
    public  event CollisionEventHandler onCollisionEnterWall;
    public event CollisionEventHandler onCollisionEnterEnemy;
    public event CollisionEventHandler onCollisionEnterFriend;
    public event CollisionEventHandler onCollisionStayEnemy;
    public event CollisionEventHandler onCollisionStayFriend;
    public event CollisionEventHandler onCollisionExit;

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

    void Init(Character _owner)
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
        if (go.layer == WallLayer)
        {
            if (!ignoreWall)
            {
                if (onCollisionEnterWall != null)
                {
                    onCollisionEnterWall(go);
                    ++collisions;
                    return;
                }
            }
        }

        if (go.layer == CharacterLayer)
        {
            if (ownerTeam == go.tag)
            {
                if (!ignoreFriend)
                    return;
                else
                {
                    ++collisions;
                    onCollisionEnterFriend(go);
                    return;
                }
            }
            // check the ignore ground/flying flags here
            // if we are interested in hitting this object :
            onCollisionEnterEnemy(go);
            ++collisions;
            return;
        }
    }

    void OnTriggerExit(Collider other)
    {

    }
	
}

