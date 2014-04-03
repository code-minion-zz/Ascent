using UnityEngine;
using System.Collections;


public class EffectFactory : MonoBehaviour
{
    private BloodSplatter bloodSplatter = new BloodSplatter();

    void Awake()
    {
        bloodSplatter.LoadResources();
    }

    public void CreateBloodSplatter(Vector3 position, Quaternion rotation, Transform parent)
    {
        bloodSplatter.CreateBloodSplatter(position, rotation, parent, 3.0f);
    }
}
