using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIAgent : MonoBehaviour 
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

	public void Initialise(Transform t)
	{
		steeringAgent.Initialise(t.GetComponent<CharacterMotor>());
		mindAgent.Initialise(t);
	}

    public List<Character> GetSensedCharacters()
    {
        return mindAgent.SensedCharacters;
    }

	private Character targetCharacter;
	public Character TargetCharacter
	{
		get { return targetCharacter; }
		set 
		{
			targetCharacter = value;
			steeringAgent.TargetCharacter = value;
		}
	}

	public void OnEnable()
	{
		steeringAgent.SetActive(true);
		mindAgent.SetActive(true);
	}

	public void OnDisable()
	{
		steeringAgent.SetActive(false);
		mindAgent.SetActive(false);
	}

#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        steeringAgent.DebugDraw();
        mindAgent.DebugDraw();
    }
#endif
}
