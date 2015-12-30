using UnityEngine;
using System.Collections;

[System.Serializable]
public class GameSettings : MonoBehaviour {

    #region Variables

    [Tooltip("The speed of the objects")]
    public float gameSpeed = 2f;

    [Tooltip("The weight of the player")]
    public float playerWeight = 2f;

    [Tooltip("Check to see if the game restarts")]
    public bool gameRestart = false;

    [Tooltip("Check to see if the game starts")]
    public bool gameStart = false;

    private float gameSpeedDefault;
    private float playerWeightDefault;
    

    #endregion Variables

    #region Monobehvaiour

    void Awake()
    {
        // Set the defaults of each variables
        gameSpeedDefault = gameSpeed;
        playerWeightDefault = playerWeight;
    }

    void Update()
    {
        // When the game restarts, set variables back to the defaults
        if (gameRestart)
        {
            gameSpeed = gameSpeedDefault;
            playerWeight = playerWeightDefault;
            gameRestart = false;
        }
        else if (gameStart)
        {
            // Needs tweeking
            gameSpeed += 0.01f;
            playerWeight += 0.01f;
        }
    }

    #endregion
}
