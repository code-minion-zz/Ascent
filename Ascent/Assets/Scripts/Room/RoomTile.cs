using UnityEngine;
using System.Collections;

public class RoomTile : MonoBehaviour 
{
#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(transform.position, new Vector3(2.0f, 0.0f, 2.0f));
    }
#endif
}
