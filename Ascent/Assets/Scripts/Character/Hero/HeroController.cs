using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeroController : MonoBehaviour
{
	private Hero hero;
	private HeroAnimator animator;
	private CharacterMotor motor;

    private InputDevice inputDevice;
    private THeroActionButtonPair actionButtonPair = new THeroActionButtonPair();
	private MoveableBlock grabbedObject;
	private bool vertGrab;

	public bool GrabbingObject
	{
		get { return grabbedObject != null; }
	}

	public bool CanUseInput
	{
		get;
		set;
	}

	public void Initialise(Hero hero, InputDevice inputDevice, HeroAnimator animator, CharacterMotor motor)
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
			// If damage is taken, control is taken away briefy to perform this take hit animation.
			if (hero.HitTaken)
			{
				// If an object is being carried. It needs to be dropped and motion halted.
				if (GrabbingObject)
				{
					ReleaseGrabbedObject();
					motor.StopMovingAlongGrid();
				}

				// Action's that cannot be interrupted will not cause this to happen.
				if (hero.CanInterruptActiveAbility)
				{
					animator.PlayReactionAction(HeroAnimator.EReactionAnimation.TakingHit, 0.5f);
				}
			}

			// Process everything as normal if no cast action is being used
			else if (actionButtonPair.action == null && !hero.HitTaken)
			{
				if (grabbedObject != null)
				{
					ProcessPushableBlockMovement();
				}
				else
				{
					ProcessMovement(inputDevice);
					ProcessFaceButtons(inputDevice);
					ProcessTriggersAndBumpers(inputDevice);
					ProcessDPad(inputDevice);
				}
			}

			// A cast action is being used so check for cast release
			// No other inputs are processed.
			else
			{
				if (actionButtonPair.control.WasReleased)
				{
					hero.UseAbility(actionButtonPair.action);
					actionButtonPair.action = null;
				}
			}

#if UNITY_EDITOR
			// Restore character to original state.
            if (inputDevice.Back.WasPressed)
            {
                hero.RefreshEverything();
            }
#endif
		}
    }

	public void ProcessTriggersAndBumpers(InputDevice device)
	{
		// If an action is an instant cast, it will be perform as soon as the button is pressed.
		// If it has a cast component then the cast will occur when the button is pressed...
		// the action is performed after the button has been released.
		// Actions will be successfully performed only if conditions such as cooldowns are met.

		// Left Bump
		if (device.LeftBumper.WasPressed)
		{
            Action action = hero.Abilities[(int)EHeroAction.Action2];
            if (action.IsInstanctCast)
            {
                hero.UseAbility((int)EHeroAction.Action2);
            }
            else
            {
				if (hero.CanCastAbility((int)EHeroAction.Action2))
				{
					if (hero.UseCastAbility((int)EHeroAction.Action2))
					{
						actionButtonPair.action = action;
						actionButtonPair.control = device.LeftBumper;
					}
				}
            }
		}

		// Left Trigger
		else if (device.LeftTrigger.WasPressed)
		{
			Action action = hero.Abilities[(int)EHeroAction.Action1];
			if (action.IsInstanctCast)
			{
				hero.UseAbility((int)EHeroAction.Action1);
			}
			else
			{
				if (hero.CanCastAbility((int)EHeroAction.Action1))
				{
					if (hero.UseCastAbility((int)EHeroAction.Action1))
					{
						actionButtonPair.action = action;
						actionButtonPair.control = device.LeftTrigger;
					}
				}
			}
		}

		// Right bump
		else if (device.RightBumper.WasPressed)
		{
            Action action = hero.Abilities[(int)EHeroAction.Action3];
            if (action.IsInstanctCast)
            {
                hero.UseAbility((int)EHeroAction.Action3);
            }
            else
            {
                if (hero.CanCastAbility((int)EHeroAction.Action3))
                {
                    if (hero.UseCastAbility((int)EHeroAction.Action3))
					{
						actionButtonPair.action = action;
						actionButtonPair.control = device.RightBumper;
					}
                }
            }
		}

		// Right Trigger
		else if (device.RightTrigger.WasPressed)
		{
			Action action = hero.Abilities[(int)EHeroAction.Action4];
			if (action.IsInstanctCast)
			{
				hero.UseAbility((int)EHeroAction.Action4);
			}
			else
			{
				if (hero.CanCastAbility((int)EHeroAction.Action4))
				{
					if (hero.UseCastAbility((int)EHeroAction.Action4))
					{
						actionButtonPair.action = action;
						actionButtonPair.control = device.RightTrigger;
					}
				}
			}
		}
	}

    public void ProcessDPad(InputDevice device)
    {
		// Items will only be successfully used if they actually exist, and it is off cooldown.

        int itemToUse = -1;
        if (device.DPadUp.WasPressed)
        {
            itemToUse = (int)EHeroAction.Consumable1;
        }
        else if (device.DPadLeft.WasPressed)
        {
            itemToUse = (int)EHeroAction.Consumable2;
        }
        else if (device.DPadRight.WasPressed)
        {
            itemToUse = (int)EHeroAction.Consumable3;
        }

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
		// Reset movement speed
		bool newMovementThisFrame = false;

		// The Hero can move if there is no action being performed and the Hero does not have a status effect impeding movement.
		// If the action allows it, the action can also be interrupted to allow movement.
		if ((motor.IsHaltingMovementToPerformAction || hero.CanInterruptActiveAbility) && hero.CanMove)
		{
			// L Stick
			if ((device.LeftStickX.IsNotNull || device.LeftStickY.IsNotNull))
			{
				Vector3 moveDirection = new Vector3(inputDevice.LeftStickX.Value, 0, inputDevice.LeftStickY.Value);

				// Check that substantial movement has been made (hopefully greater than deadzone)
				newMovementThisFrame = (moveDirection.sqrMagnitude > 0.001f);

				// Keyboard functions differently with diagonals.
				// IE. On Keyboard X + Y == 2.0f. On XBox X + Y == 1.5f.
				if (!device.isJoystick) // Then assume keyboard
				{
					if (Mathf.Abs(moveDirection.x) + Mathf.Abs(moveDirection.z) >= 1.9f)
					{
						// 0.7f is the value given on XBox for perfectly diagonal movement
						moveDirection.x *= 0.7f;
						moveDirection.z *= 0.7f;
					}
				}

				// Look at the direction of movement and push character forward
				transform.LookAt(transform.position + moveDirection);
				motor.Move(moveDirection);

				// Only need to update the animator if significant movement was made.
				if (newMovementThisFrame)
				{
					// The greatest axis value is fed to the animator
					float moveX = Mathf.Abs(moveDirection.x);
					float moveY = Mathf.Abs(moveDirection.z);

					// The animator does not accept values above 1.0f or less than 0.0f
					float animMoveSpeed = 0.0f;
					if (moveX >= 1.0f || moveY >= 1.0f)
					{
						// Cap the value to 1.0f
						animMoveSpeed = 1.0f;
					}
					else
					{
						// Take the highest value from X or Y
						animMoveSpeed = moveX > moveY ? moveX : moveY;
					}

					// Change anim state to play the walking animation.
					animator.PlayMovement(HeroAnimator.EMoveAnimation.Moving);

					// Feed the highest axis value to the animator to be used for animation blending.
					animator.Move(animMoveSpeed);
				}
			}
		}
		else // Hero cannot move
		{
			if (hero.IsInState(EStatus.Stun))
			{
				animator.PlayMovement(HeroAnimator.EMoveAnimation.StunnedIdling);
			}
		}

		// If none of the above and no new movement this frame then just IDLE!
		if (!newMovementThisFrame)
		{
			animator.PlayMovement(HeroAnimator.EMoveAnimation.CombatIdling);
		}
	}

	public void ProcessPushableBlockMovement()
	{
		// TODO: Play grabbing animation

		// If the block isn't already moving then I can move it
		if (!grabbedObject.IsInMotion)
		{
			Vector3 moveDirection = Vector3.zero;

			// TODO: Prevent movement tunneling through walls and other objects.
			// I can only move it in the direction I grabbed it. Either vertically or horiztonally.
			if (vertGrab)
			{
				moveDirection = new Vector3(0, 0, inputDevice.LeftStickY.Value);

				if (Mathf.Abs(inputDevice.LeftStickY.Value) > 0.1f)
				{
					grabbedObject.MoveAlongGrid(moveDirection);
					motor.MoveAlongGrid(moveDirection);

					// TODO: Play push/pull animation
				}
			}
			else // horizonal movement
			{
				moveDirection = new Vector3(inputDevice.LeftStickX.Value, 0, 0);

				if (Mathf.Abs(inputDevice.LeftStickX.Value) > 0.1f)
				{
					grabbedObject.MoveAlongGrid(moveDirection);
					GetComponent<CharacterMotor>().MoveAlongGrid(moveDirection);

					// TODO: Play push/pull animation
				}
			}
		}

#if UNITY_EDITOR
		// Draw a line from me to the object
		Debug.DrawLine(transform.position, grabbedObject.transform.position);
#endif
	}

	public void ProcessFaceButtons(InputDevice device)
	{
		// If an object is grabbed...
		if (grabbedObject != null)
		{
			// Release the object if the button is released
			if (device.Y.WasReleased)
			{
				ReleaseGrabbedObject();
			}
		}

		// TODO: Remove X or A depending on what people think is more intuitive
		if (device.X.WasPressed || device.A.WasPressed) 
		{
			hero.UseAbility((int)EHeroAction.Strike);
		}

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

	public enum EHeroAction
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

	private struct THeroActionButtonPair
	{
		public Action action;
		public InputControl control;
	}

}
