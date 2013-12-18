﻿using UnityEngine;
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
        bool inBounds = false;
        do
        {
            randomPos.x = Random.Range((start.x - radius), (start.x + radius));
            randomPos.z = Random.Range((start.z - radius), (start.z + radius));

            if (IsWithinBounds(randomPos))
            {
                return randomPos;
            }

            if (randoms > 15)
            {
                return start;
            }

            ++randoms;
        } 
        while (inBounds == false);

        return randomPos;
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

    public bool IsWithinBounds(Vector3 position)
    {
        return (position.x > transform.position.x - groundBounds.extents.x &&
                position.x < transform.position.x + groundBounds.extents.x &&
                position.z > transform.position.z - groundBounds.extents.z &&
                position.z < transform.position.z + groundBounds.extents.z);
    }
}
