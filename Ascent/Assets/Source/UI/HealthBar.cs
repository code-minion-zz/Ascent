using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour 
{
    private Texture2D healthTexture;

    public float barLength = 100.0f;
    private float healthRatio = 1.0f;

    private Vector3 guiPosition;

    private HealthStat health;

    void Start()
    {
        healthTexture = Resources.Load("Actors/Textures/Chrysanthemum") as Texture2D;
        CharacterStatistics stats = gameObject.GetComponent<CharacterStatistics>();
        health = stats.GetComponent<HealthStat>();
    }
	
	void Update () 
    {
        healthRatio = health.Min / health.Max;
        barLength = healthRatio * 100.0f;

        Vector3 vTargetPositon = transform.position;
        vTargetPositon.y += 2.0f;

        Game game = GameObject.Find("Game").GetComponent<Game>();

        guiPosition = game.MainCamera.WorldToViewportPoint(vTargetPositon);

        guiPosition.x = (Screen.width * guiPosition.x) - 50.0f;
        guiPosition.y = Screen.height - (guiPosition.y * Screen.height);
	}

    void OnGUI()
    {
        if (health.Min > 0)
        {
            GUI.DrawTexture(new Rect(guiPosition.x, guiPosition.y, barLength, 10.0f), healthTexture);
        }
    }
}
