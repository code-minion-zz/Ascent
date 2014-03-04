using UnityEngine;
using System.Collections.Generic;

public class UITown_MainPanel : UITown_RadialPanel
{
	public GameObject ButtonPrefab;
	static float ANGLE_CORRECTION = 90f * Mathf.Deg2Rad;
	static float BUTTON_SCALE = 0.3f;
	static int ANGLE_DIVISION = 8;

	public override void Initialise()
	{
		base.Initialise();

		buttons = new UIButton[7];

		// Create and position buttons for the main menu in a circle
		Transform button;
		int i;
		Vector3 heading;
		float angle = 360f/8;
		float radians = angle * Mathf.Deg2Rad;
		List<float> radianList = new List<float>();
		Angular_Tolerance = angle/2;
		for (i = 0; i < 7; ++i)
		{
			int mod = 0;
			GameObject buttonGO = NGUITools.AddChild(gameObject, ButtonPrefab);
			buttons[i] = buttonGO.GetComponent<UIButton>();

			if (i > 3)
			{
				mod = 1;
			}
			AngleIndex.Add(i, (i+mod) * angle);
			radianList.Add((i+mod) * radians);
		}
		radianList.Add(i*radians);

		i = 0;
		// Setting individual button values
		buttons[i].gameObject.name += " Tower";
		button = buttons[i].transform;
		button.Find("Icon").GetComponent<UISprite>().spriteName = "Quit_64";
		heading = MathUtility.ConvertHeadingToVector(radianList[i] + ANGLE_CORRECTION);
		button.position = townParent.pointerTransform.position + (heading * 0.3f);

		++i;
		buttons[i].gameObject.name += " AccShop";
		button = buttons[i].transform;
		button.Find("Icon").GetComponent<UISprite>().spriteName = "JewelryShop_64";
		heading = MathUtility.ConvertHeadingToVector(radianList[i] + ANGLE_CORRECTION);
		button.position = townParent.pointerTransform.position + (heading * 0.3f);

		++i;
		buttons[i].gameObject.name += " ConShop";
		button = buttons[i].transform;
		button.Find("Icon").GetComponent<UISprite>().spriteName = "JewelryShop_64";
		heading = MathUtility.ConvertHeadingToVector(radianList[i] + ANGLE_CORRECTION);
		button.position = townParent.pointerTransform.position + (heading * 0.3f);
		
		++i;
		buttons[i].gameObject.name += " Tavern";
		button = buttons[i].transform;
		button.Find("Icon").GetComponent<UISprite>().spriteName = "Quit_64";
		heading = MathUtility.ConvertHeadingToVector(radianList[i] + ANGLE_CORRECTION);
		button.position = townParent.pointerTransform.position + (heading * 0.3f);

		++i;
		buttons[i].gameObject.name += " Backpack";
		button = buttons[i].transform;
		button.Find("Icon").GetComponent<UISprite>().spriteName = "Ascent_BackPackIcon_64";
		heading = MathUtility.ConvertHeadingToVector(radianList[i] + ANGLE_CORRECTION);
		button.position = townParent.pointerTransform.position + (heading * 0.3f);
		
		++i;
		buttons[i].gameObject.name += " Skills";
		button = buttons[i].transform;
		button.Find("Icon").GetComponent<UISprite>().spriteName = "Ascent_Skill_Icon_64";
		heading = MathUtility.ConvertHeadingToVector(radianList[i] + ANGLE_CORRECTION);
		button.position = townParent.pointerTransform.position + (heading * 0.3f);
		
		++i;
		buttons[i].gameObject.name += " Chapel";
		button = buttons[i].transform;
		button.Find("Icon").GetComponent<UISprite>().spriteName = "Ascent_Skill_Icon_64";
		heading = MathUtility.ConvertHeadingToVector(radianList[i] + ANGLE_CORRECTION);
		button.position = townParent.pointerTransform.position + (heading * 0.3f);

		// set current selection to first button
		currentSelection = buttons[0];
		currentHighlightedButton = 0;

		// set flags
		initialised = true;
		updatePointer = true;
	}


    public override void OnEnable()
	{
		base.OnEnable();

		if (initialised) 
		{
			(parent as UITownWindow).SetTitle("Town");
		}
    }

    public void Update()
    {
		if (updatePointer)
		{
			if (HighlightButton())
			{
				SetInfoLabel();
			}
			updatePointer = false;
		}

		if (initialised)
		{
			SetInfoLabel();
		}
	}

#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		if (!initialised) return;
		Gizmos.DrawWireSphere(townParent.pointerTransform.position, 0.3f);
	}
#endif

	#region Input Handling
	public override void OnMenuLeftStickMove(InputDevice device)
	{
		if (!gameObject.activeInHierarchy) return;
		//HighlightButton();
		updatePointer = true;
	}

	public override void OnMenuOK(InputDevice device)
	{
		if (currentSelection == null) return;
		if (currentSelection.gameObject.activeInHierarchy)	ButtonAction();
	}
	
	public override void OnMenuCancel(InputDevice device)
	{
	}

	public override void OnMenuUp(InputDevice device)
	{
		
	}
	public override void OnMenuDown(InputDevice device)
	{
		
	}
	public override void OnMenuLeft(InputDevice device)
	{
		
	}
	public override void OnMenuRight(InputDevice device)
	{
		
	}
	#endregion 

	void ButtonAction()
	{
		switch (currentHighlightedButton)
		{
		case 0 : // tower
			(parent as UITownWindow).RequestTransitionToPanel(2);
			break;
		case 1 : // accshop
			(parent as UITownWindow).RequestTransitionToPanel(4);
			break;
		case 2 : // conshop
			(parent as UITownWindow).RequestTransitionToPanel(5);
			break;
		case 3 : // tavern
			(parent as UITownWindow).RequestTransitionToPanel(6);
			break;
		case 4 : // backpack
			(parent as UITownWindow).RequestTransitionToPanel(1);
			break;
		case 5 : // skills
			(parent as UITownWindow).RequestTransitionToPanel(3);
			break;
		case 6 : // chapel
			(parent as UITownWindow).RequestTransitionToPanel(7);
			break;
		default : // nothing meaningful is highlighted
			break;
		}
	}

	void SetInfoLabel()
	{
		//UITownWindow townParent = (parent as UITownWindow);

		switch (currentHighlightedButton)
		{
		case 0: // tower
			townParent.SetInfo("Enter the Tower");
			break;
		case 1: // conshop
			townParent.SetInfo("Shop in the Gem Shop");
			break;
		case 2: // accshop
			townParent.SetInfo("Shop in the Item Shop");
			break;
		case 3: // tavern
			townParent.SetInfo("Go to the Tavern");
			break;
		case 4: // backpack
			townParent.SetInfo("Manage your Equipment");
			break;
		case 5: // skills
			townParent.SetInfo("Manage your Skills");
			break;
		case 6: // chapel
			townParent.SetInfo("Visit the Chapel");
			break;
		default:
			townParent.SetInfo("");
			break;
		}
	}

}
