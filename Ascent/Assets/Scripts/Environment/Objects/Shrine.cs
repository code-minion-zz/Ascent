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

	public Renderer[] thingsToOutline;

	public Transform pool;

	public Light shrineLight;

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
                //render.material.color = Color.red;
                break;

            case ShrineType.manaSP:
                //render.material.color = Color.blue;
                break;
        }
    }

	public void Update()
	{
		if(activated)
			shrineLight.intensity = Mathf.Lerp(2.5f, 0.0f, ((TweenPosition)animations[0]).mFactor);
	}

    public void Activate(Hero hero)
    {
		SoundManager.PlaySound(AudioClipType.drink, transform.position, 1f);
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

		foreach(UITweener tween in animations)
		{
			tween.enabled = true;
		}

        activated = true;
    }

	public override void EnableHighlight(Color color)
	{
		foreach (Renderer render in thingsToOutline)
		{
			foreach (Material mat in render.materials)
			{
				mat.shader = Shader.Find("Outlined/Diffuse");
				mat.SetColor("_OutlineColor", color);
				mat.SetFloat("_Outline", 0.003f);
			}
		}
	}

	public override void StopHighlight()
	{
		foreach (Renderer render in thingsToOutline)
		{
			foreach (Material mat in render.materials)
			{
				mat.shader = Shader.Find("Diffuse");
			}
		}
	}
}
