using UnityEngine;
using System.Collections;

public class WeaponSlot : MonoBehaviour
{
    private GameObject slot;

    public GameObject Slot
    {
        get { return slot; }
    }

    void Awake()
    {
        slot = this.transform.gameObject;
    }
}
