using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour
{
    #region Fields

    public static Game Singleton;
    // Number of players
    private List<Player> players;
    private InputHandler inputHandler;
    private Floor floor;

    #endregion

    #region Properties

    public int NumberOfPlayers
    {
        get { return players.Count; }
    }

    public InputHandler InputHandler
    {
        get { return inputHandler; }
    }

    public List<Player> Players
    {
        get { return players; }
    }

    public Floor Floor
    {
        get { return floor; }
    }

    #endregion

    #region Initialization

    public void OnEnable()
    {
        if (Singleton == null)
            Singleton = this;
    }

    public void Initialise(GameInitialiser.GameInitialisationValues initValues)
    {
        DontDestroyOnLoad(gameObject);

        Character.EHeroClass[] playerCharacterType = initValues.playerCharacterType;
        Application.targetFrameRate = initValues.targetFrameRate;

        // Add monoehaviour components
        inputHandler = gameObject.GetComponent("InputHandler") as InputHandler;

        CreatePlayers(playerCharacterType);

        if (initValues.useVisualDebugger)
        {
            Instantiate(Resources.Load("Prefabs/VisualDebugger"));
        }

        floor = GetComponent<Floor>();
    }

    // This function is always called immediately when Instantiated and is called before the Start() function
    // The game should add all components here so that they are ready for use when other objects require them.

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        Character.EHeroClass[] playerCharacterType = new Character.EHeroClass[1];
        playerCharacterType[0] = Character.EHeroClass.Warrior;
        Application.targetFrameRate = 60;

        // Add monoehaviour components
        inputHandler = gameObject.GetComponent("InputHandler") as InputHandler;

        CreatePlayers(playerCharacterType);

        floor = GetComponent<Floor>();
    }

    // Use this for initialization
    void Start()
    {
        // Not used atm...
    }

    void Update()
    {
        // Not used atm...
    }

    private void CreatePlayers(Character.EHeroClass[] playerCharacterType)
    {
        players = new List<Player>();

        for (int i = 0; i < NumberOfPlayers; ++i)
        {
            GameObject player = Instantiate(Resources.Load("Prefabs/Player")) as GameObject;
            Player newPlayer = player.GetComponent<Player>();
            newPlayer.PlayerID = i;
            players.Add(newPlayer);

            int iDevice = i;

            if (inputHandler.NumberOfDevices > 1)
            {
                iDevice += 1;
            }

            InControl.InputDevice device = inputHandler.GetDevice(iDevice);
            if (device == null)
            {
                device = inputHandler.GetDevice(0);
            }

            newPlayer.SetInputDevice(device);

            //newPlayer.SetInputDevice(inputHandler.GetDevice(i));


            newPlayer.CreateHero(playerCharacterType[i]);
        }
    }

    #endregion

}
