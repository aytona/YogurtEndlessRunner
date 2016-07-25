using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    [SerializeField, Tooltip("Drag in all Canvases that need to be controlled.")]
    private Canvas[] menuCanvases;

    [Tooltip("The index of the first Canvas to be shown.")]
    public int initialCanvas = 0;

    private int currentCanvas = 0;

    private float timeToSwitch = 0;

    public GameObject[] characters;

    public int characterSelectCanvas;

	void Start () 
    {
        currentCanvas = initialCanvas;
        HideAllCanvases();
        ShowCurrentCanvas();
	}

    /// <summary>
    /// Switch hides the current canvas and shows the canvas of the index passed in.
    /// </summary>
    /// <param name="canvasNum">The index of the canvas to switch to.</param>
    public void GoToCanvas(int canvasNum)
    {
        HideCurrentCanvas();
        currentCanvas = canvasNum;
        ShowCurrentCanvas();
        if(currentCanvas == characterSelectCanvas)
        {
            FindObjectOfType<CharacterSelectAnimation>().Select(0);
        }
    }

    /// <summary>
    /// Load a new scene.
    /// </summary>
    /// <param name="sceneNum">The index of the scene to load.</param>
    public void GoToScene(int sceneNum)
    {
        SceneManager.LoadScene(sceneNum);
    }

    /// <summary>
    /// Set the time it takes before changing canvas.
    /// </summary>
    /// <param name="t">Time delay.</param>
    public void SetTimeToSwitch(float t)
    {
        timeToSwitch = t;
    }

    /// <summary>
    /// Hide all the canvases from view.
    /// </summary>
    public void HideAllCanvases()
    {
        for (int i = 0; i < menuCanvases.Length; ++i)
        {
            menuCanvases[i].gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Hide all canvases after a time.
    /// </summary>
    public void TimedHideAllCanvases()
    {
        StartCoroutine(WaitForHide());
    }

    private void ShowCharacters()
    {
        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].SetActive(true);
        }
    }

    private void HideCharacters()
    {
        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].SetActive(false);
        }
    }

    private void HideCurrentCanvas()
    {
        menuCanvases[currentCanvas].gameObject.SetActive(false);
    }

    private void ShowCurrentCanvas()
    {
        menuCanvases[currentCanvas].gameObject.SetActive(true);
    }

    private IEnumerator WaitForHide()
    {
        yield return new WaitForSeconds(timeToSwitch);
        HideAllCanvases();
    }
}
