using UnityEngine;
using System.Collections;

[System.Serializable]
public class GameSettings : MonoBehaviour {

    #region Variables

    [Tooltip("The speed of the objects")]
    public float gameSpeed = 2f;

    [Tooltip("The weight of the player")]
    public float playerWeight = 2f;

    [Tooltip("The delay between each speed increase in seconds")]
    public float coroutineDelay = 10f;

    [Tooltip("The amount that the speed increases per tick")]
    public float speedIncreaser = 0.05f;

    [HideInInspector]
    public bool gameRestart = false;

    [HideInInspector]
    public bool gameStart = false;

    private float gameSpeedDefault;
    private float playerWeightDefault;
    private float maxSpeed = 1f;
    private bool afterDelay = true;

    #endregion Variables

    #region Monobehaviour

    void Awake()
    {
        gameSpeedDefault = gameSpeed;
        playerWeightDefault = playerWeight;
    }

    void Update()
    {
        if (gameRestart)
        {
            gameSpeed = gameSpeedDefault;
            playerWeight = playerWeightDefault;
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
        maxSpeed++;
        afterDelay = true;
    }

    #endregion Private Methods
}
