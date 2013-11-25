using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Player : MonoBehaviour
{
	#region Fields

	int playerId = 0;
    private GameObject heroObject;
    private Hero heroScript;
    private InputDevice input;

    public UIPlayerPanel activePlayerPanel;

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

    #endregion

    public void BindInputDevice(InputDevice device)
	{
		this.input = device;
	}

	public void UnbindInputDevice()
	{
		this.input = null;
	}

    public void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void Update()
    {
        // TODO: Handle lost input device.
        //          Subscribe to detach event and disable input until it is reattached. 
        //          Then rebind the device to the hero controller.
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
                    Debug.Log("Made Warrior");
                    go = Resources.Load("Prefabs/Warrior") as GameObject;
                    heroObject = GameObject.Instantiate(go) as GameObject;
                    heroScript = heroObject.AddComponent<Warrior>();
                }
                break;
            case Character.EHeroClass.Rogue:
                {
                    Debug.Log("Made Rogue");
                    go = Resources.Load("Prefabs/Rogue") as GameObject;
                    heroObject = GameObject.Instantiate(go) as GameObject;
                    heroScript = heroObject.AddComponent<Rogue>();
                }
                break;
           case Character.EHeroClass.Mage:
                {
                    Debug.Log("Made Mage");
                    go = Resources.Load("Prefabs/Mage") as GameObject;
                    heroObject = GameObject.Instantiate(go) as GameObject;
                    heroScript = heroObject.AddComponent<Mage>();
                }
                break;
            default:
                {
                    Debug.LogError("Tried to make character of invalid type.");
                }
                break;
        }

       
        // This is test code to assign players colours
        switch (playerId)
        {
            case 0:
                {
                    heroScript.SetColor(Color.red);
                }
                break;
            case 1:
                {
                    heroScript.SetColor(Color.green);
                }
                break;
            case 2:
                {
                    heroScript.SetColor(Color.blue);
                }
                break;
        }

        // Create the animator and controller for this hero (binds the input with the controller)
        heroScript.Initialise(input, null);
        // TODO: Do not make it active until gameplay starts
        heroObject.SetActive(true);
        heroObject.transform.parent = this.transform;
        heroObject.transform.localPosition = Vector3.zero;
        heroObject.transform.localRotation = Quaternion.identity;
    }

}
