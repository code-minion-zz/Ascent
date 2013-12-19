using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TreasureChest : MonoBehaviour
{
	protected enum EChestState
	{
		Closed = 0,
		Openning,
		DroppingLoot,
		Openned,

		NEXT,
		MAX,

	}

	protected enum EChestType
	{
		Loot = 0, // bit of everything
		Consumables, // only consumables
		Accessories, // only accessories
		Trap, // a variation of a trap
	}

	protected EChestState curState = EChestState.Closed;

	protected float timeAccum = 0.0f;
	protected float[] stateTimes = new float[(int)EChestState.MAX] { 0.0f, 0.25f, 0.15f, 0.0f, 0.0f };

	protected EChestType chestType = EChestType.Loot;

	public GameObject baseMesh;
	public GameObject lidMesh;

    protected Quaternion defaultRot;
	protected Quaternion openRot;

	protected int quantityOfLoot;
	protected List<Item> loot;

	protected Room containedRoom;

	protected TriggerRegion triggerRegion;
	public TriggerRegion TriggerRegion
	{
		get { return triggerRegion; }
	}

	public bool IsClosed
	{
		get { return (curState == EChestState.Closed); }
	}

    // Use this for initialization
	void Start () 
	{
		triggerRegion = GetComponent<TriggerRegion>();

		defaultRot = lidMesh.transform.rotation;
        openRot = new Quaternion(0.0f, 0.6f, 0.8f, 0.0f);

		//RandomlySetChestType();

		if (chestType == EChestType.Trap)
		{
			RandomlySetTrapProperties();
		}
		else
		{
			RandomlyGenerateLootDrops();
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		timeAccum += Time.deltaTime;
		if (timeAccum > stateTimes[(int)curState])
		{
			timeAccum = stateTimes[(int)curState];
		}

		switch (curState)
		{
			case EChestState.Closed:
				{
					// Do nothing until a player opens it.
					// Opens via HeroController calling into Open chest.
				}
				break;
			case EChestState.Openning:
				{
					// Open over time
					lidMesh.transform.rotation = Quaternion.Slerp(defaultRot, openRot, timeAccum / stateTimes[(int)EChestState.Openning]);
					
					if (timeAccum >= stateTimes[(int)EChestState.Openning])
					{
						ChangeState(EChestState.NEXT);
					}
				}
				break;
			case EChestState.DroppingLoot:
				{
					if (chestType != EChestType.Trap)
					{
						if (timeAccum >= stateTimes[(int)EChestState.DroppingLoot])
						{
							// TODO: Change this to be the correct representation for this item.
							GameObject go = Game.Singleton.Tower.CurrentFloor.CurrentRoom.InstantiateGameObject(Room.ERoomObjects.Loot);
							go.transform.parent = this.transform;
							Vector3 pos = this.transform.position;
							pos.y = 5.0f;
							go.transform.position = pos;

							LootDrop lootDrop = go.GetComponent<LootDrop>();

							// Assign this drop the randomly generated item
							lootDrop.Item = loot[0];

							lootDrop.StartFalling(containedRoom);

							loot.RemoveAt(0);

							timeAccum = 0.0f;

							if (loot.Count == 0)
							{
								ChangeState(EChestState.NEXT);
							}
						}

						//    foreach (GameObject lootObject in lootObjects)
						//    {
						//        //lootObject.SetActive(true);
						//        //lootObject.rigidbody.AddExplosionForce(30.0f, lootSpawn.position, 20.0f);

						//        lootObject.SetActive(true);


						//        float radius = 100.0f;

						//        Vector3 rand = Random.insideUnitSphere;
						//        Vector3 newVec = new Vector3(Mathf.Clamp(rand.x, -1, 1), 30.0f, Mathf.Clamp(rand.z, -1, 1));
						//        Vector3 force = new Vector3(newVec.x * radius, 30.0f, newVec.z * radius);

						//        lootObject.rigidbody.AddForce(force);
						//    }
					}
					else
					{
						// TODO: Make the trap things happen here...
						
						ChangeState(EChestState.NEXT);
					}


				}
				break;
			case EChestState.Openned:
				{
					// Do nothing.
				}
				break;
		}
	}

	public void OpenChest()
	{
		if(curState == EChestState.Closed)
		{
			containedRoom = Game.Singleton.Tower.CurrentFloor.CurrentRoom;

			ChangeState(EChestState.NEXT);
		}
	}	

	protected void RandomlySetChestType()
	{
		// Randomly choose the chest type
		int iRandom = Random.Range(1, 10000);
		float fRandom = (float)(iRandom / 10000);

		// It is a weighted random (these values are in the GDD)
		if (fRandom < 0.5f) // 50% chance
		{
			chestType = EChestType.Accessories;
		}
		else if (fRandom < 0.7f) // 20% chance
		{
			chestType = EChestType.Consumables;
		}
		else if (fRandom < 0.9f) // 20% chance
		{
			chestType = EChestType.Loot;
		}
		else if (fRandom < 1.0f) // 10% chance
		{
			chestType = EChestType.Trap;
		}
	}

	protected void RandomlyGenerateLootDrops()
	{
		// Refer to GDD on how this works.
		// Right now just random a quantity and make bags drop over time.
		// Drops do not need to be uniform.

		quantityOfLoot = Random.Range(5, 10); // TODO: include current bonuses to the roll
		
		// TODO: Randomly generate items to drop and add them into a list
		loot = new List<Item>(quantityOfLoot);
		
		//GameObject.Instantiate(Resources.Load("Prefabs/Rooms/CoinSack"));
		
		for (int i = 0; i < quantityOfLoot;  ++i)
		{
			Item newItem = LootGenerator.RandomlyGenerateAccessory(Game.Singleton.Tower.CurrentFloorNumber);
			loot.Add(newItem);
		}

		// Adjust loot dropping time
		stateTimes[(int)EChestState.DroppingLoot] = stateTimes[(int)EChestState.DroppingLoot] * quantityOfLoot;
	}

	protected void RandomlySetTrapProperties()
	{
		// TODO: Set Trap properties
	}

	protected void ChangeState(EChestState state)
	{
		if (state == EChestState.NEXT)
		{
			++curState;

			if (curState == EChestState.MAX)
			{
				curState = 0;
			}
		}
		else
		{
			curState = state;
		}

		timeAccum = 0.0f;
	}

	//protected float SwingOpen()
	//{
	//    time += Time.deltaTime * smoothing;
	//    hinge.transform.eulerAngles = Vector3.Lerp(currentRot, openRot, time);

	//    return time;
	//}

	//protected float SwingClose()
	//{
	//    time += Time.deltaTime * smoothing;
	//    hinge.transform.eulerAngles = Vector3.Lerp(currentRot, openRot, time);

	//    return time;
	//}

	////void OnTriggerEnter(Collider enter)
	////{
	////    string tag = enter.tag;

	////    switch (tag)
	////    {
	////        case "Hero":
	////            {
	////                canUse = true;
	////                Hero hero = enter.GetComponent<Hero>();
	////                hero.HeroController.Input.OnY += OnY;
	////            }
	////            break;
	////    }        
	////}

	//void OnTriggerStay(Collider stay)
	//{
	//    Debug.Log("Inside chest open area");
	//    string tag = stay.tag;

	//    switch (tag)
	//    {
	//        case "Hero":
	//            {
	//                Hero hero = stay.GetComponent<Hero>();
	//                hero.HeroController.EnableActionBinding = true;

	//                Debug.Log("Inside chest open area");

	//                //if (hero.HeroController.Input.Y.WasPressed)
	//                //{
	//                //    openChest = true;
	//                //}
	//            }
	//            break;
	//    }
	//}

	//void OnTriggerExit(Collider exit)
	//{
	//    string tag = exit.tag;

	//    switch (tag)
	//    {
	//        case "Hero":
	//            {
	//                canUse = false;
	//                // Switch the action keys.
	//                Hero hero = exit.GetComponent<Hero>();
	//                hero.HeroController.EnableActionBinding = false;
	//                Debug.Log("Exiting chest open area");
	//            }
	//            break;
	//    }
	//}

	//public void OnY(InputDevice device)
	//{
	//    if (canUse)
	//    {
	//        openChest = true;
	//    }
	//}

	///// <summary>
	///// The chest will spawn loot out of the object.
	///// </summary>
	//protected void SpawnLoot()
	//{
	//    foreach (GameObject lootObject in lootObjects)
	//    {
	//        //lootObject.SetActive(true);
	//        //lootObject.rigidbody.AddExplosionForce(30.0f, lootSpawn.position, 20.0f);

	//        lootObject.SetActive(true);


	//        float radius = 100.0f;

	//        Vector3 rand = Random.insideUnitSphere;
	//        Vector3 newVec = new Vector3(Mathf.Clamp(rand.x, -1, 1), 30.0f, Mathf.Clamp(rand.z, -1, 1));
	//        Vector3 force = new Vector3(newVec.x * radius, 30.0f, newVec.z * radius);

	//        lootObject.rigidbody.AddForce(force);
	//    }
	//}

	//protected void ResetLoot()
	//{
	//    foreach (GameObject lootObject in lootObjects)
	//    {
	//        lootObject.SetActive(false);
	//        lootObject.transform.position = lootSpawn.transform.position;
	//    }
	//}


}
