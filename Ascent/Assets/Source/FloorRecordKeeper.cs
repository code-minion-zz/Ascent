using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FloorRecordKeeper 
{
    public class HeroFloorRecord
    {
        public int trapsHit;
        public int goldPickedUp;
        public int chestsOpened;
        public int livesLost;
        public int enemiesDefeated;
        public int damageDealt;
        public int damageTaken;
        public int itemsUsed;
        public int itemsPickedUp;
        public int experienceAccum;
    }

    Dictionary<Hero, HeroFloorRecord> heroRecords;
    public Dictionary<Hero, HeroFloorRecord> HeroRecords
    {
        get { return heroRecords; }
    }

    public class TeamFloorRecord
    {
        public int roomsVisited;
        public float timeElapsed;
        public float timeTakenToDefeatBoss;
    }

    TeamFloorRecord teamRecord;
    public TeamFloorRecord TeamRecord
    {
        get { return teamRecord; }
    }

    public void Initialise(List<Hero> heroes)
    {
        heroRecords = new Dictionary<Hero, HeroFloorRecord>();

        foreach (Hero hero in heroes)
        {
            heroRecords.Add(hero, new HeroFloorRecord());
        }
    }

    #region individual hero record keeping

    public void OnHitByTrap(Hero hero)
    {
        heroRecords[hero].trapsHit += 1;
    }

    public void OnGoldPickUp(Hero hero, int gold)
    {
        heroRecords[hero].goldPickedUp += gold;
    }

    public void OnChestOpened(Hero hero)
    {
        heroRecords[hero].chestsOpened += 1;
    }

    public void OnLifeLost(Hero hero)
    {
        heroRecords[hero].livesLost += 1;
    }

    public void OnEnemyDefeated(Hero hero, Enemy enemy)
    {
        heroRecords[hero].enemiesDefeated += 1;
        heroRecords[hero].experienceAccum += enemy.CharacterStats.ExperienceBounty;
    }

    public void OnDamageDealt(Hero hero, int damage)
    {
        heroRecords[hero].damageDealt += damage;
    }

    public void OnDamageTaken(Hero hero, int damage)
    {
        heroRecords[hero].damageTaken += damage;
    }

    public void OnItemUsed(Hero hero)
    {
        heroRecords[hero].itemsUsed += 1;
    }

    public void OnItemPickedUp(Hero hero)
    {
        heroRecords[hero].itemsPickedUp += 1;
    }

    #endregion

    #region team record keeping

    public void OnRoomEntered()
    {
        teamRecord.roomsVisited += 1;
    }

    public void OnFloorStart()
    {
        teamRecord.timeElapsed = Time.time;
    }

    public void OnFloorEnd()
    {
        teamRecord.timeElapsed = Time.time - teamRecord.timeElapsed;
    }

    public void OnBossFightStart()
    {
        teamRecord.timeTakenToDefeatBoss = Time.time;
    }

    public void OnBossFightEnd()
    {
        teamRecord.timeTakenToDefeatBoss = Time.time - teamRecord.timeElapsed;
    }

    #endregion

}
