using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool
{
    public class PoolObject
    {
        public Component script;
        public GameObject go;
    }

    private PoolObject[] pool;

    public PoolObject[] Pool
    {
        get { return pool; }
    }

    public ObjectPool(GameObject objectToPool, int poolSize, Transform parent, string scriptName)
    {
        // Create container for the pool
        pool = new PoolObject[poolSize];

        // Populate the pool
        for (int i = 0; i < pool.Length; ++i )
        {
            pool[i] = new PoolObject();
            pool[i].go = GameObject.Instantiate(objectToPool) as GameObject;
            pool[i].go.SetActive(false);
            pool[i].go.transform.parent = parent;
            pool[i].script = pool[i].go.GetComponent(scriptName);
        }
    }

    public PoolObject GetInactive()
    {
        for (int i = 0; i < pool.Length; ++i)
        {
            if (!pool[i].go.activeSelf)
            {
                return pool[i];
            }
        }
        return null;
    }

    public void ReturnToPool(ref GameObject objectToReturn)
    {
        objectToReturn.SetActive(false);
    }
}
