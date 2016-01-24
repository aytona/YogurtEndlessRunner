using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    #region References

    /// <summary>
    /// Reference to the player movement script.
    /// </summary>
    private PlayerMovement _player;

    /// <summary>
    /// Reference to the hand ai script.
    /// </summary>
    private HandAI _hand;

    private ItemGenerator spawnPool;

    #endregion References

    [Tooltip("Set to true to allow the demo.")]
    public bool allowDemo;

    #region PauseScreenVariables

    /// <summary>
    /// Bool that is toggled to check if the game is paused or not
    /// </summary>
    public bool paused;

    /// <summary>
    /// Placeholder for the restart object that shows when the game is paused
    /// </summary>
    public GameObject RestartMenu;

    /// <summary>
    /// Reference to the pause icon.
    /// </summary>
    public Image PauseIcon;

    /// <summary>
    /// All the sprites that are used in the pause Screen
    /// </summary>
    public Sprite pauseSprite, startSprite;

    #endregion PauseScreenVariables

    #region OpeningScreenVariables

    /// <summary>
    /// Opening Screen object, everything relating to the opening screen should be a child of this object
    /// StartButton that contains the event listener
    /// </summary>
    public GameObject OpeningScreen, StartButton;

    /// <summary>
    /// Just a bool to check if the game has started
    /// </summary>
    private bool gameStarted = false;

    /// <summary>
    /// The original transform of the button
    /// </summary>
    private Transform buttonOrigin;

    private Image startImage, placeHolder;
    private RawImage openingImage;
    #endregion OpeningScreenVariables

    #region EndScreenVariables

    public GameObject endScreen, inGameScore, pauseButton;

    public Text finalScore;

    private bool gameOver = false;

    #endregion EndScreenVariables

    #region MonoBehaviour

    void Start () {
        _player = FindObjectOfType<PlayerMovement>();
        _hand = FindObjectOfType<HandAI>();
        score.text = "0000000";
        //if (allowDemo)
            
        StartButton.SetActive(true);
        GameManager.Instance.gameSettings.gameRestart = true;
        PauseIcon.GetComponentInChildren<Button>().interactable = false;
        paused = false;     // Might want to initialize pause as true if we want the game to be paused at the start
        buttonOrigin = StartButton.transform;
        startImage = StartButton.GetComponent<Image>();
        openingImage = OpeningScreen.GetComponent<RawImage>();
        placeHolder = OpeningScreen.GetComponentInChildren<Image>();
	}

	void Update () {
        //if (allowDemo)
        //{
        //    StartButton.SetActive(true);
        //    allowDemo = false;
        //}
        if (gameStarted)
        {
            StartCoroutine(SceneTransition());
            StartButton.transform.Translate(-Vector3.right * Time.deltaTime, Space.Self);
        }
        if (paused)
        {
            Time.timeScale = 0;
            PauseIcon.sprite = startSprite;
            RestartMenu.SetActive(true);
            AudioListener.pause = true;
        }
        else if (!paused)
        {
            Time.timeScale = 1;
            PauseIcon.sprite = pauseSprite;
            RestartMenu.SetActive(false);
            AudioListener.pause = false;
        }
        if (gameOver)
        {
            inGameScore.transform.Translate(Vector3.up * Time.deltaTime * 0.2f, Space.Self);
            pauseButton.transform.Translate(Vector3.up * Time.deltaTime * 0.2f, Space.Self);
            StartCoroutine(ShowEndScreen());
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
        //_hand.StartHandAI();
        GameManager.Instance.gameSettings.gameStart = true;
        PauseIcon.GetComponentInChildren<Button>().interactable = true;
        AddLevel();
        gameStarted = true;
    }

    /// <summary>
    /// Reload the level.
    /// </summary>
    public void RestartLevel()
    {
        GameManager.Instance.gameSettings.gameRestart = true;
        GameManager.Instance.gameSettings.gameStart = false;
        Application.LoadLevel(0);
    }

    public Text score, message, level;
    public int playerScore = 0;
    public int currentLevel = 0;

    public void IncrementScore(int inc)
    {
        string scoreText;
        playerScore+=inc;

        /* Try scoreText = string.Format("{0:0000000}", playerScore); */
        
        if (playerScore < 0)
        {
            playerScore = 0;
            scoreText = "000000" + playerScore;
        }
        else if (playerScore < 1000)
        {
            scoreText = "0000" + playerScore;
        }
        else if (playerScore < 10000)
        {
            scoreText = "000" + playerScore;
        }
        else if (playerScore < 100000)
        {
            scoreText = "00" + playerScore;
        }
        else if (playerScore < 1000000)
        {
            scoreText = "0" + playerScore;
        }
        else
        {
            if (playerScore > 99999999)
            {
                playerScore = 99999999;
            }
            scoreText = playerScore.ToString();
        }

        score.text = scoreText;
    }

    public void AddLevel()
    {
        currentLevel++;
        level.text = "Level: " + currentLevel;
        spawnPool.UpdateRepeatRate();
    }

    public void ShowMessage(string m)
    {
        message.text = m;
        StartCoroutine(EraseMessage());
    }

    public void TogglePause()
    {
        paused = !paused;
    }

    IEnumerator SceneTransition()
    {
        
        StartButton.GetComponentInChildren<Button>().interactable = false;
        startImage.CrossFadeAlpha(0, 1, false);
        openingImage.CrossFadeAlpha(0, 1, false);
        placeHolder.CrossFadeAlpha(0, 1, false);
        yield return new WaitForSeconds(2);
        StartButton.transform.position = buttonOrigin.position;
        OpeningScreen.SetActive(false);
        gameStarted = false;
    }

    IEnumerator EraseMessage()
    {
        yield return new WaitForSeconds(2);
        message.text = "";
    }

    public void SetGameOver(bool b)
    {
        gameOver = b;
    }

    IEnumerator ShowEndScreen()
    {
        yield return new WaitForSeconds(1.5f);
        endScreen.SetActive(true);
        finalScore.text = score.text;
    }
}
