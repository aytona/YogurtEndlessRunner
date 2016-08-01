using UnityEngine;
using System.Collections.Generic;

public class BackgroundManager : MonoBehaviour
{
    #region Public Variables
    /// <summary>
    /// Parent background class that has the gameobject
    /// and a checkers with it
    /// </summary>
    [System.Serializable]
    public class ParentBackground
    {
        public GameObject backGround;
        public bool timeForChange;
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

    /// <summary>
    /// The list of active BGs being cycled
    /// </summary>
    public List<ParentBackground> activeBackground;
    #endregion

    #region Private Variables
    /// <summary>
    /// The waitlist for the changing background
    /// </summary>
    private Queue<GameObject> waitingList = new Queue<GameObject>();

    /// <summary>
    /// Just a bool to call a function once
    /// </summary>
    private bool reachedDist;
    #endregion

    #region Monobehaviour
    void Start()
    {
        int halfOfActiveBG = Mathf.FloorToInt(activeBackground.Count / 2);
        foreach (GameObject i in ChangingBGPrefab)
            for (int j = 0; j < halfOfActiveBG; j++)
                AddToList(i);
        for (int i = 0; i < activeBackground.Count; i += 2)
            activeBackground[i].hasChangingBG = true;
    }

    void FixedUpdate()
    {
        ContiniousMovement(activeBackground);
        DistanceTracker();
    }
    #endregion

    #region Private Functions
    /// <summary>
    /// Moves all the objects in the list
    /// </summary>
    /// <param name="C_Background"></param>
    private void ContiniousMovement(List<ParentBackground> C_Background)
    {
        foreach (ParentBackground i in C_Background)
        {
            if (i.backGround.transform.position.x >= GameManager.Instance.lengthBeforeDespawn)
                i.backGround.transform.Translate(Vector3.left * Time.deltaTime * GameManager.Instance.gameSettings.gameSpeed);
            else
            {
                i.backGround.transform.Translate(widthOfPlatform, 0, 0);
                CheckForChange(i);
            }
        }
    }

    /// <summary>
    /// Checks the parentBG if its time to change childBG
    /// And swaps childBG to the next changing BG in waitingList
    /// </summary>
    /// <param name="parentBG"></param>
    private void CheckForChange(ParentBackground parentBG)
    {
        if (parentBG.hasChangingBG && parentBG.timeForChange)
        {
            // Get prev child
            GameObject prevChild = parentBG.backGround.transform.Find("childBG").gameObject;
            prevChild.transform.Translate(new Vector3(widthOfPlatform, 0, 0));
            prevChild.transform.parent = null;
            waitingList.Enqueue(prevChild);
            prevChild.SetActive(false);

            // Set new child
            waitingList.Peek().SetActive(true);
            waitingList.Peek().transform.parent = parentBG.backGround.transform;
            waitingList.Peek().transform.localPosition = Vector3.zero;
            waitingList.Dequeue();

            parentBG.timeForChange = false;
        }
    }

    /// <summary>
    /// Checks a ticker of each parentBG that
    /// its time to change childBG
    /// </summary>
    private void TimeToChange()
    {
        reachedDist = true;
        foreach (ParentBackground i in activeBackground)
            if (i.hasChangingBG)
                i.timeForChange = true;
    }

    /// <summary>
    /// Initializing the list by adding the objs
    /// </summary>
    /// <param name="obj"></param>
    private void AddToList(GameObject obj)
    {
        GameObject childBG =  Instantiate(obj, new Vector3(widthOfPlatform, 0, 0), Quaternion.identity) as GameObject;
        waitingList.Enqueue(childBG);
        childBG.name = "childBG";
        childBG.SetActive(false);
    }

    /// <summary>
    /// Tracks the distance of the game
    /// </summary>
    private void DistanceTracker()
    {
        if (Mathf.Floor(GameManager.Instance.gameSettings.distance % distBetweenChange) + 1 == distBetweenChange
            && !reachedDist)
            TimeToChange();
        else
            reachedDist = false;
    }
    #endregion
}

