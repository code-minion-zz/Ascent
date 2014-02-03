using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeroAnimator : CharacterAnimator  
{
	/// <summary>
	///  These mimics layers in AnimatorController
	/// </summary>
	public enum ELayer
	{
		None = -1,

		Base = 0,
		Movement = 1,		// EMoveAnimation
		Interactions = 2,	// EInteractionAnimation
		Combat = 3,			// A custom enum should be used by each hero class.
		Reactions = 4,		// EReactionAnimation
	}

	/// <summary>
	///  These mimic states in the Movement layer in the AnimatorController
	/// </summary>
	public enum EMoveAnimation
	{
		None = -1,

		Idle = 0,
		IdleLook = 1,
		CombatIdling = 2,
		StunnedIdling = 3,
		BatterdIdling = 4,
		Moving = 5,
		BatteredMoving = 6,
		GrabbingBlock = 7,
	}

	/// <summary>
	///  These mimic states in the Interaction layer in the AnimatorController
	/// </summary>
	public enum EInteractionAnimation
	{
		None = -1,

		UsingItem = 0,
		UsingPotion = 1,
		ThrowingItem = 2,
		PickingUpItem = 3,
	}

	/// <summary>
	///  These mimic states in the Reactions layer in the AnimatorController
	/// </summary>
	public enum EReactionAnimation
	{
		None = -1,

		TakingHit = 0,
		TakingCriticalHit = 1,
		Dying = 2,
	}


	private EMoveAnimation moveAnim = EMoveAnimation.None;
	private EInteractionAnimation interactionAnim = EInteractionAnimation.None;
	private EReactionAnimation reactionAnim = EReactionAnimation.None;
	private int combatAnim = -1;
	private ELayer layer = ELayer.None;

	public override void Initialise()
	{
		base.Initialise();

		animator.SetLayerWeight(1, 1.0f);
		animator.SetLayerWeight(2, 0.0f);
		animator.SetLayerWeight(3, 0.0f);
		animator.SetLayerWeight(3, 0.0f);

		layer = ELayer.Movement;
	}

	/// <summary>
	/// Plays a movement or idle animation.
	/// </summary>
	/// <param name="moveAnim"></param>
	/// <param name="movement"> movement param only affects Moving and GrabbingBlock. </param>
	public void PlayMovement(EMoveAnimation moveAnim)
	{
		if(layer == ELayer.Movement)
		{
			if(this.moveAnim != moveAnim)
			{
				SetActiveLayer(ELayer.Movement);

				// Reset float values for states that have blend trees
				if (this.moveAnim == EMoveAnimation.Moving)
				{
					animator.SetFloat("Movement", 0.0f);
				}
				else if (this.moveAnim == EMoveAnimation.GrabbingBlock)
				{
					animator.SetFloat("GrabMovement", 0.0f);
				}

				// Stop other layers.
				this.moveAnim = moveAnim;

				animator.SetInteger("MoveAnimation", (int)moveAnim);
				animator.SetTrigger("NewAnimation");
			}
		}
	}

	/// <summary>
	/// Sets movement float on the current animation state
	/// </summary>
	/// <param name="movement"></param>
	public void Move(float movement)
	{
		if (layer == ELayer.Movement)
		{
			// Set float values for states that have blend trees
			if (moveAnim == EMoveAnimation.Moving)
			{
				animator.SetFloat("Movement", movement);
			}
			else if (moveAnim == EMoveAnimation.GrabbingBlock)
			{
				animator.SetFloat("GrabMovement", movement);
			}
			else
			{
				Debug.LogError(moveAnim + " does not accept movement but you are trying to set it anyway.", this);
			}
		}
	}

	public void PlayCombatAction(int action)
	{
		SetActiveLayer(ELayer.Combat);
		animator.SetInteger("CombatAnimation", action);
		animator.SetTrigger("NewAnimation");
	}

	public void EndCombatAction()
	{
		//if (layer == ELayer.Combat)
		{
			SetActiveLayer(ELayer.Movement);
		}
	}

	public void SetActiveLayer(ELayer layer)
	{
		if (this.layer != layer)
		{
			animator.SetLayerWeight((int)this.layer, 0.0f);
			animator.SetLayerWeight((int)layer, 1.0f);

			this.layer = layer;
		}
	}

	public override void Update() 
    {
		if (layer == ELayer.Movement)
		{
			if (!animator.GetCurrentAnimatorStateInfo(0).IsName(this.moveAnim.ToString()))
			{
				animator.SetInteger("MoveAnimation", (int)moveAnim);
				animator.SetTrigger("New Trigger");
			}
		}
#if UNITY_EDITOR
        DebugKeys();
#endif
	}

#if UNITY_EDITOR
    void DebugKeys()
    {
		if (Input.GetKeyUp(KeyCode.Alpha1))
		{
			PlayMovement(EMoveAnimation.Idle);
		}
		else if (Input.GetKeyUp(KeyCode.Alpha2))
		{
			PlayMovement(EMoveAnimation.IdleLook);
		}
		else if (Input.GetKeyUp(KeyCode.Alpha3))
		{
			PlayMovement(EMoveAnimation.Moving);
		}
		else if (Input.GetKeyUp(KeyCode.Alpha4))
		{
			PlayMovement(EMoveAnimation.StunnedIdling);
		}
		else if (Input.GetKeyUp(KeyCode.Alpha5))
		{
			PlayMovement(EMoveAnimation.BatterdIdling);
		}
		else if (Input.GetKeyUp(KeyCode.Alpha6))
		{
			PlayMovement(EMoveAnimation.BatteredMoving);
		}
		else if (Input.GetKeyUp(KeyCode.Alpha7))
		{
			PlayMovement(EMoveAnimation.CombatIdling);
		}
		else if (Input.GetKeyUp(KeyCode.Alpha8))
		{
			PlayMovement(EMoveAnimation.GrabbingBlock);
		}
    }
#endif


	public void AnimEnd()
	{
		SetActiveLayer(ELayer.Movement);
	}

	public void OnCombatAnimationEnd()
	{
		SetActiveLayer(ELayer.Movement);
	}

	public void OnInteractionAnimationEnd()
	{
		SetActiveLayer(ELayer.Movement);
	}

	public void OnReactionAnimationEnd()
	{
		SetActiveLayer(ELayer.Movement);
	}
}
