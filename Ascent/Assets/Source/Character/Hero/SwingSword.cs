using UnityEngine;
using System.Collections;

public class SwingSword : IAbility
{
    Character owner;

    public void Initialise(Character owner)
    {
        this.owner = owner;
    }

    public void StartAbility()
    {
        owner.Animator.PlayAnimation("SwingAttack");
        owner.Weapon.SetAttackProperties(10, Character.EDamageType.Physical);
        owner.Weapon.EnableCollision = true;
    }

    public void UpdateAbility()
    {

    }

    public void EndAbility()
    {
        owner.Weapon.EnableCollision = false;
    }
}
