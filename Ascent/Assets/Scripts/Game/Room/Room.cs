using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// The room class should be attached to every room on a floor. It contains references to all of the objects in the room.
/// </summary>
public class Room : MonoBehaviour
{
	
	public enum ERoomObjects
	{
		INVALID = -1,

		Enemy,
		Chest,
		Loot,

		MAX,
	}

    #region Fields

    private Dictionary<int, GameObject> parentRootNodes = new Dictionary<int, GameObject>();

	public Vector3 minCamera = new Vector3(-2.0f, 24.0f, -2.0f);
	public Vector3 maxCamera = new Vector3(2.0f, 24.0f, 2.0f);
	private Vector3 curMinCamera = new Vector3(-2.0f, 24.0f, -2.0f);
	private Vector3 curMaxCamera = new Vector3(2.0f, 24.0f, 2.0f);

	private const int maxDoors = 4;
	protected Doors doors;
    public bool startRoom = false;

    public Doors Doors
    {
        get { return doors; }
        set { doors = value; }
    }
    
    private Door entryDoor;
    public Door EntryDoor
    {
        get { return entryDoor; }
        set { entryDoor = value; }
    }

    protected List<Character> enemies;
    public List<Character> Enemies
    {
        get { return enemies; }
    }

	protected List<TreasureChest> chests;
	public List<TreasureChest> Chests
	{
		get { return chests; }
	}

	protected List<LootDrop> lootDrops;
	public List<LootDrop> LootDrops
	{
		get { return lootDrops; }
	}


	protected List<MoveableBlock> moveables;
	public List<MoveableBlock> Moveables
	{
		get { return moveables; }
	}

    protected RoomFloorNav navMesh;
	public RoomFloorNav NavMesh
	{
		get 
        {
            if (navMesh == null)
            {
                GameObject go = GameObject.Instantiate(Resources.Load("Prefabs/RoomPieces/RoomNav")) as GameObject;
                go.transform.position = transform.position + go.transform.position;
                go.transform.parent = transform;

                navMesh = go.GetComponent<RoomFloorNav>();
            }
            return navMesh; 
        }
		set { navMesh = value; }
	}

    private GameObject monstersNode;

    #endregion

	public void Initialise()
	{
		// Make sure all the nodes are found and references.
		FindAllNodes();

        // Find the monsters for this room
        monstersNode = GetNodeByLayer("Monster");

        if (monstersNode == null)
        {
            // Obviously it does not exist so we can create one.
            monstersNode = AddNewParentCategory("Monsters", LayerMask.NameToLayer("Monster"));
            Debug.LogWarning("Could not find Monsters GameObject in Room. Creating one now.", gameObject);
        }

		navMesh = NavMesh;

        // Find the doors for this room
        doors = GetNodeByLayer("Environment").GetComponentInChildren<Doors>();

        if (doors == null)
        {
            Debug.LogWarning("Could not find any doors for this room");
        }

		if (enemies == null)
		{
			Enemy[] roomEnemies = gameObject.GetComponentsInChildren<Enemy>() as Enemy[];

			if (roomEnemies.Length > 0)
			{
				enemies = new List<Character>();

				foreach (Enemy e in roomEnemies)
				{
					if (!enemies.Contains(e))
					{
						e.ContainedRoom = this;
						enemies.Add(e);
					}
				}
			}
		}
	}

    public void OnEnable()
    {
		Game.Singleton.Tower.CurrentFloor.FloorCamera.minCamera = transform.position + minCamera;
		Game.Singleton.Tower.CurrentFloor.FloorCamera.maxCamera = transform.position + maxCamera;
		curMinCamera = minCamera;
		curMaxCamera = maxCamera;

		if(chests == null)
		{
			TreasureChest[] roomChest = gameObject.GetComponentsInChildren<TreasureChest>() as TreasureChest[];

			if (roomChest.Length > 0)
			{
				chests = new List<TreasureChest>();

				foreach (TreasureChest t in roomChest)
				{
					if (!chests.Contains(t))
					{
						chests.Add(t);
					}
				}
			}
		}

		if (chests != null)
		{
			if (lootDrops == null)
			{
				foreach (TreasureChest c in chests)
				{
					LootDrop[] roomLoot = c.gameObject.GetComponentsInChildren<LootDrop>() as LootDrop[];

					if (roomLoot.Length > 0)
					{
						lootDrops = new List<LootDrop>();

						foreach (LootDrop t in roomLoot)
						{
							if (!lootDrops.Contains(t))
							{
								lootDrops.Add(t);
							}
						}
					}
				}
			}
		}

		if(moveables == null)
		{
			MoveableBlock[] roomMoveables = gameObject.GetComponentsInChildren<MoveableBlock>() as MoveableBlock[];
			if (roomMoveables.Length > 0)
			{
				moveables = new List<MoveableBlock>();

				foreach (MoveableBlock m in roomMoveables)
				{
					if (!moveables.Contains(m))
					{
						moveables.Add(m);
					}
				}
			}
		}
    }

