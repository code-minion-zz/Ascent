using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TreasureChest : MonoBehaviour
{
    #region Fields

    public float doorOpenAngle = 90.0f;
    public float smoothing = 20.0f;
    public bool isOpened = false;
    public bool openChest = false;
    public int spawnCount = 10;
    public GameObject loot;

    private bool canUse = false;
    private Transform hinge;
    private Transform lootSpawn;
    private List<GameObject> lootObjects = new List<GameObject>();

    private Vector3 defaultRot;
    private Vector3 currentRot;
    private Vector3 openRot;

    float time = 0.0f;

    #endregion

    #region Properties

    public bool IsOpen
    {
        get { return isOpened; }
        set { isOpened = value; }
    }

    #endregion

    void Awake()
    {
        // The first child of the treasure chest should be the hinge.
        hinge = transform.FindChild("pCylinder2");
        lootSpawn = transform.FindChild("Loot");
        loot = Resources.Load("Prefabs/CoinSack") as GameObject;

        for (int i = 0; i < spawnCount; ++i)
        {
            GameObject spawnLoot = Instantiate(loot) as GameObject;
            spawnLoot.transform.parent = lootSpawn;
            spawnLoot.transform.localPosition = Vector3.zero;
            spawnLoot.SetActive(false);
            lootObjects.Add(spawnLoot);
        }
    }

    // Use this for initialization
	void Start () 
	{
        defaultRot = hinge.transform.eulerAngles;
        currentRot = defaultRot;

        openRot = new Vector3(currentRot.x - doorOpenAngle, defaultRot.y, defaultRot.z);
	}
	
	// Update is called once per frame
	void Update () 
	{
        if (isOpened == false && openChest == true)
        {
            if (SwingOpen() >= 1.0f)
            {
                currentRot = hinge.transform.eulerAngles;
                openRot = new Vector3(currentRot.x + doorOpenAngle, currentRot.y, currentRot.z);
                time = 0.0f;
                isOpened = true;

                // Spawn all the loot.
                SpawnLoot();
            }
        }
        else if (isOpened == true && openChest == false)
        {
            if (SwingClose() >= 1.0f)
            {
                currentRot = hinge.transform.eulerAngles;
                openRot = new Vector3(currentRot.x - doorOpenAngle, currentRot.y, currentRot.z);
                time = 0.0f;
                isOpened = false;
                ResetLoot();
            }
        }
	}

    private float SwingOpen()
    {
        time += Time.deltaTime * smoothing;
        hinge.transform.eulerAngles = Vector3.Lerp(currentRot, openRot, time);

        return time;
    }

    private float SwingClose()
    {
        time += Time.deltaTime * smoothing;
        hinge.transform.eulerAngles = Vector3.Lerp(currentRot, openRot, time);

        return time;
    }

    void OnTriggerEnter(Collider enter)
    {
        string tag = enter.tag;

        switch (tag)
        {
            case "Hero":
                {
                    canUse = true;
                    Hero hero = enter.GetComponent<Hero>();
                    hero.HeroController.Input.OnY += OnY;
                }
                break;
        }        
    }

    //void OnTriggerStay(Collider stay)
    //{
    //    string tag = stay.tag;

    //    switch (tag)
    //    {
    //        case "Hero":
    //            {
    //                canUse = true;
    //                //Hero hero = stay.GetComponent<Hero>();
    //            }
    //            break;
    //    }
    //}

    void OnTriggerExit(Collider exit)
    {
        string tag = exit.tag;

        switch (tag)
        {
            case "Hero":
                {
                    canUse = false;
                    Hero hero = exit.GetComponent<Hero>();
                    hero.HeroController.Input.OnY -= OnY;
                }
                break;
        }
    }

    public void OnY(ref InputDevice device)
    {
        if (canUse)
        {
            openChest = true;
        }
    }

    /// <summary>
    /// The chest will spawn loot out of the object.
    /// </summary>
    private void SpawnLoot()
    {
        foreach (GameObject lootObject in lootObjects)
        {
            //lootObject.SetActive(true);
            //lootObject.rigidbody.AddExplosionForce(30.0f, lootSpawn.position, 20.0f);

            lootObject.SetActive(true);


            float radius = 100.0f;

            Vector3 rand = Random.insideUnitSphere;
            Vector3 newVec = new Vector3(Mathf.Clamp(rand.x, -1, 1), 30.0f, Mathf.Clamp(rand.z, -1, 1));
            Vector3 force = new Vector3(newVec.x * radius, 30.0f, newVec.z * radius);

            lootObject.rigidbody.AddForce(force);
        }
    }

    private void ResetLoot()
    {
        foreach (GameObject lootObject in lootObjects)
        {
            lootObject.SetActive(false);
            lootObject.transform.position = lootSpawn.transform.position;
        }
    }
}
