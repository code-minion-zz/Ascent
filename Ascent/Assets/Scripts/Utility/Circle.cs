using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Circle : Shape2D
{
	public float radius;

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
		set { transform = value; }
	}

	public Vector3 Position
	{
		get
		{
			return transform.position + Offset;
		}
	}

	public Circle(Transform transform, float radius, Vector3 offset)
	{
		this.transform = transform;
		this.radius = radius;
		this.offset = offset;
		type = Shape2D.EType.Circle;
	}

#if UNITY_EDITOR
	public override void DebugDraw()
	{
        Handles.CircleCap(0, transform.position + Offset, Quaternion.LookRotation(Vector3.down, Vector3.up), radius);
		//Gizmos.DrawWireSphere(Position, radius);
	}
#endif
}
