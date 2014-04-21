using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#pragma warning disable 0162 // hides unreachable code warning

public class AIMindAgent : MonoBehaviour
{
    public enum EBehaviour
    {
        Passive,
        Evasive,
        Aggressive,
        Defensive,
    }

	private const bool drawLabels = true;

    protected Dictionary<EBehaviour, AIBehaviour> behaviours = new Dictionary<EBehaviour, AIBehaviour>();

	protected EBehaviour curBehaviour;
	public EBehaviour CurrentBehaviour
	{
		get { return curBehaviour; }
	}

    protected List<Character> sensedCharacters;
    public List<Character> SensedCharacters
    {
        get 
        {
            if (sensedCharacters == null)
            {
                sensedCharacters = new List<Character>();
            }
            return sensedCharacters; 
        }
        set { sensedCharacters = value; }
    }

	private Character targetCharacter;
	public Character TargetCharacter
	{
		get { return targetCharacter; }
		set { targetCharacter = value; }
	}

#if UNITY_EDITOR
#pragma warning disable 0414 // unused var 
	private GUIText label;

	private bool renderedGizmos;
#endif

    public void ResetBehaviour(EBehaviour e)
    {
        if (behaviours.ContainsKey(e))
        {
            behaviours[e].Reset();
        }
    }

    public void AddBehaviour(EBehaviour e, AIBehaviour b)
    {
        behaviours[e] = b;
    }

    public AIBehaviour AddBehaviour(EBehaviour e)
    {
		AIBehaviour b = new AIBehaviour(AIMindAgent.EBehaviour.Defensive);
        behaviours[e] = b;
        return b;
    }

	public void SetBehaviour(EBehaviour e)
	{
		curBehaviour = e;
	}

    public void Process()
    {
		if (!behaviours.ContainsKey(curBehaviour))
		{
			Debug.LogError("Trying to process a behaviour that has not been initialised: " + curBehaviour);
		}
		else
		{
			behaviours[curBehaviour].Process();
		}

#if UNITY_EDITOR
		if (drawLabels && label != null)
		{
			label.gameObject.SetActive(false);
		}
#endif
    }

#if UNITY_EDITOR
	public void OnDrawGizmos()
	{
		if (!drawLabels ||
			behaviours == null ||
			!behaviours.ContainsKey(curBehaviour) ||
			Game.Singleton.Tower.CurrentFloor == null)
		{
			return;
		}

        if (label == null)
        {
			GameObject go = GameObject.Instantiate(Resources.Load("Prefabs/AIDebugText")) as GameObject;
			label = go.GetComponent<GUIText>();
			go.transform.parent = transform;
        }

		label.gameObject.SetActive(true);

		Camera camera = Game.Singleton.Tower.CurrentFloor.MainCamera;

		Vector3 pos = transform.position;
		pos.x += 0.5f;
		pos = camera.WorldToViewportPoint(pos);

		label.transform.position = pos;


		label.text = "";
		label.text = curBehaviour.ToString() + "\n";

		int i = 0;
		List<AITrigger> triggers = behaviours[curBehaviour].Triggers;
		foreach (AITrigger t in triggers)
		{
			if (drawLabels)
			{
				if (t.name != null)
				{
					label.text += " " + t.name + "\n";
				}
				else
				{	
					label.text += "	Trigger" + i + "\n";
				}
			}

			List<KeyValuePair<AICondition, AITrigger.EConditional>> conditions = t.Conditions;

			int j = 0;
			foreach (KeyValuePair<AICondition, AITrigger.EConditional> c in conditions)
			{
				if (drawLabels)
				{
					if (j == 0)
					{
						label.text += "		" + c.Key.ToString() + "\n";
					}
					else
					{
						label.text += "		" + c.Value + " " + c.Key.ToString() + "\n";
					}
				}

				c.Key.DebugDraw();

				++j;
			}
			++i;
		}
    }
#endif
}