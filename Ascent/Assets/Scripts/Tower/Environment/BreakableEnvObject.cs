using UnityEngine;
using System.Collections.Generic;

public class BreakableEnvObject : Environment
{
    protected bool isDestroyed;

    public bool IsDestroyed
    {
        get { return isDestroyed; }
        set { isDestroyed = value; }
    }

    public virtual void Update()
    {

    }

    public virtual void BreakObject()
    {
        isDestroyed = true;
    }
}

