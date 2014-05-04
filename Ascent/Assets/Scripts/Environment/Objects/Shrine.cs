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

	public Transform pool;

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
				{
					var players = Game.Singleton.Players;
					foreach (Player p in players)
					{
						if (!p.Hero.IsDead)
						{
							p.Hero.Stats.CurrentHealth = hero.Stats.MaxHealth;
							p.Hero.Stats.CurrentSpecial = p.Hero.Stats.MaxSpecial;
						}
					}
				}
				break;

			case ShrineType.manaSP:
				{
					var players = Game.Singleton.Players;
					foreach (Player p in players)
					{
						p.Hero.Stats.CurrentSpecial = p.Hero.Stats.MaxSpecial;
					}
				}
				break;
        }

		pool.localScale = new Vector3(0.96f, 0.63f, 0.96f);
		pool.position = new Vector3(0.0f, 1.3f, 0.0f);

        activated = true;
    }
}
