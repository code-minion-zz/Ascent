using UnityEngine;
using System.Collections.Generic;

public class BreakableEnvObject : MonoBehaviour
{
    private bool isDestroyed;

    public bool IsDestroyed
    {
        get { return isDestroyed; }
        set { isDestroyed = value; }
    }

    public void Explode()
    {
        isDestroyed = true;
        gameObject.SetActive(false);
    }
}

