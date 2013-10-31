using UnityEngine;
using System.Collections;

public abstract class MonsterAIState
{
    #region Member Variables

    protected bool isConstant;
    protected float duration;

    #endregion


    #region Properties

    public bool IsContant
    {
        get;
        set;
    }

    public float Duration
    {
        get;
        set;
    }

    #endregion

    abstract public void Update();
    abstract public bool EvaluateEndCondition();

    //protected delegate void OnEndConditionMetEvent(GameObject _gameObject);
    //protected event OnEndConditionMetEvent OnEndConditionsMet;
}