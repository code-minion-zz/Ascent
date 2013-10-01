using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VisualDebugger : MonoBehaviour 
{
    public Transform floatingTextPrefab;

    private List<Player> players;
    private List<Monster> monsters = new List<Monster>();

    private List<Transform> floatingTexts = new List<Transform>();

    public bool drawPlayers = true;
    public bool drawMonsters = true;

    private bool playersLoaded = false;
    private bool monstersLoaded = false;

	void Start () 
    {
        LoadPlayers();
        LoadMonsters();
	}

    void Update () 
    {
        // Update the relevant text
        //TextMesh textMesh = floatingText.GetComponent<TextMesh>();
        //textMesh.text = "ads";

        if (drawPlayers)
        {
            if (!playersLoaded)
            {
                LoadPlayers();
            }

            UpdatePlayers();
        }
        else if (!drawPlayers && playersLoaded)
        {
            UnloadPlayers();
        }

        if (drawMonsters)
        {
            if (!monstersLoaded)
            {
                LoadMonsters();
            }

            UpdateMonsters();
        }
        else if (!drawMonsters && monstersLoaded)
        {
            UnloadMonsters();
        }

        MakeAllTextFaceForward();
	}

    void MakeAllTextFaceForward()
    {
        // Get rid of anything that doesnt exist anymore
        floatingTexts.RemoveAll(
                delegate(Transform tform)
                {
                    if(tform == null)
                    {
                        return true;
                    }
                    return false;
                }
            );

        foreach (Transform floatingText in floatingTexts)
        {
            // Make the text face forward
            floatingText.transform.localPosition = new Vector3(0.0f, 2.0f, 0.0f);
            floatingText.transform.rotation = Quaternion.identity;
        }
    }

    void LoadPlayers()
    {
        Game game = GameObject.Find("Game").GetComponent<Game>();
        players = game.Players;

        foreach (Player player in players)
        {
            Transform floatingText = (Transform)Instantiate(floatingTextPrefab);
            floatingText.gameObject.transform.parent = player.gameObject.transform;
            floatingTexts.Add(floatingText);
        }

        playersLoaded = true;
    }

    void UpdatePlayers()
    {
        foreach (Player player in players)
        {
            TextMesh textMesh = player.transform.FindChild("FloatingText(Clone)").GetComponent<TextMesh>();
            textMesh.text = player.name;
        }
    }

    void UnloadPlayers()
    {
        floatingTexts.RemoveAll(
            delegate(Transform tform)
            {
                if (tform.parent.name == "Player(Clone)")
                {
                    Destroy(tform.gameObject);
                    return true;
                }
                return false;
            }
        );

        playersLoaded = false;
    }

    void LoadMonsters()
    {
        GameObject[] monstersArray = GameObject.FindGameObjectsWithTag("Monster");
        for (int i = 0; i < monstersArray.Length; ++i)
        {
            Monster monster = monstersArray[i].GetComponent<Monster>();
            monsters.Add(monster);

            Transform floatingText = (Transform)Instantiate(floatingTextPrefab);
            floatingText.gameObject.transform.parent = monster.gameObject.transform;
            floatingTexts.Add(floatingText);
        }

        monstersLoaded = true;
    }

    void UpdateMonsters()
    {
        monsters.RemoveAll(
                delegate(Monster monster)
                {
                    if (monster == null)
                    {
                        return true;
                    }
                    return false;
                }
            );

        foreach (Monster monster in monsters)
        {
            TextMesh textMesh = monster.transform.FindChild("FloatingText(Clone)").GetComponent<TextMesh>();
            textMesh.text = monster.name;
        }
    }

    void UnloadMonsters()
    {
        floatingTexts.RemoveAll(
            delegate(Transform tform)
            {
                if (tform.parent.name == "Monster")
                {
                    Destroy(tform.gameObject);
                    return true;
                }
                return false;
            }
        );

        monstersLoaded = false;
    }
}
