using UnityEngine;
using System.Collections;

public class HandAI : MonoBehaviour
{

    #region Variables

    /// <summary>
    /// Reference to the HandMovement script attached to the hand.
    /// </summary>
    private HandMovement _movement;

    /// <summary>
    /// Refenrence to the PlayerMovement script.
    /// </summary>
    private PlayerMovement _player;

    /// <summary>
    /// The current state of the player
    /// </summary>
    private PlayerMovement.State _playerState;

    [Tooltip("Use an empty GameObject to set the position of the hand when hidden.")]
    public Transform[] hidePosition;

    [Tooltip("Use an empty GameObject to set the position of the hand when threatening the player.")]
    public Transform[] threatPosition;

    /// <summary>
    /// Device the game is currently being played on. 0 - iPhone4, 1 - iPhone5, 2 - iPad.
    /// </summary>
    private int currentDevice = 2;

    /// <summary>
    /// Is the hand following the player?
    /// </summary>
    private bool followPlayer = false;

    #endregion Variables

    #region Monobehaviour

    void Start()
    {
        _movement = GetComponent<HandMovement>();   // Set the reference.
        _player = FindObjectOfType<PlayerMovement>();

        // Get the current device.  Sets the positions for the hand to move to and from.
        currentDevice = (int)GameManager.Instance.currentAspect;
        if (currentDevice > 2)
        {
            currentDevice = 2;
        }
    }

    void Update()
    {
        CheckPlayerState();
        DecideAction();
        FollowPlayer();
    }

    #endregion Monobehaviour

    #region Private Methods
    /// <summary>
    /// Checks the player's state.
    /// </summary>
    private void CheckPlayerState()
    {
        _playerState = _player._currentState;
    }

    /// <summary>
    /// Decides what action to take depending on the player's state.
    /// </summary>
    private void DecideAction()
    {
        switch (_playerState)
        {
            case PlayerMovement.State.Normal:
                // Player has not been hit
                // If the player's state is normal, then the hand will hide where the player cannot see it
                _movement.SetFreeMove(true);
                followPlayer = false;
                _movement.SetNextPosition(hidePosition[currentDevice]);
                break;
            case PlayerMovement.State.Hit:
                // Player has been hit once
                // If player hit once then gets hit again the hand will grab the player
                _movement.SetFreeMove(false);
                _movement.SetNextPosition(threatPosition[currentDevice]);
                followPlayer = true;
                break;
            case PlayerMovement.State.TwoHit:
                // Player has been hit a second time
                // If the player gets to this state then the hand will grab the player and the game will be over
                _movement.SetFreeMove(true);
                followPlayer = false;
                _movement.SetNextPosition(_player.transform);
                break;
            case PlayerMovement.State.EndGame:
                _movement.EndGameHide(hidePosition[currentDevice]);
                _player._currentState = PlayerMovement.State.None;
                break;
        }
    }

    /// <summary>
    /// Sets the plane that the hand is in to be the same one as the player's
    /// </summary>
    private void FollowPlayer()
    {
        if(followPlayer)
            _movement.SetNextPosition(_player.GetPlayerPlane());    // Follow Player position.
    }

    #endregion Private Methods

    #region Old AI
    /*
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

    /// <summary>
    /// Has the hand failed to grab the player.
    /// </summary>
    private bool failedGrab = false;

    /// <summary>
    /// Used to store the coroutines to be able to stop it later.
    /// </summary>
    private Coroutine aiLoop, searchLoop;

	void Start () {
        _movement = GetComponent<HandMovement>();   // Set the reference.
        _player = FindObjectOfType<PlayerMovement>();
        _movement.SetMove(true);                    // Sets the hand to move.
        if (!_player.GetGameOver())
        {
            StartHandAI();   // Starts the "AI" loop.
        }
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
            failedGrab = false;
            searchTime = Random.Range(minSearchTime, maxSearchTime);
            keepSearching = true;
            searchLoop = StartCoroutine(Searching());
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
                failedGrab = false;
                _player.AttachToHand(this.transform);
                FindObjectOfType<GameController>().RestartButton.SetActive(true); // For demo
            }
            else
            {
                // Reset Position - Hand has not grabbed yogurt
                failedGrab = true;
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

    /// <summary>
    /// Start hand AI.
    /// </summary>
    public void StartHandAI()
    {
        aiLoop = StartCoroutine(StateLoop());   // Starts the "AI" loop.
    }

    /// <summary>
    /// Stop all Hand AI coroutines.
    /// </summary>
    public void StopHandCoroutines()
    {
        StopCoroutine(aiLoop);
        StopCoroutine(searchLoop);
    }

    /// <summary>
    /// Has the hand succeeded in grabbing the character.
    /// </summary>
    /// <returns></returns>
    public bool SucceededGrab()
    {
        //Debug.Log("Checked for grab success. Status: " + failedGrab);
        return failedGrab;
    }
    */
    #endregion Old AI
}
