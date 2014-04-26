using UnityEngine;
using System.Collections;

public class RoomFloorNav : MonoBehaviour 
{
    private Bounds groundBounds;

    public void OnEnable()
    {
        groundBounds = gameObject.collider.bounds;
    }

    // Get a random position in the rect
    public Vector3 GetRandomPosition()
    {
        Vector3 randomPos = new Vector3();

        randomPos.x = Random.Range(transform.position.x - groundBounds.extents.x, transform.position.x + groundBounds.extents.x);
        randomPos.z = Random.Range(transform.position.z - groundBounds.extents.z, transform.position.z + groundBounds.extents.z);

        return randomPos;
    }

    // Get a random position inside rect and circle
    public Vector3 GetRandomPositionWithinRadius(Vector3 start, float radius)
    {
        Vector3 randomPos = new Vector3();

		int randoms = 0;

		for (; ; )
		{
            Vector2 rand = Random.insideUnitCircle * Random.Range(radius * 0.2f, radius * 1.5f);
			randomPos.x = start.x + (rand.x);
			randomPos.z = start.z + (rand.y);

			if (IsWithinBounds(randomPos))
			{
				return randomPos;
			}

			++randoms;

			if(randoms > 15)
			{
				return start;
			}
		}
    }

	public Vector3 GetRandomOrthogonalPositionWithinRadius(Vector3 start, float radius)
	{
		Vector3 randomPos = new Vector3();

		int randoms = 0;

		for (; ; )
		{
            Vector2 rand = Random.insideUnitCircle * Random.Range(radius * 0.2f, radius * 1.5f);

			if(Random.Range(0, 201) < 100)
			{
				randomPos.x = start.x + (rand.x);
				randomPos.z = start.z;
			}
			else
			{
				randomPos.x = start.x;
				randomPos.z = start.z + (rand.y);
			}

			if (IsWithinBounds(randomPos))
			{
				return randomPos;
			}

			++randoms;

			if (randoms > 15)
			{
				return start;
			}

			randomPos = Vector3.zero;
		}
	}

    // Get a random position inside rect and on circumference on a circle
    public Vector3 GetRandomPositionOnCircumference(Vector3 start, float radius)
    {
        Vector3 randomPos = new Vector3();

        float randAngle = Random.Range(0.0f, Mathf.PI * 2.0f);
        randomPos.x = Mathf.Cos(randAngle) * radius;
        randomPos.z = Mathf.Sin(randAngle) * radius;

        return randomPos;
    }

    public Vector3 GetRandomPositionWithinArc(Vector3 startPos, Vector3 facing, float radius, float arcDegrees)
    {
        return Vector3.zero;
    }

	public Vector3 GetRandomPositionOutsideRect(Vector3 startPos, Vector3 size)
	{
		Vector3 pos = Vector3.zero;

		do
		{
			pos = GetRandomPosition();
		}
		while (MathUtility.IsWithinRect(pos, startPos, size));

		return pos;
	}

    public bool IsWithinBounds(Vector3 position)
    {
        return (position.x > transform.position.x - groundBounds.extents.x &&
                position.x < transform.position.x + groundBounds.extents.x &&
                position.z > transform.position.z - groundBounds.extents.z &&
                position.z < transform.position.z + groundBounds.extents.z);
    }
}
