using UnityEngine;
using System.Collections;


public class EffectFactory : MonoBehaviour
{
    public GameObject iceBlockEffect;
    public GameObject largeBloodEffect;
	public GameObject arcaneExplosion;
    public GameObject[] hitEffects;

	public GameObject arrowHit;
	public GameObject arrowFire;

    public GameObject moveBlockEffect;
    public GameObject chargedIntoWallEffect;
	public GameObject blueFlame;

	public GameObject lightningCastCircle;
	public GameObject fireCastCircle;
	public GameObject iceCastCircle;

	public GameObject stompHitEffect;

	public GameObject stunnedEffect;

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
        DontDestroyOnLoad(this.gameObject);
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
            position.y += 0.5f;
            int id = UnityEngine.Random.Range(0, hitEffects.Length);

            hitEffect = GameObject.Instantiate(hitEffects[id], position, rotation) as GameObject;
            hitEffect.transform.parent = transform;
        }

        return hitEffect;
    }

    public GameObject CreateChargedIntoWallEffect(Vector3 position, Quaternion rotation)
    {
        GameObject effect = null;

        if (chargedIntoWallEffect != null)
        {

            effect = GameObject.Instantiate(chargedIntoWallEffect, position, rotation) as GameObject;
            effect.transform.parent = transform;
        }

        return effect;
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

	public GameObject CreateArrowHit(Vector3 position, Quaternion rotation)
	{
		GameObject effect = null;

		if (arrowHit != null)
		{
			effect = GameObject.Instantiate(arrowHit, position, rotation) as GameObject;
			effect.transform.parent = transform;
		}

		return effect;
	}

	public GameObject CreateArrowFire(Vector3 position, Quaternion rotation)
	{
		GameObject effect = null;

		if (arrowFire != null)
		{
			effect = GameObject.Instantiate(arrowFire, position, rotation) as GameObject;
			effect.transform.parent = transform;
		}

		return effect;
	}

    public GameObject CreateMoveBlockDust(Vector3 position, Quaternion rotation)
    {
        GameObject effect = null;

        if (moveBlockEffect != null)
        {
            Vector3 pos = position;
            pos.y += 0.5f;
            effect = GameObject.Instantiate(moveBlockEffect, pos, rotation) as GameObject;
            effect.transform.parent = transform;
        }

        return effect;
    }

	public GameObject CreateBlueFlame(Vector3 position, Quaternion rotation)
	{
		GameObject effect = null;

		if (blueFlame != null)
		{
			Vector3 pos = position;
			effect = GameObject.Instantiate(blueFlame, pos, rotation) as GameObject;
			effect.transform.parent = transform;
		}

		return effect;
	}

	public GameObject CreateLightningCastCircle(Vector3 position, Quaternion rotation)
	{
		GameObject effect = null;

		if (lightningCastCircle != null)
		{
			Vector3 pos = position;
			effect = GameObject.Instantiate(lightningCastCircle, pos, rotation) as GameObject;
			effect.transform.parent = transform;
		}

		return effect;
	}

	public GameObject CreateFireCastCircle(Vector3 position, Quaternion rotation)
	{
		GameObject effect = null;

		if (fireCastCircle != null)
		{
			Vector3 pos = position;
			effect = GameObject.Instantiate(fireCastCircle, pos, rotation) as GameObject;
			effect.transform.parent = transform;
		}

		return effect;
	}

	public GameObject CreateIceCastCircle(Vector3 position, Quaternion rotation)
	{
		GameObject effect = null;

		if (iceCastCircle != null)
		{
			Vector3 pos = position;
			effect = GameObject.Instantiate(iceCastCircle, pos, rotation) as GameObject;
			effect.transform.parent = transform;
		}

		return effect;
	}

	public GameObject CreateStompHitEffect(Vector3 position, Quaternion rotation)
	{
		GameObject effect = null;

		if (stompHitEffect != null)
		{
			Vector3 pos = position;
			effect = GameObject.Instantiate(stompHitEffect, pos, rotation) as GameObject;
			effect.transform.parent = transform;
		}

		return effect;
	}

	public GameObject CreateStunnedEffect(Transform target)
	{
		GameObject effect = null;

		if (stunnedEffect != null)
		{
			Vector3 pos = target.transform.position;
			pos.y += 1.0f;
			effect = GameObject.Instantiate(stunnedEffect, pos, target.transform.rotation) as GameObject;
			effect.transform.parent = transform;

			effect.GetComponent<StunnedEffect>().Initialise(target);
		}

		return effect;
	}


}
