using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour {

    #region References

    /// <summary>
    /// Reference to the player movement script.
    /// </summary>
    private PlayerMovement _player;

    private ItemGenerator spawnPool;

    /// <summary>
    /// Hold a reference to the menu manager
    /// </summary>
    public MenuManager menuManager;

    #endregion References

    #region PauseScreenVariables

    /// <summary>
    /// Bool that is toggled to check if the game is paused or not
    /// </summary>
    public bool paused;

    public Button pauseButton;

    #endregion PauseScreenVariables

    #region OpeningScreenVariables

    /// <summary>
    /// Just a bool to check if the game has started
    /// </summary>
    private bool gameStarted = true;

    #endregion OpeningScreenVariables

    #region EndScreenVariables

    public GameObject gameUI;

    public Text finalScore;

    public Text distanceTraveled;

    private bool gameOver = false;

    private string sceneName;

    #endregion EndScreenVariables

    #region MonoBehaviour

    void Start()
    {
        _player = FindObjectOfType<PlayerMovement>();
        menuManager = FindObjectOfType<MenuManager>();
        score.text = "0000000";

        GameManager.Instance.gameSettings.gameRestart = true;
        paused = false;     // Might want to initialize pause as true if we want the game to be paused at the start
                            // Dangerous because pause stops time, so coroutines cannot be used while paused.
        sceneName = SceneManager.GetActiveScene().name;
    }

    void Update()
    {
        if (gameStarted)
        {
            pauseButton.interactable = false;
            /* removing the current challenge for the time being */
            //menuManager.SetTimeToSwitch(3.0f);
            //menuManager.TimedHideAllCanvases();
            menuManager.HideAllCanvases();
            StartCoroutine(SceneTransition(1.0f));
        }
        if (paused)
        {
            Time.timeScale = 0;
            //AudioListener.pause = true;
            pauseButton.interactable = false;
        }
        else if (!paused)
        {
            Time.timeScale = 1;
            //AudioListener.pause = false;
            pauseButton.interactable = true;
        }
        if (gameOver)
        {
            gameUI.transform.Translate(Vector3.up * Time.deltaTime * 0.2f, Space.Self);
            if (!calledEndScreen) // Calls this coroutine only once
            {
                StartCoroutine(ShowEndScreen());
            }
        }
    }

    #endregion MonoBehaviour

    /// <summary>
    /// Start the game.
    /// </summary>
    public void StartGame()
    {
        _player.SetGameOver(false);
        spawnPool = FindObjectOfType<ItemGenerator>();
        GameManager.Instance.gameSettings.gameStart = true;
        pauseButton.interactable = true;
        AddLevel();
    }

    /// <summary>
    /// Reload the level.
    /// </summary>
    public void RestartLevel()
    {
        GameManager.Instance.gameSettings.gameRestart = true;
        GameManager.Instance.gameSettings.gameStart = false;
        SceneManager.LoadScene(sceneName);
    }

    public Text score;
    public int playerScore = 0;
    public int currentLevel = 0;

    public void IncrementScore(int inc)
    {
        string scoreText;
        playerScore += inc;

        scoreText = string.Format("{0:0000000}", playerScore);

        score.text = scoreText;
    }

    public void AddLevel()
    {
        currentLevel++;
        spawnPool.UpdateRepeatRate();
    }

    public void TogglePause()
    {
        paused = !paused;
    }

    IEnumerator SceneTransition(float timeToWait)
    {
        gameStarted = false;
        yield return new WaitForSeconds(timeToWait);
        StartGame();
    }

    public void SetGameOver(bool b)
    {
        gameOver = b;
    }

    private bool calledEndScreen = false;

    IEnumerator ShowEndScreen()
    {
        calledEndScreen = true;
        yield return new WaitForSeconds(1.5f);
        menuManager.GoToCanvas(3);
        finalScore.text = score.text;
        distanceTraveled.text = (int)GameManager.Instance.gameSettings.distance + "m";
        GameManager.Instance.gameData.SetTotalDistance(GameManager.Instance.gameSettings.distance);
        GameManager.Instance.gameData.SetTotalScore(playerScore);
        GameManager.Instance.gameData.SetBestScore(playerScore);
        GameManager.Instance.gameData.SetBestDistance(GameManager.Instance.gameSettings.distance);
    }
}
