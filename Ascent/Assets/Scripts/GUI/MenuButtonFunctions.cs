using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuButtonFunctions : MonoBehaviour 
{
    public UIButton[] Buttons;

    public void Update()
	{
		if (UICamera.selectedObject == null || !NGUITools.GetActive(UICamera.selectedObject))
		{
			UICamera.currentScheme = UICamera.ControlScheme.Controller;
			UICamera.selectedObject = Buttons[0].gameObject;
		}

        int playerCount = 0;
        for (int i = 0; i < InputManager.Devices.Count; ++i)
        {
            if (i > 2)
            {
                break;
            }

            Buttons[i].enabled = true;
            ++playerCount;
        }
        
        if (playerCount == 3)
        {
            Buttons[0].GetComponent<UIButtonKeys>().selectOnUp = Buttons[3].GetComponent<UIButtonKeys>();
            Buttons[0].GetComponent<UIButtonKeys>().selectOnDown = Buttons[1].GetComponent<UIButtonKeys>();

            Buttons[1].GetComponent<UIButtonKeys>().selectOnUp = Buttons[0].GetComponent<UIButtonKeys>();
            Buttons[1].GetComponent<UIButtonKeys>().selectOnDown = Buttons[2].GetComponent<UIButtonKeys>();

            Buttons[2].GetComponent<UIButtonKeys>().selectOnUp = Buttons[1].GetComponent<UIButtonKeys>();
            Buttons[2].GetComponent<UIButtonKeys>().selectOnDown = Buttons[3].GetComponent<UIButtonKeys>();

            Buttons[3].GetComponent<UIButtonKeys>().selectOnUp = Buttons[2].GetComponent<UIButtonKeys>();
            Buttons[3].GetComponent<UIButtonKeys>().selectOnDown = Buttons[0].GetComponent<UIButtonKeys>();
        }
        else if (playerCount == 2)
        {
            Buttons[0].GetComponent<UIButtonKeys>().selectOnUp = Buttons[3].GetComponent<UIButtonKeys>();
            Buttons[0].GetComponent<UIButtonKeys>().selectOnDown = Buttons[1].GetComponent<UIButtonKeys>();

            Buttons[1].GetComponent<UIButtonKeys>().selectOnUp = Buttons[0].GetComponent<UIButtonKeys>();
            Buttons[1].GetComponent<UIButtonKeys>().selectOnDown = Buttons[3].GetComponent<UIButtonKeys>();

            Buttons[3].GetComponent<UIButtonKeys>().selectOnUp = Buttons[1].GetComponent<UIButtonKeys>();
            Buttons[3].GetComponent<UIButtonKeys>().selectOnDown = Buttons[0].GetComponent<UIButtonKeys>();
        }
        else if (playerCount <= 1)
        {
            Buttons[0].GetComponent<UIButtonKeys>().selectOnUp = Buttons[3].GetComponent<UIButtonKeys>();
            Buttons[0].GetComponent<UIButtonKeys>().selectOnDown = Buttons[3].GetComponent<UIButtonKeys>();
			
			Buttons[3].GetComponent<UIButtonKeys>().selectOnUp = Buttons[0].GetComponent<UIButtonKeys>();
            Buttons[3].GetComponent<UIButtonKeys>().selectOnDown = Buttons[0].GetComponent<UIButtonKeys>();
        }
    }

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
