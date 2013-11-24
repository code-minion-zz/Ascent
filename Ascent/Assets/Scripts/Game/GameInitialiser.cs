using UnityEngine;
using System.Collections;

public class GameInitialiser : MonoBehaviour 
{
    public Character.EHeroClass[] playerCharacterType = new Character.EHeroClass[3];
    public bool useVisualDebugger = true;
    public int targetFrameRate = 60;

    public class GameInitialisationValues
    {
        public bool useVisualDebugger;
        public int targetFrameRate;
        public Character.EHeroClass[] playerCharacterType;

        // TODO:
        // Player values
        // Player input device/s
        // Hero values
        // Hero items
        // Progression
    }

	// Use this for initialization
	void Awake () 
    {
        // Check if the game already exists.

        GameObject theGameObject = GameObject.Find("Game");
		if (theGameObject != null)
		{
			Debug.Log("Game already exists no need to initialise a new one.");
			// Get rid of the game initialiser
			Destroy(this.gameObject);
			return;
		}

        if(theGameObject == null)
        {
            theGameObject = GameObject.Find("Game(Clone)");
			if (theGameObject != null)
			{
				Debug.Log("Game already exists no need to initialise a new one.");
				// Get rid of the game initialiser
				Destroy(this.gameObject);
				return;
			}

            if (theGameObject == null)
            {
                // The game doesn't exist so make it.

                theGameObject = Instantiate(Resources.Load("Prefabs/Game")) as GameObject;
            }

        }

        // Set the game with the right values.
		Game game = theGameObject.GetComponent<Game>();

        game.Initialise(new GameInitialisationValues()
        {
            useVisualDebugger = useVisualDebugger,
            targetFrameRate = this.targetFrameRate,
            playerCharacterType = this.playerCharacterType,

        });


        // Get rid of the game initialiser
        Destroy(this.gameObject);
	}
}
