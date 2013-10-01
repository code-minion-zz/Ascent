using UnityEngine;
using System.Collections;

public class MonsterVDO : GenericVDO<Monster>, IVisulDebugObject
{
    public void Start()
    {
        base.Start();
    }

    public void Update()
    {
        base.Update();
    }

    override protected void SetText()
    {
        foreach (GameObject go in DebugObjects)
        {
            TextMesh textMesh = go.transform.FindChild("FloatingText(Clone)").GetComponent<TextMesh>();
            textMesh.text = go.name;
        }
    }
}
