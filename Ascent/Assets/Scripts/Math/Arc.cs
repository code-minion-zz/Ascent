using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Arc : Shape2D
{
	public float radius;
	public float arcAngle;

	public Vector3 arcLine;
	public Vector3 arcLine2;

	public Transform transform;

	public Arc()
	{
		type = Shape2D.EType.Arc;
	}

	public void Process()
	{
		arcLine = MathUtility.RotateAboutPoint(transform.forward * radius, transform.position, -arcAngle * 0.5f);
		arcLine2 = MathUtility.RotateAboutPoint(transform.forward * radius, transform.position, arcAngle * 0.5f);
	}

	public void DebugDraw()
	{
		Debug.DrawLine(transform.position, transform.position + arcLine); // To the rotated arc
		Debug.DrawLine(transform.position, transform.position + arcLine2); // To the rotated arc

#if UNITY_EDITOR
		Handles.DrawWireArc(transform.position, Vector3.up, arcLine, arcAngle, radius);
#endif
	}
}
