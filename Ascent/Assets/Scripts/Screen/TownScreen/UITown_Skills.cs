﻿using UnityEngine;
using System.Collections;

public class UITown_Skills : UITown_RadialPanel {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void OnEnable()
	{		
		base.OnEnable();
		
		if (initialised) (parent as UITownWindow).SetTitle("Skill Tree");
	}
}
