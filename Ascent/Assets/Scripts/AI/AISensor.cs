using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class AISensor 
{
    public enum EType
    {
        FirstFound,
        Closest,
        All,
		Target,
    }

    public enum EScope
    {
        Allies,
        Enemies,
        All
    }

    protected EType type;
    protected EScope scope;
    protected Transform transform;
	protected AIAgent agent;

    public AISensor(Transform transform, EType type, EScope scope)
    {
        this.transform = transform;
        this.type = type;
        this.scope = scope;
		
		agent = transform.GetComponentInChildren<AIAgent>();
    }

	public abstract bool SenseCharacter(Character c);

    public bool Sense(Transform transform, ref List<Character> sensedCharacters)
    {
        switch(type)
        {
            case EType.FirstFound: return SenseFirstFound(ref sensedCharacters);
            case EType.Closest: return SenseClosest(ref sensedCharacters);
            case EType.All: return SenseAll(ref sensedCharacters);
			case EType.Target: return SenseTarget(ref sensedCharacters);
			default: Debug.LogError("Unhandled case"); break;
        }
        return false;
    }

	public bool SenseTarget(ref List<Character> sensedCharacters)
	{
		Character c = agent.TargetCharacter; // Add target

		bool foundTarget = c != null;

		if (foundTarget)
		{
			foundTarget = SenseCharacter(c);
			if (foundTarget)
			{
				AddCharacter(ref sensedCharacters, c);
			}
		}

		return foundTarget;
	}

	public bool SenseFirstFound(ref List<Character> sensedCharacters)
	{
		List<Character> characters = new List<Character>();
		GetCharacterList(ref characters);

		// Check all characters to see if there is a collision
		foreach (Character c in characters)
		{
			if (SenseCharacter(c))
			{
				AddCharacter(ref sensedCharacters, c);
				return true;
			}
		}

		return false;
	}

	public bool SenseClosest(ref List<Character> sensedCharacters)
	{
		List<Character> characters = new List<Character>();
		GetCharacterList(ref characters);

		List<Character> foundChars = new List<Character>();

		// Check all characters to see if there is a collision
		foreach (Character c in characters)
		{
			if (SenseCharacter(c))
			{
				AddCharacter(ref foundChars, c);
			}
		}

		sensedCharacters.Add(GetClosestCharacter(transform, ref foundChars));

		if (sensedCharacters.Count > 0)
		{
			return true;
		}

		return false;
	}

	public bool SenseAll(ref List<Character> sensedCharacters)
	{
		List<Character> characters = new List<Character>();
		GetCharacterList(ref characters);

		// Check all characters to see if there is a collision
		foreach (Character c in characters)
		{
			if (SenseCharacter(c))
			{
				AddCharacter(ref sensedCharacters, c);
			}
		}

		if (sensedCharacters.Count > 0)
		{
			return true;
		}

		return false;
	}

    public void AddCharacter(ref List<Character> sensedCharacters, Character c)
    {
        if (!sensedCharacters.Contains(c))
        {
            sensedCharacters.Add(c);
        }
    }

    public void GetCharacterList(ref List<Character> characters)
    {
        if (scope == EScope.Allies || scope == EScope.All)
        {
            // Get list of enemies
            characters = Game.Singleton.Tower.CurrentFloor.CurrentRoom.Enemies;
        }

        if (scope == EScope.Enemies || scope == EScope.All)
        {
            // Add the heroes
            List<Player> players = Game.Singleton.Players;
            foreach (Player p in players)
            {
                characters.Add(p.Hero.GetComponent<Hero>());
            }
        }
    }

    public Character GetClosestCharacter(Transform transform, ref List<Character> characters)
    {
        Character closestCharacter = null;
        float closestDistance = 1000000.0f;

        foreach (Character c in characters)
        {
            float distance = (transform.position - c.transform.position).sqrMagnitude;

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestCharacter = c;
            }
        }

        return closestCharacter;
    }

#if UNITY_EDITOR
    public virtual void OnGizmosDraw()
    {
    }

    public virtual void DebugDraw()
    {
    }
#endif
}
