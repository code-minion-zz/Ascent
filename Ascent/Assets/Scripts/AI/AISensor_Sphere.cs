using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class AISensor_Sphere : AISensor
{
    public float radius = 5.0f;
	private Vector3 offset;
	public Vector3 Offset
	{
		get 
		{
			return MathUtility.RotateAboutPoint(offset, Vector3.zero, Mathf.Rad2Deg * (MathUtility.ConvertVectorToHeading(transform.forward))); 
		}
		set { offset = value; }
	}

    public AISensor_Sphere(Transform transform, EType type, EScope scope, float radius, Vector3 offset)
                            : base(transform, type, scope)
    {
        this.radius = radius;
		this.offset = offset;
    }

	public override bool SenseCharacter(Character c)
	{
		return (MathUtility.IsWithinCircle(c.transform.position, transform.position, radius));
	}

#if UNITY_EDITOR
	public override void DebugDraw()
    {
        Handles.CircleCap(0, transform.position + Offset, Quaternion.LookRotation(Vector3.down, Vector3.up), radius);
		//Gizmos.DrawWireSphere(transform.position + Offset, radius);
    }
#endif
}
