using UnityEngine;

public class Utilities
{
	public static float ANGLETOLERANCE = 20f;

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

	public static bool CloseTo(float a, float b)
	{
		return (Mathf.Abs(a - b) <= ANGLETOLERANCE);
	}
}