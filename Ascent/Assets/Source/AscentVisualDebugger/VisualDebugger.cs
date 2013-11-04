using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VisualDebugger : MonoBehaviour 
{
    private PlayerVDO players;
    private EnemyVDO monsters;

	void Start () 
    {
        players = gameObject.AddComponent<PlayerVDO>();
        monsters = gameObject.AddComponent<EnemyVDO>();
	}

    void Update () 
    {
        if(Input.GetKeyUp(KeyCode.Alpha9))
        {
            players.Toggle();
        }
        if (Input.GetKeyUp(KeyCode.Alpha0))
        {
            monsters.Toggle();
        }
	}
}
