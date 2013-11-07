using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Hero : Character 
{
    protected HeroController heroController;

	protected HeroEquipment equipment;

    public int teamId = 1;

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

    #endregion
}
