using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuButtonFunctions : MonoBehaviour 
{
//    public GameObject defaultSelection;
//    InputDevice[] devices;
	
//    void OnEnable()
//    {
//        UICamera.fallThrough = this.gameObject;
//    }
	
//    void Awake()
//    {
//    }
	
//    public void GoToLevel()
//    {
//        if (enabled)
//        {
//            Game.Singleton.LoadLevel(Game.EGameState.HeroSelect);
//        }
//    }	
	
//    void Update()
//    {
//        foreach (InputDevice id in InputManager.Devices)
//        {
//            if(id.Start.IsPressed)
//            {
//                GoToLevel();
//                return;
//            }
//        }
////		if (UICamera.selectedObject == null)
////		{
////			UICamera.selectedObject = this.gameObject;
////		}
//    }
	
//    void OnKey (KeyCode key)
//    {
////		if (UICamera.selectedObject == this.gameObject)
////		{
////			UICamera.selectedObject = defaultSelection;
////			UICamera.Notify(defaultSelection,"OnHover",false);
////		}
//    }



    public void OnPlayerOnePressed()
    {
        if (InputManager.Devices.Count >= 1)
        {
            Game.Singleton.Tower.numberOfPlayers = 1;
            Game.Singleton.Tower.currentFloorNumber = 1;
            Game.Singleton.Tower.keys = 0;
            Game.Singleton.Tower.lives = 1;
            Game.Singleton.LoadLevel(Game.EGameState.TowerPlayer1);
        }
    }

    public void OnPlayerTwoPressed()
    {
        if (InputManager.Devices.Count >= 2)
        {
            Game.Singleton.Tower.numberOfPlayers = 2;
            Game.Singleton.Tower.currentFloorNumber = 1;
            Game.Singleton.Tower.keys = 0;
            Game.Singleton.Tower.lives = 1;
            Game.Singleton.LoadLevel(Game.EGameState.TowerPlayer2);
        }
    }

    public void OnPlayerThreePressed()
    {
        if (InputManager.Devices.Count >= 3)
        {
            Game.Singleton.Tower.numberOfPlayers = 3;
            Game.Singleton.Tower.currentFloorNumber = 1;
            Game.Singleton.Tower.keys = 0;
            Game.Singleton.Tower.lives = 1;
            Game.Singleton.LoadLevel(Game.EGameState.TowerPlayer3);
        }
    }

    public void Exit()
    {
        Debug.Log("Exit");
    }
}
