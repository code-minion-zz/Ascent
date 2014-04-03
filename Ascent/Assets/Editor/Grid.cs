using UnityEngine;
using System.Collections.Generic;

namespace Ascent
{
    public class Grid : MonoBehaviour
    {
        public float tileWidth = 1.0f;
        public float tileLength = 1.0f;
        public float gridWidth = 18.0f;
        public float gridLength = 32.0f;

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
            //float count = 0.0f;
            //// Draw the grid
            //for (float x = gridPosition.x - gridWidth - width; x < gridPosition.x + gridWidth; x += width)
            //{
            //    Vector3 startHorrizontal = new Vector3(gridPosition.x - gridWidth, 0.0f, (gridPosition.z - gridWidth) + count * width);
            //    Vector3 endHorrizontal = new Vector3(gridPosition.x + gridWidth, 0.0f, (gridPosition.z - gridWidth) + count * width);

            //    Gizmos.DrawLine(startHorrizontal, endHorrizontal);
            //    count++;
            //}
            //Gizmos.DrawLine(gridPosition, Vector3.up * 10.0f);

            //count = 0.0f;
            //for (float z = gridPosition.z - gridLength - length; z < gridPosition.z + gridLength; z += length)
            //{
            //    Gizmos.DrawLine(new Vector3((gridPosition.x - gridLength) + count * length, 0.0f, gridPosition.z - gridLength),
            //                    new Vector3((gridPosition.x - gridLength) + count * length, 0.0f, gridPosition.z + gridLength));
            //    count++;
            //}

            // Draw the grid

            // Left to right
            for (float x = 0.0f; x < gridWidth+1; ++x)
            {
                Vector3 startHorrizontal = new Vector3(gridPosition.x - gridLength / 2.0f, 0.1f, (gridPosition.z - gridWidth / 2.0f) + x * tileWidth);
				Vector3 endHorrizontal = new Vector3(gridPosition.x + gridLength / 2.0f, 0.1f, (gridPosition.z - gridWidth / 2.0f) + x * tileWidth);

                Gizmos.DrawLine(startHorrizontal, endHorrizontal);
            }

            // Up to down
            for (float y = 0.0f; y < gridLength+1; ++y)
            {
				Vector3 startVert = new Vector3((gridPosition.x - gridLength / 2.0f) + y * tileLength, 0.1f, gridPosition.z - (gridWidth / 2.0f));
				Vector3 endVert = new Vector3((gridPosition.x - gridLength / 2.0f) + y * tileLength, 0.1f, gridPosition.z + (gridWidth / 2.0f));

                Gizmos.DrawLine(startVert, endVert);
            }
        }
    }
}