	void Update()
	{
		CheckDoors();

		if (minCamera != curMinCamera)
		{
			curMinCamera = minCamera;
			Game.Singleton.Tower.CurrentFloor.FloorCamera.minCamera = transform.position + minCamera;
		}
		if (maxCamera != curMaxCamera)
		{
			curMaxCamera = maxCamera;
			Game.Singleton.Tower.CurrentFloor.FloorCamera.maxCamera = transform.position + maxCamera;
		}
	}

	void CheckDoors()
	{
        if (doors == null)
        {
            return;
        }

		Door[] roomDoors = doors.doors;
		for (int i = 0; i < roomDoors.Length; ++i)
		{
			if (roomDoors[i] != null)
			{
				roomDoors[i].Process();
			}
		}
	}

    /// <summary>
    /// Adds a root node child to the room tree. This will serve as a transform for adding items
    /// of same layer type to.
    /// </summary>
    /// <param name="name">The name of the node</param>
    /// <param name="layer">The layer objects that the node holds</param>
    public GameObject AddNewParentCategory(string name, int layer)
    {
        GameObject go = new GameObject(name);
        go.transform.parent = transform;
        go.tag = "RoomNode";
        go.layer = layer;

        parentRootNodes.Add(layer, go);

        return go;
    }

    public GameObject AddSubParent(string name, GameObject parent, int layer)
    {
        GameObject go = null;

        if (parent != null)
        {
            go = new GameObject(name);
            go.transform.parent = parent.transform;
            go.tag = "RoomNode";
            go.layer = layer;
        }

        return go;
    }

    /// <summary>
    /// Gets the node associated with the given layer. These nodes are used for organizing 
    /// objects of same layer into 1 node for the room.
    /// </summary>
    /// <param name="layer">The layer of the node we want to retrieve.</param>
    /// <returns></returns>
    public GameObject GetNodeByLayer(string layer)
    {
        GameObject go = null;
        int nameLayer = LayerMask.NameToLayer(layer);

        if (parentRootNodes.ContainsKey(nameLayer))
        {
            go = parentRootNodes[nameLayer];

            if (go == null)
            {
                Debug.Log("The node for layer: " + layer + " does not exist");
            }
        }
        else
        {
            Debug.Log("Could not find node with layer " + layer + 
                " you may want to use AddNewParentCategory to add it");
        }

        return go;
    }

    /// <summary>
    /// Fixes the tree structure by categorizing all objects into layers and nodes.
    /// TODO: Implement this function.
    /// Note: This function could be potentially very slow.
    /// </summary>
    public void FixTreeStructure()
    {
        foreach (Transform T in transform)
        {
            // If the object is not a parent container we need to put it in the right place.
            if (T.tag != "RoomNode")
            {
                // Find out if the objects layer exists in here.
                if (!parentRootNodes.ContainsKey(T.gameObject.layer))
                {
                    // We should create this layer node
                    // and assign the transform to this 
                    GameObject go = AddNewParentCategory(LayerMask.LayerToName(T.gameObject.layer), T.gameObject.layer);
                    T.parent = go.transform;
                }
                else
                {
                    GameObject go = GetNodeByLayer(LayerMask.LayerToName(T.gameObject.layer));
                    T.parent = go.transform;
                }

                return;
            }
        }
    }

