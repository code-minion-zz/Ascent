using UnityEngine;
using System.Collections.Generic;

public class BreakableEnvObject : MonoBehaviour
{
    private bool isDestroyed;
    private Barrel barrel;

    public bool IsDestroyed
    {
        get { return isDestroyed; }
        set { isDestroyed = value; }
    }

    public void Explode()
    {
        barrel = GetComponent<Barrel>();

        if (barrel != null)
        {
            isDestroyed = true;
            barrel.IsDestroyed = true;
            return;
        }
        isDestroyed = true;
        gameObject.SetActive(false);
    }
}

