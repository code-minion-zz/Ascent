using UnityEngine;
using System.Collections;

public class AICondition_Timer : AICondition 
{
    private float timeElapsed;
    private float timeMax;

    public AICondition_Timer(float time)
    {
        timeMax = time;
        timeElapsed = 0.0f;
    }

    public override void Reset()
    {
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
