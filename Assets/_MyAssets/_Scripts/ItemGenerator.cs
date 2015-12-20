using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemGenerator : MonoBehaviour {

    #region Variables

    // List of items/objects used later when needed
    // [Tooltip("List of items to generate")]
    // public GameObject[] itemPrefabArray;
    //[Tooltip("List of obstacles to generate")]
    //public GameObject[] obstaclePrefabArray;

    [Tooltip("Candy to be spawned")]
    public GameObject item;

    [Tooltip("Obstacle to be spawned")]
    public GameObject obstacle;

    [Tooltip("Array of targets, where the items and obstacles generate at")]
    [SerializeField]
    private Transform[] targets;

    [Tooltip("Item Ratio")]
    public int itemRatio;

    [Tooltip("Obstacle Ratio")]
    public int obstacleRatio;

    [Tooltip("Current level")]
    [SerializeField]
    private int level = 0;

    // List of GameObjects to pool
    List<GameObject> items;
    List<GameObject> obstacles;

    #endregion Variables

    #region Monobehaviour

    void Start()
    {
        // Pool item objects
        items = new List<GameObject>();
        for (int i = 0; i < itemRatio; i++)
        {
            GameObject obj = Instantiate(item);
            obj.SetActive(false);
            items.Add(obj);
        }

        // Pool obstacle objects
        obstacles = new List<GameObject>();
        for (int i = 0; i < obstacleRatio; i++)
        {
            GameObject obj = Instantiate(obstacle);
            obj.SetActive(false);
            obstacles.Add(obj);
        }
    }

    void Update()
    {

    }

    #endregion Monobehaviour
}
