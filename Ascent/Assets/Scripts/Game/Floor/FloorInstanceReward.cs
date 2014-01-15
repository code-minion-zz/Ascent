using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Maintains floor instance stats and rewards.
/// </summary>
public class FloorInstanceReward
{
    private Floor floorInstance;

    public FloorInstanceReward(Floor floor)
    {
        floorInstance = floor;
    }

    /// <summary>
    /// Calculates rewards and bonuses for the floor instance and applys them.
    /// </summary>
    public void ApplyFloorInstanceRewards()
    {
        // Evaluate the hero's who did the best.
        Hero mostMonstersKilled = CalcMostMonstersKilled();
        Hero mostDamageDealt = CalcMostDamageDealt();
        Hero leastDamageTaken = CalcLeastDamageTaken();
        Hero leastDeaths = CalcDiedTheLeast();

        // For each hero we want to calculate the rewards and penalties for.
        foreach (Player player in floorInstance.Players)
        {
            Hero hero = player.Hero.GetComponent<Hero>();

            // Work out percentages
            float goldBonusPercentage = 0.0f;
            float expBonusPercentage = 0.0f;

            goldBonusPercentage += ((float)hero.FloorStatistics.NumberOfRoomsVisited * 0.25f);
            goldBonusPercentage += ((float)hero.FloorStatistics.NumberOfChestsOpened * 0.15f);
            expBonusPercentage += ((float)hero.FloorStatistics.NumberOfChestsOpened * 0.15f);
            expBonusPercentage += ((float)hero.FloorStatistics.NumberOfItemsUsed * 1.0f);
            expBonusPercentage += ((float)hero.FloorStatistics.NumberOfItemsPickedUp * 0.15f);

            // Penalties
            goldBonusPercentage -= ((float)hero.FloorStatistics.NumberOfTrapsTripped * 0.15f);
            expBonusPercentage -= ((float)hero.FloorStatistics.NumberOfTrapsTripped * 0.15f);

            if (hero.FloorStatistics.FloorCompletionTime < 10)
                goldBonusPercentage += 15.0f;
            else if (hero.FloorStatistics.FloorCompletionTime < 20)
                goldBonusPercentage += 10.0f;
            else if (hero.FloorStatistics.FloorCompletionTime < 30)
                goldBonusPercentage += 5.0f;

            if (hero.FloorStatistics.BossCompletionTime < 5)
                goldBonusPercentage += 5.0f;

            if (hero == mostMonstersKilled)
                expBonusPercentage += 2.5f;
            if (hero == mostDamageDealt)
                expBonusPercentage += 2.5f;
            if (hero == leastDamageTaken)
                expBonusPercentage += 2.5f;
            // TODO: Calc least death player rewards.

            if (hero.FloorStatistics.NumberOfDeaths == 0)
                expBonusPercentage += 10.0f;

            //Debug.Log("Bonus EXP: " + expBonusPercentage);
            //Debug.Log("Current EXP: " + hero.CharacterStats.CurrentExperience);

            // Apply rewards
            int goldBonus = (int)((float)hero.CharacterStats.Currency * goldBonusPercentage / 100.0f);
            int expBonus = (int)((float)hero.CharacterStats.CurrentExperience * expBonusPercentage / 100.0f);

            hero.CharacterStats.Currency += goldBonus;
            hero.CharacterStats.CurrentExperience += expBonus;

            Debug.Log("Hero: " + hero + " Rewards: Gold +" + goldBonus + ", EXP +" + expBonus);
            Debug.Log("Total monsters killed: " + hero.FloorStatistics.NumberOfMonstersKilled);
            Debug.Log("Total damage dealt: " + hero.FloorStatistics.TotalDamageDealt);
            Debug.Log("Total damage received: " + hero.FloorStatistics.DamageTaken);
        }
    }

    /// <summary>
    /// Evaluates the hero who killed the most monsters on the current floor instance.
    /// </summary>
    /// <returns>The hero</returns>
    public Hero CalcMostMonstersKilled()
    {
        Hero mostMonstersHero = null;
        int mostMonstersKilled = 0;

        foreach (Player player in floorInstance.Players)
        {
            Hero hero = player.Hero.GetComponent<Hero>();

            if (mostMonstersHero == null)
            {
                mostMonstersKilled = hero.FloorStatistics.NumberOfMonstersKilled;
                mostMonstersHero = hero;
            }

            if (hero.FloorStatistics.NumberOfMonstersKilled > mostMonstersKilled)
            {
                mostMonstersKilled = hero.FloorStatistics.NumberOfMonstersKilled;
                mostMonstersHero = hero;
            }
        }

        return mostMonstersHero;
    }

    /// <summary>
    /// Evaluates the hero who did the most damage on the current floor instance.
    /// </summary>
    /// <returns>The hero</returns>
    public Hero CalcMostDamageDealt()
    {
        Hero mostDamageHero = null;
        int mostDamageDealt = 0;

        foreach (Player player in floorInstance.Players)
        {
            Hero hero = player.Hero.GetComponent<Hero>();

            if (mostDamageHero == null)
            {
                mostDamageDealt = hero.FloorStatistics.TotalDamageDealt;
                mostDamageHero = hero;
            }

            // Find the most damage dealt and the player.
            if (hero.FloorStatistics.TotalDamageDealt > mostDamageDealt)
            {
                mostDamageDealt = hero.FloorStatistics.TotalDamageDealt;
                mostDamageHero = hero;
            }
        }

        return mostDamageHero;
    }

    /// <summary>
    /// Evaluates the hero who took the least damage on the current floor instance.
    /// </summary>
    /// <returns>The hero</returns>
    public Hero CalcLeastDamageTaken()
    {
        Hero leastDamageTaken = null;
        int leastDamage = 0;

        foreach (Player player in floorInstance.Players)
        {
            Hero hero = player.Hero.GetComponent<Hero>();

            if (leastDamageTaken == null)
            {
                leastDamage = hero.FloorStatistics.DamageTaken;
                leastDamageTaken = hero;
            }

            if (hero.FloorStatistics.DamageTaken < leastDamage)
            {
                leastDamage = hero.FloorStatistics.DamageTaken;
                leastDamageTaken = hero;
            }
        }

        return leastDamageTaken;
    }

    /// <summary>
    /// Evaluates the hero who died the least on the current floor instance.
    /// </summary>
    /// <returns>The hero</returns>
    public Hero CalcDiedTheLeast()
    {
        Hero diedTheLeast = null;
        int lives = 0;

        foreach (Player player in floorInstance.Players)
        {
            Hero hero = player.Hero.GetComponent<Hero>();

            if (diedTheLeast == null)
            {
                lives = hero.FloorStatistics.NumberOfDeaths;
                diedTheLeast = hero;
            }

            if (hero.FloorStatistics.NumberOfDeaths < lives)
            {
                lives = hero.FloorStatistics.NumberOfDeaths;
                diedTheLeast = hero;
            }
        }

        return diedTheLeast;
    }
}
