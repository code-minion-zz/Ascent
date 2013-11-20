using UnityEngine;
using System.Collections;

public class SummaryWindow  
{
    private enum State
    {
        Intialised,
        Animating,
        AnimCompleted,
        Finished,
    }

    State state;

    Player player;
    Hero hero;

    FloorRecordKeeper floorRecord;
    FloorRecordKeeper.HeroFloorRecord heroRecord;
    TowerRecordKeeper towerRecord;

    Transform windowUI;
    UILabel bonusAmountUI;
    UILabel bonusDescUI;
    UILabel goldUI;
    UILabel curExperienceUI;
    UILabel toNextExperienceUI;
    UISlider progressBar;

    float lerpDuration = 5.0f;
    float timeElapsed = 0.0f;


    public void Initialise(Transform window, Player player)
    {
        windowUI = window;
        this.player = player;
        hero = player.Hero.GetComponent<Hero>();

        //floorRecord = Game.Singleton.Floor.Records;
        //heroRecord = floorRecord.HeroRecords[hero];
        // TODO: Get Tower record 

        Transform bonuses = windowUI.FindChild("Bonuses");

        bonusAmountUI = bonuses.FindChild("Bonus Desc").GetComponent<UILabel>();

        bonusDescUI = bonuses.FindChild("Reward Amount").GetComponent<UILabel>();
        


        goldUI = windowUI.FindChild("Gold").GetComponent<UILabel>();
        goldUI.text = "Gold: " + hero.CharacterStats.Currency;

        curExperienceUI = windowUI.FindChild("Exp").GetComponent<UILabel>();
        curExperienceUI.text = "Current Experience: " + hero.CharacterStats.CurrentExperience;

        toNextExperienceUI = windowUI.FindChild("Exp To Level").GetComponent<UILabel>();
        toNextExperienceUI.text = "To Next Level: " + ((hero.CharacterStats.MaxExperience - hero.CharacterStats.CurrentExperience % hero.CharacterStats.MaxExperience));

        progressBar = windowUI.FindChild("Exp Bar").GetComponent<UISlider>();
        progressBar.value = ((float)hero.CharacterStats.CurrentExperience / (float)hero.CharacterStats.MaxExperience);
        progressBar.ForceUpdate();

        state = State.Intialised;
    }

    public void Process() 
    {
	    // Get the current records from this level.
        // List out all the rewards that this player has earnt
        // List out the derived rewards that the player has earnt
        // Separate the gold and exp bonuses
        // Lerp the gold and exp gain on the exp bar and gold scale.

        // If player presses something A - immediately complete the lerp, wait half a second then go to the next panel.

        switch (state)
        {
            case State.Intialised:
                {
                    timeElapsed += Time.deltaTime;
                    if (timeElapsed > 1.5f)
                    {
                        state = State.Animating;
                        timeElapsed = 0.0f;
                    }
                }
                break;
            case State.Animating:
                {
                    timeElapsed += Time.deltaTime;
                    if (timeElapsed > lerpDuration)
                    {
                        state = State.AnimCompleted;
                        timeElapsed = 0.0f;
                    }
                }
                break;
            case State.AnimCompleted:
                {
                    timeElapsed += Time.deltaTime;
                    if (timeElapsed > 0.5f)
                    {
                        timeElapsed = 0.0f;
                    }
                }
                break;
            case State.Finished:
                {

                }
                break;
        }
	}

    public void StartAnimating()
    {
        if(state == State.Intialised)
        {
            state = State.Animating;
        }
    }
}
