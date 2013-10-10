using UnityEngine;
using System.Collections;
using InControl;

public class PlayerController : MonoBehaviour 
{
    private InputHandler inputHandler;
    private InputDevice inputDevice;
    private Player player;
    private int playerID;

	// Use this for initialization
	void Start () 
    {
        player = GetComponent<Player>();
        playerID = player.PlayerID;
        inputHandler = Game.Singleton.InputHandler;
        inputDevice = inputHandler.GetDevice(playerID);

        if (inputDevice == null)
        {
            Debug.Log("Player " + playerID + "'s inputDevice does not exist");
            return;
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (inputDevice == null)
        {
            Debug.Log("Player " + playerID + "'s inputDevice does not exist");
            return;
        }

        // Update the transform by the movement
        if (inputDevice.Action1.IsPressed)
        {
            //	Debug.Log("Action One: " + playerId);
            player.Skill(0);
            return;
        }
        // Update the transform by the movement
        if (inputDevice.Action2.IsPressed)
        {
            //Debug.Log("Action Two: " + playerId);
            player.Skill(1);
            return;
        }

        float x = inputDevice.LeftStickX.Value * Time.deltaTime * player.movementSpeed;
        float z = inputDevice.LeftStickY.Value * Time.deltaTime * player.movementSpeed;

        Vector3 direction = Vector3.Normalize(new Vector3(x, 0.0f, z));

        player.Move(direction);

        //if (player.jumping)
        //{
        //    Physics.Raycast(new Ray(transform.position, -transform.up), 5.0f);
        //    Debug.DrawRay(transform.position, -transform.up, Color.red);
            
        //}

        //if (transform.position.y < 0.0f)
        //{
        //    player.jumping = false;
        //}
	}
}
