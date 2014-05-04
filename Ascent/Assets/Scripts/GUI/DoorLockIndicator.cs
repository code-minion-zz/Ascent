using UnityEngine;
using System.Collections;


public class DoorLockIndicator : MonoBehaviour
{
    private enum EState
    {
        Standard,
        Growing,
        Shrinking,
    }

    public UISprite buttonSprite;
    public Transform owner;

    private EState state;
    private float timeElapsed;
    private float growTime = 0.15f;
    private float shrinkTime = 0.09f;

    private float maxScale = 2.5f;
    private float prevScale;

    public void Initialise(Transform owner)
    {
        this.owner = owner;
        buttonSprite.cachedTransform.localScale = Vector3.zero;
    }

    public void Update()
    {
		if (owner == null)
			return;

        // Shrink or Grow
        switch (state)
        {
            case EState.Standard:
                {
                    // Do nothing;
                }
                break;
            case EState.Growing:
                {
                    // Update timer for lerping scale
                    timeElapsed += Time.deltaTime;
                    if (timeElapsed >= growTime)
                    {
                        timeElapsed = growTime;
                    }

                    buttonSprite.cachedTransform.localScale = Vector3.Lerp((Vector3.one * maxScale) * 0.1f, Vector3.one * maxScale, timeElapsed / growTime);
                }
                break;
            case EState.Shrinking:
                {
                    // Update timer for lerping scale
                    timeElapsed += Time.deltaTime;
                    if (timeElapsed >= shrinkTime)
                    {
                        timeElapsed = shrinkTime;
                    }

                    buttonSprite.cachedTransform.localScale = Vector3.Lerp(Vector3.one * prevScale, Vector3.zero, timeElapsed / shrinkTime);
                }
                break;
            default:
                {
                    Debug.LogError("Unhandled case: " + state);
                }
                break;
        }

        // Track the target
        Vector3 screenPos = Game.Singleton.Tower.CurrentFloor.MainCamera.WorldToViewportPoint(owner.transform.position);
        Vector3 barPos = FloorHUDManager.Singleton.hudCamera.ViewportToWorldPoint(screenPos);
        barPos = new Vector3(barPos.x, barPos.y);
        buttonSprite.transform.position = barPos;
    }

    public void Enable(bool b)
    {
        if (b && state != EState.Growing)
        {
            state = EState.Growing;

            timeElapsed = 0.0f;

            buttonSprite.enabled = true;
        }
        else if (b == false)
        {
            // Set timer to what is remaining.
            prevScale = buttonSprite.cachedTransform.localScale.x;
            timeElapsed = shrinkTime - ((prevScale / maxScale) * shrinkTime);

            state = EState.Shrinking;
        }
    }
}
