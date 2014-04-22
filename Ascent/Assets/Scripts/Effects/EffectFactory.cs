using UnityEngine;
using System.Collections;


public class EffectFactory : MonoBehaviour
{
    public GameObject iceBlockEffect;
    public GameObject largeBloodEffect;
	public GameObject arcaneExplosion;
    public GameObject[] hitEffects;

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
    }

    public GameObject CreateBloodSplatter(Vector3 position, Quaternion rotation)
    {
        GameObject bloodEffect = null;

        if (largeBloodEffect != null)
        {
            bloodEffect = GameObject.Instantiate(largeBloodEffect, position, rotation) as GameObject;
            bloodEffect.transform.parent = transform;
        }

        return bloodEffect;
    }

    public GameObject CreateRandHitEffect(Vector3 position, Quaternion rotation)
    {
        GameObject hitEffect = null;

        if (hitEffects != null && hitEffects.Length > 0)
        {
            int id = UnityEngine.Random.Range(0, hitEffects.Length);

            hitEffect = GameObject.Instantiate(hitEffects[id], position, rotation) as GameObject;
            hitEffect.transform.parent = transform;
        }

        return hitEffect;
    }

	public void CreateArcaneExplosion(Vector3 position, Quaternion rotation)
	{
		GameObject explosion = GameObject.Instantiate(arcaneExplosion, position, rotation) as GameObject;
		explosion.transform.parent = transform;
	}

    public GameObject CreateIceblock(Vector3 position, Quaternion rotation)
    {
        GameObject effect = null;

        if (iceBlockEffect != null)
        {

            effect = GameObject.Instantiate(iceBlockEffect, position, rotation) as GameObject;
            effect.transform.parent = transform;
        }

        return effect;
    }

    public GameObject CreateCastFireballEffect(Vector3 position, Quaternion rotation)
    {
        GameObject effect = null;

        return effect;
    }
}
