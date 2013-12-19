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

	public static bool IsWithinCircle(Vector3 point, Vector3 circlePos, float radius)
	{
		float squareDist = (Mathf.Pow((circlePos.x - point.x), 2.0f)) + (Mathf.Pow((circlePos.z - point.z),2.0f));
		
		return squareDist <= Mathf.Pow(radius, 2.0f);
	}

	public static bool IsWithinCircleArc(Vector3 point, Vector3 circlePos, Vector3 sectorStart, Vector3 sectorEnd, float radius)
	{
		Vector3 relPoint = new Vector3();
		relPoint.x = point.x - circlePos.x;
		relPoint.z = point.z - circlePos.z;

		Debug.DrawLine(sectorStart, relPoint, Color.green);
		Debug.DrawLine(sectorEnd, relPoint, Color.green);
		Debug.DrawLine(relPoint, circlePos, Color.green);

		return !AreClockwise(sectorEnd, relPoint) &&
				AreClockwise(sectorStart, relPoint) &&
				IsWithinCircle(point, circlePos, radius);
	}

	private static bool AreClockwise(Vector3 v1, Vector3 v2)
	{
		return -v1.x * v2.z + v1.z * v2.x > 0.0f;
	}

	public static Vector3 RotateAboutPoint(Vector3 pointToRotate, Vector3 centrePoint, float degreeRot)
	{
		float rads = degreeRot * Mathf.Deg2Rad;

		float sin = Mathf.Sin(rads);
		float cos = Mathf.Cos(rads);

		Vector3 newPoint = pointToRotate;

		//newPoint.x =	cos * (pointToRotate.x - centrePoint.x) - 
		//                sin * (pointToRotate.z - centrePoint.z) + centrePoint.x;

		//newPoint.z =	sin * (pointToRotate.x - centrePoint.x) + 
		//                cos * (pointToRotate.z - centrePoint.z) + centrePoint.z;

		newPoint.x = cos * pointToRotate.x + sin * pointToRotate.z;
		newPoint.z = -sin * pointToRotate.x + cos * pointToRotate.z;

		return newPoint;
	}

	public static float ConvertVectorToHeading(Vector3 vector)
	{
		return (Mathf.Atan2(vector.x, vector.y * -1.0f));
	}

	public static Vector3 ConvertHeadingToVector(float heading)
	{
		Vector3 vHeading = new Vector3();

		vHeading.x = (Mathf.Cos(heading));
		vHeading.z = (Mathf.Sin(heading));
		vHeading.Normalize();

		return (vHeading);
	}
}
