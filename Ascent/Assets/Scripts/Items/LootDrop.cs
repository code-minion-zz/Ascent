using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LootDrop : MonoBehaviour 
{
	public bool falling = false;
	private Vector3 startPos;
	private Vector3 targetPos;
	private float fallTime = 1.5f;
	private float timeAccum = 0.0f;
	public bool picked = false;
	private Room containedRoom;

	protected TriggerRegion triggerRegion;
	public TriggerRegion TriggerRegion
	{
		get { return triggerRegion; }
	}

	// This loot drop a representation of an item.
	protected Item item;
	public Item Item
	{
		get { return item; }
		set { item = value; }
	}

	public void OnEnable()
	{
		triggerRegion = GetComponent<TriggerRegion>();

		if (picked)
		{
			gameObject.SetActive(false);
		}
	}

	public bool CanBePickedUp
	{
		get { return !falling && !picked; }
	}

	public void StartFalling(Room containedRoom)
	{
		this.containedRoom = containedRoom;
		falling = true;
		startPos = transform.position;
		targetPos = containedRoom.NavMesh.GetRandomPositionOutsideRect(transform.position, new Vector3(3.5f, 0.0f, 3.5f));

        if (rigidbody != null)
        {
            rigidbody.AddTorque(new Vector3(100.0f, 100.0f, 100.0f));
        }
        else
        {
            GetComponentInChildren<Rigidbody>().AddTorque(new Vector3(100.0f, 100.0f, 100.0f));
        }
	}

	public void Update()
	{
		if (falling)
		{
			if (timeAccum < fallTime)
			{
				timeAccum += Time.deltaTime;
				if (timeAccum > fallTime)
				{
					timeAccum = fallTime;
					falling = false;
				}

				transform.position = Vector3.Lerp(startPos, targetPos, timeAccum / fallTime);
			}
		}
	}

	public void PickUp(HeroInventory inventory)
	{
		if (!falling)
		{
			inventory.AddItem(item);
			gameObject.SetActive(false);
			picked = true;
			containedRoom.RemoveObject(Room.ERoomObjects.Loot, this.gameObject);
		}
	}
}
