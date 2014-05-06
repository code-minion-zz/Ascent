using UnityEngine;
using System.Collections;

public class UVScroller : MonoBehaviour 
{
	public int targetMaterialSlot = 0;
	public float speedX = 0.5f;
	public float speedY = 0.5f;

	private float timeWentX = 0.0f;
	private float timeWentY = 0.0f;

	void Update()
	{
		timeWentX += Time.deltaTime * speedX;
		timeWentY += Time.deltaTime * speedY;

		renderer.materials[targetMaterialSlot].SetTextureOffset("_MainTex", new Vector2(timeWentX, timeWentY));
	}
}
