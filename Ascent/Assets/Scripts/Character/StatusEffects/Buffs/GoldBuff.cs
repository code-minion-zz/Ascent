using UnityEngine;
using System.Collections;

public class GoldBuff : Buff 
{
    protected float value;
    public float GoldGainBonus
    {
        get { return value; }
    }
}
