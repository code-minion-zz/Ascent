using UnityEngine;
using System.Collections;

public class CharacterTilt : MonoBehaviour 
{
	Transform parentTrans;

	public Vector3 forwardRotation = new Vector3(12.5f, 0.0f, 0.0f);
	public Vector3 rightRotation = new Vector3(0.0f, 0.0f, 12.5f);

	void Start()
	{
		parentTrans = transform.parent.transform;
	}

	public void Process ()
	{
		parentTrans = transform.parent.transform;
		if (parentTrans.forward == Vector3.right)
		{
			transform.localRotation = Quaternion.Euler(rightRotation);
		}
		else if (parentTrans.forward == Vector3.left)
		{
			transform.localRotation = Quaternion.Euler(-rightRotation);
		}
		else if (parentTrans.forward == Vector3.forward)
		{
			transform.localRotation = Quaternion.Euler(forwardRotation);
		}
		else if (parentTrans.forward == Vector3.back)
		{
			transform.localRotation = Quaternion.Euler(-forwardRotation);
		}
	}
}
