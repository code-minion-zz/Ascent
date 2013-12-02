using UnityEngine;
using System.Collections;

public class UIHeroSelect : UIPlayerPanel
{
    private enum Button
    {
        Load = 0,
        New,

        Max
    }

    Button currentButton = Button.Load;

    UIButton[] buttons = new UIButton[(int)Button.Max];

    void Awake()
    {
        // Grab button references
        buttons[(int)Button.Load] = transform.FindChild("Load Button").GetComponent<UIButton>();
        buttons[(int)Button.New] = transform.FindChild("New Button").GetComponent<UIButton>();
    }

    UIButton NextButton()
    {
        currentButton = ++currentButton;

        if (currentButton >= Button.Max)
        {
            currentButton = Button.Load;
        }

        return (buttons[(int)currentButton]);
    }

    UIButton PrevButton()
    {
        currentButton = --currentButton;

        if (currentButton < Button.Load)
        {
            currentButton = (Button.Max - 1);
        }

        return (buttons[(int)currentButton]);
    }

    protected override void SetInitialState()
    {
        Button currentButton = Button.Load;

        // Select the load button to start
        UICamera.Notify(buttons[(int)currentButton].gameObject, "OnHover", true);
    }

    protected override void OnUp()
    {
        UICamera.Notify(buttons[(int)currentButton].gameObject, "OnHover", false);
        UICamera.Notify(PrevButton().gameObject, "OnHover", true);
    }

    protected override void OnDown()
    {
        UICamera.Notify(buttons[(int)currentButton].gameObject, "OnHover", false);
        UICamera.Notify(NextButton().gameObject, "OnHover", true);
    }

    protected override void OnLeft()
    {

    }

    protected override void OnRight()
    {

    }

    protected override void OnA()
    {

    }

    protected override void OnB()
    {

    }

    protected override void OnX()
    {

    }

    protected override void OnY()
    {

    }

    protected override void OnStart()
    {

    }

    protected override void OnBack()
    {

    }

    protected override void OnLeftTrigger()
    {

    }

    protected override void OnLeftBumper()
    {

    }

    protected override void OnRightTrigger()
    {
    }

    protected override void OnRightBumper()
    {

    }
}
