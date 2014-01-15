using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIAgent// : MonoBehaviour 
{
    protected Character actor;

    protected AISteeringAgent steeringAgent = new AISteeringAgent();
    public AISteeringAgent SteeringAgent
    {
        get { return steeringAgent; }
    }
    
    protected AIMindAgent mindAgent = new AIMindAgent();
    public AIMindAgent MindAgent
    {
        get { return mindAgent; }
    }

}
