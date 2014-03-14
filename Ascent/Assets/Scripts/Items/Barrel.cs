using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Barrel : BreakableEnvObject
{
    protected List<Item> loot;
    protected Room containedRoom;
    protected int quantityOfLoot;

    public void Start()
    {
        quantityOfLoot = Random.Range(2, 5); // TODO: include current bonuses to the roll

        loot = new List<Item>(quantityOfLoot);

        for (int i = 0; i < quantityOfLoot; ++i)
        {
            Item newItem = LootGenerator.RandomlyGenerateItem(Game.Singleton.Tower.CurrentFloorNumber, LootGenerator.ELootType.Gold, true);
            loot.Add(newItem);
        }
    }

    public override void Update()
    {
        base.Update();

        if (isDestroyed)
        {
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
                this.gameObject.SetActive(false);
            }
        }
    }
}
