using UnityEngine;
using System.Collections;

public class ArrowShooter : EnvironmentHazard
{
    public GameObject projectile;
    public int projectilePoolCount = 5;
    public float frequency = 1.0f;
    public float projectileSpeed = 5.0f;
    public int projectileDamage = 2;
    public float arrowLifeSpan = 2.0f;
	private Vector3 direction;

    private ObjectPool arrowPool;
    private Vector3 spawnPoint;
    private float timeElapsed = 0.0f;
    public bool activateArrows;

    private Vector3 shootLocalPosition;
   

	// Use this for initialization
	void Start () 
    {
        arrowPool = new ObjectPool(projectile, projectilePoolCount, this.transform, "Arrow");
       // direction = (transform.FindChild("Shooter").transform.position - transform.FindChild("Base").transform.position).normalized;
        //spawnPoint = transform.FindChild("Shooter").transform.position + (direction * 1.0f);
		direction = transform.forward;

        shootLocalPosition = transform.FindChild("Shooter").transform.position;
        spawnPoint = shootLocalPosition + direction * .50f;
		
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
        if (timeElapsed > frequency)
        {
            timeElapsed -= frequency;

            // Instantiate an arrow
            ObjectPool.PoolObject po = arrowPool.GetInactive();
            if (po != null)
            {
				SoundManager.PlaySound(AudioClipType.arrowwoosh,transform.position,.1f);

                int layerMask = (((1 << (int)Layer.Block)));
                RaycastHit hitInfo;

                if (Physics.Raycast(new Ray(shootLocalPosition, direction), out hitInfo, 0.50f, layerMask))
                {
                    Debug.Log(hitInfo.collider.gameObject);
                    return;
                }

                Arrow newArrow = po.script as Arrow;
                newArrow.Initialise(arrowLifeSpan, this.gameObject, direction, projectileSpeed, projectileDamage);
				po.go.transform.position = spawnPoint;
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
