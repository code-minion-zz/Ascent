using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIMindAgent 
{
    public enum EBehaviour
    {
        Passive,
        Evasive,
        Aggressive,
        Defensive,
    }

    protected Dictionary<EBehaviour, AIBehaviour> behaviours = new Dictionary<EBehaviour, AIBehaviour>();

    public void AddBehaviour(EBehaviour e, AIBehaviour b)
    {
        behaviours[e] = b;
    }

    public AIBehaviour AddBehaviour(EBehaviour e)
    {
        AIBehaviour b = new AIBehaviour();
        behaviours[e] = b;
        return b;
    }

    public void Process()
    {
        foreach(KeyValuePair<EBehaviour, AIBehaviour> b in behaviours)
        {
            b.Value.Process();
        }
    }
}
