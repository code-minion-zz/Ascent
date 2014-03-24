using UnityEngine;
using System.Collections;

public enum ShrineType
{
    health,
    manaSP
}

public class Shrine : Interactable
{
    private bool activated;
    private Renderer render;
    public ShrineType refilType;

    public bool Activated
    {
        get { return activated; }
        set { activated = value; }
    }

    public override void Start()
    {
        base.Start();
        render = this.gameObject.transform.FindChild("Quad").GetComponent<Renderer>();

        switch (refilType)
        {
            case ShrineType.health:
                render.material.color = Color.red;
                break;

            case ShrineType.manaSP:
                render.material.color = Color.blue;
                break;
        }
    }

    public void Activate(Hero hero)
    {
        switch (refilType)
        {
            case ShrineType.health:
                hero.Stats.CurrentHealth = hero.Stats.MaxHealth;
                break;

            case ShrineType.manaSP:
                hero.Stats.CurrentSpecial = hero.Stats.MaxSpecial;
                break;
        }

        activated = true;
    }
}
