using UnityEngine;
using System.Collections;

public class CharacterTilt : MonoBehaviour 
{
	Transform parentTrans;

    private bool applyTilt = true;
    public float tiltAmount = 12.5f;

	private int lastFrame = 0;

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

        if (applyTilt)
        {
			Vector3 forwardRotation = new Vector3(tiltAmount, 0.0f, 0.0f);
			Vector3 rightRotation = new Vector3(0.0f, 0.0f, tiltAmount);

            int curFrame = Time.frameCount;

            if (lastFrame != curFrame)
            {
				Vector3 combinedRot = Vector3.zero;

				if (parentTrans.forward.x > 1.0f)
				{
					combinedRot.z = parentTrans.forward.x / rightRotation.z;
				}
				else if (Mathf.Approximately(parentTrans.forward.x, -1.0f))
				{
					combinedRot.z = -(parentTrans.forward.x / rightRotation.z);
				}

				if (parentTrans.forward.z > 1.0f)
				{
					combinedRot.x = parentTrans.forward.z / forwardRotation.x;
				}
				else if (Mathf.Approximately(parentTrans.forward.x, -1.0f))
				{
					combinedRot.x = -(parentTrans.forward.z / forwardRotation.x);
				}

				transform.localRotation = Quaternion.Euler(combinedRot);

                lastFrame = curFrame;
            }
        }
	}
}
