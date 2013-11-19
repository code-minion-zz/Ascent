using UnityEngine;
using System.Collections;

public class TowerRecordKeeper
{
    int floorsCompletedInSuccession;
    float experienceBonusMultipler;
    float goldBonusMultiplier;
    float lootQualityChanceMultiplier;
    float lootQuantityChanceMultiplier;

    public float ExperienceBonusMultipler
    {
        get { return experienceBonusMultipler; }
        set { experienceBonusMultipler = value; }
    }

    public float GoldBonusMultiplier
    {
        get { return goldBonusMultiplier; }
        set { goldBonusMultiplier = value; }
    }

    public float LootQualityChanceMultiplier
    {
        get { return lootQualityChanceMultiplier; }
        set { lootQualityChanceMultiplier = value; }
    }

    public float LootQuantityChanceMultiplier
    {
        get { return lootQuantityChanceMultiplier; }
        set { lootQuantityChanceMultiplier = value; }
    }

    public void OnFloorCompleted()
    {
        floorsCompletedInSuccession += 1;
    }

    public void Initialise()
    {
        OnTowerRunStart();
    }

    public void OnTowerRunStart()
    {
        floorsCompletedInSuccession = 0;

        experienceBonusMultipler = 0.0f;
        goldBonusMultiplier = 0.0f;
        lootQualityChanceMultiplier = 0.0f;
        lootQuantityChanceMultiplier = 0.0f;
    }
}
