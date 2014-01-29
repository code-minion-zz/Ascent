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

    public Hero Hero
    {
        get { return heroScript; }
        set { heroScript = value; }
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
		heroScript = HeroFactory.CreateNewHero(heroType);
		heroObject = heroScript.gameObject;

		// This is test code to assign players colours
		switch (playerId)
		{
			case 0:
				{
					heroScript.OriginalColor = Color.red;
					heroScript.SetColor(Color.red);
				}
				break;
			case 1:
				{
					heroScript.OriginalColor = Color.green;
					heroScript.SetColor(Color.green);
				}
				break;
			case 2:
				{
					heroScript.OriginalColor = Color.blue;
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
