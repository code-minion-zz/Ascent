using UnityEngine;
using System.Collections;
using InControl;

public class PlayerController : MonoBehaviour 
{
    private InputHandler inputHandler;
    private InputDevice inputDevice;
    private Player player;
    private int playerID;
    private Vector3 direction = Vector3.zero;
    private float gravity = 20.0f;

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

        // Jumping skill
        if (inputDevice.Action1.IsPressed)
        {
            player.Skill(0);
        }
        // Attacking skill
        else if (inputDevice.Action2.IsPressed)
        {
            player.Skill(1);
        }

        // Handle payer movements
        // First we get access to the physics controller
        CharacterController controller = GetComponent<CharacterController>();

        float x = inputDevice.LeftStickX.Value;
        float z = inputDevice.LeftStickY.Value;

        // Player should not move while jumping or attacking.
        if (player.attacking || player.jumping)
        {
            x = 0.0f;
            z = 0.0f;
        }

        if (controller.isGrounded)
        {
            direction = new Vector3(x, 0, z).normalized;
            //direction = transform.TransformDirection(direction);
            direction *= player.movementSpeed;

           if (inputDevice.Action1.IsPressed)
               direction.y = player.jumpSpeed * 1.5f;
        }

        // Apply gravity to our direction.
        direction.y -= gravity * Time.deltaTime;
        // Move the controller. This step is necessary as the controller will not do any
        // collisions if this call is not made.
        controller.Move(direction * Time.deltaTime);
        // Move the character to face the direction we will move this controller.
        player.Move(direction);
	}
}
