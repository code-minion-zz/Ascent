using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Arc : Shape2D
{
	public float radius;
	public float arcAngle;

    public Vector3 Line1
    {
        get { return MathUtility.RotateAboutPoint(transform.forward * radius, transform.position, -arcAngle * 0.5f); }
    }

    public Vector3 Line2
    {
        get { return MathUtility.RotateAboutPoint(transform.forward * radius, transform.position, arcAngle * 0.5f); }
    }

	public Transform transform;

	public Arc()
	{
		type = Shape2D.EType.Arc;
	}

#if UNITY_EDITOR
	public void DebugDraw()
	{

        Debug.DrawLine(transform.position, transform.position + Line1); // To the rotated arc
		Debug.DrawLine(transform.position, transform.position + Line2); // To the rotated arc

        Handles.DrawWireArc(transform.position, Vector3.up, Line1, arcAngle, radius);
	}
#endif
}
