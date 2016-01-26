using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Note: This script can only generate 1 variation of each type of objects
// Option: Just change the mesh/obj looks through a different script
public class ItemGenerator : MonoBehaviour {

    #region Variables

    [Tooltip("Candy to be spawned")]
    public GameObject item;

    [Tooltip("Obstacle to be spawned")]
    public GameObject obstacle;

    [Tooltip("Array of targets, where the items and obstacles generate at")]
    [SerializeField]
    private Transform targets;

    [Tooltip("Item Ratio")]
    public int itemRatio;

    [Tooltip("Obstacle Ratio")]
    public int obstacleRatio;

    [Tooltip("Spawn Repeat Rate")]
    public float repeatRate;

    public float maxRepeatRate;

    // List of GameObjects to pool
    private List<GameObject> items;
    private List<GameObject> obstacles;

    // List of everything to spawn
    public List<GameObject> spawns;

    // private bool willGrow = true;
    private GameController _gc;

    private GameSettings _settings;

    #endregion Variables

    #region Monobehaviour

    void Start()
    {
        // TODO: Current level number should affect the itemRatio and obstacleRatio in someway.
        // EX: itemRatio *= level * 0.x; (Or any variation)
        _gc = FindObjectOfType<GameController>();
        _settings = FindObjectOfType<GameSettings>();
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

        // Initiate Spawns - Maybe better to make it into a Queue
        spawns = new List<GameObject>();
        foreach (GameObject i in items)
            spawns.Add(i);
        foreach (GameObject i in obstacles)
            spawns.Add(i);
        for (int i = 0; i < spawns.Count; i++)
        {
            GameObject temp = spawns[i];
            int randIndex = Random.Range(i, spawns.Count);
            spawns[i] = spawns[randIndex];
            spawns[randIndex] = temp;
        }
        
        //InvokeRepeating("Spawn", 1f, repeatRate);
        StartCoroutine(SpawnLoop());
    }

    #endregion Monobehaviour

    #region Private Methods
    
    private void Spawn()
    {
        for (int i = 0; i < spawns.Count; i++)
        {
            // randTarget = Random.Range(0, targets.Length);

            // Note: Whenever an object deactivates, it will reactive the same one
            // Current work around is making the number of objects and obstacles small
            if (!spawns[i].activeInHierarchy)
            {
                spawns[i].GetComponent<RandomObjectSet>().SetRandomObject();
                spawns[i].transform.position = targets.transform.position;
                spawns[i].transform.rotation = targets.transform.rotation;
                spawns[i].SetActive(true);
                //GameObject temp = spawns[i];
                //spawns.Remove(spawns[i]);

                break;
            }
        }
    }

    private IEnumerator SpawnLoop()
    {
        Spawn();
        if (_settings.level % 3 == 0)
        {
            Shuffle();
        }
        yield return new WaitForSeconds(repeatRate);
        StartCoroutine(SpawnLoop());
    }

    private void Shuffle(/*IList<GameObject> spawnPool*/)
    {
        int randomNum;
        GameObject temp;
        for (int i = spawns.Count - 1; i < 0; i--)
        {
            randomNum = Random.Range(0, i);
            temp = spawns[randomNum];
            spawns[randomNum] = spawns[i];
            spawns[i] = temp;
        }
    }

    #endregion Private Methods

    #region Public Methods

    public void UpdateRepeatRate()
    {
        if (_gc.currentLevel % 3 == 0)
        {
            repeatRate -= 0.3f;
            if (repeatRate < maxRepeatRate)
            {
                repeatRate = maxRepeatRate;
            }
        }
    }

    #endregion Public Methods
}
