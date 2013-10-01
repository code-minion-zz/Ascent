using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour 
{
    public Texture2D healthTexture;

    private float curHealth = 100;
    private float maxHealth = 100;

    public float barLength = 100.0f;
    private float healthRatio = 1.0f;

    private Vector3 guiPosition;

	
	void Update () 
    {
        healthRatio = curHealth / maxHealth;
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
        if(curHealth > 0)
        {
            GUI.DrawTexture(new Rect(guiPosition.x, guiPosition.y, barLength, 10.0f), healthTexture);
        }
    }
}
