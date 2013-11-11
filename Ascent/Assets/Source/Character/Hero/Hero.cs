using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Hero : Character 
{
    protected HeroController heroController;

	protected HeroEquipment equipment;

    public int teamId = 1;

    public HeroController HeroController
    {
        get { return heroController; }
    }

    #region Initialization

	public abstract void Initialise(AscentInput input, HeroSave saveData);

    public void SetColor(Color color)
    {
        SkinnedMeshRenderer[] renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer render in renderers)
        {
            render.material.color = color;
        }
    }

    /// <summary>
    /// Tells the hero to open the specified chest
    /// </summary>
    /// <param name="chest">The chest which needs to be opened</param>
    public void OpenChest(TreasureChest chest)
    {

    }

    #endregion
}
