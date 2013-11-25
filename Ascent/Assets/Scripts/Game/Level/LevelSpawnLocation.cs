using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelSpawnLocation : MonoBehaviour
{


    void Awake()
    {

    }

    public void GoToNextLevel()
    {
        Game.Singleton.LoadLevel("Level2");
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        if (other.tag == "Hero")
        {
            GoToNextLevel();
        }
    }
}
