using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenericVDO<T> : MonoBehaviour
{
    protected List<GameObject> debugObjects = new List<GameObject>();

    protected List<Transform> floatingTexts = new List<Transform>();

    public bool isDrawing = true;
    protected bool isLoaded = false;
    protected bool isActive = true;

    protected List<GameObject> DebugObjects
    {
        get { return debugObjects; }
    }

    protected List<Transform> FloatingText
    {
        get { return floatingTexts; }
    }

	public void Start () 
    {
        if (isDrawing && !isLoaded)
        {
            Load();
        }
	}

    public void Update() 
    {
        if (!isDrawing && isLoaded)
        {
            Unload();
            isActive = false;
        }
        else if (isDrawing && !isLoaded)
        {
            Load();
            isActive = false;
        }
        else if (isDrawing && isLoaded)
        {
            debugObjects.RemoveAll(delegate(GameObject go)
            {
                if (go == null)
                {
                    return true;
                }
                return false;
            });

            isActive = true;
        }



        MakeAllTextFaceForward();

        if (isActive)
        {
            SetText();
        }
	}

    public void Load()
    {
        GameObject[] objects = GameObject.FindObjectsOfType(typeof(T)) as GameObject[];

        if (objects == null )
        {
            objects = GameObject.FindGameObjectsWithTag(typeof(T).ToString()) as GameObject[];
        }

        foreach (GameObject obj in objects)
        {
            GameObject floatingText = (GameObject)Instantiate(Resources.Load("Prefabs/FloatingText"));
            floatingText.transform.parent = obj.transform;

            debugObjects.Add(obj);
            floatingTexts.Add(floatingText.transform);
        }

        isLoaded = true;
    }

    public void Unload()
    {
        foreach (Transform floatingText in floatingTexts)
        {
            Destroy(floatingText.gameObject);
        }

        floatingTexts.Clear();

        isLoaded = false;
    }

    void MakeAllTextFaceForward()
    {
        // Get rid of anything that doesnt exist anymore
        floatingTexts.RemoveAll(delegate(Transform tform)
        {
            if (tform == null)
            {
                return true;
            }
            return false;
        });

        foreach (Transform floatingText in floatingTexts)
        {
            // Make the text face forward
            floatingText.transform.localPosition = new Vector3(0.0f, 2.0f, 0.0f);
            floatingText.transform.rotation = Quaternion.identity;
        }
    }

    virtual protected void SetText()
    {
        foreach (GameObject go in debugObjects)
        {
            TextMesh textMesh = go.transform.FindChild("FloatingText(Clone)").GetComponent<TextMesh>();
            textMesh.text = "default\n" + go.name;
        }
    }

    public void Toggle()
    {
        isDrawing = !isDrawing;
    }
}
