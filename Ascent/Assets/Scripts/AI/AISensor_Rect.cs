using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class AISensor_Rect : AISensor
{
	Rect2D rect;

	public AISensor_Rect(Transform transform, EType type, EScope scope, Vector3 size, Vector3 offset)
		: base(transform, type, scope)
	{
		rect = new Rect2D(transform, size, offset);
	}

	public override bool SenseCharacter(Character c)
	{
		return (MathUtility.IsWithinRect(c.transform.position, transform.position, rect.size));
	}

#if UNITY_EDITOR
	public override void DebugDraw()
	{
		rect.DebugDraw();
	}
#endif
}
