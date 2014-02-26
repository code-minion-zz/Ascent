using UnityEngine;
using System.Collections.Generic;

public class UITown_MainPanel : UITown_RadialPanel
{
	public GameObject ButtonPrefab;
	static float ANGLE_OFFSET = 0f;
	static float BUTTON_SCALE = 0.3f;

	public override void Initialise()
	{
		base.Initialise();

		buttons = new UIButton[7];

		Transform button;
		int i;
		Vector3 heading;
		float angle = 20f;//360f/8;
		for (i = 0; i < 7; ++i)
		{
			GameObject buttonGO = NGUITools.AddChild(gameObject, ButtonPrefab);
			buttons[i] = buttonGO.GetComponent<UIButton>();
			AngleIndex.Add(i, i * angle);
		}

		i = 0;
		// Setting individual button values
		buttons[i].gameObject.name += " Tower";
		button = buttons[i].transform;
		button.Find("Icon").GetComponent<UISprite>().spriteName = "Quit_64";
		heading = MathUtility.ConvertHeadingToVector(AngleIndex[i] + ANGLE_OFFSET);
		button.position = townParent.pointerTransform.position + (heading * 0.3f);
		Debug.Log(heading);

		++i;
		buttons[i].gameObject.name += " AccShop";
		button = buttons[i].transform;
		button.Find("Icon").GetComponent<UISprite>().spriteName = "JewelryShop_64";
		heading = MathUtility.ConvertHeadingToVector(AngleIndex[i] + ANGLE_OFFSET);
		button.position = townParent.pointerTransform.position + (heading * 0.3f);
		Debug.Log(heading);

		++i;
		buttons[i].gameObject.name += " ConShop";
		button = buttons[i].transform;
		button.Find("Icon").GetComponent<UISprite>().spriteName = "JewelryShop_64";
		heading = MathUtility.ConvertHeadingToVector(AngleIndex[i] + ANGLE_OFFSET);
		button.position = townParent.pointerTransform.position + (heading * 0.3f);
		Debug.Log(heading);
		
		++i;
		buttons[i].gameObject.name += " Tavern";
		button = buttons[i].transform;
		button.Find("Icon").GetComponent<UISprite>().spriteName = "Quit_64";
		heading = MathUtility.ConvertHeadingToVector(AngleIndex[i] + ANGLE_OFFSET);
		button.position = townParent.pointerTransform.position + (heading * 0.3f);
		Debug.Log(heading);

		++i;
		buttons[i].gameObject.name += " Backpack";
		button = buttons[i].transform;
		button.Find("Icon").GetComponent<UISprite>().spriteName = "Ascent_BackPackIcon_64";
		heading = MathUtility.ConvertHeadingToVector(AngleIndex[i] + ANGLE_OFFSET);
		button.position = townParent.pointerTransform.position + (heading * 0.3f);
		Debug.Log(heading);
		
		++i;
		buttons[i].gameObject.name += " Skills";
		button = buttons[i].transform;
		button.Find("Icon").GetComponent<UISprite>().spriteName = "Ascent_Skill_Icon_64";
		heading = MathUtility.ConvertHeadingToVector(AngleIndex[i] + ANGLE_OFFSET);
		button.position = townParent.pointerTransform.position + (heading * 0.3f);
		Debug.Log(heading);
		
		++i;
		buttons[i].gameObject.name += " Chapel";
		button = buttons[i].transform;
		button.Find("Icon").GetComponent<UISprite>().spriteName = "Ascent_Skill_Icon_64";
		heading = MathUtility.ConvertHeadingToVector(AngleIndex[i] + ANGLE_OFFSET);
		button.position = townParent.pointerTransform.position + (heading * 0.3f);
		Debug.Log(heading);

//		buttons[1].gameObject.name += " Backpack";
//		button = buttons[1].transform;
//		button.Find("Icon").GetComponent<UISprite>().spriteName = "Ascent_BackPackIcon_64";
//		ray.origin = new Vector2(transform.position.x, transform.position.y);
//		rot = Quaternion.AngleAxis(AngleIndex[1],Vector3.up);
//		Vector3 heading = MathUtility.ConvertHeadingToVector(AngleIndex[1]);
//		tempDirection = rot * transform.forward;
//		ray.direction = new Vector2(tempDirection.x, tempDirection.y);
//		tempPos = ray.GetPoint(0.5f);
//		button.position = new Vector3(tempPos.x, tempPos.y, 0f);
//
//		buttons[1] = transform.Find("Button ConShop").GetComponent<UIButton>();
//		AngleIndex.Add(45f, 1);
//
//		buttons[3] = transform.Find("Button Backpack").GetComponent<UIButton>();
//		AngleIndex.Add(-90f, 3);
//		
//		buttons[4] = transform.Find("Button AccShop").GetComponent<UIButton>();
//		AngleIndex.Add(-45f, 4);
//		
//		buttons[5] = transform.Find("Button Skilltree").GetComponent<UIButton>();
//		AngleIndex.Add(-135f, 5);
//		
//		buttons[6] = transform.Find("Button Tavern").GetComponent<UIButton>();
//		AngleIndex.Add(-215f, 6);
//
//		buttons[7] = transform.Find("Button Chapel").GetComponent<UIButton>();
//		AngleIndex.Add(-180f, 7);

		currentSelection = buttons[0];
		currentHighlightedButton = 0;

		//justInitialized = true;
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
	
//	void HighlightButton ()
//	{
//		float angle = (parent as UITownWindow).PointerAngle - 90f;
//
//		foreach (KeyValuePair<float,int> p in AngleIndex)
//		{
//			//Debug.Log("Testing Angle:" + angle + " against:" + p.Key);
//			if (CloseTo(angle,p.Key))
//			{
//				//Debug.Log("WIN!! Angle:" + angle + " against:" + p.Key);
//				UICamera.Notify(currentSelection.gameObject, "OnHover", false);
//				currentSelection = buttons[p.Value];
//				currentHighlightedButton = p.Value;
//				UICamera.Notify(currentSelection.gameObject, "OnHover", true);
//				SetInfoLabel();
//			}
//			else
//			{
//				//Debug.Log("FAIL!! Angle:" + angle + " against:" + p.Key);
//			}
//		}
//	}

	#region Input Handling
	public override void OnMenuLeftStickMove(InputDevice device)
	{
		if (!gameObject.activeInHierarchy) return;
		//HighlightButton();
		updatePointer = true;
	}

	public override void OnMenuOK(InputDevice device)
	{
		if (currentSelection.gameObject.activeInHierarchy)	ButtonAction();
	}
	
	public override void OnMenuCancel(InputDevice device)
	{
	}

	public override void OnMenuUp(InputDevice device)
	{
		// TODO: Change character's equipment
		
	}
	public override void OnMenuDown(InputDevice device)
	{
		// TODO: Change character's equipment
		
	}
	public override void OnMenuLeft(InputDevice device)
	{
		// TODO: Change character's equipment
		
	}
	public override void OnMenuRight(InputDevice device)
	{
		// TODO: Change character's equipment
		
	}
	#endregion 

	void ButtonAction()
	{
		switch (currentHighlightedButton)
		{
		case 0 : // tower
			(parent as UITownWindow).RequestTransitionToPanel(2);
			break;
		case 1 : // conshop
			(parent as UITownWindow).RequestTransitionToPanel(5);
			break;
		case 2 : // quit
			Game.Singleton.LoadLevel(Game.EGameState.MainMenu);
			break;
		case 3 : // backpack
			(parent as UITownWindow).RequestTransitionToPanel(1);
			break;
		case 4 : // accshop
			(parent as UITownWindow).RequestTransitionToPanel(4);
			break;
		case 5 : // skills
			(parent as UITownWindow).RequestTransitionToPanel(3);
			break;
		case 6 : // tavern
			(parent as UITownWindow).RequestTransitionToPanel(6);
			break;
		default : // nothing meaningful is highlighted
			break;
		}
	}

	void SetInfoLabel()
	{
		UITownWindow townWindow = (parent as UITownWindow);

		switch (currentHighlightedButton)
		{
		case 0: // tower
			townWindow.SetInfo("Enter the Tower" + AngleIndex[currentHighlightedButton]);
			break;
		case 1: // conshop
			townWindow.SetInfo("Visit the Item Shop" + AngleIndex[currentHighlightedButton]);
			break;
		case 2: // quit
			townWindow.SetInfo("Quit to Main Menu" + AngleIndex[currentHighlightedButton]);
			break;
		case 3: // backpack
			townWindow.SetInfo("Manage your Equipment" + AngleIndex[currentHighlightedButton]);
			break;
		case 4: // accshop
			townWindow.SetInfo("Visit the Gem shop" + AngleIndex[currentHighlightedButton]);
			break;
		default:
			townWindow.SetInfo("");
			break;
		}
	}

}
