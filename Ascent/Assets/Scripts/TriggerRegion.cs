using UnityEngine;
using System.Collections;

public class TriggerRegion : MonoBehaviour
{
	public Vector2 size;

	public bool IsHit
	{
		get;
		set;
	}

	public bool IsInside(Vector3 pointToTest)
	{
		IsHit = MathRectHelper.IsWithinBounds(pointToTest, this.transform.position, new Vector3(size.x, 1.0f, size.y));
		return IsHit;
	}

	void OnDrawGizmos()
	{
		Vector3 pos = transform.position;
		
		if(IsHit)
		{
			Gizmos.color = Color.red;
		}

		Gizmos.DrawWireCube(new Vector3(pos.x, 0.1f, pos.z), new Vector3(size.x, 0.1f, size.y));
		
	}
}
