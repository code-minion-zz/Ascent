// Developed by Mana Khamphanpheng 2013

// Dependencies
using UnityEngine;
using System.Collections;

public class Rat : Enemy 
{
    private float deathSequenceTime = 0.0f;
    private float deathSequenceEnd = 1.0f;
    private Vector3 deathRotation = Vector3.zero;
    private float deathSpeed = 5.0f;

	public override void Start()
	{
		Initialise();

        deathRotation = new Vector3(0.0f, 0.0f, transform.eulerAngles.z + 90.0f);
	}

    public override void Update()
    {
        base.Update();

        if (isDead)
        {
            deathSequenceTime += Time.deltaTime;

            // When the rat dies we want to make him kinematic and disabled the collider
            // this is so we can walk over the dead body.
            if (this.transform.rigidbody.isKinematic == false)
            {
                this.transform.rigidbody.isKinematic = true;
                this.transform.collider.enabled = false;
            }

            // Death sequence end
            if (deathSequenceTime >= deathSequenceEnd)
            {
                // When the death sequence has finished we want to make this object not active
                // This ensures that he will dissapear and not be visible in the game but we can still re-use him later.
                deathSequenceTime = 0.0f;
                this.gameObject.SetActive(false);
            }
            else
            {
                // During death sequence we can do some thing in here
                // For now we will rotate the rat on the z axis.
                this.transform.eulerAngles = Vector3.Lerp(this.transform.eulerAngles, deathRotation, Time.deltaTime * deathSpeed);

                // If the rotation is done early we can end the sequence.
                if (this.transform.eulerAngles == deathRotation)
                {
                    deathSequenceTime = deathSequenceEnd;
                }
            }
        }
    }

	public override void Initialise()
	{
		// Populate with stats

        characterStatistics = new CharacterStatistics();
        characterStatistics.MaxHealth = 100;
        characterStatistics.CurrentHealth = 100;

        // Add abilities
        IAction swordSwing = new SwingSword();

        swordSwing.Initialise(this);
        abilities.Add(swordSwing);

        // Add abilities
        IAction charge = new Charge();

        charge.Initialise(this);
        abilities.Add(charge);
	}

    // We want to override the on death for this rat as we have some specific behaviour here.
    public override void OnDeath()
    {
        base.OnDeath();
        // Play some cool animation
        // Maybe even play a sound here
        // Maybe even drop some loot here

        // Rat is going to destroy itself now
        //DestroyObject(this.gameObject);
        //this.gameObject.SetActive(false);
    }
}
