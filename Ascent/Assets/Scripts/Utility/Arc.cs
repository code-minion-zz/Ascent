using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Arc : Shape2D
{
	public float radius;
	public float arcAngle;

	private Vector3 offset;
	public Vector3 Offset
	{
		get
		{
			return MathUtility.RotateAboutPoint(offset, Vector3.zero, Mathf.Rad2Deg * (MathUtility.ConvertVectorToHeading(transform.forward)));
		}
		set { offset = value; }
	}

	private Transform transform;
	public Transform Transform
	{
		//get { return transform; }
		set { transform = value; }
	}

	public Vector3 Position
	{
		get 
		{
			return transform.position + Offset;
		}
	}

    public Vector3 Line1
    {
		get { return MathUtility.RotateAboutPoint((transform.forward * radius), Position, -arcAngle * 0.5f); }
    }

    public Vector3 Line2
    {
		get { return MathUtility.RotateAboutPoint((transform.forward * radius), Position, arcAngle * 0.5f); }
    }


	public Arc(Transform transform, float radius, float angle, Vector3 offset)
	{
		this.transform = transform;
		this.radius = radius;
		this.arcAngle = angle;
		this.offset = offset;
		type = Shape2D.EType.Arc;
	}

#if UNITY_EDITOR
	public override void DebugDraw()
	{
		Debug.DrawLine(Position, Position + Line1); // To the rotated arc
		Debug.DrawLine(Position, Position + Line2); // To the rotated arc

		Handles.DrawWireArc(Position, Vector3.up, Line1, arcAngle, radius);
	}
#endif
}