    /// <summary>
    /// Helper function to find all the nodes and add them to 
    /// the dictionairy of node references.
    /// </summary>
    private void FindAllNodes()
    {
        // We want to get the second level nodes from this room object.
        foreach (Transform T in transform)
        {
            if (!parentRootNodes.ContainsKey(T.gameObject.layer))
            {
                // We want to make sure that we have a reference to 
                // all of the nodes in our room object.
                parentRootNodes.Add(T.gameObject.layer, T.gameObject);
            }
        }
    }

	public GameObject InstantiateGameObject(ERoomObjects type, string name)
	{
		GameObject newObject = null;

		switch (type)
		{
			case ERoomObjects.Chest:
				{
				}
				break;
			case ERoomObjects.Loot:
				{
                    newObject = GameObject.Instantiate(Resources.Load("Prefabs/RoomPieces/" + name)) as GameObject;

					if(lootDrops == null)
					{
						lootDrops = new List<LootDrop>();
					}

					lootDrops.Add(newObject.GetComponent<LootDrop>());
				}
				break;
			case ERoomObjects.Enemy:
				{
                    newObject = GameObject.Instantiate(Resources.Load("Prefabs/Enemies/" + name)) as GameObject;

                    if (enemies == null)
                    {
                        enemies = new List<Character>();
                    }

                    Enemy enemy = newObject.GetComponent<Enemy>();
                    enemies.Add(enemy);
                    enemy.transform.parent = monstersNode.transform;
                    enemy.ContainedRoom = this;
                    enemy.Initialise();
					enemy.InitiliseHealthbar();
				}
				break;
		}

		return newObject;
	}

	public void RemoveObject(ERoomObjects type, GameObject go)
	{
		switch(type)
		{
			case ERoomObjects.Loot:
				{
					lootDrops.Remove(go.GetComponent<LootDrop>());
					Destroy(go);
				}
				break;
		}
	}


	public bool CheckCollisionArea(Shape2D shape, Character.EScope scope, ref List<Character> charactersColliding)
	{
		Shape2D.EType type = shape.type;

		List<Character> characters = GetCharacterList(scope);

		if (characters == null || characters.Count == 0)
		{
			return false;
		}

		switch(type)
		{
			case Shape2D.EType.Circle:
				{
					foreach (Character c in characters)
                    {
						if (c.IsDead)
                        {
                            continue;
                        }

						if (CheckCircle(shape as Circle, c))
                        {
                            charactersColliding.Add(c);
                        }
                    }
				}
				break;
			case Shape2D.EType.Arc:
				{
					foreach (Character c in characters)
					{
						if(c.IsDead)
						{
							continue;
						}					

						if(CheckArc(shape as Arc, c))
						{
							charactersColliding.Add(c);
						}
					}
				}
				break;
			default:
				{
					Debug.LogError("Unhandled case");
				}
				break;
		}


		return charactersColliding.Count > 0;
	}

	public List<Character> GetCharacterList(Character.EScope scope)
	{
		List<Character> characters = null;

		switch(scope)
		{
			case Character.EScope.All:
				{
					List<Player> players = Game.Singleton.Players;

					if (enemies != null)
					{
						characters = new List<Character>(enemies);
					}

					foreach (Player p in players)
					{
						characters.Add(p.Hero.GetComponent<Hero>());
					}
				}
				break;
			case Character.EScope.Enemy:
				{
					if (enemies != null)
					{
						characters = new List<Character>(enemies);
					}
				}
				break;
			case Character.EScope.Hero:
				{
					List<Player> players = Game.Singleton.Players;

					characters = new List<Character>();

					foreach (Player p in players)
					{
						characters.Add(p.Hero.GetComponent<Hero>());
					}

					return characters;
				}
			default:
				{
					Debug.LogError("Invalid case");
				}
				break;
		}

		return characters;
	}

	public bool CheckCircle(Circle circle, Character c)
	{
		// Check the radius of the circle shape against the extents of the enemy.
		Vector3 extents = new Vector3(c.collider.bounds.extents.x, 0.5f, c.collider.bounds.extents.z);
		Vector3 pos = new Vector3(c.transform.position.x, 0.5f, c.transform.position.z);

		return (MathUtility.IsCircleCircle(circle.Position, circle.radius, pos, (extents.x + extents.z * 0.5f)));
	}

