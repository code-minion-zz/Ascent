using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Barrel : EnvironmentBreakable
{
	protected Transform barrelStatic;
	protected Transform barrelDynamic;
	protected List<Transform> barrelParts;
	private float timeDead = 0f;
    private float sleepTime = 3.0f;
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

            if (timeDead >= sleepTime)
			{
				Vector3 pos = transform.position;
				pos += Vector3.down * Time.smoothDeltaTime;
				transform.position = pos;

				if (pos.y <= -1f)
				{
					gameObject.SetActive(false);
					isDead = true;
				}
            }

            if (barrelExploded == false)
            {
				barrelDynamic.rotation = barrelStatic.rotation;
				barrelDynamic.position = barrelStatic.position;
                barrelStatic.gameObject.SetActive(false);
                barrelDynamic.gameObject.SetActive(true);

                foreach (Transform trans in barrelParts)
                {
                    Vector3 randForce;
                    randForce.x = Random.Range(-2000, 2000);
                    randForce.y = Random.Range(-2000, 200);
                    randForce.z = Random.Range(-2000, 2000);
                    trans.rigidbody.AddTorque(randForce, ForceMode.Force);
                }

				barrelExploded = true;
				isDestroyed = true;
            }
						
			foreach (Transform t in barrelParts)
			{
				Material mat = t.GetComponent<Renderer>().material;
				Color color = mat.color;
				color.a -= Time.deltaTime;//0.1f;
				mat.color = color;
				
				if (color.a <= 0.0f)
				{
					t.rigidbody.isKinematic = true;
					t.collider.enabled = false;
				}
			}
        }
    }

	public override void BreakObject ()
	{
		SoundManager.PlaySound(AudioClipType.woodHit, transform.position, 0.2f);

		base.BreakObject ();
	}
}
