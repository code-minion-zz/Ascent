using UnityEngine;
using System.Collections;

public static class EnemyStatLoader 
{
	public static EnemyStats Load(Enemy.EEnemy enemyType, Enemy enemy)
	{
		// TODO: Load all stats from file. Instead of this massive switch case!

		EnemyStats stats = new EnemyStats();
        stats.enemy = enemy;

		switch(enemyType)
		{
			case Enemy.EEnemy.Rat:
				{
					stats.Level = 1;
					stats.experienceBounty = 50;
					stats.goldBounty = 0;


					stats.PrimaryStatsGrowthRates = new PrimaryStatsGrowthRates()
					{
						minPower = 25,
						maxPower = 80,
						minFinesse = 5,
						maxFinesse = 34,
						minVitality = 1,
						maxVitality = 39,
						minSpirit = 5,
						maxSpirit = 34
					};

					stats.PrimaryStats = new PrimaryStats()
					{
						power = stats.PrimaryStatsGrowthRates.minPower,
						finesse = stats.PrimaryStatsGrowthRates.minFinesse,
						vitality = stats.PrimaryStatsGrowthRates.minVitality,
						spirit = stats.PrimaryStatsGrowthRates.minSpirit
					};

					stats.SecondaryStats = new SecondaryStats()
					{
						health = 20.0f,
						special = 15.0f,
						attack = 2.0f,
						physicalDefense = 1.0f,
						magicalDefense = 1.0f,
						criticalHitChance = 5.0f,
						criticalHitMultiplier = 25.0f,
						dodgeChance = 2.5f,
					};

					stats.SecondaryStatsGrowthRates = new SecondaryStatsGrowthRates()
					{
						healthPerVit = 5.0f,
						specialPerSpirit = 1.0f,
						attackPerPow = 1.0f,
						physicalDefPerVit = 0.5f,
						magicalDefPerSpr = 1.0f,
						critPerFin = 0.15f,
						critMultPerFin = 0.5f,
						dodgePerFin = 0.15f,
					};
				}
				break;
			case Enemy.EEnemy.Abomination:
				{
					stats.Level = 2;
					stats.experienceBounty = 1000;
					stats.goldBounty = 0;

					stats.PrimaryStatsGrowthRates = new PrimaryStatsGrowthRates()
					{
						minPower = 20,
						maxPower = 68,
						minFinesse = 5,
						maxFinesse = 34,
						minVitality = 3,
						maxVitality = 50,
						minSpirit = 5,
						maxSpirit = 34
					};

					stats.PrimaryStats = new PrimaryStats()
					{
						power = stats.PrimaryStatsGrowthRates.minPower,
						finesse = stats.PrimaryStatsGrowthRates.minFinesse,
						vitality = stats.PrimaryStatsGrowthRates.minVitality,
						spirit = stats.PrimaryStatsGrowthRates.minSpirit
					};

					stats.SecondaryStats = new SecondaryStats()
					{
						health = 40.0f,
						special = 15.0f,
						attack = 10.0f,
						physicalDefense = 1.0f,
						magicalDefense = 1.0f,
						criticalHitChance = 5.0f,
						criticalHitMultiplier = 25.0f,
						dodgeChance = 2.5f,
					};

					stats.SecondaryStatsGrowthRates = new SecondaryStatsGrowthRates()
					{
						healthPerVit = 10.0f,
						specialPerSpirit = 1.0f,
						attackPerPow = 1.0f,
						physicalDefPerVit = 0.5f,
						magicalDefPerSpr = 1.0f,
						critPerFin = 0.15f,
						critMultPerFin = 0.5f,
						dodgePerFin = 0.15f,
					};
				}
				break;
			default:
				{
					Debug.LogError("Unhandled case.");
				}
				break;
		}

		stats.Reset();
		return stats;
		
	}
}
