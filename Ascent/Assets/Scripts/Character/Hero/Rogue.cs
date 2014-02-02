using UnityEngine;
using System.Collections;

public class Rogue : Hero 
{
    // public is called once per frame
    public override void Update()
    {

    }

    public override void Initialise(InputDevice input, HeroSaveData saveData)
    {
		heroClass = EHeroClass.Rogue;

    }
}
