using UnityEngine;
using System.Collections;

public static class HeroFactory  
{
    static Warrior CreateWarrior()
    {
        GameObject go = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Warrior"));

        return go.GetComponent<Warrior>();
    }

    static Rogue CreateRogue()
    {
        GameObject go = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Rogue"));

        return go.GetComponent<Rogue>();
    }

    static Mage CreateMage()
    {
        GameObject go = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Mage"));

        return go.GetComponent<Mage>();
    }
}
