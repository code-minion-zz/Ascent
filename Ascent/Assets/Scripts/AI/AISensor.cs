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

    public AISensor(Transform transform, EType type, EScope scope)
    {
        this.transform = transform;
        this.type = type;
        this.scope = scope;
    }

    public abstract bool SenseFirstFound(ref List<Character> sensedCharacters);
    public abstract bool SenseClosest(ref List<Character> sensedCharacters);
    public abstract bool SenseAll(ref List<Character> sensedCharacters);

    public bool Sense(Transform transform, ref List<Character> sensedCharacters)
    {
        switch(type)
        {
            case EType.FirstFound: return SenseFirstFound(ref sensedCharacters);
            case EType.Closest: return SenseClosest(ref sensedCharacters);
            case EType.All: return SenseAll(ref sensedCharacters);
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
