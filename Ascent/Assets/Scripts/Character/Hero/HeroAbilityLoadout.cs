﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeroAbilityLoadout  : AbilityLoadout
{
    public HeroAbilityLoadout()
    {
        abilities = new Ability[(int)HeroController.EHeroAction.ActionMax];
    }
}