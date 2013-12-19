using UnityEngine;
using System.Collections;

public static class MathRectHelper 
{
	// Checks X and Z
	public static bool IsWithinBounds(Vector3 point, Vector3 rectPosition, Vector3 rectSize)
	{
		return (point.x > rectPosition.x - rectSize.x * 0.5f &&
				point.x < rectPosition.x + rectSize.x * 0.5f &&
				point.z > rectPosition.z - rectSize.z * 0.5f &&
				point.z < rectPosition.z + rectSize.z * 0.5f);
	}
}
