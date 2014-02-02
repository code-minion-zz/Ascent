using UnityEngine;
using System.Collections;

public class EnemyStats : CharacterStats 
{
	public float experienceBounty;
	public float goldBounty;

	public PrimaryStats PrimaryStats
	{
		get { return primaryStats; }
		set { primaryStats = value; }
	}

	public PrimaryStatsGrowthRates PrimaryStatsGrowthRates
	{
		get { return primaryStatsGrowth; }
		set { primaryStatsGrowth = value; }
	}

	public SecondaryStats SecondaryStats
	{
		get { return secondaryStats; }
		set { secondaryStats = value; }
	}


	public SecondaryStatsGrowthRates SecondaryStatsGrowthRates
	{
		get { return secondaryStatsGrowth; }
		set { secondaryStatsGrowth = value; }
	}

	public float ExperienceBounty
	{
		get { return experienceBounty; }
		set { experienceBounty = value; }
	}

	public float GoldBounty
	{
		get { return goldBounty; }
		set { goldBounty = value; }
	}
}
