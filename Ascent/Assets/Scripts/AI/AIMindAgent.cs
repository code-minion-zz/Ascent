using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#pragma warning disable 0162 // hides unreachable code warning

public class AIMindAgent 
{
    public enum EBehaviour
    {
        Passive,
        Evasive,
        Aggressive,
        Defensive,
    }

	private const bool drawLabels = false;

	protected bool active;

    protected Dictionary<EBehaviour, AIBehaviour> behaviours = new Dictionary<EBehaviour, AIBehaviour>();

	protected Transform transform;
	public Transform Transform
	{
		get { return transform; }
		set { transform = value; }
	}

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

#if UNITY_EDITOR
#pragma warning disable 0414 // unused var 
	private GUIText label;
#endif

	public void Initialise(Transform t)
	{
		active = true;
		transform = t;

#if UNITY_EDITOR
		if (drawLabels)
		{
			if (label == null)
			{
				GameObject go = GameObject.Instantiate(Resources.Load("Prefabs/AIDebugText")) as GameObject;
				label = go.GetComponent<GUIText>();
				go.SetActive(false);
				go.transform.parent = transform;
			}
		}
#endif
	}

	public void SetActive(bool active)
	{
		this.active = active;
	}


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
		#if UNITY_EDITOR
        if (!behaviours.ContainsKey(curBehaviour))
        {
            Debug.LogError("Trying to process a behaviour that has not been initialised.");
        }
        else
        {
            if (drawLabels)
            {
                label.gameObject.SetActive(false);
            }
        }
		#endif

		behaviours[curBehaviour].Process();
    }

#if UNITY_EDITOR
    public void DebugDraw()
    {
		if (behaviours == null)
		{
			return;
		}

		if (!behaviours.ContainsKey(curBehaviour))
		{
			return;
		}
		
		Floor floor = Game.Singleton.Tower.CurrentFloor;
		if (floor != null)
		{
			if (drawLabels)
			{
                if (label == null)
                {
                    return;
                }

				label.gameObject.SetActive(true);

				Camera camera = Game.Singleton.Tower.CurrentFloor.MainCamera;

				Vector3 pos = transform.position;
				pos.x += 0.5f;
				pos = camera.WorldToViewportPoint(pos);

				label.transform.position = pos;


				label.text = "";
				label.text = curBehaviour.ToString() + "\n";
			}

			int i = 0;
			List<AITrigger> triggers = behaviours[curBehaviour].Triggers;
			foreach (AITrigger t in triggers)
			{
				if (drawLabels)
				{
					label.text += "	Trigger" + i + "\n";
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
    }
#endif
}