using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class AISensor_Sphere : AISensor
{
    public float radius = 5.0f;

    public AISensor_Sphere(Transform transform, EType type, EScope scope, float radius)
                            : base(transform, type, scope)
    {
        this.radius = radius;
    }

    public override bool SenseFirstFound(ref List<Character> sensedCharacters)
    {
        List<Character> characters = new List<Character>();
        GetCharacterList(ref characters);

        // Check all characters to see if there is a collision
        foreach (Character c in characters)
        {
            if (MathUtility.IsWithinCircle(c.transform.position, transform.position, radius))
            {
                AddCharacter(ref sensedCharacters, c);
                return true;
            }
        }

        return false;
    }

    public override bool SenseClosest(ref List<Character> sensedCharacters)
    {
        List<Character> characters = new List<Character>();
        GetCharacterList(ref characters);

        List<Character> foundChars = new List<Character>();

        // Check all characters to see if there is a collision
        foreach (Character c in characters)
        {
            if (MathUtility.IsWithinCircle(c.transform.position, transform.position, radius))
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

    public override bool SenseAll(ref List<Character> sensedCharacters)
    {
        List<Character> characters = new List<Character>();
        GetCharacterList(ref characters);

        // Check all characters to see if there is a collision
        foreach (Character c in characters)
        {
            if (MathUtility.IsWithinCircle(c.transform.position, transform.position, radius))
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


#if UNITY_EDITOR
    public override void DebugDraw()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
#endif
}
