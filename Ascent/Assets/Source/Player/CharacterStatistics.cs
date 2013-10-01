using UnityEngine;
using System.Collections;

public class CharacterStatistics : MonoBehaviour 
{
    private HealthStat health;

    public HealthStat Health
    {
        get { return health; }
    }

    public void Init()
    {
        health = gameObject.AddComponent<HealthStat>();
    }
}
