using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    /// <summary>
    /// Reference to the player movement script.
    /// </summary>
    private PlayerMovement _player;

    /// <summary>
    /// Reference to the hand ai script.
    /// </summary>
    private HandAI _hand;

    [Tooltip("Set to true to allow the demo.")]
    public bool allowDemo;

    [Tooltip("Bool indicating if the game is paused or not")]
    public bool paused;

    /// <summary>
    /// Reference to the start, restart buttons.
    /// </summary>
    public GameObject StartButton, RestartButton;

    /// <summary>
    /// Reference to the pause icon.
    /// </summary>
    public Image PauseIcon;

    /// <summary>
    /// Reference to the pause textures.
    /// </summary>
    public Sprite pauseSprite, startSprite;

	void Start () {
        _player = FindObjectOfType<PlayerMovement>();
        _hand = FindObjectOfType<HandAI>();
        score.text = "Score: " + playerScore;
        if(allowDemo)
            StartButton.SetActive(true);
        GameManager.Instance.gameSettings.gameRestart = true;

        paused = false;     // Might want to initialize pause as true if we want the game to be paused at the start
	}

	void Update () {
        if (allowDemo)
        {
            StartButton.SetActive(true);
            allowDemo = false;
        }
        if (paused)
        {
            Time.timeScale = 0;
            PauseIcon.sprite = startSprite;
            
        }
        else if (!paused)
        {
            Time.timeScale = 1;
            PauseIcon.sprite = pauseSprite;
        }
	}

    /// <summary>
    /// Start the game.
    /// </summary>
    public void StartGame()
    {
        _player.SetGameOver(false);
        //_hand.StartHandAI();
        StartButton.SetActive(false);
        RestartButton.SetActive(true);
        GameManager.Instance.gameSettings.gameStart = true;
        AddLevel();
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
    public void AddScore()
    {
        playerScore++;
        score.text = playerScore.ToString();
    }

    public void AddLevel()
    {
        currentLevel++;
        level.text = "Level: " + currentLevel;
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

    IEnumerator EraseMessage()
    {
        yield return new WaitForSeconds(2);
        message.text = "";
    }
}
