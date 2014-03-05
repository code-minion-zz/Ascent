using UnityEngine;
using System.Collections;

public class FallingDebris : Projectile
{
    private Character owner;
    private RayDown rayDown;

    public void Initialise(Vector3 startPosition, Character owner)
    {
        startPosition.y = 15.0f;
        transform.position = startPosition;
        this.owner = owner;
        rayDown = GetComponentInChildren<RayDown>();
    }

    public void Update()
    {
        rigidbody.AddForce(-Vector3.up * 5.0f, ForceMode.Force);
    }

    public void OnCollisionEnter(Collision collision)
    {
        // TODO: DAMAGE IN AREA

        // Check what it has collided with
        Layer layer = (Layer)collision.gameObject.layer;

        switch (layer)
        {
            case Layer.Environment:
            case Layer.Floor:
                {
                    // Blow up
                    Debug.Log("Hit Environment/Floor");
                }
                break;
            case Layer.Hero:
            case Layer.Monster:
                {
                    // Blow up and deal damage to this unit
                    Debug.Log("Hit Character");
                }
                break;
            default:
                {
                    Debug.LogError("Unhandled case, " + layer.ToString() + ", " + collision.collider.gameObject.name);
                }
                break;
        }


        GameObject explosion = GameObject.Instantiate(Resources.Load("Prefabs/Projectiles/DebrisExplosion")) as GameObject;
        explosion.GetComponent<DebrisExplosion>().Initialise(transform.position, owner);

        GameObject.Destroy(this.gameObject);
    }
}
