using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class AI_Sensor  : MonoBehaviour
{
    public virtual void Update()
    {
        // To be overridden if needed.
    }

    public abstract bool SenseAll(ref List<Character> sensedCharacters);
    public abstract bool SenseAllies(ref List<Character> sensedCharacters);
    public abstract bool SenseEnemies(ref List<Character> sensedCharacters);
}
