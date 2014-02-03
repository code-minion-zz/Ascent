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
        Action4 = 4,

        Consumable1 = 0,
        Consumable2 = 1,
        Consumable3 = 2,
        Consumable4 = 3,


    }

	private Hero hero;
	private CharacterAnimator animator;
	private CharacterMotor motor;
    private InputDevice inputDevice;
    private bool actionBindingsEnabled = false;

	private MoveableBlock grabbedObject;
	public bool GrabbingObject
	{
		get { return grabbedObject != null; }
	}
	private bool vertGrab;

    public InputDevice Input
    {
        get { return inputDevice; }
    }

	public bool CanUseInput
	{
		get;
		set;
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

	public void Initialise(Hero hero, InputDevice inputDevice, CharacterAnimator animator, CharacterMotor motor)
	{
		this.hero = hero;
		this.inputDevice = inputDevice;
		this.animator = animator;
		this.motor = motor;

				CanUseInput = true;
	}


    void Update()
    {
		if (CanUseInput && InputManager.IsPolling)
		{
			InputDevice device = inputDevice;

            if (motor.canMove)
            {
                if (!hero.IsStunned)
                {
                    ProcessFaceButtons(device);
                    ProcessTriggersAndBumpers(device);
				    ProcessMovement(device);
                }

                ProcessDPad(device);
            }

#if UNITY_EDITOR
            if (device.Back.WasPressed)
            {
                hero.RefreshEverything();
            }
#endif
		}
    }

	public void ProcessTriggersAndBumpers(InputDevice device)
	{
		if (device.LeftBumper.WasPressed)
		{
			if (animator.TakeHit == false && animator.Dying == false)
			{
				hero.UseAbility((int)HeroAction.Action1);
			}
		}
		else if (device.LeftTrigger.WasPressed)
		{
			if (animator.TakeHit == false && animator.Dying == false)
			{
				hero.UseAbility((int)HeroAction.Action4);
			}
		}
		else if (device.RightBumper.WasPressed)
		{
			if (animator.TakeHit == false && animator.Dying == false)
			{
				hero.UseAbility((int)HeroAction.Action2); // pass in the ability binded to this key
			}

		}
		else if (device.RightTrigger.WasPressed)
		{
			if (animator.TakeHit == false && animator.Dying == false)
			{
				hero.UseAbility((int)HeroAction.Action3);
			}
		}
	}

    public void ProcessDPad(InputDevice device)
    {
        int itemToUse = -1;
        if (device.DPadUp.WasPressed)
        {
            itemToUse = (int)HeroAction.Consumable1;
        }
        else if (device.DPadLeft.WasPressed)
        {
            itemToUse = (int)HeroAction.Consumable2;
        }
        else if (device.DPadRight.WasPressed)
        {
            itemToUse = (int)HeroAction.Consumable3;
        }
        //else if (device.DPadDown.WasPressed)
        //{
        //    itemToUse = (int)HeroAction.Consumable4;
        //}

        if (itemToUse != -1)
        {
            if (hero.Backpack.ConsumableItems[itemToUse] != null)
            {
                hero.Backpack.ConsumableItems[itemToUse].UseItem(hero);
            }
        }
    }

	public void ProcessMovement(InputDevice device)
	{
		if (GetComponent<CharacterMotor>().canMove && !hero.IsStunned)
		{
			// L Stick
			if ((device.LeftStickX.IsNotNull || device.LeftStickY.IsNotNull))
			{
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
				}

				transform.LookAt(transform.position + moveDirection);
				GetComponent<CharacterMotor>().Move(moveDirection);

				float animMoveSpeed = 0.0f;
				float moveX = Mathf.Abs(moveDirection.x);
				float moveY = Mathf.Abs(moveDirection.z);

				if (moveX >= 1.0f || moveY >= 1.0f)
				{
					animMoveSpeed = 1.0f;
				}
				else
				{
					animMoveSpeed = moveX > moveY ? moveX : moveY;
				}
				animator.PlayAnimation("Speed", animMoveSpeed);
			}
		}
	}

	public void ProcessFaceButtons(InputDevice device)
	{
		if (grabbedObject != null)
		{
			if (device.Y.WasReleased)
			{
				ReleaseGrabbedObject();
			}
		}

		if (device.X.WasPressed)
		{
			hero.UseAbility((int)HeroAction.Strike);
		}

		// We can bind something to this key.
		if (device.Y.WasPressed)
		{
			ProcessInteractions();
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
                        hero.FloorStatistics.NumberOfChestsOpened++;
						
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
                    hero.FloorStatistics.NumberOfItemsPickedUp++;

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
