using UnityEngine;
using System.Collections;

public class CharacterStatistics
{
    #region Fields

    private HealthStat health;

    #endregion

    #region Properties

    public HealthStat Health
    {
        get { return health; }
    }

    #endregion

    #region Initialization

    public void Init()
    {
        health = new HealthStat();
    }

    #endregion
}
