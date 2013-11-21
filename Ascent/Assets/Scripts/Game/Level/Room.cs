using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// The room class should be attached to every room on a floor. It contains references to all of the objects in the room.
/// </summary>
public class Room : MonoBehaviour
{
    #region Fields

    private Transform enemyTransform;
    private Transform chestsTransform;

    private List<Enemy> enemies = new List<Enemy>();
    private List<TreasureChest> chests = new List<TreasureChest>();
    public List<Door> doors = new List<Door>();

    #endregion

    #region Properties

    public Enemy[] Enemies
    {
        get { return enemies.ToArray(); }
    }

    public TreasureChest[] Chests
    {
        get { return chests.ToArray(); }
    }

    public Door[] Doors
    {
        get { return doors.ToArray(); }
    }

    #endregion

    /// <summary>
    /// Populates the class with data obtained from the room.
    /// </summary>
    void Awake()
    {
        enemyTransform = transform.FindChild("Enemies");
        chestsTransform = transform.FindChild("TreasureChests");

        if (enemyTransform != null)
        {
            // We are going to find the enemy component script inside all the enemies attached to this
            // enemy transform.
            foreach (Enemy enemy in enemyTransform.gameObject.GetComponentsInChildren<Enemy>())
            {
                enemies.Add(enemy);
            }
        }
        else
        {
            Debug.Log("Enemies not found");
        }

        if (chestsTransform != null)
        {
            foreach (TreasureChest chest in chestsTransform.gameObject.GetComponentsInChildren<TreasureChest>())
            {
                chests.Add(chest);
            }
        }
    }

    void Start()
    {
        Debug.Log("ChestRoom(EnemyCount: " + enemies.Count + " " +
                  "ChestsCount: " + chests.Count);
    }

}
