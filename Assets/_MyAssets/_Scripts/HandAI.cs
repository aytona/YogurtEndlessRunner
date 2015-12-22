using UnityEngine;
using System.Collections;

public class HandAI : MonoBehaviour {

    /// <summary>
    /// Reference to the HandMovement script attached to the hand.
    /// </summary>
    private HandMovement _movement;

    /// <summary>
    /// Refenrence to the PlayerMovement script.
    /// </summary>
    private PlayerMovement _player;

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

    [Tooltip("Percentage chance that the hand will choose to follow the player's position instead of going to a random plane. 0.0 - 1.0.")]
    /// <summary>
    /// Percent that the hand will follow the player's position.
    /// </summary>
    public float followPercent = 0.4f;

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
        _player = FindObjectOfType<PlayerMovement>();
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
                _player.AttachToHand(this.transform);
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
            float randomChance = Random.Range(0.0f, 1.0f);  // Get a random chance to use player's location.

            if (randomChance <= followPercent)              // Decides if the hand will follow the player or use a random position.
            {
                _movement.SetNextPosition(_player.GetPlayerPlane());    // Player position.
            }
            else
            {
                int randomIndex = Random.Range(0, 3);       // Get a random plane to go to.
                _movement.SetNextPosition(randomIndex);     // Set the hand's next position.
            }

            yield return new WaitForSeconds(randomNum);
        } while (keepSearching);
    }

}
