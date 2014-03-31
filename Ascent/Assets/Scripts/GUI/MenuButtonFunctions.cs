using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuButtonFunctions : MonoBehaviour 
{
    public UIButton[] playButtons;

    public void Update()
    {
        int playerCount = 0;
        for (int i = 0; i < InputManager.Devices.Count; ++i)
        {
            if (i > 2)
            {
                break;
            }

            playButtons[i].enabled = true;
            ++playerCount;
        }
        
        if (playerCount == 3)
        {
            playButtons[0].GetComponent<UIButtonKeys>().selectOnUp = playButtons[3].GetComponent<UIButtonKeys>();
            playButtons[0].GetComponent<UIButtonKeys>().selectOnDown = playButtons[1].GetComponent<UIButtonKeys>();

            playButtons[1].GetComponent<UIButtonKeys>().selectOnUp = playButtons[0].GetComponent<UIButtonKeys>();
            playButtons[1].GetComponent<UIButtonKeys>().selectOnDown = playButtons[2].GetComponent<UIButtonKeys>();

            playButtons[2].GetComponent<UIButtonKeys>().selectOnUp = playButtons[1].GetComponent<UIButtonKeys>();
            playButtons[2].GetComponent<UIButtonKeys>().selectOnDown = playButtons[3].GetComponent<UIButtonKeys>();

            playButtons[3].GetComponent<UIButtonKeys>().selectOnUp = playButtons[2].GetComponent<UIButtonKeys>();
            playButtons[3].GetComponent<UIButtonKeys>().selectOnDown = playButtons[0].GetComponent<UIButtonKeys>();
        }
        else if (playerCount == 2)
        {
            playButtons[0].GetComponent<UIButtonKeys>().selectOnUp = playButtons[3].GetComponent<UIButtonKeys>();
            playButtons[0].GetComponent<UIButtonKeys>().selectOnDown = playButtons[1].GetComponent<UIButtonKeys>();

            playButtons[1].GetComponent<UIButtonKeys>().selectOnUp = playButtons[0].GetComponent<UIButtonKeys>();
            playButtons[1].GetComponent<UIButtonKeys>().selectOnDown = playButtons[3].GetComponent<UIButtonKeys>();

            playButtons[3].GetComponent<UIButtonKeys>().selectOnUp = playButtons[1].GetComponent<UIButtonKeys>();
            playButtons[3].GetComponent<UIButtonKeys>().selectOnDown = playButtons[0].GetComponent<UIButtonKeys>();
        }
        else if (playerCount <= 1)
        {
            playButtons[0].GetComponent<UIButtonKeys>().selectOnUp = playButtons[3].GetComponent<UIButtonKeys>();
            playButtons[0].GetComponent<UIButtonKeys>().selectOnDown = playButtons[3].GetComponent<UIButtonKeys>();

            playButtons[3].GetComponent<UIButtonKeys>().selectOnUp = playButtons[0].GetComponent<UIButtonKeys>();
            playButtons[3].GetComponent<UIButtonKeys>().selectOnDown = playButtons[0].GetComponent<UIButtonKeys>();
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
