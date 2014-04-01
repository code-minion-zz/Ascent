using UnityEngine;
using System.Collections;

public class GoldBuff : StatusEffect 
{
    protected float value;
    public float GoldGainBonus
    {
        get { return value; }
    }
}
