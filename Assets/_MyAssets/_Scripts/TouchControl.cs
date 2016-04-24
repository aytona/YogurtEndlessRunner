using UnityEngine;
using System.Collections;

public class TouchControl : MonoBehaviour
{
    /// <summary>
    /// Direction of the swipe that was detected.
    /// </summary>
    public enum TouchInput { swipeUp, swipeDown, swipeLeft, swipeRight, touch, doubleTouch, none };

    #region Variables

    [Tooltip("Percentage of the height of the screen that needs to be swiped for a swipe to count.")]
    /// <summary>
    /// Percentage of the height of the screen that needs to be swiped for a swipe to count.
    /// </summary>
    public float swipePercentage = 1.0f;

    /// <summary>
    /// Stores the screen height.
    /// </summary>
    private float screenHeight;

    /// <summary>
    /// Minimum length for a swipe to be counted.
    /// </summary>
    private float minSwipeLength;

    /// <summary>
    /// Position of the begining of the swipe.
    /// </summary>
    private Vector2 startPos;

    /// <summary>
    /// Vector direction of the swipe.
    /// </summary>
    private Vector2 direction;

    /// <summary>
    /// Has the swipe been completed.
    /// </summary>
    private bool directionChosen;

    /// <summary>
    /// Which direction the is the swipe in.
    /// </summary>
    private TouchInput m_TouchInput;

    /// <summary>
    /// Reference to the player movement script attached to the character.  Used to call the movement methods.
    /// </summary>
    private PlayerMovement m_Player;

    #endregion Variables

    #region Monobehaviour

    // Use this for initialization
	void Start () {
        m_Player = GetComponent<PlayerMovement>();           // Get PlayerMovement component
        screenHeight = Screen.height;                       // Get screen's height
        minSwipeLength = swipePercentage * screenHeight;    // Get the minimum swipe length
	}
	
	// Update is called once per frame
	void Update () {
        if (m_Player.m_CurrentState != PlayerMovement.State.TwoHit)
        {
            DetectTouchInput();
        }
	}

    #endregion Monobehaviour

    #region Private Methods - Touch Input

    /// <summary>
    /// Detects touch and swipe and converts it to game input.
    /// </summary>
    private void DetectTouchInput()
    {
        if (Input.touchCount > 0)                           // If there is a touch on the screen...
        {
            Touch touch = Input.GetTouch(0);                // ...get the first detected touch currently on the screen
            switch (touch.phase)
            {
                case TouchPhase.Began:                      // Records initial touch
                    startPos = touch.position;
                    directionChosen = false;
                    break;
                case TouchPhase.Moved:                      // Determine swipe direction
                    direction = touch.position - startPos;
                    break;
                case TouchPhase.Stationary:
                    direction = touch.position - startPos;
                    break;
                case TouchPhase.Ended:                      // Report that a direction has been chosen, end of the swipe
                    directionChosen = true;
                    break;
            }
        }

        // Once a swipe is completed
        if (directionChosen)
        {
            if (direction.magnitude >= minSwipeLength)              // If the swipe is longer than the minimum swipe allowed.
            {
                if (direction.y > 0)// && direction.y > direction.x)   // Swiped up
                {
                    //Debug.Log("Swipe Up");
                    m_TouchInput = TouchInput.swipeUp;
                }
                if (direction.y < 0)// && direction.y < direction.x)   // Swiped down
                {
                    //Debug.Log("Swipe Down");
                    m_TouchInput = TouchInput.swipeDown;
                }
                /*if (direction.x > 0 && direction.x > direction.y)   // Swiped right
                {
                    //Debug.Log("Swipe Right");
                    _touchInput = TouchInput.swipeRight;
                }
                if (direction.x < 0 && direction.x < direction.y)   // Swiped left
                {
                    //Debug.Log("Swipe Left");
                    _touchInput = TouchInput.swipeLeft;
                }*/
            }
            else
            {
                m_TouchInput = TouchInput.touch;
            }
            // Reset the touch
            directionChosen = false;
        }
        MovePlayer();
    }

    /// <summary>
    /// Calls movement methods on the Player Movement script depending on touch input.
    /// </summary>
    private void MovePlayer()
    {
        switch (m_TouchInput)
        {
            case TouchInput.swipeUp:
                m_Player.MoveUp();
                break;
            case TouchInput.swipeDown:
                m_Player.MoveDown();
                break;
            case TouchInput.touch:
                //Debug.Log("Touch");
                m_Player.Jump();
                break;
        }
        m_TouchInput = TouchInput.none;    // Reset the touch.
    }

    #endregion Private Methods - Touch Input

}
