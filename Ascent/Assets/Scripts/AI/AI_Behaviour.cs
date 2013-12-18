using UnityEngine;
using System.Collections;

public class AI_Behaviour 
{
    public enum State
    {
        AI_Idle = 0,
        AI_Wander,
    }

    public virtual void Process()
    {
    }

}
