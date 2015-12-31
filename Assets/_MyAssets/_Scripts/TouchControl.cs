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

    [Tooltip("Delay to detect a double touch.")]
    /// <summary>
    /// Delay to detect a double touch
    /// </summary>
    public float doubleTouchDelay = 0.3f;

    /// <summary>
    /// Can a double touch be checked.
    /// </summary>
    private bool checkDoubleTouch = true;

    private float lastTime;

    private float touchCount = 0f;

    private bool startTouch = false;

    /// <summary>
    /// Saves the last touch to get it's position.
    /// </summary>
    public Touch lastTouch;

    /// <summary>
    /// Which direction the is the swipe in.
    /// </summary>
    private TouchInput _touchInput;

    /// <summary>
    /// Reference to the player movement script attached to the character.  Used to call the movement methods.
    /// </summary>
    private PlayerMovement _player;

    #endregion Variables

    #region Monobehaviour

    // Use this for initialization
	void Start () {
        _player = GetComponent<PlayerMovement>();           // Get PlayerMovement component
        screenHeight = Screen.height;                       // Get screen's height
        minSwipeLength = swipePercentage * screenHeight;    // Get the minimum swipe length
	}
	
	// Update is called once per frame
	void Update () {
        DetectTouchInput();
        MovePlayer();
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
                    startTouch = true;
                    break;
                case TouchPhase.Moved:                      // Determine swipe direction
                    direction = touch.position - startPos;
                    break;
                case TouchPhase.Ended:                      // Report that a direction has been chosen, end of the swipe
                    directionChosen = true;
                    break;
            }

            //if (touch.tapCount == 1)
            if(startTouch)
            {
                if (!checkDoubleTouch)
                {
                    touchCount = 0;
                    checkDoubleTouch = true;
                    lastTime = Time.time;
                }
                else if(checkDoubleTouch && touchCount < doubleTouchDelay)
                {
                    touchCount += Time.time - lastTime;
                    lastTime = Time.time;
                }
                else if(checkDoubleTouch && touchCount >= doubleTouchDelay)
                {
                    checkDoubleTouch = false;
                    _touchInput = TouchInput.touch;
                    lastTouch = touch;
                    startTouch = false;
                    Debug.Log("Single Touch");
                }
            }

            if (checkDoubleTouch && touch.tapCount == 2)
            {
                _touchInput = TouchInput.doubleTouch;       // Double Touch
                checkDoubleTouch = false;
                startTouch = false;
                touchCount = 0;
                Debug.Log("Double Touch");
            }
        }

        // Once a swipe is completed
        if (directionChosen)
        {
            if (direction.magnitude >= minSwipeLength)              // If the swipe is longer than the minimum swipe allowed.
            {
                if (direction.y > 0 && direction.y > direction.x)   // Swiped up
                {
                    //Debug.Log("Swipe Up");
                    _touchInput = TouchInput.swipeUp;
                }
                if (direction.y < 0 && direction.y < direction.x)   // Swiped down
                {
                    //Debug.Log("Swipe Down");
                    _touchInput = TouchInput.swipeDown;
                }
                if (direction.x > 0 && direction.x > direction.y)   // Swiped right
                {
                    //Debug.Log("Swipe Right");
                    _touchInput = TouchInput.swipeRight;
                }
                if (direction.x < 0 && direction.x < direction.y)   // Swiped left
                {
                    //Debug.Log("Swipe Left");
                    _touchInput = TouchInput.swipeLeft;
                }
            }
            // Reset the touch
            directionChosen = false;
        }
    }

    /// <summary>
    /// Calls movement methods on the Player Movement script depending on touch input.
    /// </summary>
    private void MovePlayer()
    {
        switch (_touchInput)
        {
            /*case TouchInput.swipeUp:
                _player.MoveUp();
                break;
            case TouchInput.swipeDown:
                _player.MoveDown();
                break;*/
            case TouchInput.doubleTouch:
                _player.Jump();
                break;
            case TouchInput.touch:
                // Call a raycast to check for the lane
                _player.MovePlayerToPlane(lastTouch);
                break;
        }
        _touchInput = TouchInput.none;    // Reset the touch.
    }

    #endregion Private Methods - Touch Input

}
