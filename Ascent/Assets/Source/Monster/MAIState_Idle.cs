using UnityEngine;
using System.Collections;

public class MAIState_Idle : MonsterAIState
{
    override public void Update()
    {

    }

    override public bool EvaluateEndCondition()
    {
        return true;
    }
}
