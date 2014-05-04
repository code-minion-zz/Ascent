using UnityEngine;
using System.Collections;

[System.Serializable]
public class AICondition_Timer : AICondition 
{
    [SerializeField]
    private float timeElapsed;
    [SerializeField]
    private float timeMax;
    [SerializeField]
    private float randMin;
    [SerializeField]
    private float randMax;

    public AICondition_Timer(float time)
    {
        timeMax = time;
        timeElapsed = 0.0f;
    }

    public AICondition_Timer(float randMin, float randMax)
    {
        this.randMin = randMin;
        this.randMax = randMax;

        Reset();
    }

    public AICondition_Timer(float randMin, float randMax, bool finish)
        : this(randMin, randMax)
    {
        if (finish)
        {
            timeElapsed = timeMax;
        }
    }

    public override void Reset()
    {
		if (randMin != 0.0f && randMax != 0.0f)
		{
			timeMax = Random.Range(randMin, randMax);
		}
        timeElapsed = 0.0f;
    }

    public override bool HasBeenMet()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed > timeMax)
        {
            timeElapsed = timeMax;
            return true;
        }

        return false;
    }


	public override string ToString()
	{
        return "Timer: " + System.Math.Round(timeMax, 2) + " / " + System.Math.Round(timeElapsed, 2);
	}
}
