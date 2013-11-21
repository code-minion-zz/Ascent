using UnityEngine;
using System.Collections;

/// <summary>
/// Hud manager.
/// </summary>
public class HudManager : MonoBehaviour {

    public static HudManager Singleton;
	public	GameObject 	hudCamera;
	private	Game		gameScript;
	private	int			numPlayers;
	public	PlayerHUD	Player1;
	public	PlayerHUD	Player2;
	public	PlayerHUD	Player3;

    private GameObject floatingText;

    public void OnEnable()
    {
        if (Singleton == null)
            Singleton = this;
    }
	
	void Awake()
	{
		GameObject gameLoop = GameObject.Find("Game");
		if (gameLoop == null)
		{
			Debug.LogError("HudManager : 'Game' GameObject does not exist!", this);
			return;
		}
		gameScript = gameLoop.GetComponent<Game>();

        floatingText = Resources.Load("Prefabs/FloatingText") as GameObject;
	}
	
	// Use this for initialization
	void Start () 
	{
		int numPlayers = gameScript.NumberOfPlayers;

		if (numPlayers > 0)
		{		
			Player1.Init(gameScript.Players[0].Hero.GetComponent<Character>());

			if (numPlayers > 1)
			{
				Player2.gameObject.SetActive(true);
				Player2.Init(gameScript.Players[1].Hero.GetComponent<Character>());

				if (numPlayers > 2)
				{				
					Player3.Init(gameScript.Players[2].Hero.GetComponent<Character>());
					Player3.gameObject.SetActive(true);
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

    public void SpawnDamageText(GameObject target, int damage)
    {
        //GameObject go = Instantiate(floatingText) as GameObject;
        //go = NGUITools.AddChild(go);
        GameObject go = NGUITools.AddChild(target, floatingText);
        FloatingText ft = go.GetComponent<FloatingText>();

        if (ft != null)
        {
            ft.SpawnAt(target);
            ft.UILabel.text = "" + damage;
            ft.UILabel.color = Color.red;
        }
        else
        {
            Debug.Log("Could not find floating text component");
        }
    }
}
