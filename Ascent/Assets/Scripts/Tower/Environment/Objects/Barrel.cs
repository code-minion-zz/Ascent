using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Barrel : EnvironmentBreakable
{
	protected Transform barrelStatic;
	protected Transform barrelDynamic;
	protected List<Transform> barrelParts;
	private float timeDead = 0f;
    private float fadeTime = 3.0f;
    private bool barrelExploded = false;
    private bool isDead = false;

    public void Start()
    {
		Transform model = transform.FindChild("Model");
		barrelStatic = model.FindChild("barrel_static");
		barrelDynamic = model.FindChild("barrel_dynamic");

        if (barrelStatic == null || barrelDynamic == null)
        {
            Debug.LogWarning("Could not find barrel children");
        }

		barrelParts = new List<Transform>();

		for (int j = 0; j < barrelDynamic.childCount; ++j)
		{
			barrelParts.Add(barrelDynamic.GetChild(j));
		}

		barrelDynamic.gameObject.SetActive(false);
    }

    public override void Update()
    {
        base.Update();

        if (barrelStatic == null || barrelDynamic == null)
        {
            return;
        }

        if (isDestroyed && !isDead)
        {
			timeDead += Time.deltaTime;

            if (timeDead >= fadeTime)
            {
                foreach (Transform t in barrelParts)
                {
                    Material mat = t.GetComponent<Renderer>().material;
                    Color color = mat.color;
                    color.a -= 0.1f;
                    mat.color = color;

                    if (color.a <= 0.0f)
                    {
                        isDead = true;
                        gameObject.SetActive(false);
                    }
                }
            }

            if (barrelExploded == false)
            {
                barrelStatic.gameObject.SetActive(false);
                barrelDynamic.gameObject.SetActive(true);
                collider.enabled = false;

                foreach (Transform trans in barrelParts)
                {
                    Vector3 randForce;
                    randForce.x = Random.Range(-2000, 2000);
                    randForce.y = Random.Range(-2000, 20);
                    randForce.z = Random.Range(-2000, 2000);
                    trans.rigidbody.AddTorque(randForce);
                }

                //collider.enabled = false;
                barrelExploded = true;
            }
        }
    }

	public override void BreakObject ()
	{
		SoundManager.PlaySound(AudioClipType.woodHit, transform.position, 2f);

		base.BreakObject ();
	}
}
