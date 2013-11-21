using UnityEngine;
using System.Collections;

public static class StatHelper
{
    public static int Power(Character character, HeroEquipment equipment)
    {
        // Get character base statistics
        int power = character.CharacterStats.Power;

        if (equipment != null)
        {
            // Go through the list of items and add power
            
        }

        // Add status buff modifiers

        return power;
    }

    public static int Attack(Character character, HeroEquipment equipment)
    {
        // Get character base statistics
        int power = character.CharacterStats.Power;

        if (equipment != null)
        {
            // Go through the list of items and add power

        }

        // Add status buff modifiers

        return power;
    }
}
