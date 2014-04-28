using UnityEngine;
using System.Collections.Generic;

public class EnvironmentBreakable : EnvironmentObj
{
    public bool isDestroyed;
    public bool isBreakable;

    public bool IsDestroyed
    {
        get { return isDestroyed; }
        set { isDestroyed = value; }
    }

    public bool IsBreakable
    {
        get { return isBreakable; }
        set { isBreakable = value; }
    }

    public override void Update()
    {
        base.Update();
    }

    public virtual void BreakObject()
    {
        isDestroyed = true;
    }
}

