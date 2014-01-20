using UnityEngine;
using System.Collections;

public class AICondition_Timer : AICondition 
{
    private float timeElapsed;
    private float timeMax;

    private float randOffset;
    private float randMin;
    private float randMax;

    public AICondition_Timer(float time)
    {
        timeMax = time;
        timeElapsed = 0.0f;
    }

    public AICondition_Timer(float time, float randMin, float randMax)
    {
        timeMax = time;

        this.randMin = randMin;
        this.randMax = randMax;

        Reset();
    }

    public override void Reset()
    {
        randOffset = Random.Range(randMin, randMax);
        timeElapsed = 0.0f;
    }

    public override bool HasBeenMet()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed > timeMax + randOffset)
        {
            timeElapsed = timeMax + randOffset;
            return true;
        }

        return false;
    }


	public override string ToString()
	{
        return "Timer: " + System.Math.Round(timeMax + randOffset, 2) + " / " + System.Math.Round(timeElapsed, 2);
	}
}
