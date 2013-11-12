using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Enemy : Character
{
    #region Enums

    public enum STATE
    {
        IDLE,
        SEEK,
        ATTACKING,
        WAIT,
        DEAD,
        HIT,
    }

    #endregion

    #region Fields

    public int teamId = 2;
    public STATE state = STATE.IDLE;
    public Transform hitBoxPrefab; // hitboxes represent projectiles

    private Player targetPlayer;
    //private float waiting = 0.0f;
    private Vector3 originalScale;
    private List<Transform> activeHitBoxes; // active melee attacks

    // Expose these to AI Rig

    protected RAIN.Core.AIRig ai;

    #endregion

    #region Initialization

	public abstract void Initialise();

    public override void Awake()
    {
        // To be derived
    }

	public override void Start () 
    {
        // To be derived
	}

    #endregion

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void InterruptAbility()
    {
        if (activeAbility != null)
        {
            activeAbility.EndAbility();
            activeAbility = null;

            
            ai.AI.WorkingMemory.SetItem<bool>("acting", false);
        }
    }

    public override void StopAbility()
    {
        if (activeAbility != null)
        {
            activeAbility.EndAbility();
            activeAbility = null;

            ai.AI.WorkingMemory.SetItem<bool>("acting", false);
        }
    }
}
