using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TriggerRegion : MonoBehaviour
{
	public List<Vector2> regions = new List<Vector2>();

	public bool IsHit
	{
		get;
		set;
	}

	public bool IsInside(Vector3 pointToTest)
	{
		foreach (Vector2 v in regions)
		{
			IsHit = MathUtility.IsWithinBounds(pointToTest, this.transform.position, new Vector3(v.x, 1.0f, v.y));
			
			if(IsHit)
				return IsHit;
		}

		return IsHit;
	}

#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		Vector3 pos = transform.position;
		
		if(IsHit)
		{
			Gizmos.color = Color.red;
		}

		foreach (Vector2 v in regions)
		{
			Gizmos.DrawWireCube(new Vector3(pos.x, 0.1f, pos.z), new Vector3(v.x, 0.1f, v.y));
		}
	}
#endif
}
