using UnityEngine;
using System.Collections;

[System.Serializable]
public class GameSettings : MonoBehaviour {

    #region Variables

    [Tooltip("The speed of the objects")]
    public float gameSpeed;

    [Tooltip("The weight of the player")]
    public float playerWeight;

    [Tooltip("The delay between each speed increase in seconds")]
    public float coroutineDelay;

	[Tooltip("Next level speed multiplier")]
	public float speedMultiplier;

    [Tooltip("The speed cap")]
    public float speedCap;
    
    [HideInInspector]
    public float distance = 0;

    [HideInInspector]
    public bool gameRestart = false;

    [HideInInspector]
    public bool gameStart = false;

    //[HideInInspector]
    public int level;

    private float gameSpeedDefault;
    private float playerWeightDefault;
    private float speedIncreaser = 0.05f;
    private float maxSpeed;
    private bool afterDelay = true;
    private GameController _gc;
    private Game gameController;

    #endregion Variables

    #region Monobehaviour

    void Awake()
    {
        gameSpeedDefault = gameSpeed;
        playerWeightDefault = playerWeight;
        maxSpeed = gameSpeedDefault;
        distance = 0;
        _gc = FindObjectOfType<GameController>();
        gameController = FindObjectOfType<Game>();
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
            _gc = FindObjectOfType<GameController>();
            gameController = FindObjectOfType<Game>();
			distance = 0;
        }

        else if (gameStart)
        {
            distance += Time.deltaTime * gameSpeed / gameSpeedDefault;
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
        //Debug.Log(Time.timeScale);
    }

    void OnGUI()
    {
        GUI.Label(new Rect(Vector2.zero, Vector2.one), "test");
    }
    #endregion Monobehaviour

    #region Public Methods

    public void ZeroSpeed()
    {
        if(!gameStart)
            gameSpeed = 0;
    }

    #endregion Public Methods

    #region Private Methods

    private IEnumerator SpeedDelay(float delay)
    {
        afterDelay = false;
        yield return new WaitForSeconds(delay);
        if (_gc != null)
            _gc.AddLevel();
        else if (gameController != null)
            gameController.AddLevel();
        if (maxSpeed < speedCap)
            maxSpeed += speedMultiplier;
        else
            maxSpeed = speedCap;
        afterDelay = true;
    }

    #endregion Private Methods
}
