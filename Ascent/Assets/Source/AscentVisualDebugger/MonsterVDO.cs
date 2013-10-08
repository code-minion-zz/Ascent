using UnityEngine;
using System.Collections;

public class MonsterVDO : GenericVDO<Monster>, IVisulDebugObject
{
    new public void Start()
    {
        base.Start();
    }

    new public void Update()
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
