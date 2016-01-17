using UnityEngine;
using System.Collections;

public class RandomObjectSet : MonoBehaviour {

    #region Variables

    [Tooltip("The objects in the children")]
    public GameObject[] setObjects;

    [Tooltip("Check if you want multiple copies in a row")]
    public bool multi;

    /// <summary>
    /// Random number variable
    /// </summary>
    private float randNum;

    #endregion Variables

    #region MonoBehaviour

    void Awake()
    {
        randNum = Random.Range(0f, 75f);
    }

    void OnDisable()
    {
        randNum = Random.Range(0f, 75f);
        for (int i = 0; i < setObjects.Length; i++)
        {
            setObjects[i].SetActive(true);
        }
    }

    void OnEnable()
    {
        int randPos = Random.Range(0, 2);
        if (multi)
        {
            // Have all 3 items active
            if (randNum < 25f)
            {
                for (int i = 0; i < setObjects.Length; i++)
                {
                    setObjects[i].SetActive(true);
                }
            }
            // Have only 2 items active
            if (randNum > 50f)
            {
                setObjects[randPos].SetActive(false);
            }
        }
        // Have only 1 item active
        else if (!multi || (randNum >= 25f && randNum <= 50f))
        {
            for (int i = 0; i < setObjects.Length; i++)
            {
                if (i != randPos)
                {
                    setObjects[i].SetActive(false);
                }
            }
        }
    }

    #endregion MonoBehaviour
}
