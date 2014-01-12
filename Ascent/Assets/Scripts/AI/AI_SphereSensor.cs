using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class AI_SphereSensor : AI_Sensor 
{
	public float radius = 5.0f;

	Vector3 arcLine;
	Vector3 arcLine2;

#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
		if(enabled)
		{
			Gizmos.DrawWireSphere(transform.position, radius);
		}
    }
#endif

    public override bool SenseAll(ref List<Character> sensedCharacters)
    {
        // Get list of enemies
        List<Character> characters = Game.Singleton.Tower.CurrentFloor.CurrentRoom.Enemies;

        // Add the heroes
        List<Player> players = Game.Singleton.Players;
        foreach(Player p in players)
        {
            characters.Add(p.Hero.GetComponent<Hero>());
        }

        // Check all characters to see if there is a collision
        foreach (Character c in characters)
        {
			if (MathUtility.IsWithinCircle(c.transform.position, transform.position, radius))
            {
                if (!sensedCharacters.Contains(c))
                {
                    sensedCharacters.Add(c);
                }
            }
        }

        if (sensedCharacters.Count > 0)
        {
            return true;
        }

        return false;
    }

    public override bool SenseAllies(ref List<Character> sensedCharacters)
    {
        // Get list of enemies
        List<Character> characters = Game.Singleton.Tower.CurrentFloor.CurrentRoom.Enemies;

        // Check all characters to see if there is a collision
        foreach (Character c in characters)
        {
			if (MathUtility.IsWithinCircle(c.transform.position, transform.position, radius))
            {
                if (!sensedCharacters.Contains(c))
                {
                    sensedCharacters.Add(c);
                }
            }
        }

        if (sensedCharacters.Count > 0)
        {
            return true;
        }

        return false;
    }

    public override bool SenseEnemies(ref List<Character> sensedCharacters)
    {
        List<Character> characters = new List<Character>();
        
        // Add the heroes
        List<Player> players = Game.Singleton.Players;
        foreach (Player p in players)
        {
            characters.Add(p.Hero.GetComponent<Hero>());
        }
		
        // Check all characters to see if there is a collision
        foreach (Character c in characters)
        {
			if (MathUtility.IsWithinCircle(c.transform.position, transform.position, radius))
            {
                if (!sensedCharacters.Contains(c))
                {
                    sensedCharacters.Add(c);
                }
            }
        }

        if (sensedCharacters.Count > 0)
        {
            return true;
        }

        return false;
    }
}
