using UnityEngine;
using System.Collections;

public abstract class ConsumableItem : Item
{
	public enum EConsumableType
	{
		INVALID = -1,

		Health,
		Special,

		Key,
		Bomb,

		MAX
	}


	protected int charges;
	public int Charges
	{
		get{ return charges; }
		set { charges = value; }
	}

    protected float cooldown = 0.0f;
    public float Cooldown
    {
        get { return cooldown; }
    }

	protected float cooldownMax = 2.0f;
	public float CooldownMax
	{
		set { cooldownMax = value; }
	}

	public bool perishable = true;

	protected bool HasCooldownAndCharges
	{
		get { return charges > 0 && cooldown == 0.0f; }
	}

	public void Process()
	{
		if (cooldown > 0.0f)
		{
			cooldown -= Time.deltaTime;

			if (cooldown < 0.0f)
			{
				cooldown = 0.0f;
			}
		}
	}

	public void UseItem(Hero user)
    {
        if (HasCooldownAndCharges && CanUse(user))
		{
			Consume(user);
			cooldown = cooldownMax;

			if (perishable)
			{
				charges -= 1;
			}
		}
    }

    protected abstract bool CanUse(Hero user);
    protected abstract void Consume(Hero user);
}
