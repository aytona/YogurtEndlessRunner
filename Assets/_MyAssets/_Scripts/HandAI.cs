using UnityEngine;
using System.Collections;

public class HandAI : MonoBehaviour {

    /// <summary>
    /// Reference to the HandMovement script attached to the hand.
    /// </summary>
    private HandMovement _movement;

    /// <summary>
    /// Will the AI run?
    /// </summary>
    private bool canLoop = true;

    [Tooltip("Minimum time the hand will be hidden.")]
    /// <summary>
    /// Minimum time the hand will be hidden.
    /// </summary>
    public float minHideTime = 5.0f;

    [Tooltip("Maximum time the hand will be hidden.")]
    /// <summary>
    /// Maximum time the hand will be hidden.
    /// </summary>
    public float maxHideTime = 10.0f;

    [Tooltip("Minimum time the hand will search.")]
    /// <summary>
    /// Minimum time the hand will search.
    /// </summary>
    public float minSearchTime = 5.0f;

    [Tooltip("Maximum time the hand will search.")]
    /// <summary>
    /// Maximum time the hand will search.
    /// </summary>
    public float maxSearchTime = 10.0f;

    [Tooltip("Time the hand will wait, at it chosen plane, before grabbing.")]
    /// <summary>
    /// Time the hand will wait, at it chosen plane, before grabbing.
    /// </summary>
    public float waitToGrabTime = 1.0f;

    [Tooltip("Time the hand will wait with the yogurt grabbed.")]
    /// <summary>
    /// Time the hand will wait with the yogurt grabbed.
    /// </summary>
    public float waitToLeaveTime = 1.0f;

    /// <summary>
    /// Used to pass random time to coroutines.
    /// </summary>
    private float hideTime, searchTime;

    /// <summary>
    /// Can the hand keep searching?
    /// </summary>
    private bool keepSearching = false;

	void Start () {
        _movement = GetComponent<HandMovement>();   // Set the reference.
        _movement.SetMove(true);                    // Sets the hand to move.
        StartCoroutine(StateLoop());                // Starts the "AI" loop.
	}

    /// <summary>
    /// The Hide, Search, Grab, Pattern the hand takes.
    /// </summary>
    /// <returns></returns>
    private IEnumerator StateLoop()
    {
        do
        {
            // Hide
            hideTime = Random.Range(minHideTime, maxHideTime);
            _movement.SetHide(true);
            yield return new WaitForSeconds(hideTime);
            _movement.SetHide(false);

            // Search
            searchTime = Random.Range(minSearchTime, maxSearchTime);
            keepSearching = true;
            StartCoroutine(Searching());
            yield return new WaitForSeconds(searchTime);
            keepSearching = false;

            // Position Hand
            _movement.SetGrabPosition(true);
            yield return new WaitForSeconds(waitToGrabTime);

            // Attempt Grab
            _movement.SetGrab(true);
            yield return new WaitForSeconds(waitToLeaveTime);

            if (_movement.HasGrabbed())
            {
                // Hand has grabbed the yougurt
                Debug.Log("TIME FOR FROZEN YOGURT!!");
            }
            else
            {
                // Reset Position - Hand has not grabbed yogurt
                _movement.SetGrabPosition(false);
                _movement.SetGrab(false);
                yield return new WaitForSeconds(2);
            }

        } while (canLoop);
    }

    /// <summary>
    /// Separate loop for getting random planes to search at.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Searching()
    {
        do
        {
            float randomNum = Random.Range(0.0f, 1.0f);     // Get a random time before moving along.
            int randomIndex = Random.Range(0, 3);           // Get a random plane to go to.
            _movement.SetNextPosition(randomIndex);         // Set the hand's next position.

            yield return new WaitForSeconds(randomNum);
        } while (keepSearching);
    }

}
