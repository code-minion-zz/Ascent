using UnityEngine;
using System.Collections;

public class SM_destroyThisTimedd : MonoBehaviour 
{
    public float destroyTime = 5;

	void Start () 
    {
        Destroy(gameObject, destroyTime);
	}
}
