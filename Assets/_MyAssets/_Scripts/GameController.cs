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

    /// <summary>
    /// Reference to the start and restart buttons.
    /// </summary>
    public GameObject StartButton, RestartButton;

	void Start () {
        _player = FindObjectOfType<PlayerMovement>();
        _hand = FindObjectOfType<HandAI>();
        score.text = "Score: " + playerScore;
        if(allowDemo)
            StartButton.SetActive(true);
	}

	void Update () {
        if (allowDemo)
        {
            StartButton.SetActive(true);
            allowDemo = false;
        }
	}

    /// <summary>
    /// Start the game.
    /// </summary>
    public void StartGame()
    {
        _player.SetGameOver(false);
        _hand.StartHandAI();
        StartButton.SetActive(false);
        GameManager.Instance.gameSettings.currentState = GameSettings.gameState.Playing;
    }

    /// <summary>
    /// Reload the level.
    /// </summary>
    public void RestartLevel()
    {
        GameManager.Instance.gameSettings.currentState = GameSettings.gameState.Reset;
        Application.LoadLevel(0);
    }

    public Text score, message;
    public int playerScore = 0;
    public void AddScore()
    {
        playerScore++;
        score.text = "Score: " + playerScore;
    }

    public void ShowMessage(string m)
    {
        message.text = m;
        StartCoroutine(EraseMessage());
    }

    IEnumerator EraseMessage()
    {
        yield return new WaitForSeconds(2);
        message.text = "";
    }
}
