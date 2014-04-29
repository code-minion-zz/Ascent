using UnityEngine;
using System.Collections;

public class ArrowShooter : EnvironmentHazard
{
    public GameObject projectile;
    public int projectilePoolCount = 5;
    public float secondsBetweenShot = 1.0f;
    public float projectileSpeed = 5.0f;
    public int projectileDamage = 2;
    public float arrowLifeSpan = 2.0f;
	public float startDelayInSeconds = 0.0f;

    private ObjectPool arrowPool;
    private float timeElapsed = 0.0f;
    public bool activateArrows = true;

	private Transform baseThatGoesInTheWall;
    private Transform shootLocal;

	private Vector3 Direction
	{
		get { return transform.forward; }
	}

	private Vector3 SpawnPoint
	{
		get { return shootLocal.position + Direction * .50f; }
	}

	// Use this for initialization
	void Start () 
    {
        arrowPool = new ObjectPool(projectile, projectilePoolCount, transform.root, "Arrow");
		shootLocal = transform.FindChild("Shooter").transform;

		baseThatGoesInTheWall = transform.FindChild("Base").transform;

		timeElapsed -= startDelayInSeconds;
		
	}

    public override void ActivateHazard()
    {
        activateArrows = true;
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (activateArrows == false)
        {
            return;
        }

        timeElapsed += Time.deltaTime;
        if (timeElapsed > secondsBetweenShot)
        {
            timeElapsed -= secondsBetweenShot;

            // Instantiate an arrow
            ObjectPool.PoolObject po = arrowPool.GetInactive();
            if (po != null)
            {
				SoundManager.PlaySound(AudioClipType.arrowwoosh,transform.position,.1f);

				Vector3 position = baseThatGoesInTheWall.position;
				position.y += 0.5f;

                int layerMask = ((1 << (int)Layer.Block));
                RaycastHit hitInfo;

				if (Physics.Raycast(new Ray(position, Direction * 1.0f), out hitInfo, 0.50f, layerMask))
                {
                    Debug.Log("a");
                    return;
                }

                Arrow newArrow = po.script as Arrow;
				newArrow.Initialise(arrowLifeSpan, this.gameObject, Direction, projectileSpeed, projectileDamage);
				po.go.transform.position = SpawnPoint;
				//po.go.transform.Rotate(new Vector3(1.0f, 0.0f, 0.0f), 0.0f);
				//po.go.transform.LookAt(direction * 5.0f);
				po.go.transform.rotation = transform.rotation;
                po.go.SetActive(true);
				po.go.rigidbody.angularVelocity = Vector3.zero;
				po.go.rigidbody.velocity = Vector3.zero;
            }
        }	
	}
}
