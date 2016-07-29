using UnityEngine;
using System.Collections.Generic;

public class BackgroundManager : MonoBehaviour
{
    #region Public Variables
    /// <summary>
    /// Parent background class that has the gameobject
    /// and a bool coupled with it
    /// </summary>
    [System.Serializable]
    public class ParentBackground
    {
        public GameObject backGround;
        public bool beingWorkedOn;
        public bool hasChangingBG;
    }

    /// <summary>
    /// Prefabs out of scene
    /// </summary>
    public GameObject[] ChangingBGPrefab;

    /// <summary>
    /// Some numbers that is needed
    /// </summary>
    public float widthOfPlatform;
    public float distBetweenChange;
    public float backgroundDelay;

    public List<ParentBackground> activeBackground;
    #endregion

    #region Private Variables
    /// <summary>
    /// The waitlist for the changing background
    /// </summary>
    private List<GameObject> waitingList = new List<GameObject>();
    #endregion

    #region Monobehaviour
    void Start()
    {
        int halfOfActiveBG = Mathf.FloorToInt(activeBackground.Count / 2);
        foreach (GameObject i in ChangingBGPrefab)
            for (int j = 0; j < halfOfActiveBG; j++)
                AddToList(i);
    }

    void FixedUpdate()
    {
        ContiniousMovement(activeBackground);
    }
    #endregion

    #region Private Functions
    private void ContiniousMovement(GameObject[] C_Background)
    {
        foreach(GameObject i in C_Background)
        {
            if (i.transform.position.x >= GameManager.Instance.lengthBeforeDespawn)
                i.transform.Translate(Vector3.left * Time.deltaTime * GameManager.Instance.gameSettings.gameSpeed);
            else
                i.transform.Translate(widthOfPlatform, 0, 0);
        }
    }

    private void ContiniousMovement(List<ParentBackground> C_Background)
    {
        foreach (ParentBackground i in C_Background)
        {
            if (i.backGround.transform.position.x >= GameManager.Instance.lengthBeforeDespawn)
                i.backGround.transform.Translate(Vector3.left * Time.deltaTime * GameManager.Instance.gameSettings.gameSpeed);
            else
                i.backGround.transform.Translate(widthOfPlatform, 0, 0);
        }
    }

    private void DiffBackground(GameObject[] D_Background)
    {

    }

    /// <summary>
    /// Chooses the next background to be spawned
    /// </summary>
    /// <param name="N_Background"></param>
    private void NextBackground(GameObject[] N_Background)
    {
        int nextActiveIndex;
        for (int i = 0; i < N_Background.Length; i++)
        {
            if (N_Background[i].activeInHierarchy)
            {
                if (N_Background.Length < i + 1)
                    nextActiveIndex = 0;
                else
                    nextActiveIndex = i;
            }
        }

        if (Mathf.Floor(GameManager.Instance.gameSettings.distance % distBetweenChange) + 1 == distBetweenChange)
        {
            // New background change
        }
    }

    private void AddToList(GameObject obj)
    {
        Instantiate(obj, Vector3.zero, Quaternion.identity);
        waitingList.Add(obj);

    }

    private void NewBackgroundChange()
    {

    }
    #endregion
}

