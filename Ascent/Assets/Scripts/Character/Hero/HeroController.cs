using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeroController : MonoBehaviour
{
    public enum HeroAction
    {
        None = -1,

        Strike = 0,
        Interaction,
        Confirm,
        Cancel,

        Action1 = 1,
        Action2 = 2,
        Action3 = 3,
        Action4 = 4 

    }

    private HeroAnimator heroAnimator;
    private Hero hero;
    private InputDevice input;
    private bool actionBindingsEnabled = false;
	public Character actor;

	private MoveableBlock grabbedObject;
	public bool GrabbingObject
	{
		get { return grabbedObject != null; }
	}
	private bool vertGrab;

	bool vert;
	bool horiz;

    public InputDevice Input
    {
        get { return input; }
    }

    /// <summary>
    /// Enables or disables the action binding, what this means is that
    /// we can repurpose buttons for example the Y button for opening chests.
    /// </summary>
    public bool EnableActionBinding
    {
        get { return actionBindingsEnabled; }
        set { actionBindingsEnabled = value; }
    }

    #region Intialization

	public void Initialise(Hero hero)
	{
		this.hero = hero;
		heroAnimator = hero.Animator as HeroAnimator;
		//actor = GameObject.Find("Cube").GetComponent<C>;

	}


	public void EnableInput(InputDevice inputDevice)
	{
		input = inputDevice;
	}

	public void DisableInput()
	{
		input = null;
	}

    #endregion


    void Update()
    {
		if (InputManager.IsPolling)
		{
			InputDevice device = input;

            if (GetComponent<CharacterMotor>().canMove)
            {
                if (grabbedObject != null)
                {
                    if (device.Y.WasReleased)
                    {
                        ReleaseGrabbedObject();
                    }
                }

                // L Stick
                if ((device.LeftStickX.IsNotNull || device.LeftStickY.IsNotNull))
                {
                    if (Mathf.Abs(device.LeftStickX.Value) > Mathf.Abs(device.LeftStickY.Value))
                    {
                        vert = false;

                    }
                    else
                    {
                        vert = true;
                    }

                    Vector3 moveDirection = Vector3.zero;

                    if (grabbedObject != null)
                    {
                        Debug.DrawLine(transform.position, grabbedObject.transform.position);

                        if (!grabbedObject.moving)
                        {
                            if (vertGrab)
                            {
                                moveDirection = new Vector3(0, 0, device.LeftStickY.Value);

                                if (Mathf.Abs(device.LeftStickY.Value) > 0.1f)
                                {

                                    grabbedObject.Move(moveDirection);
                                    GetComponent<CharacterMotor>().MoveAlongGrid(moveDirection);
                                }

                            }
                            else
                            {
                                moveDirection = new Vector3(device.LeftStickX.Value, 0, 0);

                                if (Mathf.Abs(device.LeftStickX.Value) > 0.1f)
                                {
                                    grabbedObject.Move(moveDirection);
                                    GetComponent<CharacterMotor>().MoveAlongGrid(moveDirection);
                                }
                            }
                        }

                    }
                    else if (!GetComponent<CharacterMotor>().moving)
                    {
                        moveDirection = new Vector3(device.LeftStickX.Value, 0, device.LeftStickY.Value);

                        if (vert)
                        {
                            transform.LookAt(transform.position + new Vector3(0.0f, 0.0f, device.LeftStickY.Value));
                        }
                        else
                        {
                            transform.LookAt(transform.position + new Vector3(device.LeftStickX.Value, 0.0f, 0.0f));
                        }
                    }

                    GetComponent<CharacterMotor>().Move(moveDirection);
                    GetComponent<Character>().OnMove();

                    float speed = (device.LeftStickX.Value * device.LeftStickX.Value) + (device.LeftStickY.Value * device.LeftStickY.Value);
                    speed *= heroAnimator.MovementSpeed * Time.deltaTime;
                    speed *= 10000.0f;

                    // Direction vector to hold the input key press.
                    Vector3 direction = new Vector3(device.LeftStickX.Value, 0, device.LeftStickY.Value).normalized;
                    heroAnimator.AnimMove(direction, speed);

                    transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);

                    //transform.Rotate(new Vector3(0.0f, device.LeftStickX.Value * 3.0f, 0.0f));
                }
            }
           

			//Debug.DrawLine(transform.position, transform.position + transform.forward * 2.5f);


            if (device.X.WasPressed)
            {
                hero.UseAbility((int)HeroAction.Strike);
            }

            if(device.Back.WasPressed)
            {
                hero.RefreshEverything();
            }

            if (device.LeftBumper.WasPressed)
            {
                //hero.UseAbility(1);
                hero.UseAbility((int)HeroAction.Action1);
            }
            else if (device.LeftTrigger.WasPressed)
            {
                hero.UseAbility((int)HeroAction.Action4);
            }
            else if(device.RightBumper.WasPressed)
            {
                hero.UseAbility((int)HeroAction.Action2); // pass in the ability binded to this key
                
            }
            else if (device.RightTrigger.WasPressed)
            {
                hero.UseAbility((int)HeroAction.Action4);
            }

			// We can bind something to this key.
			if (device.Y.WasPressed)
			{
				ProcessInteractions();
			}


			//if (!actionBindingsEnabled)
			//{
			//    // We can bind something to this key.
			//    if (device.Y.WasPressed)
			//    {
                    
			//    }
			//}
		}
    }

	public void ProcessInteractions()
	{
		// NOTE: Only one of these may occur each time the button is pressed

		Room curRoom = Game.Singleton.Tower.CurrentFloor.CurrentRoom;
		Vector3 position = transform.position;

		// Is there a chest to open?
		List<TreasureChest> chests = curRoom.Chests;
		if (chests != null)
		{
			// Are we in range of it?
			foreach (TreasureChest c in chests)
			{
				if(c.TriggerRegion.IsInside(position))
				{
					// Can it be opened?
					if (c.IsClosed)
					{
						c.OpenChest(); // I open the chest. No one else can.
						
						return; // An interaction has occured. Exit function now.
					}
				}
			}
		}

		// Is there an item?
		List<LootDrop> loot = curRoom.LootDrops;
		if (loot != null)
		{
			// Find the closest item
			LootDrop closestDrop = null;
			float closestDistance = 10000.0f;
			foreach (LootDrop l in loot)
			{
				if (!l.CanBePickedUp)
				{
					continue;
				}

				float distance = (position - l.transform.position).sqrMagnitude;

				if (distance < closestDistance)
				{
					closestDistance = distance;
					closestDrop = l;
				}
			}

			if (closestDrop != null)
			{
				// Am I within range of the item?
				if (closestDrop.TriggerRegion.IsInside(position))
				{
					closestDrop.PickUp(hero.HeroInventory); // I pick it up. No one else can!

					return; // An interaction has occured. Exit function now.
				}
			}
		}


		// Is there a door?
		// Find closest door
		// Has it already been opened?

		// Is there a block?
		List<MoveableBlock> moveables = curRoom.Moveables;
		if (moveables != null)
		{
			// Find the closest block
			MoveableBlock closestBlock = null;
			float closestDistance = 10000.0f;

			foreach (MoveableBlock m in moveables)
			{
				if (m.grabbed)
				{
					continue;
				}

				float distance = (position - m.transform.position).sqrMagnitude;

				if (distance < closestDistance)
				{
					closestDistance = distance;
					closestBlock = m;
				}
			}
			// Are we in range of it?
			if (closestBlock.TriggerRegion.IsInside(position))
			{
				// Is it in front?
				Vector3 pos = closestBlock.transform.position;
				pos.y = transform.position.y;
				Vector3 direction = (transform.position - pos).normalized;
				float dot = Vector3.Dot(direction, transform.forward);

				if (dot < -0.8f)
				{
					// Has it been grabbed yet?
					if (!closestBlock.grabbed)
					{
						// Grab it
						grabbedObject = closestBlock;
						closestBlock.grabbed = true;

						vertGrab = Mathf.Approximately(transform.forward.x, 0.0f);

						
						return;
					}
				}
			}
		}
	}

	public void ReleaseGrabbedObject()
	{
		grabbedObject.grabbed = false;
		grabbedObject = null;
	}
}
