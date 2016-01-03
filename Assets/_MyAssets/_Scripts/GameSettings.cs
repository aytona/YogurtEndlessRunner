using UnityEngine;
using System.Collections;

[System.Serializable]
public class GameSettings : MonoBehaviour {

    #region Enums

    public enum gameState
    {
        Reset,
        StandBy,
        Playing
    }

    #endregion Enums

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
    public gameState currentState;

    private float gameSpeedDefault = 5f;
    private float playerWeightDefault = 5f;
    private float speedIncreaser = 0.05f;
    private float maxSpeed = 0f;
    private bool afterDelay = true;


    #endregion Variables

    #region Monobehaviour

    void Awake()
    {
        gameSpeedDefault = gameSpeed;
        playerWeightDefault = playerWeight;
        currentState = gameState.StandBy;
    }

    void Update()
    {
        if (currentState == gameState.Reset)
        {
            gameSpeedDefault = gameSpeed;
            playerWeightDefault = playerWeight;
            currentState = gameState.StandBy;
        }

        if (currentState == gameState.Playing)
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

    // TODO levels
    void OnGUI()
    {
        
        
        
    }

    #endregion Monobehaviour

    #region Private Methods

    private IEnumerator SpeedDelay(float delay)
    {
        afterDelay = false;
        yield return new WaitForSeconds(delay);
        if (maxSpeed < speedCap)
            maxSpeed += speedMultiplier;
        afterDelay = true;
    }

    #endregion Private Methods
}
