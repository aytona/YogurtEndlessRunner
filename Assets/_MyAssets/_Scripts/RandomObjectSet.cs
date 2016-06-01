using UnityEngine;
using System.Collections;

public class RandomObjectSet : MonoBehaviour {

    #region Variables

    [Tooltip("The objects in the children")]
    public GameObject[] setObjects;

    [Tooltip("Check if you want multiple copies in a row")]
    public bool multi;

    [Range(2, 3), Tooltip("If multi is checked, the max amount of objects that can be displayed of the object's set.")]
    public int maxNumOfObjects = 3;

    private int minPercent = 0;

    private int maxPercent = 100;

    //private int lastPosition;

    #endregion Variables

    #region MonoBehaviour

    void Awake()
    {
        if (multi)
        {
            
            switch (maxNumOfObjects)
            {
                case 2:
                    minPercent = 26;
                    maxPercent = 100;
                    break;
                case 3:
                    minPercent = 0;
                    maxPercent = 100;
                    break;
                default:
                    Debug.Log("Number of items not set correctly, num: " + maxNumOfObjects);
                    minPercent = 25;
                    maxPercent = 50;
                    multi = false;
                    break;
            }
        }
    }

    public void ResetObjectSet()
    {
        for (int i = 0; i < setObjects.Length; i++)
        {
            setObjects[i].SetActive(true);
        }
    }

    public void SetRandomObject()
    {
        int randPos = Random.Range(0, 3);
        int randNum = Random.Range(minPercent, maxPercent);

        if (multi)
        {
            // Have all 3 items active
            if (randNum < 25 && maxNumOfObjects > 2)
            {
                for (int i = 0; i < setObjects.Length; i++)
                {
                    setObjects[i].SetActive(true);
                }
            }
            // Have only 2 items active
            if (randNum > 50)
            {
                setObjects[randPos].SetActive(false);
            }
        }
        // Have only 1 item active
        if (!multi || (randNum >= 25 && randNum <= 50))
        {
            for (int i = 0; i < setObjects.Length; i++)
                setObjects[i].SetActive(false);
            setObjects[randPos].SetActive(true);
        }
    }

    #endregion MonoBehaviour
}
