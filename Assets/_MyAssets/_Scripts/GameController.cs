using UnityEngine;
using System.Collections;

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
    }

    /// <summary>
    /// Reload the level.
    /// </summary>
    public void RestartLevel()
    {
        Application.LoadLevel(0);
    }
}
