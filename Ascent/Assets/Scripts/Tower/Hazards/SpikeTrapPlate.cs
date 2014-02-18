using UnityEngine;
using System.Collections;

public class SpikeTrapPlate : MonoBehaviour 
{
    private bool activated = false;

    public bool IsStepped()
    {
        if (activated)
        {
            activated = false;
            return true;
        }
        
        return false;
    }

    void OnCollisionEnter(Collision collision)
    {
        activated = true;
    }
}