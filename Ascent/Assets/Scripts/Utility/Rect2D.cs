using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Don't Use. Currently broken but probably not even needed. Use circles.
/// </summary>
public class Rect2D : Shape2D
{
	public Vector3 size;
	public Vector3 offset;

	public Transform transform;

	Bounds bounds;

	public Rect2D(Transform transform, Vector3 size, Vector3 offset)
	{
		this.transform = transform;
		this.size = size;
		this.offset = offset;
		type = Shape2D.EType.Rect;
	}

#if UNITY_EDITOR
	public void DebugDraw()
	{
		//Vector3 pos = transform.position;
		//pos.y = 0.1f;
		//Gizmos.DrawWireCube(pos + MathUtility.RotateAboutPoint(offset, transform.position, Mathf.Rad2Deg * (MathUtility.ConvertVectorToHeading(transform.forward))), size);
		//Debug.DrawLine(transform.position, transform.position + transform.forward* 2.0f);
		//Debug.DrawLine(transform.position, transform.position + transform.right * 2.0f);
		
		//Vector3 pos = transform.position;
		//Vector3 center = bounds.center;
		//Vector3 extents = bounds.size;
		//Vector3 bl = pos + center - extents;
		//Vector3 tl = pos + center + new Vector3(-extents.x, 0.0f, extents.z);
		//Vector3 tr = pos + center + extents;
		//Vector3 br = pos + center + new Vector3(extents.x, 0.0f, -extents.z);

		//Debug.DrawLine(bl, tl);
		//Debug.DrawLine(tl, tr);
		//Debug.DrawLine(tr, br);
		//Debug.DrawLine(br, bl);
	}
#endif
}
