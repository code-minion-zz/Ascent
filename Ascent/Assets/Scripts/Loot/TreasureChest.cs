using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TreasureChest : Interactable
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


	public bool IsClosed
	{
		get { return (curState == EChestState.Closed); }
	}

    // Use this for initialization
	public override void Start () 
	{
		base.Start();
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


    /// <summary>
    /// Releases poison gas cloud inflicting nearby players and monsters. 
    /// - Damage dealt
    /// - Duration
    /// - Radius
    /// - Poison Status effect
    /// Values should be randomised later with the level generator.
    /// </summary>
    public void ReleasePoisonGas()
    {
        GameObject go = Resources.Load("Prefabs/PoisonGasCloud") as GameObject;
        go = Instantiate(go, gameObject.transform.position, Quaternion.identity) as GameObject;
        ParticleSystem ps = go.GetComponent<ParticleSystem>();

        if (ps != null)
        {
            ps.Play();
        }
    }

    /// <summary>
    /// Releases chain lightning that continuously seeks the closest target and deals damage until it has reached
    /// a specified number of targets
    /// - Damage dealt
    /// - Bounces (Number of targets)
    /// Values should be randomised later with the level generator.
    /// </summary>
    public void ReleaseChainLightning()
    {

    }

    /// <summary>
    /// Releases expanding ring of ice from the center of the chest. Deals damage and inflicts a slow. 
    /// a specified number of targets
    /// - Damage dealt
    /// - Radius
    /// - Slow status effect
    /// Values should be randomised later with the level generator.
    /// </summary>
    public void ReleaseIceNova()
    {

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
}
