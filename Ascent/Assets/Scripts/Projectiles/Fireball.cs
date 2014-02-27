using UnityEngine;
using System.Collections;

public class Fireball :  Projectile
{
    public Projectile projectile;

    private Character owner;
    private Vector3 velocity;
	private Vector3 curVelocity;

    public void Initialise(Vector3 startPos, Vector3 velocity, Character owner)
    {
        this.owner = owner;
        this.velocity = velocity;
		projectile.transform.position = new Vector3(startPos.x, 1.0f, startPos.z);
		projectile.rigidbody.AddForce(velocity, ForceMode.VelocityChange);
    }

    public void Update()
    {
		projectile.rigidbody.AddForce(velocity, ForceMode.Force);
		//projectile.transform.position += velocity * Time.deltaTime;
    }

	public void OnCollisionEnter(Collision collision)
	{
		//// Check what it has collided with
		//Layer layer = (Layer)collision.gameObject.layer;

		//switch (layer)
		//{
		//    case Layer.Environment:
		//        {
		//            // Blow up
		//            Debug.Log("Hit Environment");
		//        }
		//        break;
		//    case Layer.Hero:
		//    case Layer.Monster:
		//        {
		//            // Blow up and deal damage to this unit
		//            Debug.Log("Hit Character");
		//        }
		//        break;
		//    default:
		//        {
		//            Debug.LogError("Unhandled case, " + layer.ToString());
		//        }
		//        break;
		//}

		GameObject explosion = GameObject.Instantiate(Resources.Load("Prefabs/Projectiles/FireballExplosion")) as GameObject;
		explosion.GetComponent<FireballExplosion>().Initialise(transform.position, owner);

		GameObject.Destroy(this.gameObject);
	}
}
