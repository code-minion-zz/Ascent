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
        Game.Singleton.LoadLevel(Game.EGameState.Tower);
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
