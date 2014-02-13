using UnityEngine;
using System.Collections;

public class ExperienceBuff : Buff 
{
    protected float value;
    public float ExperienceGainBonus
    {
        get { return value; }
    }
}
