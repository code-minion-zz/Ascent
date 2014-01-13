using UnityEngine;
using System.Collections;

//public static class HeroFactory  
//{
//    static Hero CreateWarrior()
//    {
//        GameObject go = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Warrior"));

//        return go.GetComponent<Warrior>();
//    }

//    static Hero CreateRogue()
//    {
//        GameObject go = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Rogue"));

//        return go.GetComponent<Rogue>();
//    }

//    static Hero CreateMage()
//    {
//        GameObject go = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Mage"));

//        return go.GetComponent<Mage>();
//    }
//}

public static class HeroFactory
{
    public static Hero CreateNewHero(Character.EHeroClass type)
    {
        GameObject go = null;

        switch (type)
        {
            case Character.EHeroClass.Warrior:
                go = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Warrior"));
                go.AddComponent<Warrior>();
                break;

            case Character.EHeroClass.Rogue:
                go = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Rogue"));
                go.AddComponent<Rogue>();
                break;

            case Character.EHeroClass.Mage:
                go = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Mage"));
                go.AddComponent<Mage>();
                break;
        }

        return go.GetComponent<Hero>();
    }
}
