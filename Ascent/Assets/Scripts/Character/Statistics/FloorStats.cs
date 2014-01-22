using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Statistics recorded for every player during a floor instance.
/// </summary>
public class FloorStats
{
    #region Fields

    private int experience;
    private int roomsVisited;
    private int timeTaken;
    private int bossKillTime;
    private int numberOfTrapsTripped;
    private int numberOfChestsOpened;
    private int totalCoinsPickedUp;
    private int livesLost;
    private int monstersKilled;
    private int damageDealt;
    private int damageTaken;
    private int itemsUsed;
    private int itemsPickedUp;

    #endregion

    #region Properties

    public int ExperienceGained
    {
        get { return experience; }
        set { experience = value; }
    }

    public int NumberOfRoomsVisited
    {
        get { return roomsVisited; }
        set { roomsVisited = value; }
    }

    public int FloorCompletionTime
    {
        get { return timeTaken; }
        set { timeTaken = value; }
    }

    public int BossCompletionTime
    {
        get { return bossKillTime; }
        set { bossKillTime = value; }
    }

    public int NumberOfTrapsTripped
    {
        get { return numberOfTrapsTripped; }
        set { numberOfTrapsTripped = value; }
    }

    public int NumberOfChestsOpened
    {
        get { return numberOfChestsOpened; }
        set { numberOfChestsOpened = value; }
    }

    public int TotalCoinsLooted
    {
        get { return totalCoinsPickedUp; }
        set { totalCoinsPickedUp = value; }
    }

    public int NumberOfDeaths
    {
        get { return livesLost; }
        set { livesLost = value; }
    }

    public int NumberOfMonstersKilled
    {
        get { return monstersKilled; }
        set { monstersKilled = value; }
    }

    public int TotalDamageDealt
    {
        get { return damageDealt; }
        set { damageDealt = value; }
    }

    public int DamageTaken
    {
        get { return damageTaken; }
        set { damageTaken = value; }
    }

    public int NumberOfItemsUsed
    {
        get { return itemsUsed; }
        set { itemsUsed = value; }
    }

    public int NumberOfItemsPickedUp
    {
        get { return itemsPickedUp; }
        set { itemsPickedUp = value; }
    }

    #endregion

    public FloorStats()
    {
        ResetStatistics();
    }

    public void ResetStatistics()
    {
        experience = 0;
        roomsVisited = 0;
        timeTaken = 0;
        bossKillTime = 0;
        numberOfTrapsTripped = 0;
        numberOfChestsOpened = 0;
        totalCoinsPickedUp = 0;
        livesLost = 0;
        monstersKilled = 0;
        damageDealt = 0;
        damageTaken = 0;
        itemsUsed = 0;
        itemsPickedUp = 0;
    }
}