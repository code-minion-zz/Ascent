﻿using UnityEngine;
using System.Collections;

public class Mage : Hero 
{
	public override void Initialise(InputDevice input, HeroSaveData saveData)
    {
		classType = EHeroClass.Mage;

    }
}
