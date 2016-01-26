using UnityEngine;
using System.Collections;

public class RandomObjectSet : MonoBehaviour {

    #region Variables

    [Tooltip("The objects in the children")]
    public GameObject[] setObjects;

    [Tooltip("Check if you want multiple copies in a row")]
    public bool multi;

    private int lastPosition;

    #endregion Variables

    #region MonoBehaviour

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
        int randNum = Random.Range(0, 100);
        if (multi)
        {
            // Have all 3 items active
            if (randNum < 25)
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
        else if (!multi || (randNum >= 25 && randNum <= 50))
        {
            for (int i = 0; i < setObjects.Length; i++)
                setObjects[i].SetActive(false);
            setObjects[randPos].SetActive(true);
        }
    }

    #endregion MonoBehaviour
}
