﻿using UnityEngine;
using System.Collections;

public class AICondition_Attacked : AICondition
{
    bool wasHit;

    public AICondition_Attacked(Character actor)
    {
        actor.OnDamageTaken += OnHit;
    }

    public override bool HasBeenMet()
    {
        bool conditionMet = wasHit;
        wasHit = false;

        return conditionMet;
    }

    public void OnHit(float f)
    {
        wasHit = true;
    }
}
