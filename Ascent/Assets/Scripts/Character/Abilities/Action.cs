using UnityEngine;
using System.Collections;

public abstract class Action : IAction
{
    protected float animationLength = 0.0f;
    protected float coolDownTime = 0.0f;
    protected float currentTime = 0.0f;
    protected string animationTrigger;

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

    public float CurrentTime
    {
        get { return currentTime; }
    }

    public float Length
    {
        get { return animationLength; }
    }

    public virtual void Initialise(Character owner)
    {
       this.owner = owner;
       this.name = this.GetType().ToString();
    }

    public virtual void StartAbility()
    {
        currentTime = 0.0f;
        owner.Animator.PlayAnimation(animationTrigger);
        owner.StartCoroutine(UpdateAction());
    }

    public abstract void UpdateAbility();

    public virtual void EndAbility()
    {
        owner.Animator.StopAnimation(animationTrigger);
        currentTime = 0.0f;
    }

    public virtual IEnumerator UpdateAction()
    {
        currentTime += Time.deltaTime;
        UpdateAbility();
        yield return new WaitForSeconds(animationLength);
        owner.StopAbility();
    }
}
