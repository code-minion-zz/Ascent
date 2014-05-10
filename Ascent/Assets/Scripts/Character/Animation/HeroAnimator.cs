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


    private bool newMoveAnim = false;

	private EMoveAnimation moveAnim = EMoveAnimation.CombatIdling;
    //private EInteractionAnimation interactionAnim = EInteractionAnimation.None;
    //private EReactionAnimation reactionAnim = EReactionAnimation.None;

	private ELayer layer = ELayer.None;
    private ELayer prevLayer = ELayer.None;

    private float blendTimeElapsed = 0.0f;
    private float blendTimeMax = 0.0f;

    private float reactionTimeElapsed = 0.0f;
    private float reactionTimeMax = 0.0f;

    public MeleeWeaponTrail weaponTrail;

	public override void Initialise()
	{
		base.Initialise();

		animator.SetLayerWeight(1, 1.0f);
		animator.SetLayerWeight(2, 0.0f);
		animator.SetLayerWeight(3, 0.0f);
		animator.SetLayerWeight(4, 0.0f);
		animator.SetLayerWeight(5, 0.0f);

		layer = ELayer.Movement;

        animator.SetLayerWeight(1, 1.0f);
        weaponTrail.enabled = false;
	}

    public void OnEnable()
    {
        if (animator != null)
        {
            animator.SetLayerWeight(1, 1.0f);
            weaponTrail.enabled = false;
        }
    }

	public void Reset()
	{
		animator.SetLayerWeight(1, 1.0f);
		animator.SetLayerWeight(2, 0.0f);
		animator.SetLayerWeight(3, 0.0f);
		animator.SetLayerWeight(3, 0.0f);
		animator.SetLayerWeight(4, 0.0f);
		animator.SetLayerWeight(5, 0.0f);

		layer = ELayer.Movement;
		
		playingDeath = false;
		dead = false;
		animator.SetBool("Death", false);
		animator.SetBool("DeathIdle", false);
		PlayMovement(EMoveAnimation.IdleLook);
		
	}

	/// <summary>
	/// Plays a movement or idle animation.
	/// </summary>
	/// <param name="moveAnim"></param>
	/// <param name="movement"> movement param only affects Moving and GrabbingBlock. </param>
	public void PlayMovement(EMoveAnimation moveAnim)
	{
		if (playingDeath || dead)
			return;

		//if(layer == ELayer.Movement)
		{
            if (this.moveAnim != moveAnim)
            {
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
                animator.SetBool("NewAnimation", true);

                newMoveAnim = true;
            }
		}
	}

	/// <summary>
	/// Sets movement float on the current animation state
	/// </summary>
	/// <param name="movement"></param>
	public void Move(float movement)
	{
		if (playingDeath || dead)
			return;

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

	public void PlayCombatAction(int action, string animName)
	{
		if (playingDeath || dead)
			return;

        weaponTrail.enabled = false;
        animator.SetInteger("MoveAnimation", 2);

        SetActiveLayerBlend(ELayer.Combat, 0.2f);

        animator.SetInteger("CombatAnimation", action);
        animator.SetBool("NewAnimation", true);

        newMoveAnim = true;


	}

    public bool playingDeath;
	public bool dead;
	public float deathTimer;
	public void PlayDeath()
	{
		if (!playingDeath && !dead)
		{
			playingDeath = true;
			animator.SetLayerWeight(1, 0.0f);
			animator.SetLayerWeight(2, 0.0f);
			animator.SetLayerWeight(3, 0.0f);
			animator.SetLayerWeight(3, 0.0f);
			animator.SetLayerWeight(4, 0.0f);
			animator.SetLayerWeight(5, 1.0f);
			animator.SetBool("Death", true);
		}
	}

    public void PlayReactionAction(EReactionAnimation anim, float time)
    {
		if (playingDeath || dead)
            return;

        playingDeath = anim == EReactionAnimation.Dying ? true : false;
        if (playingDeath)
        {
            animator.SetLayerWeight((int)ELayer.Reactions, 1.0f);
        }
        else
        {

            animator.SetLayerWeight((int)ELayer.Reactions, 0.5f);
        }

        reactionTimeMax = time;
        reactionTimeElapsed = 0.0f;

        animator.SetInteger("ReactionAnimation", (int)anim);
        animator.SetBool("NewAnimation", true);

        newMoveAnim = true;
    }

    public void PlayInteractionAction(EInteractionAnimation anim)
    {
		if (playingDeath || dead)
			return;

        SetActiveLayer(ELayer.Interactions);

        animator.SetInteger("InteractionAnimation", (int)anim);
        animator.SetBool("NewAnimation", true);

        newMoveAnim = true;
    }

    public void SetActiveLayer(ELayer layer)
    {
		if (playingDeath || dead)
			return;

        if (this.layer != layer)
        {
            animator.SetLayerWeight((int)this.layer, 0.0f);
            animator.SetLayerWeight((int)layer, 1.0f);

            this.layer = layer;
        }
    }

	public void SetActiveLayer(ELayer layer, float newLayerWeight)
	{
		if (playingDeath || dead)
			return;

		if (this.layer != layer)
		{
            newLayerWeight = Mathf.Min(Mathf.Abs(newLayerWeight), 0.0f);
            newLayerWeight = Mathf.Max(newLayerWeight, 1.0f);

            animator.SetLayerWeight((int)this.layer, 1.0f - newLayerWeight);
            animator.SetLayerWeight((int)layer, newLayerWeight);

			this.layer = layer;
		}
	}

    public void SetActiveLayerBlend(ELayer layer, float time)
    {
		if (playingDeath || dead)
			return;

        if (this.layer != layer)
        {
            blendTimeMax = time;
            blendTimeElapsed = 0.0f;

            prevLayer = this.layer;
            this.layer = layer;
        }
    }


	public override void Update() 
    {
		if(dead)
		{
			 animator.SetBool("Death", true);
			 return;
		}
		else if (playingDeath)
		{
			deathTimer += Time.deltaTime;
			if (deathTimer >= 1.167f)
			{
				playingDeath = false;
				dead = true;
				animator.SetBool("DeathIdle", true);
			}

			return;
		}

        int iLayer = (int)layer;
        if(iLayer >= 1 && iLayer < animator.layerCount)
        {
            if (newMoveAnim && animator.IsInTransition(iLayer))
            {
                newMoveAnim = false;
                animator.SetBool("NewAnimation", false);
            }
        }

        if (playingDeath)
        {
            return;
        }

        if(blendTimeElapsed < blendTimeMax)
        {
            blendTimeElapsed += Time.deltaTime;
            if(blendTimeElapsed > blendTimeMax)
            {
                blendTimeElapsed = blendTimeMax;
            }

            animator.SetLayerWeight((int)prevLayer, (blendTimeMax - blendTimeElapsed )/ blendTimeMax);
            animator.SetLayerWeight((int)layer, blendTimeElapsed / blendTimeMax);
        }

        if (reactionTimeElapsed < reactionTimeMax)
        {
            reactionTimeElapsed += Time.deltaTime;
            if (reactionTimeElapsed > reactionTimeMax)
            {
                reactionTimeElapsed = reactionTimeMax;
                //animator.SetLayerWeight((int)layer, 1.0f);
                animator.SetLayerWeight((int)ELayer.Reactions, 0.0f);
                animator.SetInteger("ReactionAnimation", (int)EReactionAnimation.None);
            }
        }

#if UNITY_EDITOR
        //DebugKeys();
#endif
	}

#if UNITY_EDITOR
    void DebugKeys()
    {
        // Move layer tests
		if (Input.GetKeyUp(KeyCode.Alpha1)) PlayMovement((EMoveAnimation)0);
		else if (Input.GetKeyUp(KeyCode.Alpha2)) PlayMovement((EMoveAnimation)1);
		else if (Input.GetKeyUp(KeyCode.Alpha3)) PlayMovement((EMoveAnimation)2);
		else if (Input.GetKeyUp(KeyCode.Alpha4)) PlayMovement((EMoveAnimation)3);
		else if (Input.GetKeyUp(KeyCode.Alpha5)) PlayMovement((EMoveAnimation)4);	
		else if (Input.GetKeyUp(KeyCode.Alpha6)) PlayMovement((EMoveAnimation)5);
		else if (Input.GetKeyUp(KeyCode.Alpha7)) PlayMovement((EMoveAnimation)6);
		else if (Input.GetKeyUp(KeyCode.Alpha8)) PlayMovement((EMoveAnimation)7);
    }
#endif


	public void AnimEnd()
	{
		SetActiveLayer(ELayer.Movement);
	}

    public void CombatAnimationEnd()
    {
        SetActiveLayer(ELayer.Movement);
        weaponTrail.enabled = false;
        PlayMovement(EMoveAnimation.CombatIdling);
        //SetActiveLayerBlend(ELayer.Movement, 0.5f);
       // animator.SetInteger("CombatAnimation", -1);

        //EMoveAnimation curMove = moveAnim;
        //moveAnim = EMoveAnimation.None;
        //PlayMovement(curMove);
    }

	public void OnCombatAnimationEnd()
	{
        //animator.SetInteger("CombatAnimation", -1);
        //SetActiveLayer(ELayer.Movement);
        ////SetActiveLayerBlend(ELayer.Movement, 0.1f);

        //EMoveAnimation curMove = moveAnim;
        //moveAnim = EMoveAnimation.None;
        //PlayMovement(curMove);
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
