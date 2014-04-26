using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public bool Shaking;
    private float ShakeDecay;
    private float ShakeIntensity;

    private Vector3 OriginalPos;

    private Quaternion OriginalRot;
    private Quaternion defaultRot;
	private Quaternion targetRot;

	private float totalRecoverTime = 0.25f;
	private float recoverTimeElapsed;

    void Start()
    {
        Shaking = false;
		OriginalRot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (ShakeIntensity > 0)
        {
            transform.position = OriginalPos + Random.insideUnitSphere * ShakeIntensity;
			targetRot = new Quaternion(OriginalRot.x + Random.Range(-ShakeIntensity, ShakeIntensity) * .2f,
									  OriginalRot.y + Random.Range(-ShakeIntensity, ShakeIntensity) * .2f,
									  OriginalRot.z + Random.Range(-ShakeIntensity, ShakeIntensity) * .2f,
									  OriginalRot.w + Random.Range(-ShakeIntensity, ShakeIntensity) * .2f);

			transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 2.0f);

            ShakeIntensity -= ShakeDecay;
        }
        else if (Shaking)
        {
            Shaking = false;
        }

		if (!Shaking)
		{
			if (recoverTimeElapsed < totalRecoverTime)
			{
				recoverTimeElapsed += Time.deltaTime;
				if (recoverTimeElapsed > totalRecoverTime)
				{
					recoverTimeElapsed = totalRecoverTime;
				}

				transform.rotation = Quaternion.Slerp(transform.rotation, OriginalRot, recoverTimeElapsed / totalRecoverTime);
			}
		}
    }

    public void DoShake(float intensity, float decay)
    {
        if (!Shaking)
        {
            OriginalPos = transform.position;

            ShakeIntensity = intensity;//0.05f;
            ShakeDecay = decay;//0.02f;
            Shaking = true;
			recoverTimeElapsed = 0.0f;
        }
    }


}