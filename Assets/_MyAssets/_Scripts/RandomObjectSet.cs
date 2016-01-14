using UnityEngine;
using System.Collections;

public class RandomObjectSet : MonoBehaviour {

    #region Variables

    [Tooltip("The objects in the children")]
    public GameObject[] setObjects;

    /// <summary>
    /// Random number variable
    /// </summary>
    private float randNum;

    #endregion Variables

    #region MonoBehaviour

    void Awake()
    {
        randNum = Random.Range(0f, 100f);
    }

    void OnDisable()
    {
        randNum = Random.Range(0f, 100f);
    }

    void OnEnable()
    {
        int randPos = Random.Range(0, 2);
        // Have only 2 items active
        if (randNum > 66f)
        {
            setObjects[randPos].SetActive(false);
        }
        // Have only 1 item active
        else if (randNum < 33f)
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
