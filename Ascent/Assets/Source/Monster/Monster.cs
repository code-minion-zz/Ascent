﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Monster : MonoBehaviour 
{
    List<MonsterAIState> listAIStates = new List<MonsterAIState>();

    /// <summary>
    /// Enum of monster AI movement states, multiple can be active at once
    /// </summary>	
    
    enum EMonsterAiFlag
    {
        MA_INVALID_STATE = 0x00,
        MA_STATE_WANDER = 0x01,	// Face and move random direction
        MA_STATE_SEEK = 0x02,	// Run towards
        MA_STATE_ESCAPE = 0x04,	// Run away
        MA_STATE_NOROTATE = 0x08,	// Halt rotation
        MA_STATE_NOMOVE = 0x16, // Halt movement
        MA_MAX_STATE = 0x80
    };

    protected char monsterState
    {
        get;
        set;
    }	

	// Use this for initialization
	void Start () 
    {

	}
	
	// Update is called once per frame
	void Update () 
    {

	}

}