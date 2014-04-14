using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Barrel : EnvironmentBreakable
{
    protected List<Item> loot;
    protected Room containedRoom;
    protected int quantityOfLoot;
	protected Transform barrelStatic;
	protected Transform barrelDynamic;
	protected List<Transform> barrelParts;
	private float timeDead = 0f;
	private float physicsTime = 1.5f;
	private float destroyTime = 10f;

    public void Start()
    {
        quantityOfLoot = Random.Range(2, 5); // TODO: include current bonuses to the roll

        loot = new List<Item>(quantityOfLoot);

        for (int i = 0; i < quantityOfLoot; ++i)
        {
            Item newItem = LootGenerator.RandomlyGenerateItem(Game.Singleton.Tower.currentFloorNumber, LootGenerator.ELootType.Gold, true);
            loot.Add(newItem);
        }

		Transform model = transform.FindChild("Model");
		barrelStatic = model.FindChild("barrel_static");
		barrelDynamic = model.FindChild("barrel_dynamic");

		int j;
		barrelParts = new List<Transform>();
		for (j = 0; j < barrelDynamic.childCount; ++j)
		{
			barrelParts.Add(barrelDynamic.GetChild(j));
		}
		barrelDynamic.gameObject.SetActive(false);
    }

    public override void Update()
    {
        base.Update();

        if (isDestroyed)
        {
			if (timeDead > destroyTime) return;
			timeDead += Time.deltaTime;
			if (timeDead > destroyTime)
			{
				gameObject.SetActive(false);
			}
			else if (timeDead > physicsTime)
			{
				barrelParts.ForEach(t => t.rigidbody.isKinematic = true);
			}

            if (loot.Count == 0)
            {
                return;
            }

            containedRoom = Game.Singleton.Tower.CurrentFloor.CurrentRoom;
            GameObject go = containedRoom.InstantiateGameObject(Room.ERoomObjects.Loot, "Coins");
            go.transform.parent = containedRoom.EnvironmentParent;
            Vector3 pos = this.transform.position;
            pos.y = 1.0f;
            go.transform.position = pos;

            LootDrop lootDrop = go.GetComponent<LootDrop>();
            lootDrop.Item = loot[0];

            lootDrop.StartFalling(containedRoom);

			loot.RemoveAt(0);

            if (loot.Count == 0)
            {
				barrelStatic.gameObject.SetActive(false);
				barrelDynamic.gameObject.SetActive(true);
				collider.enabled = false;

				foreach(Transform trans in barrelParts)
				{
					Vector3 randForce;
					randForce.x = Random.Range(-200,200);
					randForce.y = Random.Range(-200,200);
					randForce.z = Random.Range(-200,200);
					trans.rigidbody.constantForce.torque = randForce;
				}
            }
        }
    }

	public override void BreakObject ()
	{
		SoundManager.PlaySound(AudioClipType.woodHit, transform.position, 2f);

		base.BreakObject ();
	}
}
