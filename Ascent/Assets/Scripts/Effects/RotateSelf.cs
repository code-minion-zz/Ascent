using UnityEngine;
using System.Collections;

public class RotateSelf : MonoBehaviour 
{
    public float rotationSpeedX = 90.0f;
    public float rotationSpeedY = 0.0f;
    public float rotationSpeedZ = 0.0f;
    public bool local = true;

    void Update()
    {
        if (local)
        {
            transform.Rotate(new Vector3(rotationSpeedX, rotationSpeedY, rotationSpeedZ) * Time.deltaTime);
            return;
        }

        transform.Rotate(new Vector3(rotationSpeedX, rotationSpeedY, rotationSpeedZ) * Time.deltaTime, Space.World);
    }
}
