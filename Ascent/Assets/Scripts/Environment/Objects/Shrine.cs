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
	UITweener[] animations;

	public Transform pool;

    public bool Activated
    {
        get { return activated; }
        set { activated = value; }
    }

    public override void Start()
    {
        base.Start();
		animations = new UITweener[2];
		render = this.gameObject.transform.FindChild("Model").FindChild("Quad").GetComponent<Renderer>();
		animations[0] = render.GetComponent<TweenPosition>();
		animations[1] = render.GetComponent<TweenScale>();

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

		int i;
		for (i = 0; i < animations.Length; ++i)
		{
			animations[i].enabled = true;
		}

        activated = true;
    }
}
