using UnityEngine;
using System.Collections;

public class BloodSplatter
{
    private GameObject bloodSplat;

    public void LoadResources()
    {
        bloodSplat = Resources.Load("Prefabs/Effects/BloodSplat") as GameObject;
    }

    public void CreateBloodSplatter(Vector3 position, Quaternion rotation, Transform parent, float length)
    {
        GameObject bloodSplatter = GameObject.Instantiate(bloodSplat, position, rotation) as GameObject;
        bloodSplatter.transform.parent = parent;

        GameObject.Destroy(bloodSplatter, length);
    }
}
