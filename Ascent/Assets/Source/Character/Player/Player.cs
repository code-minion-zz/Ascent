using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

public class Player 
{
	#region Fields

	private int playerId = 0;
    private GameObject heroObject;
    private Hero heroScript;
    private InputDevice input;

    //delegate void InputUpDelegate();
    //InputUpDelegate OnInputUp;

    //delegate void InputUpDelegate();
    //InputUpDelegate OnInputDown;

    //delegate void InputUpDelegate();
    //InputUpDelegate OnInputLeft;

    //delegate void InputUpDelegate();
    //InputUpDelegate OnInputRight;


    delegate void InputDelegate(InputDevice device);
    InputDelegate Onup;


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

    public void RegisterInputDelegates(InputDevice device)
    {
        if (Onup != null)
        {
            Onup(device);
        }
    }

    public void Update()
    {
        // Up 
        //if (input.LeftStickY.Value > 0.0f)
        //{
            
        //}
        //// Down
        //else if (input.LeftStickY.Value < 0.0f)
        //{

        //}
        //// Left
        //if (input.LeftStickX.Value > 0.0f)
        //{

        //}
        //// Right
        //else if (input.LeftStickX.Value < 0.0f)
        //{

        //}
    }

    // To create a brand new Hero
    public void CreateHero(Character.ECharacterClass heroType)
    {
        // TODO: Put this function into a object creation class of some sort.
        GameObject go = null;

        switch(heroType)
        {
            case Character.ECharacterClass.Warrior:
                {
                    go = Resources.Load("Prefabs/Warrior") as GameObject;
                    heroObject = GameObject.Instantiate(go) as GameObject;
                    heroScript = heroObject.GetComponent<Warrior>();
                }
                break;
            case Character.ECharacterClass.Rogue:
                {
                    go = Resources.Load("Prefabs/Rogue") as GameObject;
                    heroObject = GameObject.Instantiate(go) as GameObject;
                    heroScript = heroObject.GetComponent<Rogue>();
                }
                break;
           case Character.ECharacterClass.Mage:
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
