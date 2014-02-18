using UnityEngine;

public class Utilities
{
	/// <summary>
	/// Vectors to angle in degrees. Note that the returned angle is oriented 90 degrees 
	/// to the 'left'. Add 90 to the return value to orient it 'up'.
	/// </summary>
	/// <returns>The to angle in degrees.</returns>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	public static float VectorToAngleInDegrees(float x, float y)
	{
		Vector2 vec = new Vector2(x,y);
		vec.Normalize();
		float retval = (Mathf.Atan2(vec.y,vec.x))*(Mathf.Rad2Deg);
		return retval;
	}
}