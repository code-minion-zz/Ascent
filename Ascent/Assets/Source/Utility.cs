using UnityEngine;
using System.Collections;

namespace Utility 
{
	public class Math
	{
        public static Quaternion SmoothLookAt(Vector3 target, Transform from, float smooth)
        {
            Vector3 dir = target - from.position;
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            return (Quaternion.Slerp(from.rotation, targetRotation, Time.deltaTime * smooth));
        }

		public static void RotateX(ref Vector3 v, float angle)
		{
			float sin = Mathf.Sin(angle);
			float cos = Mathf.Cos(angle);

			float ty = v.y;
			float tz = v.z;

			v.y = (cos * ty) - (sin * tz);
			v.z = (cos * tz) + (sin * ty);

		}

		public static void RotateY(ref Vector3 v, float angle)
		{
			float sin = Mathf.Sin(angle);
			float cos = Mathf.Cos(angle);

			float tx = v.x;
			float tz = v.z;

			v.x = (cos * tx) + (sin * tz);
			v.z = (cos * tz) - (sin * tx);
		}

		public static void RotateZ(ref Vector3 v, float angle)
		{
			float sin = Mathf.Sin(angle);
			float cos = Mathf.Cos(angle);

			float tx = v.x;
			float ty = v.y;

			v.x = (cos * tx) - (sin * ty);
			v.y = (cos * ty) + (sin * tx);
		}
	}
}


