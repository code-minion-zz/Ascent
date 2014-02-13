using UnityEngine;
using System.Collections;

public class ExperienceBuff : StatusEffect 
{
    protected float value;
    public float ExperienceGainBonus
    {
        get { return value; }
    }
}
