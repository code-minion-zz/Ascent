using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Hero : Character 
{
    protected HeroController heroController;
	protected HeroEquipment equipment;

    public HeroController HeroController
    {
        get { return heroController; }
    }

	public abstract void Initialise(InputDevice input, HeroSaveData saveData);

    public void SetColor(Color color)
    {
        SkinnedMeshRenderer[] renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer render in renderers)
        {
            render.material.color = color;
        }
    }

    public override void Respawn(Vector3 position)
    {
        base.Respawn(position);

        // Reset the health
        characterStatistics.ResetHealth();
    }

    /// <summary>
    /// Specific on death event functionality for heroes
    /// </summary>
    public override void OnDeath()
    {
        base.OnDeath();
    }

    /// <summary>
    /// Tells the hero to open the specified chest
    /// </summary>
    /// <param name="chest">The chest which needs to be opened</param>
    public void OpenChest(TreasureChest chest)
    {

    }

    void OnControllerColliderHit(ControllerColliderHit collision)
    {
        if (collision.transform.tag == "Door")
        {
            Debug.Log("Open Door");
            Door door = collision.transform.GetComponent<Door>();
            door.IsOpen = true;
        }
        else if (collision.transform.tag == "Loot")
        {
            CoinSack coins = collision.transform.GetComponent<CoinSack>();
            coins.transform.gameObject.SetActive(false);
        }
    }
}
