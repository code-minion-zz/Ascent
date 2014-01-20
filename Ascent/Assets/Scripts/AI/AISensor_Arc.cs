using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class AISensor_Arc : AISensor
{
	private Arc arc;

	public AISensor_Arc(Transform transform, EType type, EScope scope, float radius, float angle, Vector3 offset)
		: base(transform, type, scope)
	{
		arc = new Arc(transform, radius, angle, offset);
	}


	public override bool SenseCharacter(Character c)
	{
		return (MathUtility.IsWithinCircleArc(c.transform.position, transform.position, arc.Line1, arc.Line2, arc.radius));
	}


#if UNITY_EDITOR
	public override void DebugDraw()
	{
		arc.DebugDraw();
	}
#endif
}
