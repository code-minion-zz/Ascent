using UnityEngine;
using System.Collections;

public class MathUtility
{
    public static Quaternion SmoothLookAt(Vector3 target, Transform from, float smooth)
    {
        Vector3 dir = target - from.position;
        Quaternion targetRotation = Quaternion.LookRotation(dir);
        return (Quaternion.Slerp(from.rotation, targetRotation, Time.deltaTime * smooth));
    }

	public static void RotateX(ref Vector3 v, float angle)
	{
		float sin = Mathf.Sin(angle);
		float cos = Mathf.Cos(angle);

		float ty = v.y;
		float tz = v.z;

		v.y = (cos * ty) - (sin * tz);
		v.z = (cos * tz) + (sin * ty);

	}

	public static void RotateY(ref Vector3 v, float angle)
	{
		float sin = Mathf.Sin(angle);
		float cos = Mathf.Cos(angle);

		float tx = v.x;
		float tz = v.z;

		v.x = (cos * tx) + (sin * tz);
		v.z = (cos * tz) - (sin * tx);
	}

	public static void RotateZ(ref Vector3 v, float angle)
	{
		float sin = Mathf.Sin(angle);
		float cos = Mathf.Cos(angle);

		float tx = v.x;
		float ty = v.y;

		v.x = (cos * tx) - (sin * ty);
		v.y = (cos * ty) + (sin * tx);
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
		return (Mathf.Atan2(vector.x, vector.z));
	}

	public static Vector3 ConvertHeadingToVector(float heading)
	{
		Vector3 vHeading = new Vector3();

		vHeading.x = (Mathf.Cos(heading));
		vHeading.y = (Mathf.Sin(heading));
		vHeading.z = 0f;
		vHeading.Normalize();

		return (vHeading);
	}

	// Checks X and Z
	public static bool IsWithinRect(Vector3 point, Vector3 rectPosition, Vector3 rectSize)
	{
		return (point.x > rectPosition.x - rectSize.x * 0.5f &&
				point.x < rectPosition.x + rectSize.x * 0.5f &&
				point.z > rectPosition.z - rectSize.z * 0.5f &&
				point.z < rectPosition.z + rectSize.z * 0.5f);
	}

    public static bool IsCircleCircle(Vector3 point1, float radius1, Vector3 point2, float radius2)
    {
        return ((Mathf.Pow((point2.x-point1.x), 2.0f) + Mathf.Pow((point1.z-point2.z), 2.0f)) <= Mathf.Pow((radius1+radius2), 2.0f));
    }

	public static bool IsWithinCircle(Vector3 point, Vector3 circlePos, float radius)
	{
		float squareDist = (Mathf.Pow((circlePos.x - point.x), 2.0f)) + (Mathf.Pow((circlePos.z - point.z), 2.0f));
		return squareDist <= Mathf.Pow(radius, 2.0f);
	}

	public static bool IsWithinCircleArc(Vector3 point, Vector3 circlePos, Vector3 sectorStart, Vector3 sectorEnd, float radius)
	{
		Vector3 relPoint = new Vector3();
		relPoint.x = point.x - circlePos.x;
		relPoint.z = point.z - circlePos.z;

		//Debug.DrawLine(sectorStart, relPoint, Color.green);
		//Debug.DrawLine(sectorEnd, relPoint, Color.green);
		//Debug.DrawLine(relPoint, circlePos, Color.green);

        return !AreClockwise(sectorEnd, relPoint) &&
                AreClockwise(sectorStart, relPoint) &&
                IsWithinCircle(point, circlePos, radius);
	}

	private static bool AreClockwise(Vector3 v1, Vector3 v2)
	{
		return -v1.x * v2.z + v1.z * v2.x > 0.0f;
	}
}

