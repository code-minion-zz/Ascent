using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// The room class should be attached to every room on a floor. It contains references to all of the objects in the room.
/// </summary>
public class Room : MonoBehaviour
{
    #region Fields

    private List<Enemy> enemies = new List<Enemy>();

    #endregion

    #region Properties

    public Enemy[] Enemies
    {
        get { return enemies.ToArray(); }
    }

    void Awake()
    {
        foreach (Enemy enemy in gameObject.GetComponents<Enemy>())
        {
            enemies.Add(enemy);
        }
    }

    void Start()
    {

    }

    #endregion

}
