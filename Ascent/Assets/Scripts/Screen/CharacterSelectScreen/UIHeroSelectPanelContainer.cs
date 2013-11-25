using UnityEngine;
using System.Collections;

public class UIHeroSelectPanelContainer : MonoBehaviour 
{
    public enum ActivePanel
    {
        LoadNew,
        LoadHero,
        NewHero
    }

    Transform loadNew;
    Transform loadHero;
    Transform newHero;

    Transform currentPanel;

    private ActivePanel curPanel;

    public void Start()
    {
        SimpleJSON.JSONNode node = SimpleJSON.JSON.Parse("yolo");
        string value = node["someValue"].Value;


        // Grab reference to children
        loadNew = transform.FindChild("LoadNew Panel");
        loadHero = transform.FindChild("LoadHero Panel");
        newHero = transform.FindChild("NewHero Panel");

        currentPanel = loadNew;
    }

    public void ChangePanelToLoadHero()
    {
        currentPanel.gameObject.SetActive(false);
        currentPanel = loadHero;
        currentPanel.gameObject.SetActive(true);
    }

    public void ChangePanelToNewHero()
    {
        currentPanel.gameObject.SetActive(false);
        currentPanel = newHero;
        currentPanel.gameObject.SetActive(true);
    }

    //public void ChangePanel(ActivePanel newPanel)
    //{
    //    // From LoadNew
    //    if (curPanel == ActivePanel.LoadNew)
    //    {
    //        if(newPanel == ActivePanel.LoadHero)
    //        {

    //        }
    //        else if (newPanel == ActivePanel.NewHero)
    //        {

    //        }
    //    }

    //    // From LoadHero
    //    else if (curPanel == ActivePanel.LoadHero)
    //    {
    //        if (newPanel == ActivePanel.LoadNew)
    //        {

    //        }
    //    }

    //    // From NewHero
    //    else if (curPanel == ActivePanel.NewHero)
    //    {
    //        if (newPanel == ActivePanel.LoadNew)
    //        {

    //        }
    //    }
    //}
}
