using UnityEngine;
using System.Collections;

public class FallingDebris : Projectile
{
    // Spawns at Y
    // Falls on Y
    // When it hits ground it does damage in small area.

    public void Start()
    {

    }

    public void Update()
    {
        //rigidbody.AddForce(-Vector3.up, ForceMode.Force);
    }

    public void OnCollisionEnter(Collision collision)
    {
        GameObject.Destroy(this.gameObject);
    }
}
