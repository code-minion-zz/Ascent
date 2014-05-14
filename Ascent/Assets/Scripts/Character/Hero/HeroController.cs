using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeroController : MonoBehaviour
{
	private Hero hero;
	private HeroAnimator animator;
	private CharacterMotor motor;
    private HeroAbilityLoadout loadout;

    private InputDevice inputDevice;
    private THeroActionButtonPair actionButtonPair = new THeroActionButtonPair();
	private MoveableBlock grabbedObject;
	private bool vertGrab;

	private HeroButtonIndicator buttonIndicator;

	private InputDevice.InputControlType attackButton = InputDevice.InputControlType.Action3;
	private InputDevice.InputControlType interactButton = InputDevice.InputControlType.Action1;
	private InputDevice.InputControlType abilityOneButton = InputDevice.InputControlType.Action4;
	private InputDevice.InputControlType abilityTwoButton = InputDevice.InputControlType.Action2;

	//private float outOfCombatTimer;
	//private float timeTillIdleAnimation = 2.0f;

	private GameObject targetObject;
	public GameObject TargetObject
	{
		get { return targetObject; }
		set { targetObject = value; }
	}

	private Vector3 moveDirection;
	public Vector3 MoveDirection
	{
		get { return moveDirection; }
	}

	public bool GrabbingObject
	{
		get { return grabbedObject != null; }
	}

	public bool CanUseInput
	{
		get;
		set;
	}

	private bool wasInInteractionArea;

	Shape2D shapeA;
	Shape2D shapeB;
	Shape2D shapeC;

	private EBlockDirection blockDirection;

	public void Initialise(Hero hero, InputDevice inputDevice, HeroAnimator animator, CharacterMotor motor, HeroAbilityLoadout loadout)
	{
		this.hero = hero;
		this.inputDevice = inputDevice;
		this.animator = animator;
		this.motor = motor;
        this.loadout = loadout;

		CanUseInput = true;

		//shape = new Circle(hero.transform, 1.5f, new Vector3(0.0f, 0.0f, 1.7f));
		shapeA = new Arc(hero.transform, 3.5f, 80.0f, transform.forward * -0.5f);
		shapeB = new Arc(hero.transform, 15.0f, 30.0f, Vector3.zero);
		shapeC = new Circle(hero.transform, 1.5f, Vector3.zero);
	}

	public void InitialiseControllerIndicators()
	{
		GameObject buttonIndicatorGO = NGUITools.AddChild(FloorHUDManager.Singleton.mainPanel.gameObject, Resources.Load("Prefabs/UI/HeroButtonIndicator") as GameObject);
		buttonIndicator = buttonIndicatorGO.GetComponent<HeroButtonIndicator>();
		buttonIndicator.Initialise(hero);

		// Set scale similar to the character size
		//buttonIndicator.transform.localScale = Vector3.one * 2.5f;

		//buttonIndicator.Enable(false);

		hero.onDeath += OnDeath;
	}

	public void OnDeath(Character c)
	{
		buttonIndicator.Enable(false);

		if (GrabbingObject)
		{
			ReleaseGrabbedObject();
			motor.StopMovingAlongGrid();
		}

		//animator.PlayReactionAction(HeroAnimator.EReactionAnimation.Dying, 1.0f);
		animator.PlayDeath();
	}

    void Update()
    {
        //if(Input.GetKeyUp(KeyCode.Alpha1))
        //{
        //    CombatEvaluator evaluator = new CombatEvaluator(null, hero);

        //    evaluator.Add(new StatusEffectCombatProperty(new StunnedDebuff(null, hero, 1.0f)));
        //    evaluator.Apply();
        //}

		if (!InputManager.isEnabled)
		{
			return;
		}

		if (CanUseInput && InputManager.IsPolling)
		{
            if (hero.IsDead)
            {
                return;
            }

			FloorHUDManager hud = FloorHUDManager.Singleton;
			if (hud != null && hud.canPause && inputDevice.Start.WasReleased)
			{
				FloorHUDManager hudman = FloorHUDManager.Singleton;
				hudman.SetTransitionText("Paused");
				hudman.ShowPauseScreen(true);
			}

			// If damage is taken, control is taken away briefy to perform this take hit animation.
			if (hero.HitTaken)
			{
				// If an object is being carried. It needs to be dropped and motion halted.
				if (GrabbingObject)
				{
					ReleaseGrabbedObject();
				}

				if (actionButtonPair.control != null && actionButtonPair.control.WasReleased)
				{
					int abilityID = hero.Loadout.GetAbilityID(actionButtonPair.action);

					if (abilityID == -1)
					{
						Debug.Log(actionButtonPair.action);
					}

					hero.Loadout.UseAbility(abilityID);
					actionButtonPair.action = null;
					actionButtonPair.control = null;
				}
			}

            // Process stun only
            else if(!hero.CanAct && !hero.CanAttack && ! hero.CanMove)
            {
                if (hero.Loadout.CanInterruptActiveAbility)
                {
                    hero.Loadout.StopAbility();

                    animator.PlayMovement(HeroAnimator.EMoveAnimation.StunnedIdling);
                }
            }

			// Process everything as normal if no cast action is being used
			else if (actionButtonPair.action == null && !hero.HitTaken)
			{
				if (grabbedObject != null)
				{
						// Release the object if the button is released
					if (inputDevice.GetControl(interactButton).WasReleased)
					{
						ReleaseGrabbedObject();
					}
					else
					{
						ProcessPushableBlockMovement();
					}
				}
				else
				{
					ProcessMovement(inputDevice);

					FindTarget();
					
					if (!hero.Loadout.IsAbilityActive ||
						((hero.Loadout.IsAbilityActive && hero.Loadout.CanInterruptActiveAbility) ||
						(hero.Loadout.IsAbilityActive && !hero.Loadout.CanInterruptActiveAbility && hero.Loadout.ActiveAbility is BaseHeroAbility)))
					{
						ProcessFaceButtons(inputDevice);
						ProcessTriggersAndBumpers(inputDevice);
						//ProcessDPad(inputDevice);
					}
				}
			}

			// A cast action is being used so check for cast release
			// No other inputs are processed.
			else
			{
				// Movement is still process incase movement is allowed with this cast.
				ProcessRotation(inputDevice);

				if (actionButtonPair.control.WasReleased)
				{
					FindTarget();
					RotateToTarget();
					hero.Loadout.UseAbility(hero.Loadout.GetAbilityID(actionButtonPair.action));
					actionButtonPair.action = null;
					actionButtonPair.control = null;
				}
			}

#if UNITY_EDITOR
   			DebugKeys();
#endif
		}
    }

	void FindTarget()
	{
		// Targetting system
		// Select closest object in front of Hero
		Room curRoom = Game.Singleton.Tower.CurrentFloor.CurrentRoom;
		GameObject newTarget = curRoom.FindHeroTarget(hero, shapeA);
		if (newTarget == null)
		{
			newTarget = curRoom.FindHeroTarget(hero, shapeB);
		}
		if (newTarget == null)
		{
			newTarget = curRoom.FindHeroTarget(hero, shapeC);
		}

		if (targetObject != null)
		{
			Enemy enemy = targetObject.GetComponent<Enemy>();
			if (enemy != null)
			{
				enemy.StopHighlight();
			}
			else
			{
				targetObject.GetComponent<Interactable>().StopHighlight();
			}
		}

		if (newTarget != null)
		{
			targetObject = newTarget;

			Enemy enemy = targetObject.GetComponent<Enemy>();
			if (enemy != null)
			{
				targetObject.GetComponent<Enemy>().EnableHighlight(Color.red);
			}
			else
			{
				targetObject.GetComponent<Interactable>().EnableHighlight(Color.white);
			}
		}
		else
		{
			targetObject = null;
		}
	}

	void RotateToTarget()
	{
		if (targetObject != null)
		{
			Enemy enemy = targetObject.GetComponent<Enemy>();
			if (enemy != null)
				transform.LookAt(targetObject.transform);
		}
	}

#if UNITY_EDITOR
    void DebugKeys()
    {
		// Restore character to original state.
        if (inputDevice.Back.WasPressed)
        {
            hero.RefreshEverything();
        }
    }

	public void OnDrawGizmosSelected()
	{
		shapeA.DebugDraw();
		shapeB.DebugDraw();
		shapeC.DebugDraw();
	}
#endif
	
	public void ToggleInput(bool paused)
	{
		CanUseInput = !paused;
	}

	public void ToggleInput()
	{
		CanUseInput = !CanUseInput;
	}

	public void ProcessTriggersAndBumpers(InputDevice device)
	{
		// Left Trigger
		if (device.GetControl(abilityOneButton).WasPressed)
		{
			if ((int)EHeroAction.Action1 < loadout.AbilityBinds.Length)
				ProcessAbility(loadout.AbilityBinds[(int)EHeroAction.Action1], device.GetControl(abilityOneButton));
		}

        // Left Bump
		else if (device.GetControl(abilityTwoButton).WasPressed)
        {
			if ((int)EHeroAction.Action2 < loadout.AbilityBinds.Length)
				ProcessAbility(loadout.AbilityBinds[(int)EHeroAction.Action2], device.GetControl(abilityTwoButton));
        }

		// Right bump
		else if (device.RightBumper)
		{
			if ((int)EHeroAction.Action3 < loadout.AbilityBinds.Length)
				ProcessAbility(loadout.AbilityBinds[(int)EHeroAction.Action3], device.RightBumper);
		}

		// Right Trigger
		else if (device.RightTrigger)
		{
			if ((int)EHeroAction.Action4 < loadout.AbilityBinds.Length)
				ProcessAbility(loadout.AbilityBinds[(int)EHeroAction.Action4], device.RightTrigger);
		}
	}

    private bool ProcessAbility(Ability ability, InputControl control)
    {
        // If an action is an instant cast, it will be perform as soon as the button is pressed.
        // If it has a cast component then the cast will occur when the button is pressed...
        // the action is performed after the button has been released.
        // Actions will be successfully performed only if conditions such as cooldowns are met.

        int abilityID = hero.Loadout.GetAbilityID(ability);

        bool usedAbility = false;

        if (ability.IsInstanctCast)
        {
            usedAbility = hero.Loadout.UseAbility(abilityID);
        }
        else
        {
            if (hero)
            {
                if (hero.Loadout.UseCastAbility(abilityID))
                {
					RotateToTarget();
                    actionButtonPair.action = ability;
                    actionButtonPair.control = control;

                    usedAbility = true;
                }
            }
        }

        return usedAbility;
    }

    public void ProcessDPad(InputDevice device)
    {
		// Items will only be successfully used if they actually exist, and it is off cooldown.

        int itemToUse = -1;
		if (device.DPadLeft.WasPressed)
        {
            itemToUse = (int)EHeroAction.Consumable1;
        }
		else if (device.DPadUp.WasPressed)
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
		moveDirection = Vector3.zero;

		// The Hero can move if there is no action being performed and the Hero does not have a status effect impeding movement.
		// If the action allows it, the action can also be interrupted to allow movement.
        if ((!motor.IsHaltingMovementToPerformAction || hero.Loadout.CanInterruptActiveAbility) && hero.CanMove)
		{
			// L Stick
			if ((device.LeftStickX.IsNotNull || device.LeftStickY.IsNotNull))
			{
				moveDirection = new Vector3(inputDevice.LeftStickX.Value, 0, inputDevice.LeftStickY.Value);

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
            if (hero.IsInState(EStatus.Stun) || hero.IsInState(EStatus.Frozen))
			{
				animator.PlayMovement(HeroAnimator.EMoveAnimation.StunnedIdling);
			}
		}

		// If none of the above and no new movement this frame then just IDLE!
		if (!newMovementThisFrame)
		{
			if (Game.Singleton.Tower.CurrentFloor.CurrentRoom.AliveEnemies.Count > 0)
			{
				animator.PlayMovement(HeroAnimator.EMoveAnimation.CombatIdling);
				//outOfCombatTimer = 0.0f;
			}
			else
			{
				//outOfCombatTimer += Time.deltaTime;
				//if (outOfCombatTimer > timeTillIdleAnimation)
				//{
					//outOfCombatTimer = 0.0f;
					animator.PlayMovement(HeroAnimator.EMoveAnimation.IdleLook);
				//}
				//else
				//{
				//    animator.PlayMovement(HeroAnimator.EMoveAnimation.Idle);
				//}
			}
		}
	}

	public void ProcessRotation(InputDevice device)
	{
		if (hero.CanMove && !motor.IsHaltingRotationToPerformAction)
		{
			// L Stick
			if ((device.LeftStickX.IsNotNull || device.LeftStickY.IsNotNull))
			{
				Vector3 moveDirection = new Vector3(inputDevice.LeftStickX.Value, 0, inputDevice.LeftStickY.Value);

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
			}
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

				Vector3 rayDirection = Vector3.left;
                int layerMask = 1;

				if (Mathf.Abs(inputDevice.LeftStickY.Value) > 0.1f)
				{
					switch (blockDirection)
					{
						case EBlockDirection.North:
							{
								rayDirection = Vector3.forward;

								if (inputDevice.LeftStickY.Value < 0.0f)
								{
									rayDirection *= -1.0f;
                                    layerMask = ~(1 << (int)Layer.Floor);
								}
								else
								{
									rayDirection *= 2.0f;
                                    layerMask = ~(1 << (int)Layer.Block | 1 << (int)Layer.Floor);
								}
							} 
							break;
						case EBlockDirection.South:
							{
								rayDirection = Vector3.forward;

								if (inputDevice.LeftStickY.Value < 0.0f)
								{
									rayDirection *= -2.0f;
                                    layerMask = ~(1 << (int)Layer.Block | 1 << (int)Layer.Floor);
								}
								else
								{
									rayDirection *= 1.0f;
                                    layerMask = ~(1 << (int)Layer.Floor);
								}

							}
							break;
					}


					Vector3 pos = transform.position;
					pos.y = 0.5f;

					RaycastHit hit;
					if (!Physics.Raycast(new Ray(pos, rayDirection), out hit, Mathf.Abs(rayDirection.z), layerMask))
					{
						// Also check if the block is colliding against any other blocks
						if (!grabbedObject.CheckDirectionForAnotherBlock(rayDirection, Mathf.Abs(rayDirection.z)))
						{
							grabbedObject.MoveAlongGrid(moveDirection);
							motor.MoveAlongGrid(moveDirection);
						}
					}
				}
			}
			else // horizonal movement
			{
				moveDirection = new Vector3(inputDevice.LeftStickX.Value, 0, 0);

				Vector3 rayDirection = Vector3.left;
                int layerMask = 1;

				if (Mathf.Abs(inputDevice.LeftStickX.Value) > 0.1f)
				{
					switch (blockDirection)
					{
						case EBlockDirection.East:
							{
								rayDirection = Vector3.left;

								if (inputDevice.LeftStickX.Value < 0.0f)
								{
									rayDirection *= 1.0f;
                                    layerMask = ~(1 << (int)Layer.Floor);
								}
								else
								{
									rayDirection *= -2.0f;
                                    layerMask = ~(1 << (int)Layer.Block | 1 << (int)Layer.Floor);
								}
							}
							break;
						case EBlockDirection.West:
							{
								rayDirection = Vector3.left;

								if (inputDevice.LeftStickX.Value < 0.0f)
								{
									rayDirection *= 2.0f;
                                    layerMask = ~(1 << (int)Layer.Block | 1 << (int)Layer.Floor);
								}
								else
								{
									rayDirection *= -1.0f;
                                    layerMask = ~(1 << (int)Layer.Floor);
								}

							}
							break;
					}


					Vector3 pos = transform.position;
					pos.y = 0.5f;

					RaycastHit hit;
					if (!Physics.Raycast(new Ray(pos, rayDirection), out hit, Mathf.Abs(rayDirection.x), layerMask))
					{
						if (!grabbedObject.CheckDirectionForAnotherBlock(rayDirection, Mathf.Abs(rayDirection.x)))
						{					
							grabbedObject.MoveAlongGrid(moveDirection);
							motor.MoveAlongGrid(moveDirection);
						}
					}
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
		if (!hero.Loadout.IsAbilityActive || (hero.Loadout.IsAbilityActive && hero.Loadout.CanInterruptActiveAbility))
		{
			bool somethingInteractedWith =  ProcessInteractions(device.GetControl(interactButton).WasPressed);

			if (!somethingInteractedWith && inputDevice.GetControl(attackButton).WasPressed)
			{
				RotateToTarget();
				hero.Loadout.UseAbility((int)EHeroAction.Strike);
			}
		}

	}

	public bool ProcessInteractions(bool wasButtonPressed)
	{
		// NOTE: Only one of these may occur each time the button is pressed

		Room curRoom = Game.Singleton.Tower.CurrentFloor.CurrentRoom;
		Vector3 position = transform.position;

		bool inRange = false;

		// Is there a chest to open?
		List<TreasureChest> chests = curRoom.Chests;
		if (chests != null && chests.Count > 0)
		{
			// Are we in range of it?
			foreach (TreasureChest c in chests)
			{
				if(c.TriggerRegion.IsInside(position))
				{
					// Can it be opened?
					if (c.IsClosed)
					{
						if (wasButtonPressed)
						{
							c.OpenChest(); // I open the chest. No one else can.
							hero.FloorStatistics.NumberOfChestsOpened++;
							return true; // An interaction has occured. Exit function now.
						}

						inRange = true;
					}
				}
			}
		}


		// Is there a door?
		if (curRoom.Doors != null && curRoom.Doors.lockedDoorCount > 0)
		{

            if (Game.Singleton.Tower.keys > 0)
			{
				// Find closest door
				LockedDoor[] lockedDoors = curRoom.Doors.LockedDoors;

				//Debug.Log(lockedDoors.Length);
				foreach (LockedDoor door in lockedDoors)
				{
					if (!door.opened)
					{
						if (door.triggerRegion.IsInside(position))
						{
							if (wasButtonPressed)
							{
								door.Open();
                                Game.Singleton.Tower.keys--;
								return true;
							}

							inRange = true;
						}
					}
				}
			}
		}

        List<Shrine> shrines = curRoom.Shrines;

        if (shrines != null && shrines.Count != 0)
        {
            foreach (Shrine shrine in shrines)
            {
                if (shrine.Activated == false)
                {
                    if (shrine.TriggerRegion.IsInside(position))
                    {
                        if (wasButtonPressed)
                        {
                            shrine.Activate(hero);
							return true;
                        }

						inRange = true;
                    }
                }
            }

        }

		// Has it already been opened?

		// Is there a block?
		List<MoveableBlock> moveables = curRoom.Moveables;
		if (moveables != null && moveables.Count != 0)
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
            if (closestBlock != null && closestBlock.TriggerRegion != null)
            {
				int triggerID = 0;
                if (closestBlock.TriggerRegion.IsInside(position, out triggerID))
                {
                    // Is it in front?
                    Vector3 pos = closestBlock.transform.position;
                    pos.y = transform.position.y;
                    Vector3 direction = (transform.position - pos).normalized;
                    float dot = Vector3.Dot(direction, transform.forward);

                    if (dot < -0.75f)
                    {
                        // Has it been grabbed yet?
                        if (!closestBlock.grabbed || !closestBlock.IsInMotion)
                        {
                            if (wasButtonPressed)
                            {
                                // Grab it
                                grabbedObject = closestBlock;
                                closestBlock.grabbed = true;

                                //vertGrab = Mathf.Approximately(transform.forward.x, 0.0f);
								vertGrab = (triggerID == 1); // 0 is horizontal 

                                motor.StopMotion();
                                animator.PlayMovement(HeroAnimator.EMoveAnimation.GrabbingBlock);
                                buttonIndicator.Enable(false);

								// figure out direction
								if (vertGrab)
								{
									if (direction.z > 0.0f)
									{
										transform.LookAt(transform.position + new Vector3(0.0f, 0.0f, -1.0f));
										blockDirection = EBlockDirection.South;
									}
									else
									{
										transform.LookAt(transform.position + new Vector3(0.0f, 0.0f, 1.0f));
										blockDirection = EBlockDirection.North;

									}
								}
								else
								{
									if (direction.x > 0.0f)
									{
										transform.LookAt(transform.position + new Vector3(-1.0f, 0.0f, 0.0f));
										blockDirection = EBlockDirection.West;

									}
									else
									{
										transform.LookAt(transform.position + new Vector3(1.0f, 0.0f, 0.0f));
										blockDirection = EBlockDirection.East;
									}
								}

								// Increase mass so he cant be pushed.
								rigidbody.mass = 100000.0f;

								return true;
                            }
							
							inRange = true;
                        }
                    }
                }
            }
		}

		buttonIndicator.Enable(inRange);

		return false;
	}

	public void ReleaseGrabbedObject()
	{
		motor.StopMovingAlongGrid();
		grabbedObject.grabbed = false;
		grabbedObject = null;
		rigidbody.mass = 1.0f;
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

        ActionMax = 5,

		Consumable1 = 0,
		Consumable2 = 1,
		Consumable3 = 2,
		Consumable4 = 3,
	}

	private enum EBlockDirection
	{
		North,
		South,
		East,
		West
	}

	private struct THeroActionButtonPair
	{
		public Ability action;
		public InputControl control;
	}

}
