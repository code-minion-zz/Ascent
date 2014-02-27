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
        Barrel,
		MAX,
	}

    public enum EMonsterTypes
    {
        Rat,
        Slime,
        Imp,
        EnchantedStatue,
        Abomination,
        Boss,

        MAX
    }

    #region Fields

    private Dictionary<int, GameObject> parentRootNodes = new Dictionary<int, GameObject>();
    private int numberOfTilesX;
    private int numberOfTilesY;

	[HideInInspector]
	public Vector3 minCamera = new Vector3(-3.0f, 24.0f, -8.0f);
	[HideInInspector]
	public Vector3 maxCamera = new Vector3(3.0f, 24.0f, 0.0f);
	
	[HideInInspector]
	public float cameraHeight = 20.0f;
	
	[HideInInspector]
	public float cameraOffsetZ = 0.35f;

	protected Doors doors;
	[HideInInspector]
    public bool startRoom = false;

    protected List<Character> enemies = new List<Character>();
    protected List<TreasureChest> chests = new List<TreasureChest>();
    protected List<LootDrop> lootDrops = new List<LootDrop>();
    protected List<MoveableBlock> moveables = new List<MoveableBlock>();
    protected List<BreakableEnvObject> breakables = new List<BreakableEnvObject>();

    public int NumberOfTilesX
    {
        get { return numberOfTilesX; }
        set { numberOfTilesX = value; }
    }

    public int NumberOfTilesY
    {
        get { return numberOfTilesY; }
        set { numberOfTilesY = value; }
    }

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

    public List<Character> Enemies
    {
        get { return enemies; }
    }

	public List<TreasureChest> Chests
	{
		get { return chests; }
	}

	public List<LootDrop> LootDrops
	{
		get { return lootDrops; }
	}

	public List<MoveableBlock> Moveables
	{
		get { return moveables; }
	}

    public List<BreakableEnvObject> Breakables
    {
        get { return breakables; }
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

    private Transform environmentNode;
    private GameObject monstersNode;

    public Transform MonsterParent
    {
        get
        {
            if (monstersNode == null)
            {
                monstersNode = GetNodeByLayer("Monster");

                if (monstersNode != null)
                {
                    return monstersNode.transform;
                }
                else
                {
                    monstersNode = AddNewParentCategory("Monsters", (int)Layer.Monster);
					Debug.Log(monstersNode.layer);
                    return monstersNode.transform;
                }
            }
            else
            {
                return monstersNode.transform;
            }
        }
    }

    public Transform EnvironmentParent
    {
        get
        {
            if (environmentNode == null)
            {
                environmentNode = GetNodeByLayer("Environment").transform;

                if (environmentNode != null)
                {
                    return environmentNode;
                }
                else
                {
					environmentNode = AddNewParentCategory("Environment", (int)Layer.Environment).transform;
                    return environmentNode;
                }
            }
            else
            {
                return environmentNode;
            }
        }
    }

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
			monstersNode = AddNewParentCategory("Monsters", (int)Layer.Monster);
            Debug.LogWarning("Could not find Monsters GameObject in Room. Creating one now.", gameObject);
        }

		navMesh = NavMesh;

        // Find the doors for this room
        doors = EnvironmentParent.GetComponentInChildren<Doors>();

        if (doors == null)
        {
            Debug.LogWarning("Could not find any doors for this room");
        }
	}

    public void OnEnable()
    {
        if (Game.Singleton.Tower.CurrentFloor != null && Game.Singleton.Tower.CurrentFloor.initialised)
		{
			FloorCamera camera = Game.Singleton.Tower.CurrentFloor.FloorCamera;
			camera.minCamera = transform.position + minCamera;
			camera.maxCamera = transform.position + maxCamera;
			//curMinCamera = minCamera;
			//curMaxCamera = maxCamera;

			//Debug.Log(camera.maxCamera);
			//camera.CameraHeight = cameraHeight;
		}

        // Find and populate all the lists of objects in the room.
        PopulateListOfObjects(ref enemies, gameObject);
        PopulateListOfObjects(ref breakables, gameObject);
        PopulateListOfObjects(ref moveables, gameObject);
        PopulateListOfObjects(ref chests, gameObject);

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

		if (enemies != null)
		{
			foreach (Enemy e in enemies)
			{
				if (!navMesh.IsWithinBounds(e.transform.position))
				{
					e.transform.position = transform.position;
				}
			}
		}
    }

    /// <summary>
    /// Pass in a list to populate it with found objects of that type in the parent.
    /// </summary>
    /// <typeparam name="T">The type of the component.</typeparam>
    /// <param name="populateList">The list to populate.</param>
    /// <param name="parent">The parent to get components from children.</param>
    private void PopulateListOfObjects<T>(ref List<T> populateList, GameObject parent) where T: Component
    {
        T[] foundObjects = parent.GetComponentsInChildren<T>();

        if (foundObjects != null && foundObjects.Length > 0)
        {
            populateList = new List<T>();

            foreach (T type in foundObjects)
            {
                if (!populateList.Contains(type))
                {
                    if (type.GetType() == typeof(Enemy))
                    {
                        Enemy enemy = type as Enemy;
                        enemy.ContainedRoom = this;
                    }

                    populateList.Add(type);
                }
            }
        }
    }

	void Update()
	{
		CheckDoors();

		////if (minCamera != curMinCamera)
		//{
		//    curMinCamera = minCamera;
		//    Game.Singleton.Tower.CurrentFloor.FloorCamera.minCamera = transform.position + minCamera;
		//}
		////if (maxCamera != curMaxCamera)
		//{
		//    curMaxCamera = maxCamera;
		//    Game.Singleton.Tower.CurrentFloor.FloorCamera.maxCamera = transform.position + maxCamera;
		//}
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
    public void FindAllNodes()
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
                    newObject = GameObject.Instantiate(Resources.Load("Prefabs/RoomPieces/" + name), Vector3.zero, Quaternion.identity) as GameObject;

                    if (chests == null)
                    {
                        chests = new List<TreasureChest>();
                    }

                    TreasureChest chest = newObject.GetComponent<TreasureChest>();
                    chest.transform.parent = this.transform;
                    chest.transform.localPosition = Vector3.zero;
                    chests.Add(chest);
				}
				break;
			case ERoomObjects.Loot:
				{
                    newObject = GameObject.Instantiate(Resources.Load("Prefabs/RoomPieces/" + name), Vector3.zero, Quaternion.identity) as GameObject;

					if(lootDrops == null)
					{
						lootDrops = new List<LootDrop>();
					}

					lootDrops.Add(newObject.GetComponent<LootDrop>());
				}
				break;
			case ERoomObjects.Enemy:
				{
                    newObject = GameObject.Instantiate(Resources.Load("Prefabs/Enemies/" + name), Vector3.zero, Quaternion.identity) as GameObject;

                    if (enemies == null)
                    {
                        enemies = new List<Character>();
                    }

                    Enemy enemy = newObject.GetComponent<Enemy>();
                    enemies.Add(enemy);
                    enemy.transform.parent = monstersNode.transform;
                    enemy.transform.localPosition = Vector3.zero;
                    enemy.ContainedRoom = this;
                    enemy.Initialise();
					enemy.InitiliseHealthbar();
				}
				break;

            case ERoomObjects.Barrel:
                {
                    newObject = GameObject.Instantiate(Resources.Load("Prefabs/RoomPieces/" + name)) as GameObject;
                    newObject.transform.parent = EnvironmentParent;
                }
                break;
		}

		return newObject;
	}


	public GameObject InstantiateGameObject(string name)
	{
		GameObject newObject = null;

		newObject = GameObject.Instantiate(Resources.Load(name)) as GameObject;

		Vector3 localPosition = newObject.transform.position;
		Vector3 localScale = newObject.transform.localScale;
		Quaternion localRotation = newObject.transform.localRotation;

		newObject.transform.parent = this.transform;
		newObject.transform.position = localPosition;
		newObject.transform.localScale = localScale;
		newObject.transform.rotation = localRotation;

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

    public void ProcessCollisionBreakables(Shape2D shape)
    {
        Shape2D.EType type = shape.type;

        switch (type)
        {
            case Shape2D.EType.Circle:
                {
                    foreach (BreakableEnvObject b in breakables)
                    {
                        if (b.IsDestroyed)
                        {
                            continue;
                        }

                        if (CheckCircle(shape as Circle, b.GetComponentInChildren<Collider>()))
                        {
                            // Destroy the breakable.
                            b.Explode();
                        }
                    }
                }
                break;
            case Shape2D.EType.Arc:
                {
                    foreach (BreakableEnvObject b in breakables)
                    {
                        if (b.IsDestroyed)
                        {
                            continue;
                        }

                        if (CheckArc(shape as Arc, b.GetComponentInChildren<Collider>()))
                        {
                            // Destroy the breakable.
                            b.Explode();
                        }
                    }
                }
                break;
            default:
                {
                    Debug.LogError("No more shapezies for you");
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
							if (!charactersColliding.Contains(c))
							{
								charactersColliding.Add(c);
							}
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
							if (!charactersColliding.Contains(c))
							{
								charactersColliding.Add(c);
							}
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

						foreach (Player p in players)
						{
							characters.Add(p.Hero);
						}
					}
					else
					{
						characters = new List<Character>();

						foreach (Player p in players)
						{
							characters.Add(p.Hero);
						}
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

    public bool CheckCircle(Circle circle, Collider collider)
    {
        Vector3 extents = new Vector3(collider.bounds.extents.x, 0.5f, collider.bounds.extents.z);
        Vector3 pos = new Vector3(collider.transform.position.x, 0.5f, collider.transform.position.z);

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
                if ((CheckArc(arc, col)))
                {
                    return true;
                }
            }
            return false;
        }

        return CheckArc(arc, c.collider);
	}

    public bool CheckArc(Arc arc, Collider col)
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

	public GameObject FindHeroTarget(Hero hero, Shape2D shape)
	{
		GameObject target = FindTargetsInFront(hero, shape);
		//protected List<Character> enemies = new List<Character>();
		//protected List<TreasureChest> chests = new List<TreasureChest>();
		//protected List<LootDrop> lootDrops = new List<LootDrop>();
		//protected List<MoveableBlock> moveables = new List<MoveableBlock>();
		//protected List<BreakableEnvObject> breakables = new List<BreakableEnvObject>();

		// Check enemies in front first.


		
		//hero.transform.forward
		
		// Check objects in front.

		// Check enemies in area.

		// Check objects in area.

		return target;
	}

	private GameObject FindTargetsInFront(Hero hero, Shape2D shape)
	{
		GameObject target = null;
		List<Character> enemyTargets = new List<Character>();

		Character closestCharacter = null;

		//if (CheckCollisionArea(circa, Character.EScope.Enemy, ref enemyTargets))
		if (CheckCollisionArea(shape, Character.EScope.Enemy, ref enemyTargets))
		//if (CheckCollisionArea(new Arc(hero.transform, 10.0f, 30.0f, -(hero.transform.forward * 2.5f)), Character.EScope.Enemy, ref enemyTargets))
		//if (CheckCollisionArea(new Circle(hero.transform, 3.0f, new Vector3(0.0f, 0.0f, 3.0f)), Character.EScope.Enemy, ref enemyTargets))
		{
			float closestDistance = 1000000.0f;
			foreach (Character e in enemyTargets)
			{
				float distance = (hero.transform.position - e.transform.position).sqrMagnitude;

				if (distance < closestDistance)
				{
					closestDistance = distance;
					closestCharacter = e;
				}
			}
		}
	

		if (closestCharacter != null)
		{
			target = closestCharacter.gameObject;	
		}

		return target;
	}

	[ContextMenu("Go to Room")]
	public void Reposition()
	{
		Floor floor = Game.Singleton.Tower.CurrentFloor;
		Room currentRoom = floor.CurrentRoom;

		if (currentRoom == this)
		{
			return;
		}

		// Find direction to enter from
		float heading = MathUtility.ConvertVectorToHeading((transform.position - currentRoom.transform.position).normalized) * Mathf.Rad2Deg;

		bool transitioned = false;

		// Try north
		if (heading >= -0.45f && heading <= 45.0f)
		{
			if (doors.doors[(int)Floor.TransitionDirection.South] != null)
			{
				Game.Singleton.Tower.CurrentFloor.TransitionToRoom(Floor.TransitionDirection.North, doors.doors[(int)Floor.TransitionDirection.South]);
				transitioned = true;
			}
		}

		// Try East
		if (!transitioned && (heading >= 0.45f && heading <= 135.0f))
		{
			if (doors.doors[(int)Floor.TransitionDirection.West] != null)
			{
				Game.Singleton.Tower.CurrentFloor.TransitionToRoom(Floor.TransitionDirection.North, doors.doors[(int)Floor.TransitionDirection.West]);
				transitioned = true;
			}
		}

		// Try South
		if (!transitioned && ((heading >= 135.0f && heading <= 180.0f) || (heading <= -135.0f)))
		{
			if (doors.doors[(int)Floor.TransitionDirection.North] != null)
			{
				Game.Singleton.Tower.CurrentFloor.TransitionToRoom(Floor.TransitionDirection.North, doors.doors[(int)Floor.TransitionDirection.North]);
				transitioned = true;
			}
		}

		// Try West
		if (!transitioned && (heading <= -0.45f && heading >= -135.0f))
		{
			if (doors.doors[(int)Floor.TransitionDirection.East] != null)
			{
				Game.Singleton.Tower.CurrentFloor.TransitionToRoom(Floor.TransitionDirection.North, doors.doors[(int)Floor.TransitionDirection.East]);
				transitioned = true;
			}
		}

		if(!transitioned)
		{
			// Try any door :<

			if (doors.doors[(int)Floor.TransitionDirection.South] != null)
			{
				Game.Singleton.Tower.CurrentFloor.TransitionToRoom(Floor.TransitionDirection.North, doors.doors[(int)Floor.TransitionDirection.South]);
				transitioned = true;
			}
			else if (doors.doors[(int)Floor.TransitionDirection.West] != null)
			{
				Game.Singleton.Tower.CurrentFloor.TransitionToRoom(Floor.TransitionDirection.North, doors.doors[(int)Floor.TransitionDirection.West]);
				transitioned = true;
			}
			else if (doors.doors[(int)Floor.TransitionDirection.North] != null)
			{
				Game.Singleton.Tower.CurrentFloor.TransitionToRoom(Floor.TransitionDirection.North, doors.doors[(int)Floor.TransitionDirection.North]);
				transitioned = true;
			}
			else if (doors.doors[(int)Floor.TransitionDirection.East] != null)
			{
				Game.Singleton.Tower.CurrentFloor.TransitionToRoom(Floor.TransitionDirection.North, doors.doors[(int)Floor.TransitionDirection.East]);
				transitioned = true;
			}

			if (!transitioned)
			{
				Debug.Log("No Direction or no door to enter, " + heading);
			}
		}
	}
}
