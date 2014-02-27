using UnityEngine;
using System.Collections;

public class Fireball :  Projectile
{
    public Projectile projectile;

    private Character owner;
    private Vector3 velocity;

    public void Initialise(Vector3 startPos, Vector3 velocity, Character owner)
    {
        this.owner = owner;
        this.velocity = velocity;
    }

    public void Update()
    {
        projectile.transform.position += velocity;
    }
}
