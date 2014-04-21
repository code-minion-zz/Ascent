using UnityEngine;
using System.Collections;


public class EffectFactory : MonoBehaviour
{
    private BloodSplatter bloodSplatter = new BloodSplatter();

	public GameObject arcaneExplosion;

	private static EffectFactory singleton;
	public static EffectFactory Singleton
	{
		get
		{
			if (singleton == null)
			{
				singleton = GameObject.Find("EffectFactory(Clone)").GetComponent<EffectFactory>();

				if (singleton == null)
				{
					singleton = GameObject.Find("EffectFactory").GetComponent<EffectFactory>();
				}
			}

			return singleton;
		}
		private set { singleton = value; }
	}

    void Awake()
    {
		singleton = Singleton;

        bloodSplatter.LoadResources();
    }

    public void CreateBloodSplatter(Vector3 position, Quaternion rotation, Transform parent)
    {
        bloodSplatter.CreateBloodSplatter(position, rotation, parent, 3.0f);
    }

	public void CreateArcaneExplosion(Vector3 position, Quaternion rotation)
	{
		GameObject explosion = GameObject.Instantiate(arcaneExplosion, transform.position, transform.rotation) as GameObject;
		explosion.transform.parent = transform;
	}
}
