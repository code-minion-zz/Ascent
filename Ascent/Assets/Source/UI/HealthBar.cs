using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour 
{
    public float barLength = 100.0f;
    private float healthRatio = 1.0f;
    private Vector3 guiPosition;
    private Texture2D healthTexture;
    private HealthStat health;
    private Game game;

    void Awake()
    {

    }

    void Start()
    {
        // Get the game.
        game = Game.Singleton;

        // Load the resources for the texture.
        healthTexture = Resources.Load("Actors/Textures/Chrysanthemum") as Texture2D;

        // Obtain the character and get the stats health.
        Character character = GetComponent<Character>();
        health = character.CharacterStats.Health;
    }
	
	void Update () 
    {
        healthRatio = health.Min / health.Max;
        barLength = healthRatio * 100.0f;

        Vector3 vTargetPositon = transform.position;
        vTargetPositon.y += 2.0f;

        guiPosition = game.MainCamera.WorldToViewportPoint(vTargetPositon);

        guiPosition.x = (Screen.width * guiPosition.x) - 50.0f;
        guiPosition.y = Screen.height - (guiPosition.y * Screen.height);
	}

    void OnGUI()
    {
        if (health.Min > 0)
        {
            if (healthTexture != null)
                GUI.DrawTexture(new Rect(guiPosition.x, guiPosition.y, barLength, 10.0f), healthTexture);
        }
    }
}
