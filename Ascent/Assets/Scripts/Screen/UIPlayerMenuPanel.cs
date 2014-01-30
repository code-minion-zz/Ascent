using UnityEngine;
using System.Collections;

public abstract class UIPlayerMenuPanel : MonoBehaviour
{
	protected UIPlayerMenuWindow parent;
	protected UIButton currentSelection;
	protected UIButton nextSelection;

	protected UIButton[] buttons;
	protected int currentButton;
	protected int buttonMax;

	protected bool initialised = false;

	public void RegisterToInputEvents()
	{
		parent.OnMenuUp += OnMenuUp;
		parent.OnMenuDown += OnMenuDown;
		parent.OnMenuLeft += OnMenuLeft;
		parent.OnMenuRight += OnMenuRight;
		parent.OnMenuStart += OnMenuOK;
		parent.OnMenuA += OnMenuOK;
		parent.OnMenuB += OnMenuCancel;
		parent.OnMenuY += OnMenuHax;
	}

	public void DeregisterToInputEvents()
	{
		parent.OnMenuUp -= OnMenuUp;
		parent.OnMenuDown -= OnMenuDown;
		parent.OnMenuStart -= OnMenuOK;
		parent.OnMenuA -= OnMenuOK;
		parent.OnMenuB -= OnMenuCancel;
		parent.OnMenuY -= OnMenuHax;
	}

	public virtual void Initialise()
	{

	}

	public virtual void OnEnable()
	{
		if (parent == null)
		{
			parent = transform.parent.GetComponent<UIPlayerMenuWindow>();
		}

		RegisterToInputEvents();
	}

	public virtual void OnDisable()
	{
		DeregisterToInputEvents();
	}

	
	public virtual void OnMenuUp(InputDevice device)
	{
		if (currentSelection != null)
		{
			UICamera.Notify(currentSelection.gameObject, "OnHover", false);

			currentSelection = PrevButton();

			UICamera.Notify(currentSelection.gameObject, "OnHover", true);
		}
	}
	
	public virtual void OnMenuDown(InputDevice device)
	{
		if (currentSelection != null)
		{
			UICamera.Notify(currentSelection.gameObject, "OnHover", false);

			currentSelection = NextButton();

			UICamera.Notify(currentSelection.gameObject, "OnHover", true);
		}
	}

	public virtual void OnMenuLeft(InputDevice device)
	{
		
	}
	
	public virtual void OnMenuRight(InputDevice device)
	{
		
	}

	public virtual void OnMenuOK(InputDevice device)
	{

	}

	
	public virtual void OnMenuCancel(InputDevice device)
	{
		
	}

	public virtual void OnMenuHax(InputDevice device)
	{
		
	}

	protected virtual UIButton NextButton()
	{
		currentButton = ++currentButton;

		if (currentButton >= buttonMax)
		{
			currentButton = 0;
		}

		return (buttons[currentButton]);
	}

	protected virtual UIButton PrevButton()
	{
		currentButton = --currentButton;

		if (currentButton < 0)
		{
			currentButton = buttonMax - 1;
		}

		return (buttons[currentButton]);
	}
}
