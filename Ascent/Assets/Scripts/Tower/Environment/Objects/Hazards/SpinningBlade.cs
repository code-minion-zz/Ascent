using UnityEngine;
using System.Collections;

public class SpinningBlade : MonoBehaviour 
{
	struct TBlade
	{
		public GameObject go;
		public Blade script;
	}

    public GameObject blade;
    public int bladeCount;
    public float rotationSpeed;
    public float initialRotationOffset;
    public int bladeDamage;
	public float bladeLength;

	private TBlade[] blades;
	private int previousBladeCount;
	private float previousBladeLength;

	private bool blocked;

	void Start () 
    {
		Initialise();
	}

	void Initialise()
	{
		if (bladeCount < 1)
		{
			bladeCount = 1;
		}
		previousBladeCount = bladeCount;
		previousBladeLength = bladeLength;

		blades = new TBlade[bladeCount];

		Transform bladesParent = transform.FindChild("Blades");

		for (int i = 0; i < bladeCount; ++i)
		{
			GameObject newBlade = GameObject.Instantiate(blade) as GameObject;
			blades[i].go = newBlade;
			blades[i].script = newBlade.GetComponent<Blade>();
			blades[i].script.Initialise(bladeDamage);

            newBlade.transform.parent = bladesParent;

			newBlade.transform.localScale = new Vector3(newBlade.transform.localScale.x + bladeLength, newBlade.transform.localScale.y, newBlade.transform.localScale.z);

			Vector3 offset = new Vector3(0.0f, 1.0f, 1.0f + bladeLength * 0.5f);
			newBlade.transform.position += offset + new Vector3(transform.position.x, 0.0f, transform.position.z);

			float angle = (360.0f / bladeCount);
			transform.Rotate(Vector3.up, angle);
		}

        transform.rotation = Quaternion.identity;
        transform.Rotate(Vector3.up, initialRotationOffset);
	}

	void Shutdown()
	{
		for (int i = 0; i < previousBladeCount; ++i)
		{
			Object.Destroy(blades[i].go);
		}
	}
	
	void Update () 
    {
		if (!blocked)
		{
			gameObject.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f) * rotationSpeed * Time.deltaTime * 25.0f);
		}
		else
		{
			gameObject.transform.Rotate(new Vector3(0.0f, Mathf.PingPong(Time.time * rotationSpeed, -0.05f), 0.0f) * rotationSpeed * Time.deltaTime * 25.0f);
		}

		// TODO: Remove this for optimisation
	   if (previousBladeCount != bladeCount || previousBladeLength != bladeLength)
		{
			Shutdown();
			Initialise();
		}
	}

	public void HaltRotation()
	{
		blocked = true;
	}

	public void ResumeRotation()
	{
		blocked = false;
	}
}
