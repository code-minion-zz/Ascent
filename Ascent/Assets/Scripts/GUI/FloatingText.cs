using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FloatingText : MonoBehaviour
{
    private UILabel uiLabel;
    private TweenPosition tweenPos;

    // The target object to put the floating text
    public GameObject target;
    public Camera worldCamera;
    public Camera guiCamera;
    public bool follow;

    #region Properties

    public GameObject Target
    {
        get { return target; }
        set
        {
            target = value;
            worldCamera = NGUITools.FindCameraForLayer(target.layer);
            guiCamera = NGUITools.FindCameraForLayer(gameObject.layer);
        }
    }

    public TweenPosition TweenPosition
    {
        get
        {
            if (tweenPos == null)
            {
                tweenPos = gameObject.AddComponent<TweenPosition>();
            }

            return tweenPos;
        }
    }

    public UILabel UILabel
    {
        get
        {
            if (uiLabel == null)
            {
                uiLabel = gameObject.AddComponent<UILabel>();
            }

            return uiLabel;
        }
    }

    #endregion

    void Awake()
    {
        uiLabel = gameObject.GetComponent<UILabel>();
        tweenPos = gameObject.GetComponent<TweenPosition>();
        guiCamera = NGUITools.FindCameraForLayer(gameObject.layer);
    }

    void Start()
    {

    }

    public void Update()
	{
		if (target.activeInHierarchy)
		{
			transform.position = GetTargetPos();

			tweenPos.from = transform.localPosition;
			tweenPos.to = tweenPos.from + Vector3.up * 100.0f;
		}
		else
		{
			tweenPos.enabled = false;
		}
    }

    public void DestroyText(float time)
    {
		//FloorHUDManager.Singleton.TextDriver.RemoveText(this);

        Destroy(gameObject, time);
    }

    public void SpawnAt(GameObject target)
    {
        Target = target;
    }

    public void Following()
    {
		transform.position = GetTargetPos();
    }

	public Vector3 GetTargetPos()
	{
		// All these checks to stop an error that occassionally occured.
		if (target != null)
		{
			// Spawn the text at the top of the collider
			Collider col = target.GetComponentInChildren<Collider>();
			if (col != null)
			{
				if (worldCamera != null)
				{
					Vector3 pos = worldCamera.WorldToViewportPoint(target.transform.position + new Vector3(0.0f, col.bounds.extents.y, 0.0f));
					pos = guiCamera.ViewportToWorldPoint(pos);
					pos.z = 0.0f;
					return pos; 
				}
			}
		}

		return Vector3.zero;
	}
}
