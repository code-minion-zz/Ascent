using UnityEngine;
using System.Collections;

public static class HeroFactory
{
    public static Hero CreateNewHero(Character.EHeroClass type)
    {
		GameObject go = null;
		Hero hero = null;

		switch (type)
		{
			case Character.EHeroClass.Warrior:
				{
					go = GameObject.Instantiate(Resources.Load("Prefabs/Heroes/WarriorAvatar")) as GameObject;
					hero = go.AddComponent<Warrior>();
				}
				break;
			case Character.EHeroClass.Rogue:
				{
					go = GameObject.Instantiate(Resources.Load("Prefabs/Heroes/RogueAvatar")) as GameObject;
					hero = go.AddComponent<Rogue>();
				}
				break;
			case Character.EHeroClass.Mage:
				{
					go = GameObject.Instantiate(Resources.Load("Prefabs/Heroes/MageAvatar")) as GameObject;
					hero = go.AddComponent<Mage>();
				}
				break;
			default:
				{
					Debug.LogError("Tried to make character of invalid type.");
				}
				break;
		}

		return hero;
    }
}
