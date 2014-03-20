using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class CoinSack : MonoBehaviour
{
    private Rigidbody rigidBody;
    private int goldValue;

    public int GoldValue
    {
        get { return goldValue; }
        set { goldValue = value; }
    }

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
       
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Hero")
        {
            // Detect if the rigid body is sleeping
            // this means if the object has come to rest on the ground then it can be picked up.
            if (rigidBody.IsSleeping())
            {
                gameObject.SetActive(false);
            }
        }
    }
}