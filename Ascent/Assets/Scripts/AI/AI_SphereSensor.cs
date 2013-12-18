using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI_SphereSensor : AI_Sensor 
{
    public float radius;

    public void OnDrawGizmosSelected()
    {
        //Gizmos.DrawWireSphere(transform.position, radius);
        Gizmos.DrawWireCube(transform.position, new Vector3(radius * 2.0f, 1.0f, radius * 2.0f));
    }

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
            if (Physics.CheckSphere(transform.position, radius))
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
            if (Physics.CheckSphere(transform.position, radius))
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
            if ((c.transform.position.x > transform.position.x - radius &&
                c.transform.position.x < transform.position.x + radius &&
                c.transform.position.z > transform.position.z - radius &&
                c.transform.position.z < transform.position.z + radius))
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
