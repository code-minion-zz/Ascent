using UnityEngine;
using System.Collections.Generic;

public class FloatingText : MonoBehaviour
{
    private UILabel uiLabel;

    // The target object to put the floating text
    public GameObject target;
    public Camera worldCamera;
    public Camera guiCamera;

    public UILabel UILabel
    {
        get { return uiLabel; }
    }

    public GameObject Target
    {
        get { return target; }
        set
        {
            target = value;
            worldCamera = NGUITools.FindCameraForLayer(target.layer);
        }
    }

    void Awake()
    {
        uiLabel = GetComponent<UILabel>();
    }

    void Start()
    {
        uiLabel.color = Color.red;
        guiCamera = NGUITools.FindCameraForLayer(gameObject.layer);
    }

    public void LateUpdate()
    {
        worldCamera = NGUITools.FindCameraForLayer(target.layer);

        Vector3 pos = worldCamera.WorldToViewportPoint(target.transform.position);
        pos = guiCamera.ViewportToWorldPoint(pos);

        pos.z = 0.0f;
        transform.position = pos;
    }

    public void SpawnAt(GameObject target)
    {
        Target = target;
    }
}