	public bool CheckArc(Arc arc, Character c)
	{
        Transform xform = c.transform.FindChild("Colliders");
        if (xform != null)
        {
            Collider[] colliders = xform.gameObject.GetComponentsInChildren<Collider>();

            foreach (Collider col in colliders)
            {
                if ((CheckArc(arc, c, col)))
                {
                    return true;
                }
            }
            return false;
        }

        return CheckArc(arc, c, c.collider);
	}

    public bool CheckArc(Arc arc, Character character, Collider col)
    {
        Vector3 extents = new Vector3(col.bounds.extents.x, 0.1f, col.bounds.extents.z);
        Vector3 pos = col.transform.position;

        bool inside = false;

        // TL
        Vector3 point = new Vector3(pos.x - extents.x, pos.y, pos.z + extents.z);
        inside = MathUtility.IsWithinCircleArc(point, arc.Position, arc.Line1, arc.Line2, arc.radius);
#if UNITY_EDITOR
        Debug.DrawLine(col.transform.position, col.transform.position + new Vector3(-extents.x, extents.y, extents.z), Color.white, 0.2f);
#endif

        // T
        if (!inside)
        {
            point = new Vector3(pos.x + extents.x, pos.y, pos.z + extents.z);
            inside = MathUtility.IsWithinCircleArc(point, arc.Position, arc.Line1, arc.Line2, arc.radius);

#if UNITY_EDITOR
            Debug.DrawLine(col.transform.position, col.transform.position + new Vector3(0.0f, extents.y, extents.z), Color.white, 0.2f);
#endif
        }

        // TR
        if (!inside)
        {
            point = new Vector3(pos.x + extents.x, pos.y, pos.z + extents.z);
            inside = MathUtility.IsWithinCircleArc(point, arc.Position, arc.Line1, arc.Line2, arc.radius);

#if UNITY_EDITOR
            Debug.DrawLine(col.transform.position, col.transform.position + extents, Color.white, 0.2f);
#endif
        }

        // BL
        if (!inside)
        {
            point = new Vector3(pos.x - extents.x, pos.y, pos.z - extents.z);
            inside = MathUtility.IsWithinCircleArc(point, arc.Position, arc.Line1, arc.Line2, arc.radius);

#if UNITY_EDITOR
            Debug.DrawLine(col.transform.position, col.transform.position + new Vector3(-extents.x, extents.y, -extents.z), Color.white, 0.2f);
#endif
        }

        // B
        if (!inside)
        {
            point = new Vector3(pos.x + extents.x, pos.y, pos.z + extents.z);
            inside = MathUtility.IsWithinCircleArc(point, arc.Position, arc.Line1, arc.Line2, arc.radius);

#if UNITY_EDITOR
            Debug.DrawLine(col.transform.position, col.transform.position + new Vector3(0.0f, extents.y, -extents.z), Color.white, 0.2f);
#endif
        }

        // BR
        if (!inside)
        {
            point = new Vector3(pos.x + extents.x, pos.y, pos.z - extents.z);
            inside = MathUtility.IsWithinCircleArc(point, arc.Position, arc.Line1, arc.Line2, arc.radius);

#if UNITY_EDITOR
            Debug.DrawLine(col.transform.position, col.transform.position + new Vector3(extents.x, extents.y, -extents.z), Color.white, 0.2f);
#endif
        }

        // L
        if (!inside)
        {
            point = new Vector3(pos.x + extents.x, pos.y, pos.z + extents.z);
            inside = MathUtility.IsWithinCircleArc(point, arc.Position, arc.Line1, arc.Line2, arc.radius);

#if UNITY_EDITOR
            Debug.DrawLine(col.transform.position, col.transform.position + new Vector3(-extents.x, extents.y, 0.0f), Color.white, 0.2f);
#endif
        }

        // R
        if (!inside)
        {
            point = new Vector3(pos.x + extents.x, pos.y, pos.z + extents.z);
            inside = MathUtility.IsWithinCircleArc(point, arc.Position, arc.Line1, arc.Line2, arc.radius);

#if UNITY_EDITOR
            Debug.DrawLine(col.transform.position, col.transform.position + new Vector3(extents.x, extents.y, 0.0f), Color.white, 0.2f);
#endif
        }

        return inside;
    }
}
