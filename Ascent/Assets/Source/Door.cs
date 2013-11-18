using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour 
{
	
	#region Fields
	public float doorOpenAngle = 90.0f;
	public float smoothing = 2.0f;
	
	public bool isDoorOpen = false;
	public bool isLocked = false;
	
	private Vector3 defaultRot;
	private Vector3 openRot;

    public Vector3 openDirection;
    private Vector3 defaultPosition;
	
	#endregion
	
	#region Properties
	
	public bool IsOpen 
	{
		get { return isDoorOpen; }
		set { isDoorOpen = value; }
	}
	
	public bool IsLocked
	{
		get { return isLocked; }
		set { isLocked = value; }
	}
	
	#endregion
	
	// Use this for initialization
	void Start () 
	{
		defaultRot = transform.eulerAngles;
        openRot = new Vector3(defaultRot.x + doorOpenAngle, defaultRot.y, defaultRot.z);

        defaultPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (IsLocked == false)
		{
			if (IsOpen)
			{
                //SwingOpen();
                SlideOpen();
			}
            //else
            //{
            //    // SwingClose();
            //    SlideClose();
            //}
		}
		else
		{
			//Debug.Log("Door is locked");
		}
	}

    private void SlideOpen()
    {
        transform.position = Vector3.Lerp(transform.position, defaultPosition + (openDirection * 4.0f), Time.deltaTime * 5.0f);
    }

    private void SlideClose()
    {
        transform.position = Vector3.Lerp(transform.position, defaultPosition, Time.deltaTime * 5.0f);
    }

    private void SwingOpen()
    {
        transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, openRot,
            Time.deltaTime * smoothing);
    }

    private void SwingClose()
    {
        transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, defaultRot,
            Time.deltaTime * smoothing);	
    }
	
	#region Collision
	
	void OnCollisionEnter(Collision collision)
	{

        Debug.Log(collision.transform.tag);
		if (collision.transform.tag == "Hero")
		{
			if (IsOpen == false)
			{
				Debug.Log("Door opened");
				IsOpen = true;
			}
			else
			{
				Debug.Log("Door closed");
				IsOpen = false;	
			}
		}		
	}
	
	void OnCollisionExit(Collision collisionInfo)
	{
		//IsOpen = true;		
	}	
	
	#endregion
}
