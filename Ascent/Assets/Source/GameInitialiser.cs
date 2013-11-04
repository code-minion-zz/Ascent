using UnityEngine;
using System.Collections;

public class GameInitialiser : MonoBehaviour 
{
    public Character.ECharacterClass[] playerCharacterType = new Character.ECharacterClass[3];

    public int PlayerCount
    {
        get { return playerCharacterType.Length; }
    }

	// Use this for initialization
	void Start () 
    {

       
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
