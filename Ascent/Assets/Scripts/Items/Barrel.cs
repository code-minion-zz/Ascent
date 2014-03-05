using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Barrel : MonoBehaviour
{
    protected List<Item> loot;
    protected Room containedRoom;
    protected int quantityOfLoot;
    protected bool isDestroyed;

    public bool IsDestroyed
    {
        get { return isDestroyed; }
        set { isDestroyed = value; }
    }

    public void Start()
    {
        quantityOfLoot = Random.Range(2, 5); // TODO: include current bonuses to the roll

        // TODO: Randomly generate items to drop and add them into a list
        loot = new List<Item>(quantityOfLoot);

        for (int i = 0; i < quantityOfLoot; ++i)
        {
            // TODO: Generate coins not items.
            Item newItem = LootGenerator.RandomlyGenerateItem(Game.Singleton.Tower.CurrentFloorNumber, LootGenerator.ELootType.Any, true);
            loot.Add(newItem);
        }
    }

    public void Update()
    {
        if (isDestroyed)
        {
            if (loot.Count == 0)
            {
                return;
            }

            containedRoom = Game.Singleton.Tower.CurrentFloor.CurrentRoom;
            GameObject go = Game.Singleton.Tower.CurrentFloor.CurrentRoom.InstantiateGameObject(Room.ERoomObjects.Loot, "Coins");
            go.transform.parent = this.transform;
            Vector3 pos = this.transform.position;
            pos.y = 1.0f;
            go.transform.position = pos;

            LootDrop lootDrop = go.GetComponent<LootDrop>();
            lootDrop.Item = loot[0];

            lootDrop.StartFalling(containedRoom);

			loot.RemoveAt(0);

            if (loot.Count == 0)
            {
                //this.gameObject.SetActive(false);
            }
        }
    }
}
