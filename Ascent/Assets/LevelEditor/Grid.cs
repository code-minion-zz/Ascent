using UnityEngine;
using System.Collections.Generic;

namespace Ascent
{
    public class Grid : MonoBehaviour
    {
        public float width = 1.0f;
        public float length = 1.0f;

        public float gridWidth = 0.0f;
        public float gridLength = 0.0f;

        public Color color = Color.green;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnDrawGizmos()
        {
            //Vector3 pos = Camera.current.transform.position;
            Vector3 pos = new Vector3(0.0f, 0.0f, 0.0f);
            Gizmos.color = color;

            // Draw the grid
            for (float x = pos.x - gridWidth - width; x < pos.x + gridWidth; x += width)
            {
                Gizmos.DrawLine(new Vector3(-gridWidth, 0.0f, Mathf.Floor(x / width) * width + width),
                                new Vector3(gridWidth, 0.0f, Mathf.Floor(x / width) * width + width));
            }

            for (float z = pos.z - gridLength - length; z < pos.z + gridLength; z += length)
            {
                Gizmos.DrawLine(new Vector3(Mathf.Floor(z / length) * length + length, 0.0f, -gridLength),
                                new Vector3(Mathf.Floor(z / length) * length + length, 0.0f, gridLength));
            }
        }
    }
}