using UnityEngine;
using System.Collections.Generic;

namespace Ascent
{
    public class Grid : MonoBehaviour
    {
        public float width = 1.0f;
        public float length = 1.0f;

        public float gridWidth = 10.0f;
        public float gridLength = 10.0f;

        public Color color = Color.green;

        public Vector3 gridPosition = Vector3.zero;

        public bool showGrid = true;

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
            gridPosition = transform.position;

            if (showGrid)
            {
                DrawGrid();
            }
        }

        private void DrawGrid()
        {
            Gizmos.color = color;
            float count = 0.0f;
            // Draw the grid
            for (float x = gridPosition.x - gridWidth - width; x < gridPosition.x + gridWidth; x += width)
            {
                Vector3 startHorrizontal = new Vector3(gridPosition.x - gridWidth, 0.0f, (gridPosition.z - gridWidth) + count * width);
                Vector3 endHorrizontal = new Vector3(gridPosition.x + gridWidth, 0.0f, (gridPosition.z - gridWidth) + count * width);

                Gizmos.DrawLine(startHorrizontal, endHorrizontal);
                count++;
            }

            count = 0.0f;
            for (float z = gridPosition.z - gridLength - length; z < gridPosition.z + gridLength; z += length)
            {
                Gizmos.DrawLine(new Vector3((gridPosition.x - gridLength) + count * length, 0.0f, gridPosition.z - gridLength),
                                new Vector3((gridPosition.x - gridLength) + count * length, 0.0f, gridPosition.z + gridLength));
                count++;
            }
        }
    }
}