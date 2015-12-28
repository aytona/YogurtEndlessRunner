using UnityEngine;
using System.Collections;

[System.Serializable]
public class GameSettings : MonoBehaviour {

    #region Variables

    [Tooltip("The speed of the objects")]
    public float gameSpeed = 2f;

    [Tooltip("The weight of the player")]
    public float playerWeight = 2f;

    private float gameSpeedDefault;
    private float playerWeightDefault;
    private bool gameRestart = false;

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
        }
        else
        {
            // Needs tweeking
            gameSpeed += 0.01f;
            playerWeight += 0.01f;
        }
    }

    #endregion
}
