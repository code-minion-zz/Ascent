using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScreenManager : MonoBehaviour
{
    #region Fields

    public static ScreenManager Singleton;

    private List<GameScreen> screens = new List<GameScreen>();
    private List<GameScreen> tempScreensList = new List<GameScreen>();
    private bool isInitialized;
    private bool traceEnabled = false;

    #endregion

    #region Properties

    /// <summary>
    /// If true, the manager prints out a list of all the screens
    /// each time it is updated. This can be useful for making sure
    /// everything is being added and removed at the right times.
    /// </summary>
    public bool TraceEnabled
    {
        get { return traceEnabled; }
        set { traceEnabled = value; }
    }

    #endregion

    #region Initialization

    /// <summary>
    /// Here we will initialize anything fundamental to this system before we do the Start function.
    /// We will also declare DonDestroyOnLoad which means this object shall exist even when scenes are 
    /// loaded.
    /// </summary>
    void Awake()
    {
        isInitialized = true;
        DontDestroyOnLoad(this);
    }

    /// <summary>
    /// Everytime this object has been enabled or on first intialization we want to ensure the Singleton
    /// exists
    /// </summary>
    public void OnEnable()
    {
        if (Singleton == null)
            Singleton = this;
    }

    /// <summary>
    /// Start all the screens in the list.
    /// </summary>
	void Start () 
    {
        foreach (GameScreen screen in screens)
        {
            screen.Activate();
        }
	}

    #endregion

    #region Update

    /// <summary>
    /// Called once per frame.
    /// Here we will update all the game screens and handle any input required.
    /// </summary>
	void Update () 
    {
        // Read the keyboard and gamepad.
        //input.Update();

        // Make a copy of the master screen list, to avoid confusion if
        // the process of updating one screen adds or removes others.
        tempScreensList.Clear();

        foreach (GameScreen screen in screens)
            tempScreensList.Add(screen);

        bool otherScreenHasFocus = false;
        bool coveredByOtherScreen = false;

        // Loop as long as there are screens waiting to be updated.
        while (tempScreensList.Count > 0)
        {
            // Pop the topmost screen off the waiting list.
            GameScreen screen = tempScreensList[tempScreensList.Count - 1];

            tempScreensList.RemoveAt(tempScreensList.Count - 1);

            // Update the screen.
            screen.Update(otherScreenHasFocus, coveredByOtherScreen);

            if (screen.ScreenState == ScreenState.TransitionOn ||
                screen.ScreenState == ScreenState.Active)
            {
                // If this is the first active screen we came across,
                // give it a chance to handle input.
                if (!otherScreenHasFocus)
                {
                    //screen.HandleInput(gameTime, input);

                    otherScreenHasFocus = true;
                }

                // If this is an active non-popup, inform any subsequent
                // screens that they are covered by it.
                if (!screen.IsPopup)
                    coveredByOtherScreen = true;
            }
        }

        // Print debug trace?
        if (traceEnabled)
            TraceScreens();

        Draw();
	}

    /// <summary>
    /// Prints a list of all the screens, for debugging.
    /// </summary>
    void TraceScreens()
    {
        List<string> screenNames = new List<string>();

        foreach (GameScreen screen in screens)
            screenNames.Add(screen.GetType().Name);

        Debug.Log(string.Join(", ", screenNames.ToArray()));
    }

    /// <summary>
    /// Tells each screen to draw itself.
    /// </summary>
    public void Draw()
    {
        foreach (GameScreen screen in screens)
        {
            if (screen.ScreenState == ScreenState.Hidden)
                continue;

            screen.Draw();
        }
    }

    #endregion

    #region Public Methods


    /// <summary>
    /// Adds a new screen to the screen manager.
    /// </summary>
    public void AddScreen(GameScreen screen, Player controllingPlayer)
    {
        screen.ControllingPlayer = controllingPlayer;
        screen.ScreenManager = this;
        screen.IsExiting = false;

        // If we have been intialised we can activate the screen
        if (isInitialized)
        {
            screen.Activate();
        }

        screens.Add(screen);
    }


    /// <summary>
    /// Removes a screen from the screen manager. You should normally
    /// use GameScreen.ExitScreen instead of calling this directly, so
    /// the screen can gradually transition off rather than just being
    /// instantly removed.
    /// </summary>
    public void RemoveScreen(GameScreen screen)
    {
        // If we have a graphics device, tell the screen to unload content.
        if (isInitialized)
        {
            screen.Unload();
        }

        screens.Remove(screen);
        tempScreensList.Remove(screen);
    }


    /// <summary>
    /// Expose an array holding all the screens. We return a copy rather
    /// than the real master list, because screens should only ever be added
    /// or removed using the AddScreen and RemoveScreen methods.
    /// </summary>
    public GameScreen[] GetScreens()
    {
        return screens.ToArray();
    }

    #endregion
}
