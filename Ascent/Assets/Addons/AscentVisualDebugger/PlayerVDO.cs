﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerVDO : GenericVDO<Player>, IVisulDebugObject
{
    new public void Start()
    {
        base.Start();
    }

    new public void Update()
    {
        base.Update();
    }

    override protected void SetText()
    {
        //foreach (GameObject go in DebugObjects)
        //{
        //    Player player = go.GetComponent<Player>();
        //    TextMesh textMesh = go.transform.FindChild("FloatingText(Clone)").GetComponent<TextMesh>();
        //    textMesh.text = go.name + " \n " + player.CharacterStats.Health.Min;

        //    textMesh.transform.position = new Vector3(textMesh.transform.position.x, 5.0f, textMesh.transform.position.z); 
        //}
    }
}