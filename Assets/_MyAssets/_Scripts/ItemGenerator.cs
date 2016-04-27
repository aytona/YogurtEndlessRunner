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

    [Tooltip("Speed Modifiers to be spawned")]
    public GameObject speedModifier;

    [Tooltip("Surfing spoon power-up")]
    public GameObject spoonPowerup;

    [Tooltip("Array of targets, where the items and obstacles generate at")]
    [SerializeField]
    private Transform targets;

    [Tooltip("Item Ratio")]
    public int itemRatio;

    [Tooltip("Obstacle Ratio")]
    public int obstacleRatio;

    [Tooltip("Speed Modifier Ratio")]
    public int speedModifierRatio;

    [Tooltip("Spoon Powerup Ratio")]
    public int spoonPowerupRatio;

    [Tooltip("Ground Spawn Repeat Rate")]
    public float groundRepeatRate;

    [Tooltip("Air Spawn Repeat Rate")]
    public float airRepeatRate;

    public float maxRepeatRate;

    // List of GameObjects to pool
    private List<GameObject> items;
    private List<GameObject> obstacles;
    private List<GameObject> speedModifiers;
    private List<GameObject> spoonPowerupList;

    // List of everything to spawn
    public List<GameObject> groundSpawns;
    public List<GameObject> airSpawns;

    // private bool willGrow = true;
    private GameController _gc;

    private GameSettings _settings;

    private bool canShuffle = false;

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

        // Pool speed modifiers objects
        speedModifiers = new List<GameObject>();
        for (int i = 0; i < speedModifierRatio; i++)
        {
            GameObject obj = Instantiate(speedModifier);
            obj.SetActive(false);
            speedModifiers.Add(obj);
        }

        // Pool spoon powerup objects
        spoonPowerupList = new List<GameObject>();
        for (int i = 0; i < spoonPowerupRatio; i++)
        {
            GameObject obj = Instantiate(spoonPowerup);
            obj.SetActive(false);
            spoonPowerupList.Add(obj);
        }

        // Initiate Spawns - Maybe better to make it into a Queue
        groundSpawns = new List<GameObject>();
        foreach (GameObject i in items)
            groundSpawns.Add(i);
        foreach (GameObject i in obstacles)
            groundSpawns.Add(i);
        // Randomizes the objects inside the list
        for (int i = 0; i < groundSpawns.Count; i++)
        {
            GameObject temp = groundSpawns[i];
            int randIndex = Random.Range(i, groundSpawns.Count);
            groundSpawns[i] = groundSpawns[randIndex];
            groundSpawns[randIndex] = temp;
        }

        // Same thing as ground spawns
        airSpawns = new List<GameObject>();
        foreach (GameObject i in speedModifiers)
            airSpawns.Add(i);
        foreach (GameObject i in spoonPowerupList)
            airSpawns.Add(i);
        
        //InvokeRepeating("Spawn", 1f, repeatRate);
        StartCoroutine(SpawnLoop());
        StartCoroutine(AirSpawnLoop(airRepeatRate));
    }

    #endregion Monobehaviour

    #region Private Methods
    
    private void GroundSpawn()
    {
        for (int i = 0; i < groundSpawns.Count; i++)
        {
            // randTarget = Random.Range(0, targets.Length);

            // Note: Whenever an object deactivates, it will reactive the same one
            // Current work around is making the number of objects and obstacles small
            if (!groundSpawns[i].activeInHierarchy)
            {
                groundSpawns[i].GetComponent<RandomObjectSet>().SetRandomObject();
                groundSpawns[i].transform.position = targets.transform.position;
                groundSpawns[i].transform.rotation = targets.transform.rotation;
                groundSpawns[i].SetActive(true);
                //GameObject temp = groundSpawns[i];
                //groundSpawns.Remove(groundSpawns[i]);

                break;
            }
        }
    }

    private void AirSpawn()
    {
        for (int i = 0; i < airSpawns.Count; i++)
        {
            if (!airSpawns[i].activeInHierarchy)
            {
                airSpawns[i].GetComponent<RandomObjectSet>().SetRandomObject();
                airSpawns[i].transform.position = new Vector3(targets.transform.position.x, targets.transform.position.y + 1.5f, targets.transform.position.z);
                airSpawns[i].transform.rotation = targets.transform.rotation;
                airSpawns[i].SetActive(true);

                break;
            }
        }
    }

    private IEnumerator SpawnLoop()
    {
        GroundSpawn();
        if (_gc.currentLevel %3 == 0)
        {
            if (canShuffle)
            {
                Shuffle();
                canShuffle = false;
            }
        }
        else
        {
            canShuffle = true;
        }
        yield return new WaitForSeconds(groundRepeatRate);
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator AirSpawnLoop(float repeatRate)
    {
        AirSpawn();
        yield return new WaitForSeconds(repeatRate);
        float newRepeatRate = Random.Range(5f, 20f);          // TODO: Balance the min and max
        StartCoroutine(AirSpawnLoop(newRepeatRate));
    }

    private void Shuffle()
    {
        int randomNum;
        GameObject temp;
        for (int i = groundSpawns.Count - 1; i > 0; i--)
        {
            randomNum = Random.Range(0, i);
            temp = groundSpawns[randomNum];
            groundSpawns[randomNum] = groundSpawns[i];
            groundSpawns[i] = temp;
        }
    }

    #endregion Private Methods

    #region Public Methods

    public void UpdateRepeatRate()
    {
        if (_gc.currentLevel % 3 == 0)
        {
            groundRepeatRate -= 0.3f;
            if (groundRepeatRate < maxRepeatRate)
            {
                groundRepeatRate = maxRepeatRate;
            }
        }
    }

    #endregion Public Methods
}
