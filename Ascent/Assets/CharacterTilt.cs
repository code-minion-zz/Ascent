using UnityEngine;
using System.Collections;

public class CharacterTilt : MonoBehaviour 
{
	Transform parentTrans;

	public Vector3 forwardRotation = new Vector3(12.5f, 0.0f, 0.0f);
	public Vector3 rightRotation = new Vector3(0.0f, 0.0f, 12.5f);

	//int frame = 0;

	void Start()
	{
		parentTrans = transform.parent.transform;
		Process();
	}

	public void LateUpdate()
	{
		Process();
	}

	public void Process()
	{
        //int curFrame = Time.frameCount;

        //if (frame != curFrame)
        //{
        //    parentTrans = transform.parent.transform;

        //    if (Mathf.Approximately(parentTrans.forward.x, 1.0f))
        //    {
        //        transform.localRotation = Quaternion.Euler(rightRotation);
        //    }
        //    else if (Mathf.Approximately(parentTrans.forward.x, -1.0f))
        //    {
        //        transform.localRotation = Quaternion.Euler(-rightRotation);
        //    }
        //    else if (Mathf.Approximately(parentTrans.forward.z, 1.0f))
        //    {
        //        transform.localRotation = Quaternion.Euler(forwardRotation);
        //    }
        //    else if (Mathf.Approximately(parentTrans.forward.z, -1.0f))
        //    {
        //        transform.localRotation = Quaternion.Euler(-forwardRotation);
        //    }

        //    frame = curFrame;
        //}
	}
}
