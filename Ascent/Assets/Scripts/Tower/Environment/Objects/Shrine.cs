using UnityEngine;
using System.Collections;

public class Shrine : Interactable
{
    private bool activated;

    public bool Activated
    {
        get { return activated; }
        set { activated = value; }
    }

    public void Activate(Hero hero)
    {
        hero.Stats.CurrentHealth = hero.Stats.MaxHealth;
        activated = true;
    }
}
