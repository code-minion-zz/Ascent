using UnityEngine;
using System.Collections;

public class SmoothFollow : MonoBehaviour
{
	public Transform target;
	public float smoothTime = 0.3f;
	
	private Vector3 velocity;
	
	void Start()
	{

	}
	
	void Update()
	{	
		float newX = Mathf.SmoothDamp(transform.position.x,
		target.position.x, ref velocity.x, smoothTime);
			
			
		float newZ = Mathf.SmoothDamp(transform.position.z,
		target.position.z, ref velocity.z, smoothTime);
		
		transform.position = new Vector3(newX, transform.position.y, newZ); 
	}
}