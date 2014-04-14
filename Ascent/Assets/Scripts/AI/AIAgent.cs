using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIAgent : MonoBehaviour 
{
    protected Character actor;
    public AIMindAgent mindAgent;
    public AIMindAgent MindAgent
    {
        get { return mindAgent; }
    }

    public AISteeringAgent steeringAgent;
    public AISteeringAgent SteeringAgent
    {
        get { return steeringAgent; }
    }

    public List<Character> SensedCharacters
    {
        get { return mindAgent.SensedCharacters; }
    }

	private Character targetCharacter;
    public Character TargetCharacter
    {
        get { return targetCharacter; }
        set
        {
            targetCharacter = value;
        }
    }

#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        if (steeringAgent)
        {
            steeringAgent.DebugDraw();
        }

        if (mindAgent)
        {
            mindAgent.DebugDraw();
        }
    }
#endif
}
