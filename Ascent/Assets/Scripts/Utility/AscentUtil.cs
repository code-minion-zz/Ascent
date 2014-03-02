using UnityEngine;

public class Utilities
{

	/// <summary>
	/// Vectors to angle in degrees. Note that the returned angle is oriented 90 degrees 
	/// to the 'left'. Add 90 to the return value to orient it 'up'.
	/// 0 = Right, 90 = Up, 180 = Left, -90 = Down
	/// </summary>
	/// <returns>The to angle in degrees.</returns>
	public static float VectorToAngleInDegrees(float x, float y)
	{
		Vector2 vec = new Vector2(x,y);
		vec.Normalize();
		float retval = (Mathf.Atan2(vec.y,vec.x))*(Mathf.Rad2Deg);
		return retval;
	}

	public static bool CloseTo(float pointerAngle, float buttonAngle, float tolerance)
	{
		bool returnValue = false;

		float diff = Mathf.Abs(AdjustAngle(pointerAngle) - AdjustAngle(buttonAngle)) % 360;
		
		if (buttonAngle == 0)
		{
			if (pointerAngle < 0)
			{
				diff = Mathf.Abs(pointerAngle);
			}
		}

		if (diff < tolerance)
		{
			returnValue = true;
		}


		return returnValue;
	}

	public static float AdjustAngle(float f)
	{
		float retVal = 0f;
		
//		if (f > 90)
//		{
//			f -= 270;
//		}
		if (f < 0)
		{
			f += 360f;
		}

		retVal = f;

		return retVal;
	}

//	public static bool TresholdAngle(float f)
//	{
//
//	}

}