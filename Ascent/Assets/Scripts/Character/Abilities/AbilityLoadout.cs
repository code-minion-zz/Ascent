using UnityEngine;
using System.Collections;

public class AbilityLoadout  
{
    protected Ability[] abilities;
    public Ability[] AbilityBinds
    {
        get { return abilities; }
        set { abilities = value; }
    }

    protected Character owner;
    protected CharacterStats stats;
    protected CharacterMotor motor;
    private Ability activeAbility;

    public bool IsAbilityActive
    {
        get
        {
            if (activeAbility != null)
            {
                return true;
            }
            return false;
        }
    }

    public bool CanInterruptActiveAbility
    {
        get
        {
            if (activeAbility != null)
            {
                return activeAbility.CanBeInterrupted;
            }
            return true;
        }
    }

    public void Initialise(Character owner)
    {
        this.owner = owner;
        stats = owner.Stats;
        motor = owner.Motor;
    }

    public void Process()
    {
        UpdateActiveAbility();

        // Update abilities that require cooldown
        foreach (Ability ability in abilities)
        {
            if (ability.IsOnCooldown == true)
            {
                ability.UpdateCooldown();
            }
        }
    }

    private void UpdateActiveAbility()
    {
        if (activeAbility != null)
        {
            activeAbility.Update();
        }
    }

    public void SetSize(int size)
    {
        abilities = new Ability[size];
    }

    /// <summary>
    /// Heroes use this version and the loadout is a fixed number
    /// </summary>
    /// <param name="ability"></param>
    /// <param name="slot"></param>
    public void SetAbility(Ability ability, int slot)
    {
        if (ability != null)
        {
            if(slot >= abilities.Length)
            {
                Debug.LogError("The loadout needs to be larger to fit this ability.");
            }

            ability.Initialise(owner);
            abilities[slot] = ability;
        }
        else
        {
            Debug.LogError("The size has not been set.");
        }
    }

    public int GetAbilityID(Ability ability)
    {
        int i = 0;
        for (; i < abilities.Length; ++i)
        {
            if (abilities[i] == ability)
            {
                return i;
            }
        }
        return -1;
    }

    public Ability GetAbility(string ability)
    {
        for (int i = 0; i < abilities.Length; ++i)
        {
            if (abilities[i].ToString() == ability.ToString())
            {
                return abilities[i];
            }
        }

        Debug.LogError("Could not find and return ability: " + ability);

        return null;
    }

    public virtual bool UseAbility(int abilityID)
    {
        // If there no active ability then we can use a new one
        bool canUse = (activeAbility == null);

        // Or if there is an active one we can use a new one if the old one can be interupted
        bool interupt = false;
        if (!canUse)
        {
            interupt = activeAbility.CanBeInterrupted;
            canUse = interupt;
        }

        if (canUse)
        {
            Ability ability = abilities[abilityID];

            if (ability.IsOnCooldown)
            {
                FloorHUDManager.Singleton.TextDriver.SpawnDamageText(owner.gameObject, "Cooling down", Color.white);
            }
            else if ((stats.CurrentSpecial - ability.SpecialCost) < 0)
            {
                FloorHUDManager.Singleton.TextDriver.SpawnDamageText(owner.gameObject, "Insufficient SP", Color.white);
            }

            // Make sure the cooldown is off otherwise we cannot use the ability
            if (ability != null && ability.IsOnCooldown == false && (stats.CurrentSpecial - ability.SpecialCost) >= 0)
            {

                if (interupt)
                {
                    StopAbility();
                }

                // TODO: Check if we are not in a state that denies abilities to perform.
                ability.StartAbility();
                activeAbility = ability;

                stats.CurrentSpecial -= ability.SpecialCost;

                motor.StopMotion();
                motor.IsHaltingMovementToPerformAction = false;

                return true;
            }
        }

        return false;
    }

    public bool CanCastAbility(int abilityID)
    {
        // If there no active ability then we can use a new one
        bool canUse = (activeAbility == null);

        // Or if there is an active one we can use a new one if the old one can be interupted
        bool interupt = false;
        if (!canUse)
        {
            interupt = activeAbility.CanBeInterrupted;
            canUse = interupt;
        }

        return canUse;
    }

    public virtual bool UseCastAbility(int abilityID)
    {
        Ability ability = abilities[abilityID];
        // Make sure the cooldown is off otherwise we cannot use the ability

        if (ability.IsOnCooldown)
        {
            FloorHUDManager.Singleton.TextDriver.SpawnDamageText(owner.gameObject, "Cooling down", Color.white);
        }
        else if ((stats.CurrentSpecial - ability.SpecialCost) < 0)
        {
            FloorHUDManager.Singleton.TextDriver.SpawnDamageText(owner.gameObject, "Insufficient SP", Color.white);
        }

        if (ability != null && ability.IsOnCooldown == false && (stats.CurrentSpecial - ability.SpecialCost) >= 0)
        {
            if (activeAbility != null)
            {
                if (activeAbility.CanBeInterrupted)
                {
                    StopAbility();
                }
            }

            ability.StartCast();
            //activeAbility = ability;

            motor.StopMotion();
            motor.IsHaltingMovementToPerformAction = false;

            return true;
        }

        return false;
    }

    public virtual void StopAbility()
    {
        if (activeAbility != null)
        {
            activeAbility.EndAbility();
            activeAbility = null;

            motor.IsHaltingMovementToPerformAction = true;
        }
    }

    public void Refresh()
    {
        foreach (Ability a in abilities)
        {
            a.RefreshCooldown();
        }
    }

#if UNITY_EDITOR
    public void DebugDraw()
    {
        if (activeAbility != null)
        {
            activeAbility.DebugDraw();
        }
    }
#endif
}
