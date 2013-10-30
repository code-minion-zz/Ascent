using UnityEngine;
using System.Collections;

public class SpinningBlade : MonoBehaviour 
{
    public enum EBladeDirection
    {
        left = -1,
        right = 1
    }

    public GameObject blade;
    public int bladeCount;
    public EBladeDirection bladeDirection;
    public float rotationSpeed;
    public float bladeDamage;

    private GameObject[] blades;

	void Start () 
    {
        blades = new GameObject[bladeCount];

        transform.FindChild("Blades");

        for (int i = 0; i < bladeCount; ++i)
        {
            blades[i] = GameObject.Instantiate(blade) as GameObject;
            blades[i].transform.parent = this.transform;

            float angle = 45.0f;
            blades[i].transform.Rotate(Vector3.up, angle);

            Vector3 offset = new Vector3(0.0f, 1.0f, 3.5f);
            //Utility.RotateY(ref offset, -angle);

            Debug.Log(offset);

          //  blades[i].transform.rotation = quat;
                //blades[i].transform.rotation = Quaternion.AngleAxis(45.0f, Vector3.up);
           // Vector3 direction = blades[i].transform.rotation.eulerAngles.normalized;

            blades[i].transform.position = transform.position + offset;
        }
	}
	
	void Update () 
    {
        //gameObject.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f) * rotationSpeed * (float)bladeDirection);
	}
}
