using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public bool Shaking;
    private float ShakeDecay;
    private float ShakeIntensity;
    private Vector3 OriginalPos;
    //private Vector3 defaultPos;
    private Quaternion OriginalRot;
    private Quaternion defaultRot;

    void Start()
    {
        Shaking = false;
        //defaultPos = transform.position;
        defaultRot = transform.rotation;
    }


    // Update is called once per frame
    void Update()
    {
        if (ShakeIntensity > 0)
        {
            transform.position = OriginalPos + Random.insideUnitSphere * ShakeIntensity;
            transform.rotation = new Quaternion(OriginalRot.x + Random.Range(-ShakeIntensity, ShakeIntensity) * .2f,
                                      OriginalRot.y + Random.Range(-ShakeIntensity, ShakeIntensity) * .2f,
                                      OriginalRot.z + Random.Range(-ShakeIntensity, ShakeIntensity) * .2f,
                                      OriginalRot.w + Random.Range(-ShakeIntensity, ShakeIntensity) * .2f);

            ShakeIntensity -= ShakeDecay;
        }
        else if (Shaking)
        {
            Shaking = false;
        }
    }


    //void OnGUI()
    //{

    //    if (GUI.Button(new Rect(10, 200, 50, 30), "Shake"))
    //    {
    //        DoShake();
    //    }
    //}

    public void DoShake(float intensity, float decay)
    {
        if (!Shaking)
        {
            OriginalPos = transform.position;
            OriginalRot = defaultRot;

            ShakeIntensity = intensity;//0.05f;
            ShakeDecay = decay;//0.02f;
            Shaking = true;
        }
    }


}