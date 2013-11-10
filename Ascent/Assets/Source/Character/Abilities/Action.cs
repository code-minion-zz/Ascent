using UnityEngine;
using System.Collections;

public abstract class Action : IAction
{
    protected Character owner;
    public Character Owner
    {
        get { return owner; }
    }

    protected string name;
    public string Name
    {
        get { return name; }
    }

    public virtual void Initialise(Character owner)
    {
       this.owner = owner;
       this.name = this.GetType().ToString();

       Debug.Log(this.name);
    }

    public abstract void StartAbility();
    public abstract void UpdateAbility();
    public abstract void EndAbility();
}
