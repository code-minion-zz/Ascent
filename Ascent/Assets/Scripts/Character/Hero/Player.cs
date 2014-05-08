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

	private UIPlayerMenuPanel activePlayerPanel;
	public UIPlayerMenuPanel ActivePlayerPanel
	{
		get { return activePlayerPanel; }
		set { activePlayerPanel = value; }
	}

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

        //// This is test code to assign players colours
        //Color color = GetPlayerColor(playerId);
        //heroScript.OriginalColor = color;
        //heroScript.SetColor(color);

        // Create the animator and controller for this hero (binds the input with the controller)
        heroScript.Initialise(input, null);

        // TODO: Do not make it active until gameplay starts
        heroObject.SetActive(true);
        heroObject.transform.parent = this.transform;
        heroObject.transform.localPosition = Vector3.zero;
        heroObject.transform.localRotation = Quaternion.identity;
    }

	public static Color GetPlayerColor(int i)
	{
		Color color = Color.red;

		if(i == 1)
		{
			color = Color.green;
		}
		else if(i == 2)
		{
			color = Color.blue;
		}
		else if (i == 3)
		{
			color = Color.magenta;
		}

		return color;
	}
}
