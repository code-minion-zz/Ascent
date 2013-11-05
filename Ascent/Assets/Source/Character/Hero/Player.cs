using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

public class Player : MonoBehaviour
{
	#region Fields

	private int playerId = 0;
    private GameObject heroObject;
    private Hero heroScript;
    private InputDevice input;
	public AscentInput aInput;


    // Delegates


	#endregion
	
	#region Properties
	
	public int PlayerID
	{
		get { return playerId; }
		set { playerId = value; }
	}

    public InputDevice Input
    {
        get { return input; }
    }

    public GameObject Hero
    {
        get { return heroObject; }
        set { heroObject = value; }
    }

	public void SetInputDevice(InputDevice device)
	{
		this.input = device;

		Debug.Log(device);
		aInput = new AscentInput();
		aInput.Initialise(device);

		PlayerAnimController anim = heroObject.transform.GetComponent<PlayerAnimController>();
		anim.EnableInput(aInput);
	}

    public void Update()
    {
		aInput.Update();
    }

    // To create a brand new Hero
    public void CreateHero(Character.EHeroClass heroType)
    {
        // TODO: Put this function into a object creation class of some sort.
        GameObject go = null;

        switch(heroType)
        {
            case Character.EHeroClass.Warrior:
                {
                    go = Resources.Load("Prefabs/Warrior") as GameObject;
                    heroObject = GameObject.Instantiate(go) as GameObject;
                    heroScript = heroObject.GetComponent<Warrior>();
                }
                break;
            case Character.EHeroClass.Rogue:
                {
                    go = Resources.Load("Prefabs/Rogue") as GameObject;
                    heroObject = GameObject.Instantiate(go) as GameObject;
                    heroScript = heroObject.GetComponent<Rogue>();
                }
                break;
           case Character.EHeroClass.Mage:
                {
                    go = Resources.Load("Prefabs/Mage") as GameObject;
                    heroObject = GameObject.Instantiate(go) as GameObject;
                    heroScript = heroObject.GetComponent<Mage>();
                }
                break;
            default:
                {
                    Debug.LogError("Tried to make character of invalid type.");
                }
                break;
        }

        // Create the hero for the player however do not make it active.

        if (playerId == 0)
        {
            heroScript.SetColor(Color.red);
        }
        heroScript.Initialise(null);

        heroObject.SetActive(true);
    }

	
	#endregion	
}
