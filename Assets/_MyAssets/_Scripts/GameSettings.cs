using UnityEngine;
using System.Collections;

[System.Serializable]
public class GameSettings : MonoBehaviour {

    #region Variables

    [Tooltip("The speed of the objects")]
    public float gameSpeed = 5f;

    [Tooltip("The weight of the player")]
    public float playerWeight = 5f;

    [Tooltip("The delay between each speed increase in seconds")]
    public float coroutineDelay = 10f;

    [Tooltip("Next level speed multiplier")]
    public float speedMultiplier = 2f;

    [Tooltip("The speed cap")]
    public float speedCap = 50f;

    [HideInInspector]
    public bool gameRestart = false;

    [HideInInspector]
    public bool gameStart = false;

    private float gameSpeedDefault = 5f;
    private float playerWeightDefault = 5f;
    private float speedIncreaser = 0.05f;
    private float maxSpeed;
    private bool afterDelay = true;

    #endregion Variables

    #region Monobehaviour

    void Awake()
    {
        gameSpeedDefault = gameSpeed;
        playerWeightDefault = playerWeight;
        maxSpeed = gameSpeedDefault;
    }

    // Probably can make this into a switch statement instead of in update
    // Theoritically, it'll be more efficient if that function gets called on key moments
    // Instead of every tick for Update
    void Update()
    {
        if (gameRestart)
        {
            gameSpeed = gameSpeedDefault;
            playerWeight = playerWeightDefault;
            maxSpeed = gameSpeedDefault;
            gameRestart = false;
        }

        else if (gameStart)
        {
            if (afterDelay)
            {
                StartCoroutine(SpeedDelay(coroutineDelay));
            }
            else if (!afterDelay && gameSpeed < maxSpeed)
            {
                gameSpeed += speedIncreaser;
                playerWeight += speedIncreaser;
            }
        }
    }

    #endregion Monobehaviour

    #region Private Methods

    private IEnumerator SpeedDelay(float delay)
    {
        afterDelay = false;
        yield return new WaitForSeconds(delay);
        if (maxSpeed < speedCap)
            maxSpeed += speedMultiplier;
        else
            maxSpeed = speedCap;
        afterDelay = true;
    }

    #endregion Private Methods
}
