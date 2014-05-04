using UnityEngine;
using System.Collections;

public class FallingDebris : Projectile
{
	public RotateSelfRandomly rotateSelf;
	public Renderer render;
	public FallingDebrisShadow shadow;

	private float startDelay;

    private Character owner;
    private FallingDebrisShadow rayDown;

	private bool exploded;

	private float timeElapsed;

    public void Initialise(Vector3 startPosition, Character owner, float delay)
    {
		startDelay = delay;

        startPosition.y = 15.0f;
        transform.position = startPosition;
        this.owner = owner;
		rayDown = GetComponentInChildren<FallingDebrisShadow>();
    }

    public void Update()
    {
		if (exploded)
		{
			shadow.stop = true;
			rotateSelf.enabled = false;
			rigidbody.velocity = Vector3.zero;

			if (timeElapsed < 1.0f)
			{
				timeElapsed += Time.deltaTime;
				if (timeElapsed > 1.0f)
				{
					timeElapsed = 1.0f;
				}

				Color color = render.material.color;
				color.a = Mathf.Lerp(1.0f, 0.0f, timeElapsed);
				render.material.color = color;

				color = shadow.renderer.material.color;
				color.a = Mathf.Lerp(1.0f, 0.0f, timeElapsed);
				shadow.renderer.material.color = color;

				if (timeElapsed == 1.0f)
				{
					GameObject.Destroy(this.gameObject);
				}
			}

			return;
		}

		if (startDelay > 0.0f)
		{
			startDelay -= Time.deltaTime;
			return;
		}

        rigidbody.AddForce(-Vector3.up * 10.0f, ForceMode.Acceleration);
    }

    public void OnCollisionEnter(Collision collision)
    {
		if (exploded)
			return;

        GameObject explosion = GameObject.Instantiate(Resources.Load("Prefabs/Projectiles/DebrisExplosion")) as GameObject;
        explosion.GetComponent<DebrisExplosion>().Initialise(transform.position, owner);

        SoundManager.PlaySound(AudioClipType.explosion, explosion.transform.position, .5f);

		exploded = true;
    }
}
