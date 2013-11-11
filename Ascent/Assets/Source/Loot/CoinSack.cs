using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class CoinSack : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
    {
        string tag = other.collider.tag;


        if (tag == "Hero")
        {
            gameObject.SetActive(false);
        }
    }

    void OnCollisionStay(Collision other)
    {
    }
}